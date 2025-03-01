using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace eft_dma_shared.Common.Unity.LowLevel.Hooks
{
    public static class NativeHook
    {
        private static readonly Stopwatch _ratelimit = new();

        // Backing Fields
        private static ulong _codeCave;
        private static ulong _unityPlayerDll;
        private static ulong _monoDll;
        private static ulong _hookedMonoFuncAddress;
        private static ulong _hookedMonoFunc;

        /// <summary>
        /// Low Level Cache Access.
        /// </summary>
        private static LowLevelCache Cache => SharedProgram.Config.LowLevelCache;

        /// <summary>
        /// Synchronization Root for NativeHook APIs.
        /// </summary>
        internal static Lock SyncRoot { get; } = new();
        /// <summary>
        /// Code Cave Base Adress.
        /// ShellCodeData struct is placed here at 0x0.
        /// Order 1
        /// </summary>
        internal static ulong CodeCave => _codeCave.Deobfuscate();
        /// <summary>
        /// MonoRuntimeInvoke Address.
        /// </summary>
        private static ulong HookedMonoFuncAddress => _hookedMonoFuncAddress.Deobfuscate();
        /// <summary>
        /// Original MonoRuntimeInvoke Function Address.
        /// </summary>
        private static ulong HookedMonoFunc => _hookedMonoFunc.Deobfuscate();
        /// <summary>
        /// UnityPlayer.dll Base Addr
        /// </summary>
        internal static ulong UnityPlayerDll => _unityPlayerDll.Deobfuscate();
        /// <summary>
        /// mono-2.0-bdwgc.dll Base Addr
        /// </summary>
        internal static ulong MonoDll => _monoDll.Deobfuscate();

        /// <summary>
        /// True if NativeHook is initialized, otherwise False.
        /// </summary>
        public static bool Initialized => _codeCave != 0x0;
        /// <summary>
        /// Hook Fn Address (within the code cave).
        /// Order 2
        /// </summary>
        private static unsafe ulong InvokeHookFunction => MemDMABase.AlignAddress(CodeCave + (uint)sizeof(ShellCodeData)) + 8;
        /// <summary>
        /// AntiPage Fn Address (within the code cave).
        /// Order 3
        /// </summary>
        internal static ulong AntiPageFunction => MemDMABase.AlignAddress(InvokeHookFunction + (uint)_invokeHookShellcodeLength) + 8;

        static NativeHook()
        {
            MemDMABase.GameStopped += MemDMABase_GameStopped;
        }

        private static void MemDMABase_GameStopped(object sender, EventArgs e)
        {
            lock (SyncRoot)
            {
                _codeCave = default;
                _hookedMonoFuncAddress = default;
                _hookedMonoFunc = default;
                _unityPlayerDll = default;
                _monoDll = default;
                ChamsManager.Reset();
                AssetFactory.Reset();
                AntiPage.Reset();
                _ratelimit.Reset();
            }
        }

        private static readonly int _invokeHookShellcodeLength = GetInvokeHookShellcode().Length;
        /// <summary>
        /// Invoke Hook Fn Bytes for our code cave. This is the shellcode that will be executed by the ~IAT hook.
        /// </summary>
        /// <returns>Byte array</returns>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        private static byte[] GetInvokeHookShellcode()
        {
            return new byte[]
            {
                0x48, 0x83, 0xEC, 0x38, 0x48, 0xB8, 0xFA, 0xFA, // 6
                0xFA, 0xFA, 0xFA, 0xFA, 0xFA, 0xFA, 0x48, 0x89,
                0x44, 0x24, 0x28, 0x48, 0x8B, 0x44, 0x24, 0x28,
                0x33, 0xC9, 0x86, 0x08, 0x8A, 0xC1, 0x84, 0xC0,
                0x74, 0x07, 0xC6, 0x44, 0x24, 0x20, 0x01, 0xEB,
                0x05, 0xC6, 0x44, 0x24, 0x20, 0x00, 0x8A, 0x44,
                0x24, 0x20, 0x88, 0x44, 0x24, 0x21, 0x8A, 0x44,
                0x24, 0x21, 0x0F, 0xB6, 0xC0, 0x85, 0xC0, 0x74,
                0x53, 0x48, 0x8B, 0x44, 0x24, 0x28, 0x48, 0x8B,
                0x40, 0x40, 0x48, 0x8B, 0x4C, 0x24, 0x28, 0x48,
                0x8B, 0x49, 0x48, 0x48, 0x89, 0x08, 0x48, 0x8B,
                0x44, 0x24, 0x28, 0x4C, 0x8B, 0x48, 0x28, 0x48,
                0x8B, 0x44, 0x24, 0x28, 0x4C, 0x8B, 0x40, 0x20,
                0x48, 0x8B, 0x44, 0x24, 0x28, 0x48, 0x8B, 0x50,
                0x18, 0x48, 0x8B, 0x44, 0x24, 0x28, 0x48, 0x8B,
                0x48, 0x10, 0x48, 0x8B, 0x44, 0x24, 0x28, 0xFF,
                0x50, 0x08, 0x48, 0x8B, 0x4C, 0x24, 0x28, 0x48,
                0x89, 0x41, 0x38, 0x48, 0x8B, 0x44, 0x24, 0x28,
                0xC6, 0x40, 0x30, 0x01, 0x48, 0x8B, 0x44, 0x24,
                0x28, 0xFF, 0x50, 0x48, 0x48, 0x83, 0xC4, 0x38,
                0xC3
            };
        }

        /// <summary>
        /// Initialize NativeHook.
        /// </summary>
        /// <returns></returns>
        public static unsafe bool Initialize()
        {
            lock (SyncRoot)
            {
                if (Initialized)
                    return true; // Already init
                if (!SharedProgram.Config.MemWritesEnabled || !SharedProgram.Config.AdvancedMemWrites)
                    return false;
                if (_ratelimit.Elapsed < TimeSpan.FromSeconds(10))
                {
                    _ratelimit.Start();
                    return false;
                }
                byte[] orig = null;
                bool origRead = false;
                ulong preCodeCave = default;
                try
                {
                    // Attempt to init from cache.
                    if (TryInitFromCache())
                        return true;
                    Cache.Reset();
                    // Get Hook Addresses and Validate input parameters.
                    preCodeCave = Memory.GetCodeCave();
                    ulong unityDll = Memory.UnityBase;
                    ulong monoDll = Memory.MonoBase;
                    unityDll.ThrowIfInvalidVirtualAddress();
                    monoDll.ThrowIfInvalidVirtualAddress();
                    _unityPlayerDll = unityDll.Obfuscate();
                    _monoDll = monoDll.Obfuscate();
                    ulong hookedMonoFuncAddr = unityDll + NativeOffsets.mono_gc_is_incremental;
                    ulong hookedMonoFunc = Memory.ReadValueEnsure<ulong>(hookedMonoFuncAddr);
                    hookedMonoFunc.ThrowIfInvalidVirtualAddress();
                    _hookedMonoFuncAddress = hookedMonoFuncAddr.Obfuscate();
                    _hookedMonoFunc = hookedMonoFunc.Obfuscate();

                    // Read original bytes from first code cave
                    ulong invokeHookFunction = MemDMABase.AlignAddress(preCodeCave + (uint)sizeof(ShellCodeData)) + 8;
                    ulong invokeHookFunctionEnd = invokeHookFunction + (uint)_invokeHookShellcodeLength;
                    int caveSize = (int)(invokeHookFunctionEnd - preCodeCave) + 8;
                    orig = new byte[caveSize];
                    Memory.ReadBufferEnsure(preCodeCave, orig.AsSpan());
                    origRead = true;

                    // Prepare data structures
                    var invokeHookBytes = GetInvokeHookShellcode();
                    BinaryPrimitives.WriteUInt64LittleEndian(invokeHookBytes.AsSpan(6), preCodeCave);
                    // Place pre-invoke hook shellcode within code cave
                    Memory.WriteBufferEnsure(invokeHookFunction, invokeHookBytes.AsSpan());
                    // Partial Success -> Set Field
                    _codeCave = preCodeCave.Obfuscate();

                    // Alloc RWX Region to move shellcode to
                    ulong codeCave = NativeMethods.AllocRWX(); // lock re-entrancy is OK
                    codeCave.ThrowIfInvalidVirtualAddress();
                    // Place final invoke hook shellcode within code cave in new RWX region
                    invokeHookFunction = MemDMABase.AlignAddress(codeCave + (uint)sizeof(ShellCodeData)) + 8;
                    BinaryPrimitives.WriteUInt64LittleEndian(invokeHookBytes.AsSpan(6), codeCave);
                    Memory.WriteBufferEnsure(invokeHookFunction, invokeHookBytes.AsSpan());
                    // Success -> Set Cache
                    _codeCave = codeCave.Obfuscate();
                    SetCache();
                    LoneLogging.WriteLine("[NativeHook]: Initialize() -> OK");
                    return true;
                }
                catch (Exception ex)
                {
                    _codeCave = default;
                    LoneLogging.WriteLine($"[NativeHook]: Initialize() -> Exception: {ex}");
                    return false;
                }
                finally
                {
                    _ratelimit.Restart();
                    if (origRead)
                    {
                        for (int i = 0; i < 3; i++) // Restore bytes, try 3 times
                        {
                            try
                            {
                                Memory.WriteBufferEnsure(preCodeCave, orig.AsSpan());
                                break;
                            }
                            catch { Thread.Sleep(1); }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Try initialize from the Persistent Cache.
        /// </summary>
        /// <param name="codeCave">Code Cave Address for this process.</param>
        /// <returns>True if initialized OK from Cache, otherwise False.</returns>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        private static bool TryInitFromCache()
        {
            if (Memory.PID == Cache.PID && Cache.CodeCave != 0x0)
            {
                _unityPlayerDll = Cache.UnityPlayerDll;
                _monoDll = Cache.MonoDll;
                _hookedMonoFuncAddress = Cache.HookedMonoFuncAddress;
                _hookedMonoFunc = Cache.HookedMonoFunc;
                _codeCave = Cache.CodeCave;
                LoneLogging.WriteLine("[NativeHook]: Initialize() -> Initialized from cache!");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the Cache Data.
        /// </summary>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        private static void SetCache()
        {
            Cache.PID = Memory.PID;
            Cache.UnityPlayerDll = _unityPlayerDll;
            Cache.MonoDll = _monoDll;
            Cache.HookedMonoFuncAddress = _hookedMonoFuncAddress;
            Cache.HookedMonoFunc = _hookedMonoFunc;
            Cache.CodeCave = _codeCave;
            _ = Cache.SaveAsync();
        }

        /// <summary>
        /// Call a function via NativeHook.
        /// Only call within this API.
        /// </summary>
        /// <param name="function">Function to call.</param>
        /// <param name="rcx"></param>
        /// <param name="rdx"></param>
        /// <param name="r8"></param>
        /// <param name="r9"></param>
        /// <returns>Result</returns>
        public static ulong? Call(ulong function, ulong rcx = 0, ulong rdx = 0, ulong r8 = 0, ulong r9 = 0)
        {
            lock (SyncRoot)
            {
                if (!Initialized)
                {
                    LoneLogging.WriteLine("[NativeHook]: Call() -> Not initialized!");
                    return null;
                }
                if (!function.IsValidVirtualAddress())
                {
                    LoneLogging.WriteLine("[NativeHook]: Call() -> Invalid input.");
                    return null;
                }

                ShellCodeData data = new()
                {
                    calledFunction = function,
                    rcx = rcx,
                    rdx = rdx,
                    r8 = r8,
                    r9 = r9,
                    monoFuncAddress = HookedMonoFuncAddress,
                    monoFunc = HookedMonoFunc
                };

                try
                {
                    Memory.WriteValueEnsure(CodeCave, ref data);
                }
                catch
                {
                    LoneLogging.WriteLine("[NativeHook]: Call() -> Failed to write Shellcode Data.");
                    return null;
                }
                try
                {
                    Memory.WriteValue(HookedMonoFuncAddress, InvokeHookFunction); // Only write once otherwise you may overwrite the restored address
                }
                catch
                {
                    LoneLogging.WriteLine("[NativeHook]: Call() -> *DANGER* Failed to patch Mono Invoke. Game may crash.");
                    Thread.Sleep(150); // Maintain lock for a short period after this failure
                    return null;
                }

                var sw = Stopwatch.StartNew();
                while (sw.Elapsed < TimeSpan.FromSeconds(4.8d)) // Wait for up to 4.8 seconds
                {
                    Thread.Sleep(8);

                    if (ReadShellCodeData(ref data) && data.executed)
                    {
                        return data.result;
                    }
                }
                LoneLogging.WriteLine("[NativeHook]: Call() -> *DANGER* Method was never executed. Game may crash.");
                return null;
            }
        }

        /// <summary>
        /// Read the Shell Code Data from the Code Cave.
        /// </summary>
        /// <returns>True if read successful, otherwise False.</returns>
        private static bool ReadShellCodeData(ref ShellCodeData data)
        {
            try
            {
                Memory.ReadValueEnsure<ShellCodeData>(CodeCave, out data);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        private ref struct ShellCodeData
        {
            public ShellCodeData() { }

            private readonly byte sync = 1;
            public ulong calledFunction;
            public ulong rcx;
            public ulong rdx;
            public ulong r8;
            public ulong r9;
            private readonly byte _executed;
            public readonly ulong result;
            public ulong monoFuncAddress;
            public ulong monoFunc;

            public readonly bool executed => _executed != 0;
        }
    }
}

using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace eft_dma_shared.Common.Unity.LowLevel.Hooks
{
    public static class AntiPage
    {
        private const int MAX_ENTRIES = 4096;
        private static readonly ConcurrentBag<ulong> _registeredVa = new();
        private static readonly Stopwatch _initRateLimitSw = new();
        private static readonly ConcurrentDictionary<ulong, DateTime> _rateLimitDict = new();
        private static bool _set;

        /// <summary>
        /// AntiPage Function Address.
        /// Order 3
        /// </summary>
        private static ulong AntiPageFunc => NativeHook.AntiPageFunction;
        /// <summary>
        /// AntiPage List Address.
        /// Order 4
        /// </summary>
        private static ulong AntiPageList => MemDMABase.AlignAddress(AntiPageFunc + (uint)_shellcodeLength) + 8;

        /// <summary>
        /// True if AntiPage is initialized/ready.
        /// </summary>
        public static bool Initialized => NativeHook.Initialized && _set;

        static AntiPage()
        {
            new Thread(Worker)
            {
                IsBackground = true,
                Priority = ThreadPriority.Lowest
            }.Start();
        }

        /// <summary>
        /// Register a Virtual Address with Anti-Page Module.
        /// </summary>
        /// <param name="va">Virtual address to register.</param>
        /// <param name="va">Count of 'bytes of interest' at this address.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Register(ulong va, uint cb)
        {
            uint pageCount = MemDMABase.ADDRESS_AND_SIZE_TO_SPAN_PAGES(va, cb);
            ulong pagesBase = MemDMABase.PAGE_ALIGN(va);
            for (uint p = 0; p < pageCount; p++)
            {
                _registeredVa.Add(pagesBase + (p * 0x1000));
            }
        }

        private static void Worker()
        {
            while (true)
            {
                try
                {
                    if (Initialized)
                    {
                        var pages = _registeredVa
                            .Distinct()
                            .Where(page => IsValidVirtualAddress(page))
                            .ToArray();
                        _registeredVa.Clear();
                        int count = pages.Length;
                        if (count > 0)
                        {
                            if (count > MAX_ENTRIES)
                            {
                                LoneLogging.WriteLine($"[AntiPage] WARNING: Entry count '{count}' exceeded maximum of '{MAX_ENTRIES}', trimming excess entries...");
                                count = MAX_ENTRIES;
                            }
                            lock (NativeHook.SyncRoot) // Ensure init isn't called before proceeding
                            {
                                try
                                {
                                    if (!Initialized)
                                        throw new Exception("Not Initialized");
                                    Memory.WriteBufferEnsure(AntiPageList, pages.AsSpan(0, count));
                                    if (NativeHook.Call(AntiPageFunc, (ulong)count) is not ulong ret)
                                        throw new Exception("Call failed");
                                    LoneLogging.WriteLine($"[AntiPage] OK: {count} -> {ret}");
                                }
                                catch
                                {
                                    if (Initialized)
                                    {
                                        _ = NativeHook.Call(AntiPageFunc, 0); // Derefs the list to prevent pageouts
                                    }
                                    throw;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoneLogging.WriteLine($"[AntiPage] ERROR: {ex}");
                }
                finally
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }

        /// <summary>
        /// Checks if a virtual address is valid, and checks if it is being rate-limited.
        /// </summary>
        /// <param name="va"></param>
        /// <returns>True if valid, otherwise False.</returns>
        private static bool IsValidVirtualAddress(ulong va)
        {
            if (!va.IsValidVirtualAddress())
                return false;
            var now = DateTime.Now;
            if (_rateLimitDict.TryGetValue(va, out DateTime last))
            {
                if (now - last < TimeSpan.FromSeconds(10))
                    return false;
            }
            _rateLimitDict[va] = now;
            return true;
        }

        private static readonly int _shellcodeLength = GetShellCode().Length;
        /// <summary>
        /// Get the shellcode for the AntiPage function.
        /// </summary>
        /// <returns></returns>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        private static byte[] GetShellCode()
        {
            return new byte[]
            {
                0x4C, 0x89, 0x4C, 0x24, 0x20, 0x4C, 0x89, 0x44,
                0x24, 0x18, 0x48, 0x89, 0x54, 0x24, 0x10, 0x48,
                0x89, 0x4C, 0x24, 0x08, 0x57, 0x48, 0x81, 0xEC,
                0x90, 0x00, 0x00, 0x00, 0x48, 0xB8, 0xFA, 0xFA, // 30
                0xFA, 0xFA, 0xFA, 0xFA, 0xFA, 0xFA, 0x48, 0x89,
                0x44, 0x24, 0x40, 0x48, 0xB8, 0xFB, 0xFB, 0xFB, // 45
                0xFB, 0xFB, 0xFB, 0xFB, 0xFB, 0x48, 0x89, 0x44,
                0x24, 0x48, 0x48, 0x83, 0xBC, 0x24, 0xA0, 0x00,
                0x00, 0x00, 0x00, 0x0F, 0x84, 0xC5, 0x00, 0x00,
                0x00, 0x48, 0x81, 0xBC, 0x24, 0xA0, 0x00, 0x00,
                0x00, 0x00, 0x10, 0x00, 0x00, 0x0F, 0x87, 0xB3,
                0x00, 0x00, 0x00, 0x48, 0xC7, 0x44, 0x24, 0x30,
                0x00, 0x00, 0x00, 0x00, 0x48, 0x8D, 0x44, 0x24,
                0x58, 0x48, 0x8B, 0xF8, 0x33, 0xC0, 0xB9, 0x30,
                0x00, 0x00, 0x00, 0xF3, 0xAA, 0x48, 0xC7, 0x44,
                0x24, 0x28, 0x00, 0x00, 0x00, 0x00, 0xEB, 0x0D,
                0x48, 0x8B, 0x44, 0x24, 0x28, 0x48, 0xFF, 0xC0,
                0x48, 0x89, 0x44, 0x24, 0x28, 0x48, 0x8B, 0x84,
                0x24, 0xA0, 0x00, 0x00, 0x00, 0x48, 0x39, 0x44,
                0x24, 0x28, 0x73, 0x6B, 0x48, 0x8B, 0x44, 0x24,
                0x40, 0x48, 0x8B, 0x4C, 0x24, 0x28, 0x48, 0x8B,
                0x04, 0xC8, 0x48, 0x89, 0x44, 0x24, 0x38, 0x41,
                0xB8, 0x30, 0x00, 0x00, 0x00, 0x48, 0x8D, 0x54,
                0x24, 0x58, 0x48, 0x8B, 0x4C, 0x24, 0x38, 0xFF,
                0x54, 0x24, 0x48, 0x48, 0x85, 0xC0, 0x74, 0x3A,
                0x81, 0x7C, 0x24, 0x78, 0x00, 0x10, 0x00, 0x00,
                0x75, 0x30, 0x8B, 0x44, 0x24, 0x7C, 0x83, 0xE0,
                0x66, 0x85, 0xC0, 0x74, 0x25, 0x8B, 0x44, 0x24,
                0x7C, 0x25, 0x00, 0x01, 0x00, 0x00, 0x85, 0xC0,
                0x75, 0x18, 0x48, 0x8B, 0x44, 0x24, 0x38, 0x8A,
                0x00, 0x88, 0x44, 0x24, 0x24, 0x48, 0x8B, 0x44,
                0x24, 0x30, 0x48, 0xFF, 0xC0, 0x48, 0x89, 0x44,
                0x24, 0x30, 0xE9, 0x79, 0xFF, 0xFF, 0xFF, 0x48,
                0x8B, 0x44, 0x24, 0x30, 0xEB, 0x38, 0xC7, 0x44,
                0x24, 0x20, 0x00, 0x00, 0x00, 0x00, 0xEB, 0x0D,
                0x8B, 0x44, 0x24, 0x20, 0x05, 0x00, 0x02, 0x00,
                0x00, 0x89, 0x44, 0x24, 0x20, 0x81, 0x7C, 0x24,
                0x20, 0x00, 0x10, 0x00, 0x00, 0x7D, 0x15, 0x48,
                0x63, 0x44, 0x24, 0x20, 0x48, 0x8B, 0x4C, 0x24,
                0x40, 0x48, 0x8B, 0x04, 0xC1, 0x48, 0x89, 0x44,
                0x24, 0x50, 0xEB, 0xD4, 0x33, 0xC0, 0x48, 0x81,
                0xC4, 0x90, 0x00, 0x00, 0x00, 0x5F, 0xC3
            };
        }

        /// <summary>
        /// Initialize AntiPage.
        /// </summary>
        /// <returns></returns>
        public static bool Initialize()
        {
            if (Initialized)
                return true; // Already init
            if (!SharedProgram.Config.MemWritesEnabled || !SharedProgram.Config.AdvancedMemWrites || !NativeHook.Initialized)
                return false;
            if (_initRateLimitSw.IsRunning && _initRateLimitSw.Elapsed < TimeSpan.FromSeconds(10))
                return false;
            try
            {
                ulong virtualQuery = Memory.GetExport("kernel32.dll", "VirtualQuery");
                byte[] shellcode = GetShellCode();
                ulong listBase = AntiPageList;
                listBase.ThrowIfInvalidVirtualAddress();
                BinaryPrimitives.WriteUInt64LittleEndian(shellcode.AsSpan(30), listBase);
                BinaryPrimitives.WriteUInt64LittleEndian(shellcode.AsSpan(45), virtualQuery);
                ulong funcAddr = AntiPageFunc;
                funcAddr.ThrowIfInvalidVirtualAddress();
                Memory.WriteBufferEnsure(funcAddr, shellcode.AsSpan());
                _set = true;
                LoneLogging.WriteLine("[AntiPage]: Initialize() -> OK");
                return true;
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine("[AntiPage]: Initialize() -> Exception: " + ex.ToString());
                return false;
            }
            finally
            {
                _initRateLimitSw.Restart();
            }
        }

        internal static void Reset()
        {
            _set = default;
            _rateLimitDict.Clear();
            _registeredVa.Clear();
        }
    }
}

using arena_dma_radar.UI.Hotkeys;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.Arena
{
    internal static class InputManager
    {
        private static ulong _inputManager;

        static InputManager()
        {
            new Thread(Worker)
            {
                IsBackground = true
            }.Start();
        }

        /// <summary>
        /// Attempts to load Input Manager.
        /// </summary>
        /// <param name="unityBase">UnityPlayer.dll Base Addr</param>
        public static void Initialize(ulong unityBase)
        {
            try
            {
                _inputManager = Memory.ReadPtr(unityBase + UnityOffsets.ModuleBase.InputManager, false);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Initializing Input Manager", ex);
            }
        }

        /// <summary>
        /// Reset InputManager (usually after game closure).
        /// </summary>
        public static void Reset()
        {
            _inputManager = 0x0;
        }

        /// <summary>
        /// InputManager Managed thread.
        /// </summary>
        private static void Worker()
        {
            LoneLogging.WriteLine("InputManager thread starting...");
            while (true)
            {
                try
                {
                    if (MemDMABase.WaitForProcess())
                    {
                        ProcessAllHotkeys();
                    }
                }
                catch { }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }
        /// <summary>
        /// Check all hotkeys, and execute delegates.
        /// </summary>
        private static void ProcessAllHotkeys()
        {
            if (HotkeyManager.Hotkeys is IReadOnlyDictionary<UnityKeyCode, HotkeyManager.HotkeyAction> hotkeys &&
                hotkeys.Count > 0)
            {
                using var map = ScatterReadMap.Get();
                var round1 = map.AddRound();
                var round2 = map.AddRound(false);
                int i = 0;
                foreach (var kvp in hotkeys)
                {
                    ProcessHotkey(kvp.Key, kvp.Value, round1[i], round2[i]);
                    i++;
                }
                map.Execute();
            }
        }

        /// <summary>
        /// Checks if a Hotkey is pressed, and if pressed executes the related Action Controller.
        /// </summary>
        /// <param name="keycode">Hotkey key value</param>
        /// <param name="action">Hotkey action controller</param>
        /// <param name="idx1">Round1 (cached)</param>
        /// <param name="idx2">Round2 (non-cached)</param>
        private static void ProcessHotkey(UnityKeyCode keycode, HotkeyManager.HotkeyAction action, ScatterReadIndex idx1, ScatterReadIndex idx2)
        {
            uint v3 = (uint)keycode;
            uint v6 = v3 >> 5;

            idx1.AddEntry<MemPointer>(0, _inputManager + UnityOffsets.UnityInputManager.CurrentKeyState);
            idx1.Callbacks += x1 =>
            {
                if (x1.TryGetResult<MemPointer>(0, out var v10))
                {
                    uint v11 = v3 & 0x1F;
                    idx2.AddEntry<uint>(0, v10 + v6 * 0x4); // v10[v6] = Result
                    idx2.Callbacks += x2 =>
                    {
                        if (x2.TryGetResult<uint>(0, out var v12))
                        {
                            bool isKeyDown = (v12 & 1u << (int)v11) != 0;
                            action.Execute(isKeyDown);
                        }
                    };
                }
            };
        }
    }
}

using arena_dma_radar.Arena.Features.MemoryWrites;
using arena_dma_radar.Arena.GameWorld;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity.LowLevel;
using eft_dma_shared.Common.Unity.LowLevel.Hooks;

namespace arena_dma_radar.Arena.Features
{
    /// <summary>
    /// Feature Manager Thread.
    /// </summary>
    internal static class FeatureManager
    {
        internal static void ModuleInit()
        {
            new Thread(Worker)
            {
                IsBackground = true
            }.Start();
        }

        static FeatureManager()
        {
            MemDMABase.GameStarted += Memory_GameStarted;
            MemDMABase.GameStopped += Memory_GameStopped;
            MemDMABase.RaidStarted += Memory_RaidStarted;
            MemDMABase.RaidStopped += Memory_RaidStopped;
        }

        private static void Worker()
        {
            LoneLogging.WriteLine("Features Thread Starting...");
            while (true)
            {
                try
                {
                    if (MemDMABase.WaitForProcess() && MemWrites.Enabled && Memory.Ready)
                    {
                        while (MemWrites.Enabled && Memory.Ready)
                        {
                            if (MemWrites.Config.AdvancedMemWrites && !NativeHook.Initialized)
                            {
                                NativeHook.Initialize();
                            }
                            if (NativeHook.Initialized &&
                                 Chams.Config.Mode is not ChamsManager.ChamsMode.Basic &&
                                 ChamsManager.Materials.Count == 0)
                            {
                                ChamsManager.Initialize();
                            }
                            if (MemWrites.Config.AntiPage && NativeHook.Initialized && !AntiPage.Initialized)
                            {
                                AntiPage.Initialize();
                            }
                            var memWrites = IFeature.AllFeatures
                                .OfType<IMemWriteFeature>()
                                .Where(feature => feature.CanRun);
                            if (memWrites.Any())
                            {
                                ExecuteMemWrites(memWrites);
                            }
                            var patches = IFeature.AllFeatures
                                .OfType<IMemPatchFeature>()
                                .Where(feature => feature.CanRun);
                            if (patches.Any())
                            {
                                ExecuteMemPatches(patches);
                            }
                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    LoneLogging.WriteLine($"[Features Thread] CRITICAL ERROR: {ex}");
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Executes MemWrite Features.
        /// </summary>
        private static void ExecuteMemWrites(IEnumerable<IMemWriteFeature> memWrites)
        {
            try
            {
                using var hScatter = new ScatterWriteHandle();
                foreach (var feature in memWrites)
                {
                    feature.TryApply(hScatter);
                    feature.OnApply();
                }
                if (Memory.Game is LocalGameWorld game)
                {
                    hScatter.Execute(DoWrite);
                    bool DoWrite() =>
                        MemWrites.Enabled && game.IsSafeToWriteMem;
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"MemWrites [FAIL] {ex}");
            }
        }

        /// <summary>
        /// Executes MemWrite Features.
        /// </summary>
        private static void ExecuteMemPatches(IEnumerable<IMemPatchFeature> patches)
        {
            try
            {
                foreach (var feature in patches)
                {
                    feature.TryApply();
                    feature.OnApply();
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"MemPatches [FAIL] {ex}");
            }
        }

        private static void Memory_GameStarted(object sender, EventArgs e)
        {
            foreach (var feature in IFeature.AllFeatures)
            {
                feature.OnGameStart();
            }
        }

        private static void Memory_GameStopped(object sender, EventArgs e)
        {
            foreach (var feature in IFeature.AllFeatures)
            {
                feature.OnGameStop();
            }
        }


        private static void Memory_RaidStarted(object sender, EventArgs e)
        {
            foreach (var feature in IFeature.AllFeatures)
            {
                feature.OnRaidStart();
            }
        }

        private static void Memory_RaidStopped(object sender, EventArgs e)
        {
            foreach (var feature in IFeature.AllFeatures)
            {
                feature.OnRaidEnd();
            }
        }
    }

    /// <summary>
    /// Helper Class.
    /// </summary>
    internal static class MemWrites
    {
        /// <summary>
        /// For Syncing.
        /// </summary>
        public static readonly Lock SyncRoot = new();

        /// <summary>
        /// DMAToolkit/MemWrites Config.
        /// </summary>
        public static MemWritesConfig Config { get; } = Program.Config.MemWrites;

        /// <summary>
        /// True if Memory Writes are enabled, otherwise False.
        /// </summary>
        public static bool Enabled
        {
            get => Config.MemWritesEnabled;
            set => Config.MemWritesEnabled = value;
        }
    }
}
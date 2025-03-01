using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class FullBright : MemWriteFeature<FullBright>
    {
        private bool _set;

        public override bool Enabled
        {
            get => MemWrites.Config.FullBright;
            set => MemWrites.Config.FullBright = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(200);


        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && !_set)
                {
                    var levelSettings = Memory.ReadPtr(MonoLib.LevelSettingsField);

                    writes.AddValueEntry(levelSettings + Offsets.LevelSettings.AmbientMode, (int)AmbientMode.Trilight);

                    const float brightness = 0.35f; // This is a good level for all maps
                    var equatorColor = new UnityColor(brightness, brightness, brightness);
                    var groundColor = new UnityColor(0f, 0f, 0f);
                    writes.AddValueEntry(levelSettings + Offsets.LevelSettings.EquatorColor, ref equatorColor);
                    writes.AddValueEntry(levelSettings + Offsets.LevelSettings.GroundColor, ref groundColor);
                    writes.Callbacks += () =>
                    {
                        _set = true;
                        LoneLogging.WriteLine("FullBright [On]");
                    };
                }
                else if (!Enabled && _set)
                {
                    var levelSettings = Memory.ReadPtr(MonoLib.LevelSettingsField);
                    writes.AddValueEntry(levelSettings + Offsets.LevelSettings.AmbientMode, (int)AmbientMode.Flat);
                    writes.Callbacks += () =>
                    {
                        _set = false;
                        LoneLogging.WriteLine("FullBright [Off]");
                    };
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring FullBright: {ex}");
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
        }

        private enum AmbientMode : int
        {
            Skybox,
            Trilight,
            Flat = 3,
            Custom
        }
    }
}

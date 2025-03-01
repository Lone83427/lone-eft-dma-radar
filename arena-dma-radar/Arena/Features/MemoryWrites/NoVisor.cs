using arena_dma_radar.Arena.GameWorld;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using arena_dma_radar.Arena.Features;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.Features.MemoryWrites
{
    public sealed class NoVisor : MemWriteFeature<NoVisor>
    {
        public override bool Enabled
        {
            get => MemWrites.Config.NoVisor;
            set => MemWrites.Config.NoVisor = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && Memory.Game is LocalGameWorld game)
                {
                    const float newVisor = 0f;
                    var cm = game.CameraManager;
                    var visorEffect = MonoBehaviour.GetComponent(cm.FPSCamera, "VisorEffect");
                    if (visorEffect != 0x0)
                    {
                        var currentVisor = Memory.ReadValue<float>(visorEffect + Offsets.VisorEffect.Intensity, false);
                        if (currentVisor != newVisor)
                        {
                            writes.AddValueEntry(visorEffect + Offsets.VisorEffect.Intensity, newVisor);
                            LoneLogging.WriteLine($"NoVisor -> {newVisor}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring NoVisor: {ex}");
            }
        }
    }
}

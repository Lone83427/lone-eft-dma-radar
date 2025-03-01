using arena_dma_radar.Arena.GameWorld;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel;
using arena_dma_radar.Arena.Features;
using eft_dma_shared.Common.Misc.Config;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.Features.MemoryWrites
{
    public sealed class Chams : MemWriteFeature<Chams>
    {
        /// <summary>
        /// Chams Config.
        /// </summary>
        public static ChamsConfig Config { get; } = MemWrites.Config.Chams;

        public override bool Enabled
        {
            get => Config.Enabled;
            set => Config.Enabled = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && Memory.Game is LocalGameWorld game)
                {
                    var cm = game.CameraManager;
                    var mode = Config.Mode; // Cache value
                    var players = game.Players
                        .Where(x => x.IsHostileActive)
                        .Where(x => x.ChamsMode != mode);
                    if (!players.Any()) // Already Set
                        return;
                    int materialID;
                    switch (mode)
                    {
                        case ChamsManager.ChamsMode.Basic:
                            var ssaa = MonoBehaviour.GetComponent(cm.FPSCamera, "SSAA");
                            var opticMaskMaterial = Memory.ReadPtr(ssaa + UnityOffsets.SSAA.OpticMaskMaterial);
                            var opticMonoBehaviour = Memory.ReadPtr(opticMaskMaterial + ObjectClass.MonoBehaviourOffset);
                            materialID = Memory.ReadValue<MonoBehaviour>(opticMonoBehaviour).InstanceID;
                            break;
                        case ChamsManager.ChamsMode.Visible:
                            if (ChamsManager.Materials!.TryGetValue(ChamsManager.ChamsMode.Visible, out var visible) &&
                                visible.InstanceID < 0)
                            {
                                materialID = visible.InstanceID;
                            }
                            else
                            {
                                return;
                            }
                            break;
                        case ChamsManager.ChamsMode.VisCheck:
                            if (ChamsManager.Materials!.TryGetValue(ChamsManager.ChamsMode.VisCheck, out var vischeck) &&
                                vischeck.InstanceID < 0)
                            {
                                materialID = vischeck.InstanceID;
                            }
                            else
                            {
                                return;
                            }
                            break;
                        default:
                            throw new NotImplementedException(nameof(mode));
                    }

                    using var hChamsScatter = new ScatterWriteHandle();
                    foreach (var player in players)
                    {
                        player.SetChams(hChamsScatter, game, mode, materialID);
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring Chams: {ex}");
            }
        }
    }
}

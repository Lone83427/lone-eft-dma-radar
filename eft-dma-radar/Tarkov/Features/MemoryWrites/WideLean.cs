using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class WideLean : MemWriteFeature<WideLean>
    {
        public static EWideLeanDirection Direction = EWideLeanDirection.Off;
        private bool _set = false;

        public override bool Enabled
        {
            get => MemWrites.Config.WideLean.Enabled;
            set => MemWrites.Config.WideLean.Enabled = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);


        /// <summary>
        /// Wide Lean Config.
        /// </summary>
        public static WideLeanConfig Config { get; } = MemWrites.Config.WideLean;

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    var dir = Direction;
                    if (Enabled && dir is not EWideLeanDirection.Off && !_set)
                    {
                        float amt = Config.Amount * .01f * 0.2f;
                        switch (dir)
                        {
                            case EWideLeanDirection.Left:
                                var left = new Vector3(-amt, 0f, 0f);
                                writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, ref left);
                                break;
                            case EWideLeanDirection.Right:
                                var right = new Vector3(amt, 0f, 0f);
                                writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, ref right);
                                break;
                            case EWideLeanDirection.Up:
                                var up = new Vector3(0f, 0f, amt);
                                writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, ref up);
                                break;
                            default:
                                throw new InvalidOperationException("Invalid wide lean option");
                        }
                        writes.Callbacks += () =>
                        {
                            _set = true;
                            LoneLogging.WriteLine("Wide Lean [On]");
                        };
                    }
                    else if (_set && dir is EWideLeanDirection.Off)
                    {
                        var off = Vector3.Zero;
                        writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.PositionZeroSum, ref off);
                        writes.Callbacks += () =>
                        {
                            _set = false;
                            LoneLogging.WriteLine("Wide Lean [Off]");
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Direction = EWideLeanDirection.Off;
                LoneLogging.WriteLine($"Wide Lean [ERROR] {ex}");
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
        }

        public enum EWideLeanDirection
        {
            Off,
            Left,
            Right,
            Up
        }
    }
}

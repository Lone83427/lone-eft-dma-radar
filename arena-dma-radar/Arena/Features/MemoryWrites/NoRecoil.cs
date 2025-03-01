using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.Arena.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.Features.MemoryWrites
{
    public sealed class NoRecoil : MemWriteFeature<NoRecoil>
    {
        public override bool Enabled
        {
            get => MemWrites.Config.NoRecoil;
            set => MemWrites.Config.NoRecoil = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(20);

        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    float recoilAmt = MemWrites.Config.NoRecoilAmount / 100f;
                    float newSway = MemWrites.Config.NoSwayAmount / 100f;

                    Vector3 newRecoil = new Vector3(recoilAmt, recoilAmt, recoilAmt);
                    var breathEff = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Breath);
                    var shotEff = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Shootingg);
                    var newShotRecoil = Memory.ReadPtr(shotEff + Offsets.ShotEffector.NewShotRecoil);
                    var breathAmt = Memory.ReadValue<float>(breathEff + Offsets.BreathEffector.Intensity, false);
                    var shotAmt = Memory.ReadValue<Vector3>(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, false);
                    ValidateSway(breathAmt);
                    ValidateRecoil(shotAmt);
                    if (breathAmt != newSway)
                    {
                        writes.AddValueEntry(breathEff + Offsets.BreathEffector.Intensity, newSway);
                        LoneLogging.WriteLine($"NoRecoil BreathEffector {breathAmt} -> {newSway}");
                    }
                    if (shotAmt != newRecoil)
                    {
                        writes.AddValueEntry(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, newRecoil);
                        LoneLogging.WriteLine($"NoRecoil ShotEffector {shotAmt} -> {newRecoil}");
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring Reduced Recoil/Sway: {ex}");
            }
            static void ValidateSway(float amount)
            {
                if (amount < 0f || amount > 5f)
                    throw new ArgumentOutOfRangeException(nameof(amount));
            }
            static void ValidateRecoil(Vector3 amount)
            {
                if (amount.X < 0f || amount.X > 1f)
                    throw new ArgumentOutOfRangeException(nameof(amount));
                if (amount.Y < 0f || amount.Y > 1f)
                    throw new ArgumentOutOfRangeException(nameof(amount));
                if (amount.Z < 0f || amount.Z > 1f)
                    throw new ArgumentOutOfRangeException(nameof(amount));
            }
        }
    }
}

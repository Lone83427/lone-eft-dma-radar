using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class NoRecoil : MemWriteFeature<NoRecoil>
    {
        private float _lastRecoil;
        private float _lastSway;

        public override bool Enabled
        {
            get => MemWrites.Config.NoRecoil;
            set => MemWrites.Config.NoRecoil = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(50);

        public override void TryApply(ScatterWriteHandle writes)
        {
            const Enums.EProceduralAnimationMask originalPWAMask =
                Enums.EProceduralAnimationMask.MotionReaction |
                Enums.EProceduralAnimationMask.ForceReaction |
                Enums.EProceduralAnimationMask.Shooting |
                Enums.EProceduralAnimationMask.DrawDown |
                Enums.EProceduralAnimationMask.Aiming |
                Enums.EProceduralAnimationMask.Breathing;
            try
            {
                bool enabled = Enabled;
                if (Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    var recoilAmt = MemWrites.Config.NoRecoilAmount * .01f;
                    var newSway = MemWrites.Config.NoSwayAmount * .01f;
                    if (MemWriteFeature<RageMode>.Instance.Enabled) // Override if Rage Mode is enabled
                    {
                        enabled = true;
                        recoilAmt = 0f;
                        newSway = 0f;
                    }

                    if (!enabled && _lastRecoil == 1.0f && _lastSway == 1.0f)
                    {
                        return;
                    }

                    if (!enabled)
                    {
                        recoilAmt = 1.0f;
                        newSway = 1.0f;
                    }

                    var breathEff = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Breath);
                    var shotEff = Memory.ReadPtr(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Shootingg);
                    var newShotRecoil = Memory.ReadPtr(shotEff + Offsets.ShotEffector.NewShotRecoil);

                    var breathAmt = Memory.ReadValue<float>(breathEff + Offsets.BreathEffector.Intensity, false);
                    var shotAmt =
                        Memory.ReadValue<Vector3>(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, false);
                    var mask = Memory.ReadValue<int>(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask, false);
                    ValidateSway(breathAmt);
                    ValidateRecoil(shotAmt);
                    ValidateMask(mask);
                    if (breathAmt != newSway)
                    {
                        writes.AddValueEntry(breathEff + Offsets.BreathEffector.Intensity, newSway);
                        LoneLogging.WriteLine($"NoRecoil BreathEffector {breathAmt} -> {newSway}");
                    }

                    var recoilAmtVec = new Vector3(recoilAmt, recoilAmt, recoilAmt);
                    if (shotAmt != recoilAmtVec)
                    {
                        writes.AddValueEntry(newShotRecoil + Offsets.NewShotRecoil.IntensitySeparateFactors, recoilAmtVec);
                        LoneLogging.WriteLine($"NoRecoil ShotEffector {shotAmt} -> {recoilAmtVec}");
                    }

                    var resetMask = newSway > 0 && _lastSway == 0 || recoilAmt > 0 && _lastRecoil == 0;
                    if (resetMask)
                        WriteMask((int)originalPWAMask);
                    else if (recoilAmt == 0 && newSway == 0 && mask != 1)
                        WriteMask(1);
                    _lastRecoil = recoilAmt;
                    _lastSway = newSway;

                    void WriteMask(int newMask)
                    {
                        writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.Mask, newMask);
                        LoneLogging.WriteLine($"NoRecoil Mask {mask} -> {newMask}");
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring NoRecoil/NoSway: {ex}");
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

            static void ValidateMask(int mask)
            {
                if (mask < 0 || mask > 256)
                    throw new ArgumentOutOfRangeException(nameof(mask));
            }
        }

        public override void OnRaidStart()
        {
            _lastRecoil = default;
            _lastSway = default;
        }
    }
}

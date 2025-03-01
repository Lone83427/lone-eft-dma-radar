using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class MoveSpeed : MemWriteFeature<MoveSpeed>
    {
        private bool _set = false;

        public override bool Enabled
        {
            get => MemWrites.Config.MoveSpeed;
            set => MemWrites.Config.MoveSpeed = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);


        public override void TryApply(ScatterWriteHandle writes)
        {
            const float baseSpeed = 1.0f;
            const float increasedSpeed = 1.2f; // Any higher risks a ban
            try
            {
                if (Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    bool enabled = Enabled;
                    if (enabled && !_set)
                    {
                        var pAnimators = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
                        using var animators = MemArray<ulong>.Get(pAnimators);
                        var a = Memory.ReadPtrChain(animators[0], new uint[] { Offsets.BodyAnimator.UnityAnimator, ObjectClass.MonoBehaviourOffset });
                        var current = Memory.ReadValue<float>(a + UnityOffsets.UnityAnimator.Speed, false);
                        ValidateSpeed(current);
                        Memory.WriteValueEnsure(a + UnityOffsets.UnityAnimator.Speed, increasedSpeed);
                        _set = true;
                        LoneLogging.WriteLine("Move Speed [On]");
                    }
                    else if (!enabled && _set)
                    {
                        var pAnimators = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
                        using var animators = MemArray<ulong>.Get(pAnimators);
                        var a = Memory.ReadPtrChain(animators[0], new uint[] { Offsets.BodyAnimator.UnityAnimator, ObjectClass.MonoBehaviourOffset });
                        var current = Memory.ReadValue<float>(a + UnityOffsets.UnityAnimator.Speed, false);
                        ValidateSpeed(current);
                        Memory.WriteValueEnsure(a + UnityOffsets.UnityAnimator.Speed, baseSpeed);
                        _set = false;
                        LoneLogging.WriteLine("Move Speed [Off]");
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Setting Move Speed: {ex}");
            }
            static void ValidateSpeed(float speed)
            {
                if (!float.IsNormal(speed) || speed < baseSpeed - 0.2f || speed > increasedSpeed + 0.2f)
                    throw new ArgumentOutOfRangeException(nameof(speed));
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
        }
    }
}

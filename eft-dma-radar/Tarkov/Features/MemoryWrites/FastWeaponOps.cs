using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class FastWeaponOps : MemWriteFeature<FastWeaponOps>
    {
        private bool _set;
        private ulong _hands;
        public override bool Enabled
        {
            get => MemWrites.Config.FastWeaponOps;
            set => MemWrites.Config.FastWeaponOps = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromMilliseconds(100);

        public override void TryApply(ScatterWriteHandle writes)
        {
            const float fast = 4f;
            const float normal = 1f;
            try
            {
                if (Memory.LocalPlayer is LocalPlayer localPlayer && ILocalPlayer.HandsController is ulong hands && hands.IsValidVirtualAddress())
                {
                    if (hands != _hands)
                    {
                        _set = false;
                        _hands = hands;
                    }
                    string className = ObjectClass.ReadName(hands);
                    if (className.Contains("FirearmController"))
                    {
                        if (Enabled)
                        {
                            writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._aimingSpeed, 9999f); // Instant ADS
                            if (!_set)
                            {
                                var pAnimators = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
                                using var animators = MemArray<ulong>.Get(pAnimators);
                                var a = Memory.ReadPtrChain(animators[1], new uint[] { Offsets.BodyAnimator.UnityAnimator, ObjectClass.MonoBehaviourOffset });
                                var current = Memory.ReadValue<float>(a + UnityOffsets.UnityAnimator.Speed, false);
                                ThrowIfInvalidSpeed(current);
                                writes.AddValueEntry(a + UnityOffsets.UnityAnimator.Speed, fast);
                                writes.Callbacks += () =>
                                {
                                    _set = true;
                                    LoneLogging.WriteLine("FastWeaponOps [On]");
                                };
                            }
                        }
                        else if (!Enabled && _set)
                        {
                            var pAnimators = Memory.ReadPtr(localPlayer + Offsets.Player._animators);
                            using var animators = MemArray<ulong>.Get(pAnimators);
                            var a = Memory.ReadPtrChain(animators[1], new uint[] { Offsets.BodyAnimator.UnityAnimator, ObjectClass.MonoBehaviourOffset });
                            var current = Memory.ReadValue<float>(a + UnityOffsets.UnityAnimator.Speed, false);
                            ThrowIfInvalidSpeed(current);
                            writes.AddValueEntry(a + UnityOffsets.UnityAnimator.Speed, normal);
                            writes.AddValueEntry(localPlayer.PWA + Offsets.ProceduralWeaponAnimation._aimingSpeed, 1f);
                            writes.Callbacks += () =>
                            {
                                _set = false;
                                LoneLogging.WriteLine("FastWeaponOps [Off]");
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring FastWeaponOps: {ex}");
            }

            static void ThrowIfInvalidSpeed(float speed)
            {
                if (!float.IsNormal(speed) || speed < normal - 0.2f || speed > fast + 0.2f)
                    throw new ArgumentOutOfRangeException(nameof(speed));
            }
        }

        public override void OnRaidStart()
        {
            _set = default;
            _hands = default;
        }
    }
}

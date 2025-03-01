using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.Tarkov.GameWorld;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity.Collections;

namespace eft_dma_radar.Tarkov.Features.MemoryWrites
{
    public sealed class InfStamina : MemWriteFeature<InfStamina>
    {
        private const byte _infStamSourceStateName = (byte)Enums.EPlayerState.Sprint;
        private const byte _infStamTargetStateName = (byte)Enums.EPlayerState.Transition;
        private bool _bypassSet;

        public override bool Enabled
        {
            get => MemWrites.Config.InfStamina;
            set => MemWrites.Config.InfStamina = value;
        }

        protected override TimeSpan Delay => TimeSpan.FromSeconds(1);


        public override void TryApply(ScatterWriteHandle writes)
        {
            try
            {
                if (Enabled && Memory.Game is LocalGameWorld game && Memory.LocalPlayer is LocalPlayer localPlayer)
                {
                    if (!_bypassSet)
                    {
                        ApplyFatigueBypass_V2(localPlayer);
                    }

                    /// Apply Inf Stamina
                    const float maxStam = 100f; // Default Max Value (may be higher w/ skills)
                    const float maxOxy = 350f; // Default Max Value (may be higher w/ skills)
                    var phys = Memory.ReadPtr(localPlayer + Offsets.Player.Physical);
                    var stamObj = Memory.ReadPtr(phys + Offsets.Physical.Stamina);
                    var currentStam = Memory.ReadValue<float>(stamObj + Offsets.PhysicalValue.Current, false);
                    if (currentStam < 0f || currentStam > 500f)
                        throw new ArgumentOutOfRangeException("Invalid Stam value! Possible bad read");
                    var oxyObj = Memory.ReadPtr(phys + Offsets.Physical.Oxygen);
                    var currentOxy = Memory.ReadValue<float>(oxyObj + Offsets.PhysicalValue.Current, false);
                    if (currentOxy < 0f || currentOxy > 1000f)
                        throw new ArgumentOutOfRangeException("Invalid Oxy value! Possible bad read");
                    if (currentStam < maxStam / 3) // Refill when below 33%
                    {
                        writes.AddValueEntry(stamObj + Offsets.PhysicalValue.Current, maxStam);
                        LoneLogging.WriteLine($"InfStamina -> stam:{currentStam}->{maxStam}");
                    }

                    if (currentOxy < maxOxy / 3) // Refill when below 33%
                    {
                        writes.AddValueEntry(oxyObj + Offsets.PhysicalValue.Current, maxOxy);
                        LoneLogging.WriteLine($"InfStamina -> oxy:{currentOxy}->{maxOxy}");
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR configuring InfStamina: {ex}");
            }
        }

        /// <summary>
        /// Apply Fatigue Bypass for Infinite Stamina.
        /// </summary>
        private void ApplyFatigueBypass_V2(LocalPlayer localPlayer)
        {
            ulong originalStateContainer = GetOriginalStateContainer(localPlayer);
            GetStates(localPlayer, out ulong originalState, out ulong patchState);
            if (originalState == default)
            {
                _bypassSet = true;
                LoneLogging.WriteLine("InfStamina -> Bypass ALREADY SET");
                return;
            }
            int targetHash = Memory.ReadValueEnsure<int>(patchState + Offsets.MovementState.AnimatorStateHash);
            Memory.WriteValueEnsure(originalStateContainer + Offsets.PlayerStateContainer.StateFullNameHash, targetHash);
            Memory.WriteValueEnsure(originalState + Offsets.MovementState.AnimatorStateHash, targetHash);
            Memory.WriteValueEnsure(originalState + Offsets.MovementState.Name, _infStamTargetStateName);
            _bypassSet = true;
            LoneLogging.WriteLine("InfStamina -> Bypass SET");
        }

        private static void GetStates(LocalPlayer localPlayer, out ulong originalState, out ulong patchState)
        {
            using var states = GetStatesDict(localPlayer);
            originalState = states.FirstOrDefault(x => Memory.ReadValueEnsure<byte>(x.Value + Offsets.MovementState.Name) == _infStamSourceStateName).Value;
            if (originalState == default) // Already Set
            {
                patchState = default;
                return;
            }
            patchState = states.First(x => Memory.ReadValueEnsure<byte>(x.Value + Offsets.MovementState.Name) == _infStamTargetStateName).Value;
            
            static MemDictionary<byte, ulong> GetStatesDict(LocalPlayer localPlayer)
            {
                var statesPtr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._states);
                return MemDictionary<byte, ulong>.Get(statesPtr, false);
            }
        }

        private static ulong GetOriginalStateContainer(LocalPlayer localPlayer)
        {
            var movementStatesPtr = Memory.ReadPtr(localPlayer.MovementContext + Offsets.MovementContext._movementStates);
            using var movementStates = MemArray<ulong>.Get(movementStatesPtr, false);
            return movementStates.First(x => Memory.ReadValueEnsure<byte>(x + Offsets.PlayerStateContainer.Name) == _infStamSourceStateName);
        }

        public override void OnRaidStart()
        {
            _bypassSet = default;
        }
    }
}

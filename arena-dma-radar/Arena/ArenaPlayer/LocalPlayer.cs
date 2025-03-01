using arena_dma_radar.Arena.ArenaPlayer.Plugins;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.Arena.ArenaPlayer
{
    public sealed class LocalPlayer : ArenaClientPlayer, ILocalPlayer
    {
        /// <summary>
        /// Magazine information.
        /// </summary>
        public FirearmManager Firearm { get; }
        public override string Name { get; }
        /// <summary>
        /// Player is Human-Controlled.
        /// </summary>
        public override bool IsHuman { get; }

        public LocalPlayer(ulong playerBase) : base(playerBase)
        {
            string classType = ObjectClass.ReadName(this);
            if (classType != "ArenaClientPlayer")
                throw new ArgumentOutOfRangeException(nameof(classType));
            IsHuman = true;
            Name = "localPlayer";
            this.Firearm = new(this);
            ulong id = ulong.Parse(AccountID);
            ILocalPlayer.AccountId = id;
        }

        /// <summary>
        /// Additional realtime reads for LocalPlayer.
        /// </summary>
        /// <param name="index"></param>
        public override void OnRealtimeLoop(ScatterReadIndex index)
        {
            index.AddEntry<MemPointer>(-11, this.HandsControllerAddr);
            index.Callbacks += x1 =>
            {
                if (x1.TryGetResult<MemPointer>(-11, out var handsController))
                    ILocalPlayer.HandsController = handsController;
            };
            Firearm.OnRealtimeLoop(index);
            base.OnRealtimeLoop(index);
        }

        /// <summary>
        /// Get View Angles for LocalPlayer.
        /// </summary>
        /// <returns>View Angles (Vector2).</returns>
        public Vector2 GetViewAngles() =>
            Memory.ReadValue<Vector2>(this.RotationAddress, false);

        /// <summary>
        /// Checks if LocalPlayer is Aiming (ADS).
        /// </summary>
        /// <returns>True if aiming (ADS), otherwise False.</returns>
        public bool CheckIfADS()
        {
            try
            {
                return Memory.ReadValue<bool>(this.PWA + Offsets.ProceduralWeaponAnimation._isAiming, false);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CheckIfADS() ERROR: {ex}");
                return false;
            }
        }
    }
}

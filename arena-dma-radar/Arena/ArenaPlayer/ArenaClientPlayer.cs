using arena_dma_radar.Arena.GameWorld;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.Arena.ArenaPlayer
{
    public class ArenaClientPlayer : Player
    {
        /// <summary>
        /// EFT.Profile Address
        /// </summary>
        public ulong Profile { get; }
        /// <summary>
        /// Procedural Weapon Animation
        /// </summary>
        public ulong PWA { get; }
        /// <summary>
        /// PlayerInfo Address (GClass1044)
        /// </summary>
        public ulong Info { get; }
        /// <summary>
        /// Player name.
        /// </summary>
        public override string Name { get; }
        /// <summary>
        /// Account UUID for Human Controlled Players.
        /// </summary>
        public override string AccountID { get; }
        /// <summary>
        /// Team that the player belongs to.
        /// </summary>
        public override int TeamID { get; } = -1;
        /// <summary>
        /// Player's Faction.
        /// </summary>
        public override Enums.EPlayerSide PlayerSide { get; }
        /// <summary>
        /// Player is Human-Controlled.
        /// </summary>
        public override bool IsHuman { get; }
        /// <summary>
        /// MovementContext / StateContext
        /// </summary>
        public override ulong MovementContext { get; }
        /// <summary>
        /// EFT.PlayerBody
        /// </summary>
        public override ulong Body { get; }
        /// <summary>
        /// Hands Controller field address.
        /// </summary>
        public override ulong HandsControllerAddr { get; }
        /// <summary>
        /// Corpse field address..
        /// </summary>
        public override ulong CorpseAddr { get; }
        /// <summary>
        /// Player Rotation Field Address (view angles).
        /// </summary>
        public override ulong RotationAddress { get; }
        /// <summary>
        /// Player's Skeleton Bones.
        /// </summary>
        public override Skeleton Skeleton { get; }

        internal ArenaClientPlayer(ulong playerBase) : base(playerBase)
        {
            Profile = Memory.ReadPtr(this + Offsets.Player.Profile);
            Info = Memory.ReadPtr(Profile + Offsets.Profile.Info);
            PWA = Memory.ReadPtr(this + Offsets.Player.ProceduralWeaponAnimation);
            Body = Memory.ReadPtr(this + Offsets.Player._playerBody);
            HandsControllerAddr = this + Offsets.Player._handsController;
            CorpseAddr = this + Offsets.Player.Corpse;

            AccountID = GetAccountID();
            TeamID = GetTeamID();
            if (LocalGameWorld.MatchHasTeams)
                ArgumentOutOfRangeException.ThrowIfEqual(TeamID, -1, nameof(TeamID)); MovementContext = GetMovementContext();
            RotationAddress = ValidateRotationAddr(MovementContext + Offsets.MovementContext._rotation);
            /// Setup Transforms
            this.Skeleton = new Skeleton(this, GetTransformInternalChain);
            if (this is LocalPlayer) // Handled in derived class
                return;

            IsHuman = true;
            Name = GetName();
            Type = PlayerType.Player;
        }

        /// <summary>
        /// Get Player's Account ID.
        /// </summary>
        /// <returns>Account ID Numeric String.</returns>
        private string GetAccountID()
        {
            var idPTR = Memory.ReadPtr(Profile + Offsets.Profile.AccountId);
            return Memory.ReadUnityString(idPTR);
        }

        /// <summary>
        /// Gets player's Team ID.
        /// </summary>
        private int GetTeamID()
        {
            try
            {
                var inventoryController = Memory.ReadPtr(this + Offsets.Player._inventoryController);
                return GetTeamID(inventoryController);
            }
            catch { return -1; }
        }

        /// <summary>
        /// Get Player Name.
        /// </summary>
        /// <returns>Player Name String.</returns>
        private string GetName()
        {
            var namePtr = Memory.ReadPtr(Info + Offsets.PlayerInfo.Nickname);
            var name = Memory.ReadUnityString(namePtr)?.Trim();
            if (string.IsNullOrEmpty(name))
                name = "default";
            return name;
        }

        /// <summary>
        /// Get Movement Context Instance.
        /// </summary>
        private ulong GetMovementContext()
        {
            var movementContext = Memory.ReadPtr(this + Offsets.Player.MovementContext);
            var player = Memory.ReadPtr(movementContext + Offsets.MovementContext.Player, false);
            if (player != this)
                throw new ArgumentOutOfRangeException(nameof(movementContext));
            return movementContext;
        }

        /// <summary>
        /// Get the Transform Internal Chain for this Player.
        /// </summary>
        /// <param name="bone">Bone to lookup.</param>
        /// <returns>Array of offsets for transform internal chain.</returns>
        public override uint[] GetTransformInternalChain(Bones bone) =>
            Offsets.Player.GetTransformChain(bone);
    }
}

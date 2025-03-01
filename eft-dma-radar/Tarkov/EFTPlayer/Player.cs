using eft_dma_radar.Tarkov.EFTPlayer.SpecialCollections;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Radar;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.Tarkov.GameWorld;
using eft_dma_radar.Tarkov.EFTPlayer.Plugins;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;
using eft_dma_shared.Common.Unity.LowLevel;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Maps;
using eft_dma_radar.Tarkov.Features.MemoryWrites;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Misc.Pools;
using eft_dma_shared.Common.DMA;

namespace eft_dma_radar.Tarkov.EFTPlayer
{
    /// <summary>
    /// Base class for Tarkov Players.
    /// Tarkov implements several distinct classes that implement a similar player interface.
    /// </summary>
    public abstract class Player : IWorldEntity, IMapEntity, IMouseoverEntity, IESPEntity, IPlayer
    {
        #region Group Manager

        /// <summary>
        /// Wrapper Class to manage group allocations.
        /// Thread Safe.
        /// </summary>
        protected sealed class GroupManager
        {
            private readonly Dictionary<string, int> _groups = new(StringComparer.OrdinalIgnoreCase);

            /// <summary>
            /// Returns the Group Number for a given id.
            /// </summary>
            /// <param name="id">Group ID.</param>
            /// <returns>Group Number (0,1,2,etc.)</returns>
            public int GetGroup(string id)
            {
                lock (_groups)
                {
                    _groups.TryAdd(id, _groups.Count);
                    return _groups[id];
                }
            }

            /// <summary>
            /// Clears the group definitions.
            /// </summary>
            public void Clear()
            {
                lock (_groups)
                {
                    _groups.Clear();
                }
            }
        }

        #endregion

        #region Static Interfaces

        public static implicit operator ulong(Player x) => x.Base;
        private static readonly ConcurrentDictionary<ulong, Stopwatch> _rateLimit = new();
        protected static readonly GroupManager _groups = new();
        protected static int _playerScavNumber = 0;

        /// <summary>
        /// Player History Log.
        /// </summary>
        public static PlayerHistory PlayerHistory { get; } = new();

        /// <summary>
        /// Player Watchlist Entries.
        /// </summary>
        public static PlayerWatchlist PlayerWatchlist { get; } = new();

        /// <summary>
        /// Resets/Updates 'static' assets in preparation for a new game/raid instance.
        /// </summary>
        public static void Reset()
        {
            _groups.Clear();
            _rateLimit.Clear();
            PlayerHistory.Reset();
            _playerScavNumber = 0;
        }

        #endregion

        #region Allocation

        /// <summary>
        /// Allocates a player and takes into consideration any rate-limits.
        /// </summary>
        /// <param name="playerDict">Player Dictionary collection to add the newly allocated player to.</param>
        /// <param name="playerBase">Player base memory address.</param>
        /// <param name="initialPosition">Initial position to be set (Optional). Usually for reallocations.</param>
        public static void Allocate(ConcurrentDictionary<ulong, Player> playerDict, ulong playerBase)
        {
            var sw = _rateLimit.AddOrUpdate(playerBase,
                key => new Stopwatch(),
                (key, oldValue) => oldValue);
            if (sw.IsRunning && sw.Elapsed.TotalMilliseconds < 500f)
                return;
            try
            {
                var player = AllocateInternal(playerBase);
                playerDict[player] = player; // Insert or swap
                LoneLogging.WriteLine($"Player '{player.Name}' allocated.");
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR during Player Allocation for player @ 0x{playerBase.ToString("X")}: {ex}");
            }
            finally
            {
                sw.Restart();
            }
        }

        private static Player AllocateInternal(ulong playerBase)
        {
            var className = ObjectClass.ReadName(playerBase, 64);
            var isClientPlayer = className == "ClientPlayer" || className == "LocalPlayer";

            if (isClientPlayer)
                return new ClientPlayer(playerBase);
            return new ObservedPlayer(playerBase);
        }

        /// <summary>
        /// Player Constructor.
        /// </summary>
        protected Player(ulong playerBase)
        {
            ArgumentOutOfRangeException.ThrowIfZero(playerBase, nameof(playerBase));
            Base = playerBase;
        }

        #endregion

        #region Fields / Properties
        /// <summary>
        /// Player Class Base Address
        /// </summary>
        public ulong Base { get; }

        /// <summary>
        /// True if the Player is Active (in the player list).
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Type of player unit.
        /// </summary>
        public PlayerType Type { get; protected set; }

        /// <summary>
        /// Twitch.tv channel status.
        /// </summary>
        public string TwitchChannelURL { get; protected set; }

        /// <summary>
        /// Player's Rotation in Local Game World.
        /// </summary>
        public Vector2 Rotation { get; private set; }

        /// <summary>
        /// Player's Map Rotation (with 90 degree correction applied).
        /// </summary>
        public float MapRotation
        {
            get
            {
                float mapRotation = Rotation.X; // Cache value
                mapRotation -= 90f;
                while (mapRotation < 0f)
                    mapRotation += 360f;

                return mapRotation;
            }
        }

        /// <summary>
        /// Corpse field value.
        /// </summary>
        public ulong? Corpse { get; private set; }

        /// <summary>
        /// Stopwatch for High Alert ESP Feature.
        /// </summary>
        public Stopwatch HighAlertSw { get; } = new();

        /// <summary>
        /// Player's Skeleton Bones.
        /// Derived types MUST define this.
        /// </summary>
        public virtual Skeleton Skeleton => throw new NotImplementedException(nameof(Skeleton));

        /// <summary>
        /// Duration of consecutive errors.
        /// </summary>
        public Stopwatch ErrorTimer { get; } = new();

        /// <summary>
        /// Player's Gear/Loadout Information and contained items.
        /// </summary>
        public GearManager Gear { get; private set; }

        /// <summary>
        /// Contains information about the item/weapons in Player's hands.
        /// </summary>
        public HandsManager Hands { get; private set; }

        /// <summary>
        /// True if player is 'Locked On' via Aimbot.
        /// </summary>
        public bool IsAimbotLocked { get; set; }

        /// <summary>
        /// True if player is being focused via Right-Click (UI).
        /// </summary>
        public bool IsFocused { get; set; }

        /// <summary>
        /// Dead Player's associated loot container object.
        /// </summary>
        public LootContainer LootObject { get; set; }
        /// <summary>
        /// Alerts for this Player Object.
        /// Used by Player History UI Interop.
        /// </summary>
        public string Alerts { get; private set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Player name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Account UUID for Human Controlled Players.
        /// </summary>
        public virtual string AccountID { get; }

        /// <summary>
        /// Group that the player belongs to.
        /// </summary>
        public virtual int GroupID { get; } = -1;

        /// <summary>
        /// Player's Faction.
        /// </summary>
        public virtual Enums.EPlayerSide PlayerSide { get; }

        /// <summary>
        /// Player is Human-Controlled.
        /// </summary>
        public virtual bool IsHuman { get; }

        /// <summary>
        /// MovementContext / StateContext
        /// </summary>
        public virtual ulong MovementContext { get; }

        /// <summary>
        /// EFT.PlayerBody
        /// </summary>
        public virtual ulong Body { get; }

        /// <summary>
        /// Inventory Controller field address.
        /// </summary>
        public virtual ulong InventoryControllerAddr { get; }

        /// <summary>
        /// Hands Controller field address.
        /// </summary>
        public virtual ulong HandsControllerAddr { get; }

        /// <summary>
        /// Corpse field address..
        /// </summary>
        public virtual ulong CorpseAddr { get; }

        /// <summary>
        /// Player Rotation Field Address (view angles).
        /// </summary>
        public virtual ulong RotationAddress { get; }

        #endregion

        #region Boolean Getters

        /// <summary>
        /// Player is AI-Controlled.
        /// </summary>
        public bool IsAI => !IsHuman;

        /// <summary>
        /// Player is a PMC Operator.
        /// </summary>
        public bool IsPmc => PlayerSide is Enums.EPlayerSide.Usec || PlayerSide is Enums.EPlayerSide.Bear;

        /// <summary>
        /// Player is a SCAV.
        /// </summary>
        public bool IsScav => PlayerSide is Enums.EPlayerSide.Savage;

        /// <summary>
        /// Player is alive (not dead).
        /// </summary>
        public bool IsAlive => Corpse is null;

        /// <summary>
        /// True if Player is Friendly to LocalPlayer.
        /// </summary>
        public bool IsFriendly =>
            this is LocalPlayer || Type is PlayerType.Teammate;

        /// <summary>
        /// True if player is Hostile to LocalPlayer.
        /// </summary>
        public bool IsHostile => !IsFriendly;

        /// <summary>
        /// True if player is TTV Streaming.
        /// </summary>
        public bool IsStreaming => TwitchChannelURL is not null;

        /// <summary>
        /// Player is Alive/Active and NOT LocalPlayer.
        /// </summary>
        public bool IsNotLocalPlayerAlive =>
            this is not LocalPlayer && IsActive && IsAlive;

        /// <summary>
        /// Player is a Hostile PMC Operator.
        /// </summary>
        public bool IsHostilePmc => IsPmc && IsHostile;

        /// <summary>
        /// Player is human-controlled (Not LocalPlayer).
        /// </summary>
        public bool IsHumanOther => IsHuman && this is not LocalPlayer;

        /// <summary>
        /// Player is AI Controlled and Alive/Active.
        /// </summary>
        public bool IsAIActive => IsAI && IsActive && IsAlive;

        /// <summary>
        /// Player is AI Controlled and Alive/Active & their AI Role is default.
        /// </summary>
        public bool IsDefaultAIActive => IsAI && Name == "defaultAI" && IsActive && IsAlive;

        /// <summary>
        /// Player is human-controlled and Active/Alive.
        /// </summary>
        public bool IsHumanActive =>
            IsHuman && IsActive && IsAlive;

        /// <summary>
        /// Player is hostile and alive/active.
        /// </summary>
        public bool IsHostileActive => IsHostile && IsActive && IsAlive;

        /// <summary>
        /// Player is human-controlled & Hostile.
        /// </summary>
        public bool IsHumanHostile => IsHuman && IsHostile;

        /// <summary>
        /// Player is human-controlled, hostile, and Active/Alive.
        /// </summary>
        public bool IsHumanHostileActive => IsHumanHostile && IsActive && IsAlive;

        /// <summary>
        /// Player is friendly to LocalPlayer (including LocalPlayer) and Active/Alive.
        /// </summary>
        public bool IsFriendlyActive => IsFriendly && IsActive && IsAlive;

        /// <summary>
        /// Player has exfil'd/left the raid.
        /// </summary>
        public bool HasExfild => !IsActive && IsAlive;

        #endregion

        #region Methods

        private readonly Lock _alertsLock = new();
        /// <summary>
        /// Update the Alerts for this Player Object.
        /// </summary>
        /// <param name="alert">Alert to set.</param>
        public void UpdateAlerts(string alert)
        {
            if (alert is null)
                return;
            lock (_alertsLock)
            {
                if (this.Alerts is null)
                    this.Alerts = alert;
                else
                    this.Alerts = $"{alert} | {this.Alerts}";
            }
        }

        /// <summary>
        /// Validates the Rotation Address.
        /// </summary>
        /// <param name="rotationAddr">Rotation va</param>
        /// <returns>Validated rotation virtual address.</returns>
        protected static ulong ValidateRotationAddr(ulong rotationAddr)
        {
            var rotation = Memory.ReadValue<Vector2>(rotationAddr, false);
            if (!rotation.IsNormalOrZero() ||
                Math.Abs(rotation.X) > 360f ||
                Math.Abs(rotation.Y) > 90f)
                throw new ArgumentOutOfRangeException(nameof(rotationAddr));

            return rotationAddr;
        }

        /// <summary>
        /// Refreshes non-realtime player information. Call in the Registered Players Loop (T0).
        /// </summary>
        /// <param name="index"></param>
        /// <param name="registered"></param>
        /// <param name="isActiveParam"></param>
        public virtual void OnRegRefresh(ScatterReadIndex index, IReadOnlySet<ulong> registered, bool? isActiveParam = null)
        {
            if (isActiveParam is not bool isActive)
                isActive = registered.Contains(this);
            if (isActive)
            {
                this.SetAlive();
            }
            else if (this.IsAlive) // Not in list, but alive
            {
                index.AddEntry<ulong>(0, this.CorpseAddr);
                index.Callbacks += x1 =>
                {
                    if (x1.TryGetResult<ulong>(0, out var corpsePtr) && corpsePtr != 0x0)
                        this.SetDead(corpsePtr);
                    else
                        this.SetExfild();
                };
            }
        }

        /// <summary>
        /// Mark player as dead.
        /// </summary>
        /// <param name="corpse">Corpse address.</param>
        public void SetDead(ulong corpse)
        {
            Corpse = corpse;
            IsActive = false;
        }

        /// <summary>
        /// Mark player as exfil'd.
        /// </summary>
        private void SetExfild()
        {
            Corpse = null;
            IsActive = false;
        }

        /// <summary>
        /// Mark player as alive.
        /// </summary>
        private void SetAlive()
        {
            Corpse = null;
            LootObject = null;
            IsActive = true;
        }

        /// <summary>
        /// Executed on each Realtime Loop.
        /// </summary>
        /// <param name="index">Scatter read index dedicated to this player.</param>
        public virtual void OnRealtimeLoop(ScatterReadIndex index)
        {
            index.AddEntry<Vector2>(-1, this.RotationAddress); // Rotation
            foreach (var tr in Skeleton.Bones)
            {
                index.AddEntry<SharedArray<UnityTransform.TrsX>>((int)(uint)tr.Key, tr.Value.VerticesAddr,
                    (3 * tr.Value.Index + 3) * 16); // ESP Vertices
            }

            index.Callbacks += x1 =>
            {
                bool p1 = false;
                bool p2 = true;
                if (x1.TryGetResult<Vector2>(-1, out var rotation))
                    p1 = this.SetRotation(ref rotation);
                foreach (var tr in Skeleton.Bones)
                {
                    if (x1.TryGetResult<SharedArray<UnityTransform.TrsX>>((int)(uint)tr.Key, out var vertices))
                    {
                        try
                        {
                            try
                            {
                                _ = tr.Value.UpdatePosition(vertices);
                            }
                            catch (Exception ex) // Attempt to re-allocate Transform on error
                            {
                                LoneLogging.WriteLine($"ERROR getting Player '{this.Name}' {tr.Key} Position: {ex}");
                                this.Skeleton.ResetTransform(tr.Key);
                            }
                        }
                        catch
                        {
                            p2 = false;
                        }
                    }
                    else
                    {
                        p2 = false;
                    }
                }

                if (p1 && p2)
                    this.ErrorTimer.Reset();
                else
                    this.ErrorTimer.Start();
            };
        }

        /// <summary>
        /// Executed on each Transform Validation Loop.
        /// </summary>
        /// <param name="round1">Index (round 1)</param>
        /// <param name="round2">Index (round 2)</param>
        public void OnValidateTransforms(ScatterReadIndex round1, ScatterReadIndex round2)
        {
            foreach (var tr in Skeleton.Bones)
            {
                round1.AddEntry<MemPointer>((int)(uint)tr.Key,
                    tr.Value.TransformInternal +
                    UnityOffsets.TransformInternal.TransformAccess); // Bone Hierarchy
                round1.Callbacks += x1 =>
                {
                    if (x1.TryGetResult<MemPointer>((int)(uint)tr.Key, out var tra))
                        round2.AddEntry<MemPointer>((int)(uint)tr.Key, tra + UnityOffsets.TransformAccess.Vertices); // Vertices Ptr
                    round2.Callbacks += x2 =>
                    {
                        if (x2.TryGetResult<MemPointer>((int)(uint)tr.Key, out var verticesPtr))
                        {
                            if (tr.Value.VerticesAddr != verticesPtr) // check if any addr changed
                            {
                                LoneLogging.WriteLine(
                                    $"WARNING - '{tr.Key}' Transform has changed for Player '{this.Name}'");
                                this.Skeleton.ResetTransform(tr.Key); // alloc new transform
                            }
                        }
                    };
                };
            }
        }

        /// <summary>
        /// Set player rotation (Direction/Pitch)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual bool SetRotation(ref Vector2 rotation)
        {
            try
            {
                rotation.ThrowIfAbnormalAndNotZero();
                rotation.X = rotation.X.NormalizeAngle();
                ArgumentOutOfRangeException.ThrowIfLessThan(rotation.X, 0f);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(rotation.X, 360f);
                ArgumentOutOfRangeException.ThrowIfLessThan(rotation.Y, -90f);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(rotation.Y, 90f);
                Rotation = rotation;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Refresh Gear if Active Human Player.
        /// </summary>
        public void RefreshGear()
        {
            try
            {
                Gear ??= new GearManager(this, IsPmc);
                Gear?.Refresh();
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[GearManager] ERROR for Player {Name}: {ex}");
            }
        }

        /// <summary>
        /// Refresh item in player's hands.
        /// </summary>
        public void RefreshHands()
        {
            try
            {
                if (IsActive && IsAlive)
                {
                    Hands ??= new HandsManager(this);
                    Hands?.Refresh();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get the Transform Internal Chain for this Player.
        /// </summary>
        /// <param name="bone">Bone to lookup.</param>
        /// <returns>Array of offsets for transform internal chain.</returns>
        public virtual uint[] GetTransformInternalChain(Bones bone) =>
            throw new NotImplementedException();

        #endregion

        #region AI Player Types

        public readonly struct AIRole
        {
            public readonly string Name { get; init; }
            public readonly PlayerType Type { get; init; }
        }

        /// <summary>
        /// Lookup AI Info based on Voice Line.
        /// </summary>
        /// <param name="voiceLine"></param>
        /// <returns></returns>
        public static AIRole GetAIRoleInfo(string voiceLine)
        {
            switch (voiceLine)
            {
                case "BossSanitar":
                    return new AIRole()
                    {
                        Name = "Sanitar",
                        Type = PlayerType.AIBoss
                    };
                case "BossBully":
                    return new AIRole()
                    {
                        Name = "Reshala",
                        Type = PlayerType.AIBoss
                    };
                case "BossGluhar":
                    return new AIRole()
                    {
                        Name = "Gluhar",
                        Type = PlayerType.AIBoss
                    };
                case "SectantPriest":
                    return new AIRole()
                    {
                        Name = "Priest",
                        Type = PlayerType.AIBoss
                    };
                case "SectantWarrior":
                    return new AIRole()
                    {
                        Name = "Cultist",
                        Type = PlayerType.AIRaider
                    };
                case "BossKilla":
                    return new AIRole()
                    {
                        Name = "Killa",
                        Type = PlayerType.AIBoss
                    };
                case "BossTagilla":
                    return new AIRole()
                    {
                        Name = "Tagilla",
                        Type = PlayerType.AIBoss
                    };
                case "Boss_Partizan":
                    return new AIRole()
                    {
                        Name = "Partisan",
                        Type = PlayerType.AIBoss
                    };
                case "BossBigPipe":
                    return new AIRole()
                    {
                        Name = "Big Pipe",
                        Type = PlayerType.AIBoss
                    };
                case "BossBirdEye":
                    return new AIRole()
                    {
                        Name = "Birdeye",
                        Type = PlayerType.AIBoss
                    };
                case "BossKnight":
                    return new AIRole()
                    {
                        Name = "Knight",
                        Type = PlayerType.AIBoss
                    };
                case "Arena_Guard_1":
                    return new AIRole()
                    {
                        Name = "Arena Guard",
                        Type = PlayerType.AIScav
                    };
                case "Arena_Guard_2":
                    return new AIRole()
                    {
                        Name = "Arena Guard",
                        Type = PlayerType.AIScav
                    };
                case "Boss_Kaban":
                    return new AIRole()
                    {
                        Name = "Kaban",
                        Type = PlayerType.AIBoss
                    };
                case "Boss_Kollontay":
                    return new AIRole()
                    {
                        Name = "Kollontay",
                        Type = PlayerType.AIBoss
                    };
                case "Boss_Sturman":
                    return new AIRole()
                    {
                        Name = "Shturman",
                        Type = PlayerType.AIBoss
                    };
                case "Zombie_Generic":
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case "BossZombieTagilla":
                    return new AIRole()
                    {
                        Name = "Zombie Tagilla",
                        Type = PlayerType.AIBoss
                    };
                case "Zombie_Fast":
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case "Zombie_Medium":
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                default:
                    break;
            }
            if (voiceLine.Contains("scav", StringComparison.OrdinalIgnoreCase))
                return new AIRole()
                {
                    Name = "Scav",
                    Type = PlayerType.AIScav
                };
            if (voiceLine.Contains("boss", StringComparison.OrdinalIgnoreCase))
                return new AIRole()
                {
                    Name = "Boss",
                    Type = PlayerType.AIBoss
                };
            if (voiceLine.Contains("usec", StringComparison.OrdinalIgnoreCase))
                return new AIRole()
                {
                    Name = "Usec",
                    Type = PlayerType.AIScav
                };
            if (voiceLine.Contains("bear", StringComparison.OrdinalIgnoreCase))
                return new AIRole()
                {
                    Name = "Bear",
                    Type = PlayerType.AIScav
                };
            LoneLogging.WriteLine($"Unknown Voice Line: {voiceLine}");
            return new AIRole()
            {
                Name = "AI",
                Type = PlayerType.AIScav
            };
        }

        public static AIRole GetAIRoleInfo(Enums.WildSpawnType wildSpawnType)
        {
            switch (wildSpawnType)
            {
                case Enums.WildSpawnType.marksman:
                    return new AIRole()
                    {
                        Name = "Sniper",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.assault:
                    return new AIRole()
                    {
                        Name = "Scav",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.bossTest:
                    return new AIRole()
                    {
                        Name = "bossTest",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.bossBully:
                    return new AIRole()
                    {
                        Name = "Reshala",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerTest:
                    return new AIRole()
                    {
                        Name = "followerTest",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.followerBully:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossKilla:
                    return new AIRole()
                    {
                        Name = "Killa",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.bossKojaniy:
                    return new AIRole()
                    {
                        Name = "Shturman",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerKojaniy:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.pmcBot:
                    return new AIRole()
                    {
                        Name = "Raider",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.cursedAssault:
                    return new AIRole()
                    {
                        Name = "Scav",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.bossGluhar:
                    return new AIRole()
                    {
                        Name = "Gluhar",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerGluharAssault:
                    return new AIRole()
                    {
                        Name = "Assault",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerGluharSecurity:
                    return new AIRole()
                    {
                        Name = "Security",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerGluharScout:
                    return new AIRole()
                    {
                        Name = "Scout",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerGluharSnipe:
                    return new AIRole()
                    {
                        Name = "Sniper",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerSanitar:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossSanitar:
                    return new AIRole()
                    {
                        Name = "Sanitar",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.test:
                    return new AIRole()
                    {
                        Name = "test",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.assaultGroup:
                    return new AIRole()
                    {
                        Name = "Scav",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.sectantWarrior:
                    return new AIRole()
                    {
                        Name = "Cultist",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.sectantPriest:
                    return new AIRole()
                    {
                        Name = "Priest",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.bossTagilla:
                    return new AIRole()
                    {
                        Name = "Tagilla",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerTagilla:
                    return new AIRole()
                    {
                        Name = "Tagilla",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.exUsec:
                    return new AIRole()
                    {
                        Name = "Rogue",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.gifter:
                    return new AIRole()
                    {
                        Name = "Santa",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.bossKnight:
                    return new AIRole()
                    {
                        Name = "Knight",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerBigPipe:
                    return new AIRole()
                    {
                        Name = "Big Pipe",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerBirdEye:
                    return new AIRole()
                    {
                        Name = "Bird Eye",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.bossZryachiy:
                    return new AIRole()
                    {
                        Name = "Zryachiy",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerZryachiy:
                    return new AIRole()
                    {
                        Name = "Cultist",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossBoar:
                    return new AIRole()
                    {
                        Name = "Kaban",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerBoar:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.arenaFighter:
                    return new AIRole()
                    {
                        Name = "Arena Fighter",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.arenaFighterEvent:
                    return new AIRole()
                    {
                        Name = "Bloodhound",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossBoarSniper:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.crazyAssaultEvent:
                    return new AIRole()
                    {
                        Name = "Scav",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.peacefullZryachiyEvent:
                    return new AIRole()
                    {
                        Name = "peacefullZryachiyEvent",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.sectactPriestEvent:
                    return new AIRole()
                    {
                        Name = "sectactPriestEvent",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.ravangeZryachiyEvent:
                    return new AIRole()
                    {
                        Name = "ravangeZryachiyEvent",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.followerBoarClose1:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerBoarClose2:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossKolontay:
                    return new AIRole()
                    {
                        Name = "Kolontay",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.followerKolontayAssault:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.followerKolontaySecurity:
                    return new AIRole()
                    {
                        Name = "Guard",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.shooterBTR:
                    return new AIRole()
                    {
                        Name = "BTR",
                        Type = PlayerType.AIRaider
                    };
                case Enums.WildSpawnType.bossPartisan:
                    return new AIRole()
                    {
                        Name = "Partisan",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.spiritWinter:
                    return new AIRole()
                    {
                        Name = "spiritWinter",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.spiritSpring:
                    return new AIRole()
                    {
                        Name = "spiritSpring",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.peacemaker:
                    return new AIRole()
                    {
                        Name = "Peacekeeper Goon",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.pmcBEAR:
                    return new AIRole()
                    {
                        Name = "BEAR",
                        Type = PlayerType.PMC
                    };
                case Enums.WildSpawnType.pmcUSEC:
                    return new AIRole()
                    {
                        Name = "USEC",
                        Type = PlayerType.PMC
                    };
                case Enums.WildSpawnType.skier:
                    return new AIRole()
                    {
                        Name = "Skier Goon",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.sectantPredvestnik:
                    return new AIRole()
                    {
                        Name = "Partisan",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.sectantPrizrak:
                    return new AIRole()
                    {
                        Name = "Ghost",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.sectantOni:
                    return new AIRole()
                    {
                        Name = "Oni",
                        Type = PlayerType.AIBoss
                    };
                case Enums.WildSpawnType.infectedAssault:
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.infectedPmc:
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.infectedCivil:
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.infectedLaborant:
                    return new AIRole()
                    {
                        Name = "Zombie",
                        Type = PlayerType.AIScav
                    };
                case Enums.WildSpawnType.infectedTagilla:
                    return new AIRole()
                    {
                        Name = "Zombie Tagilla",
                        Type = PlayerType.AIBoss
                    };
                default:
                    LoneLogging.WriteLine("WARNING: Unknown WildSpawnType: " + (int)wildSpawnType);
                    return new AIRole()
                    {
                        Name = "defaultAI",
                        Type = PlayerType.AIScav
                    };
            }
        }

        #endregion

        #region Chams Feature

        /// <summary>
        /// 0 = None, otherwise value of enum ChamsMode
        /// </summary>
        public ChamsManager.ChamsMode ChamsMode { get; private set; }


        /// <summary>
        /// Apply Chams to CurrentPlayer (if not already set).
        /// </summary>
        /// <param name="writes">Reusable scatter write handle.</param>
        /// <param name="game">Current gameworld instance.</param>
        /// <param name="chamsMode">Chams mode being applied.</param>
        /// <param name="chamsMaterial">Chams material instance ID to write.</param>
        public void SetChams(ScatterWriteHandle writes, LocalGameWorld game, ChamsManager.ChamsMode chamsMode, int chamsMaterial)
        {
            try
            {
                if (ChamsMode != chamsMode)
                {
                    writes.Clear();
                    ApplyClothingChams(writes, chamsMaterial);
                    if (chamsMode is not ChamsManager.ChamsMode.Basic)
                    {
                        ApplyGearChams(writes, chamsMaterial);
                    }
                    writes.Execute(DoWrite);
                    LoneLogging.WriteLine($"Chams set OK for Player '{Name}'");
                    ChamsMode = chamsMode;
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR setting Chams for Player '{Name}': {ex}");
            }
            bool DoWrite()
            {
                if (Memory.ReadValue<ulong>(this.CorpseAddr, false) != 0)
                    return false;
                if (!game.IsSafeToWriteMem)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Apply Clothing Chams to this Player.
        /// </summary>
        /// <param name="writes"></param>
        /// <param name="chamsMaterial"></param>
        private void ApplyClothingChams(ScatterWriteHandle writes, int chamsMaterial)
        {
            var pRendererContainersArray = Memory.ReadPtr(this.Body + Offsets.PlayerBody._bodyRenderers);
            using var rendererContainersArray = MemArray<Types.BodyRendererContainer>.Get(pRendererContainersArray);
            ArgumentOutOfRangeException.ThrowIfZero(rendererContainersArray.Count);

            foreach (var rendererContainer in rendererContainersArray)
            {
                using var renderersArray = MemArray<ulong>.Get(rendererContainer.Renderers);
                ArgumentOutOfRangeException.ThrowIfZero(renderersArray.Count);

                foreach (var skinnedMeshRenderer in renderersArray)
                {
                    // Cached ptr to Renderer
                    var renderer = Memory.ReadPtr(skinnedMeshRenderer + UnityOffsets.SkinnedMeshRenderer.Renderer);
                    WriteChamsMaterial(writes, renderer, chamsMaterial);
                }
            }
        }

        /// <summary>
        /// Apply Gear Chams to this Player.
        /// </summary>
        /// <param name="writes"></param>
        /// <param name="chamsMaterial"></param>
        private void ApplyGearChams(ScatterWriteHandle writes, int chamsMaterial)
        {
            var slotViews = Memory.ReadValue<ulong>(this.Body + Offsets.PlayerBody.SlotViews);
            if (!Utils.IsValidVirtualAddress(slotViews))
                return;

            var pSlotViewsDict = Memory.ReadValue<ulong>(slotViews + Offsets.SlotViewsContainer.Dict);
            if (!Utils.IsValidVirtualAddress(pSlotViewsDict))
                return;

            using var slotViewsDict = MemDictionary<ulong, ulong>.Get(pSlotViewsDict);
            if (slotViewsDict.Count == 0)
                return;

            foreach (var slot in slotViewsDict)
            {
                if (!Utils.IsValidVirtualAddress(slot.Value))
                    continue;

                var pDressesArray = Memory.ReadValue<ulong>(slot.Value + Offsets.PlayerBodySubclass.Dresses);
                if (!Utils.IsValidVirtualAddress(pDressesArray))
                    continue;

                using var dressesArray = MemArray<ulong>.Get(pDressesArray);
                if (dressesArray.Count == 0)
                    continue;

                foreach (var dress in dressesArray)
                {
                    if (!Utils.IsValidVirtualAddress(dress))
                        continue;

                    var pRenderersArray = Memory.ReadValue<ulong>(dress + Offsets.Dress.Renderers);
                    if (!Utils.IsValidVirtualAddress(pRenderersArray))
                        continue;

                    using var renderersArray = MemArray<ulong>.Get(pRenderersArray);
                    if (renderersArray.Count == 0)
                        continue;

                    foreach (var renderer in renderersArray)
                    {
                        if (!Utils.IsValidVirtualAddress(renderer))
                            continue;

                        ulong rendererNative = Memory.ReadValue<ulong>(renderer + 0x10);
                        if (!Utils.IsValidVirtualAddress(rendererNative))
                            continue;

                        WriteChamsMaterial(writes, rendererNative, chamsMaterial);
                    }
                }
            }
        }

        /// <summary>
        /// Write Chams Material to the specified Renderer/Materials.
        /// </summary>
        /// <param name="writes"></param>
        /// <param name="renderer"></param>
        /// <param name="chamsMaterial"></param>
        private static void WriteChamsMaterial(ScatterWriteHandle writes, ulong renderer, int chamsMaterial)
        {
            int materialsCount = Memory.ReadValueEnsure<int>(renderer + UnityOffsets.Renderer.Count);
            ArgumentOutOfRangeException.ThrowIfLessThan(materialsCount, 0, nameof(materialsCount));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(materialsCount, 30, nameof(materialsCount));
            if (materialsCount == 0)
                return;
            var materialsArrayPtr = Memory.ReadValueEnsure<ulong>(renderer + UnityOffsets.Renderer.Materials);
            materialsArrayPtr.ThrowIfInvalidVirtualAddress();
            var materials = Enumerable.Repeat<int>(chamsMaterial, materialsCount).ToArray();
            writes.AddBufferEntry(materialsArrayPtr, materials.AsSpan());
        }

        #endregion

        #region Interfaces

        public virtual ref Vector3 Position => ref this.Skeleton.Root.Position;
        public Vector2 MouseoverPosition { get; set; }

        public void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            try
            {
                var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
                MouseoverPosition = new Vector2(point.X, point.Y);
                if (!IsAlive) // Player Dead -- Draw 'X' death marker and move on
                {
                    DrawDeathMarker(canvas, point);
                }
                else
                {
                    DrawPlayerMarker(canvas, localPlayer, point);
                    if (this == localPlayer)
                        return;
                    var height = Position.Y - localPlayer.Position.Y;
                    var dist = Vector3.Distance(localPlayer.Position, Position);
                    var lines = new List<string>();
                    if (!MainForm.Config.HideNames) // show full names & info
                    {
                        string name = null;
                        if (ErrorTimer.ElapsedMilliseconds > 100)
                            name = "ERROR"; // In case POS stops updating, let us know!
                        else
                            name = Name;
                        string health = null; string level = null;
                        if (this is ObservedPlayer observed)
                        {
                            health = observed.HealthStatus is Enums.ETagStatus.Healthy
                                ? null
                                : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
                            if (observed.Profile?.Level is int levelResult)
                                level = $"L{levelResult}:";
                        }
                        lines.Add($"{level}{name}{health}");
                        lines.Add($"H: {(int)Math.Round(height)} D: {(int)Math.Round(dist)}");
                    }
                    else // just height, distance
                    {
                        lines.Add($"{(int)Math.Round(height)},{(int)Math.Round(dist)}");
                        if (ErrorTimer.ElapsedMilliseconds > 100)
                            lines[0] = "ERROR"; // In case POS stops updating, let us know!
                    }

                    if (Type is not PlayerType.Teammate
                        && ((Gear?.Loot?.Any(x => x.IsImportant) ?? false) ||
                            (MainForm.Config.QuestHelper.Enabled && (Gear?.HasQuestItems ?? false))
                        ))
                        lines[0] = $"!!{lines[0]}"; // Notify important loot
                    DrawPlayerText(canvas, point, lines);
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"WARNING! Player Draw Error: {ex}");
            }
        }

        /// <summary>
        /// Draws a Player Marker on this location.
        /// </summary>
        private void DrawPlayerMarker(SKCanvas canvas, ILocalPlayer localPlayer, SKPoint point)
        {
            var radians = MapRotation.ToRadians();
            var paints = GetPaints();
            if (this != localPlayer && MainForm.MouseoverGroup is int grp && grp == GroupID)
                paints.Item1 = SKPaints.PaintMouseoverGroup;
            SKPaints.ShapeOutline.StrokeWidth = paints.Item1.StrokeWidth + 2f * MainForm.UIScale;

            var size = 6 * MainForm.UIScale;
            canvas.DrawCircle(point, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(point, size, paints.Item1); // draw LocalPlayer marker

            var aimlineLength = this == localPlayer || (this.IsFriendly && MainForm.Config.TeammateAimlines) ? 
                MainForm.Config.AimLineLength : 15;
            if (!IsFriendly && 
                !(this.IsAI && !MainForm.Config.AIAimlines) &&
                this.IsFacingTarget(localPlayer, Program.Config.MaxDistance)) // Hostile Player, check if aiming at a friendly (High Alert)
                aimlineLength = 9999;

            var aimlineEnd = GetAimlineEndpoint(point, radians, aimlineLength);
            canvas.DrawLine(point, aimlineEnd, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawLine(point, aimlineEnd, paints.Item1); // draw LocalPlayer aimline
        }

        /// <summary>
        /// Draws a Death Marker on this location.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawDeathMarker(SKCanvas canvas, SKPoint point)
        {
            var length = 6 * MainForm.UIScale;
            canvas.DrawLine(new SKPoint(point.X - length, point.Y + length),
                new SKPoint(point.X + length, point.Y - length), SKPaints.PaintDeathMarker);
            canvas.DrawLine(new SKPoint(point.X - length, point.Y - length),
                new SKPoint(point.X + length, point.Y + length), SKPaints.PaintDeathMarker);
        }

        /// <summary>
        /// Gets the point where the Aimline 'Line' ends. Applies UI Scaling internally.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SKPoint GetAimlineEndpoint(SKPoint start, float radians, float aimlineLength)
        {
            aimlineLength *= MainForm.UIScale;
            return new SKPoint(start.X + MathF.Cos(radians) * aimlineLength,
                start.Y + MathF.Sin(radians) * aimlineLength);
        }

        /// <summary>
        /// Draws Player Text on this location.
        /// </summary>
        private void DrawPlayerText(SKCanvas canvas, SKPoint point, List<string> lines)
        {
            var paints = GetPaints();
            if (MainForm.MouseoverGroup is int grp && grp == GroupID)
                paints.Item2 = SKPaints.TextMouseoverGroup;
            var spacing = 3 * MainForm.UIScale;
            point.Offset(9 * MainForm.UIScale, spacing);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;

                canvas.DrawText(line, point, SKPaints.TextOutline); // Draw outline
                canvas.DrawText(line, point, paints.Item2); // draw line text
                point.Offset(0, 12 * MainForm.UIScale);
            }
        }

        private ValueTuple<SKPaint, SKPaint> GetPaints()
        {
            if (IsAimbotLocked)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintAimbotLocked, SKPaints.TextAimbotLocked);
            if (IsFocused)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintFocused, SKPaints.TextFocused);
            if (this is LocalPlayer)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintLocalPlayer, SKPaints.TextLocalPlayer);
            switch (Type)
            {
                case PlayerType.Teammate:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintTeammate, SKPaints.TextTeammate);
                case PlayerType.PMC:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPMC, SKPaints.TextPMC);
                case PlayerType.AIScav:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintScav, SKPaints.TextScav);
                case PlayerType.AIRaider:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintRaider, SKPaints.TextRaider);
                case PlayerType.AIBoss:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintBoss, SKPaints.TextBoss);
                case PlayerType.PScav:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPScav, SKPaints.TextPScav);
                case PlayerType.SpecialPlayer:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintWatchlist, SKPaints.TextWatchlist);
                case PlayerType.Streamer:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintStreamer, SKPaints.TextStreamer);
                default:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPMC, SKPaints.TextPMC);
            }
        }

        public void DrawMouseover(SKCanvas canvas, LoneMapParams mapParams, LocalPlayer localPlayer)
        {
            if (this == localPlayer)
                return;
            var lines = new List<string>();
            var name = MainForm.Config.HideNames && IsHuman ? "<Hidden>" : Name;
            string health = null;
            if (this is ObservedPlayer observed)
                health = observed.HealthStatus is Enums.ETagStatus.Healthy
                    ? null
                    : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
            if (IsStreaming) // Streamer Notice
                lines.Add("[LIVE TTV - Double Click]");
            string alert = this.Alerts?.Trim();
            if (!string.IsNullOrEmpty(alert)) // Special Players,etc.
                lines.Add(alert);
            if (IsHostileActive) // Enemy Players, display information
            {
                lines.Add($"{name}{health}");
                var gear = Gear;
                var hands = Hands?.CurrentItem;
                lines.Add($"Use:{(hands is null ? "--" : hands)}");
                var faction = PlayerSide.ToString();
                string g = null;
                if (GroupID != -1)
                    g = $" G:{GroupID} ";
                lines.Add($"{faction}{g}");
                var loot = gear?.Loot;
                if (loot is not null)
                {
                    var playerValue = TarkovMarketItem.FormatPrice(gear?.Value ?? -1);
                    lines.Add($"Value: {playerValue}");
                    var iterations = 0;
                    foreach (var item in loot)
                    {
                        if (iterations++ >= 5)
                            break; // Only show first 5 Items (HV is on top)
                        lines.Add(item.GetUILabel(MainForm.Config.QuestHelper.Enabled));
                    }
                }
            }
            else if (!IsAlive)
            {
                lines.Add($"{Type.GetDescription()}:{name}");
                string g = null;
                if (GroupID != -1)
                    g = $"G:{GroupID} ";
                if (g is not null) lines.Add(g);
                var corpseLoot = LootObject?.Loot?.OrderLoot();
                if (corpseLoot is not null)
                {
                    var sumPrice = corpseLoot.Sum(x => x.Price);
                    var corpseValue = TarkovMarketItem.FormatPrice(sumPrice);
                    lines.Add($"Value: {corpseValue}"); // Player name, value
                    if (corpseLoot.Any())
                        foreach (var item in corpseLoot)
                            lines.Add(item.GetUILabel(MainForm.Config.QuestHelper.Enabled));
                    else lines.Add("Empty");
                }
            }
            else if (IsAIActive)
            {
                lines.Add(name);
            }

            Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, lines);
        }

        public void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (this == localPlayer ||
                !IsActive || !IsAlive)
                return;
            var dist = Vector3.Distance(localPlayer.Position, Position);
            if (dist > Program.Config.MaxDistance)
                return;
            var showInfo = IsAI ? ESP.Config.AIRendering.ShowLabels : ESP.Config.PlayerRendering.ShowLabels;
            var showDist = IsAI ? ESP.Config.AIRendering.ShowDist : ESP.Config.PlayerRendering.ShowDist;
            var showWep = IsAI ? ESP.Config.AIRendering.ShowWeapons : ESP.Config.PlayerRendering.ShowWeapons;
            var drawLabel = showInfo || showDist || showWep;

            if (IsHostile && (ESP.Config.HighAlertMode is HighAlertMode.AllPlayers ||
                              (ESP.Config.HighAlertMode is HighAlertMode.HumansOnly && IsHuman))) // Check High Alert
            {
                if (this.IsFacingTarget(localPlayer))
                {
                    if (!HighAlertSw.IsRunning)
                        HighAlertSw.Start();
                    else if (HighAlertSw.Elapsed.TotalMilliseconds >= 500f) // Don't draw twice or more
                        HighAlert.DrawHighAlertESP(canvas, this);
                }
                else
                {
                    HighAlertSw.Reset();
                }
            }

            if (!CameraManagerBase.WorldToScreen(ref Position, out var baseScrPos))
                return;
            var espPaints = GetESPPaints();
            var renderMode = IsAI ? ESP.Config.AIRendering.RenderingMode : ESP.Config.PlayerRendering.RenderingMode;
            if (renderMode is not ESPPlayerRenderMode.None && this is BtrOperator btr)
            {
                if (CameraManagerBase.WorldToScreen(ref btr.Position, out var btrScrPos))
                    btrScrPos.DrawESPText(canvas, btr, localPlayer, showDist, espPaints.Item2, "BTR Vehicle");
                return; // Done drawing BTR - move on
            }

            if (renderMode is ESPPlayerRenderMode.Bones)
            {
                if (!this.Skeleton.UpdateESPBuffer())
                    return;
                canvas.DrawPoints(SKPointMode.Lines, Skeleton.ESPBuffer, espPaints.Item1);
            }
            else if (renderMode is ESPPlayerRenderMode.Box)
            {
                var getBox = Skeleton.GetESPBox(baseScrPos);
                if (getBox is not SKRect box)
                    return;
                canvas.DrawRect(box, espPaints.Item1);
                baseScrPos.X = box.MidX;
                baseScrPos.Y = box.Bottom;
            }
            else if (renderMode is ESPPlayerRenderMode.Presence)
            {
                if (!CameraManagerBase.WorldToScreen(ref Skeleton.Bones[Bones.HumanSpine2].Position, out var presenceScrPos, true, true))
                    return;
                canvas.DrawCircle(presenceScrPos, 1.5f * ESP.Config.FontScale, espPaints.Item1);
            }

            if (drawLabel)
            {
                var lines = new List<string>();
                if (showInfo)
                {
                    string health = null;
                    if (this is ObservedPlayer observed)
                        health = observed.HealthStatus is Enums.ETagStatus.Healthy
                            ? null
                            : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
                    string fac = null;
                    if (IsHostilePmc) // Prepend PMC Faction
                    {
                        if (PlayerSide is Enums.EPlayerSide.Usec)
                            fac = "U:";
                        else if (PlayerSide is Enums.EPlayerSide.Bear)
                            fac = "B:";
                    }

                    lines.Add($"{fac}{Name}{health}");
                }

                if (showWep)
                    lines.Add($"({Hands?.CurrentItem})");
                if (showDist)
                {
                    if (lines.Count == 0)
                        lines.Add($"{(int)dist}m");
                    else
                        lines[0] += $" ({(int)dist}m)";
                }

                var textPt = new SKPoint(baseScrPos.X,
                    baseScrPos.Y + espPaints.Item2.TextSize * ESP.Config.FontScale);
                textPt.DrawESPText(canvas, this, localPlayer, false, espPaints.Item2, lines.ToArray());
            }

            if (ESP.Config.ShowAimLock && IsAimbotLocked) // Show aim lock
            {
                var info = MemWriteFeature<Aimbot>.Instance.Cache;
                if (info is not null &&
                    info.LastFireportPos is Vector3 fpPos &&
                    info.LastPlayerPos is Vector3 playerPos)
                {
                    if (!CameraManagerBase.WorldToScreen(ref fpPos, out var fpScreen))
                        return;
                    if (!CameraManagerBase.WorldToScreen(ref playerPos, out var playerScreen))
                        return;
                    canvas.DrawLine(fpScreen, playerScreen, SKPaints.PaintBasicESP);
                }
            }
        }

        /// <summary>
        /// Gets Aimview drawing paintbrushes based on this Player Type.
        /// </summary>
        private ValueTuple<SKPaint, SKPaint> GetESPPaints()
        {
            if (IsAimbotLocked)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintAimbotLockedESP, SKPaints.TextAimbotLockedESP);
            if (IsFocused)
                return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintFocusedESP, SKPaints.TextFocusedESP);
            switch (Type)
            {
                case PlayerType.Teammate:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintFriendlyESP, SKPaints.TextFriendlyESP);
                case PlayerType.PMC:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPMCESP, SKPaints.TextPMCESP);
                case PlayerType.AIScav:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintScavESP, SKPaints.TextScavESP);
                case PlayerType.AIRaider:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintRaiderESP, SKPaints.TextRaiderESP);
                case PlayerType.AIBoss:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintBossESP, SKPaints.TextBossESP);
                case PlayerType.PScav:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPlayerScavESP, SKPaints.TextPlayerScavESP);
                case PlayerType.SpecialPlayer:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintWatchlistESP, SKPaints.TextWatchlistESP);
                case PlayerType.Streamer:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintStreamerESP, SKPaints.TextStreamerESP);
                default:
                    return new ValueTuple<SKPaint, SKPaint>(SKPaints.PaintPMCESP, SKPaints.TextPMCESP);
            }
        }

        #endregion

        #region Types

        /// <summary>
        /// Defines Player Unit Type (Player,PMC,Scav,etc.)
        /// </summary>
        public enum PlayerType
        {
            /// <summary>
            /// Default value if a type cannot be established.
            /// </summary>
            [Description("Default")]
            Default,
            /// <summary>
            /// Teammate of LocalPlayer.
            /// </summary>
            [Description("Teammate")]
            Teammate,
            /// <summary>
            /// Hostile/Enemy PMC.
            /// </summary>
            [Description("PMC")]
            PMC,
            /// <summary>
            /// Normal AI Bot Scav.
            /// </summary>
            [Description("Scav")]
            AIScav,
            /// <summary>
            /// Difficult AI Raider.
            /// </summary>
            [Description("Raider")]
            AIRaider,
            /// <summary>
            /// Difficult AI Boss.
            /// </summary>
            [Description("Boss")]
            AIBoss,
            /// <summary>
            /// Player controlled Scav.
            /// </summary>
            [Description("Player Scav")]
            PScav,
            /// <summary>
            /// 'Special' Human Controlled Hostile PMC/Scav (on the watchlist, or a special account type).
            /// </summary>
            [Description("Special")]
            SpecialPlayer,
            /// <summary>
            /// Human Controlled Hostile PMC/Scav that has a Twitch account name as their IGN.
            /// </summary>
            [Description("Streamer")]
            Streamer
        }

        #endregion
    }
}
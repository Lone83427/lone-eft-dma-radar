using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Radar;
using arena_dma_radar.UI.Misc;
using arena_dma_radar.Arena.ArenaPlayer.Plugins;
using arena_dma_radar.Arena.GameWorld;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;
using eft_dma_shared.Common.Unity.LowLevel;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Maps;
using arena_dma_radar.Arena.Features.MemoryWrites;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Misc.Pools;
using eft_dma_shared.Common.DMA;

namespace arena_dma_radar.Arena.ArenaPlayer
{
    /// <summary>
    /// Base class for Tarkov Players.
    /// Tarkov implements several distinct classes that implement a similar player interface.
    /// </summary>
    public abstract class Player : IWorldEntity, IMapEntity, IMouseoverEntity, IESPEntity, IPlayer
    {
        #region Static Interfaces
        public static implicit operator ulong(Player x) => x?.Base ?? 0x0;
        private static readonly ConcurrentDictionary<ulong, Stopwatch> _rateLimit = new();


        /// <summary>
        /// Resets/Updates 'static' assets in preparation for a new game/raid instance.
        /// </summary>
        public static void Reset()
        {
            _rateLimit.Clear();
            lock (_focusedPlayers)
            {
                _focusedPlayers.Clear();
            }
        }
        #endregion

        #region Allocation
        /// <summary>
        /// Allocates a player and takes into consideration any rate-limits.
        /// </summary>
        /// <param name="playerDict">Player Dictionary collection to add the newly allocated player to.</param>
        /// <param name="playerBase">Player base memory address.</param>
        /// <param name="initialPosition">Initial position to be set (Optional). Usually for reallocations.</param>
        public static void Allocate(ConcurrentDictionary<ulong, Player> playerDict, ulong playerBase, Vector3? initialPosition = null)
        {
            var sw = _rateLimit.AddOrUpdate(playerBase,
                (key) => new(),
                (key, oldValue) => oldValue);
            if (sw.IsRunning && sw.Elapsed.TotalMilliseconds < 500f)
                return;
            try
            {
                var player = AllocateInternal(playerBase, initialPosition);
                playerDict[player] = player;
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

        private static Player AllocateInternal(ulong playerBase, Vector3? initialPosition)
        {
            var className = ObjectClass.ReadName(playerBase, 64);
            if (className != "ArenaObservedPlayerView")
                throw new ArgumentOutOfRangeException(nameof(className));
            return new ArenaObservedPlayer(playerBase);
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
        public string TwitchChannelURL { get; private set; }
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
        /// </summary>
        public virtual Skeleton Skeleton => throw new NotImplementedException(nameof(Skeleton));
        /// <summary>
        /// Duration of consecutive errors.
        /// </summary>
        public Stopwatch ErrorTimer { get; } = new();
        /// <summary>
        /// True if player is 'Locked On' via Aimbot.
        /// </summary>
        public bool IsAimbotLocked { get; set; }
        /// <summary>
        /// True if Player is Focused (displays a different color on Radar/ESP).
        /// </summary>
        public bool IsFocused { get; protected set; }

        #endregion

        #region Virtual Properties
        /// <summary>
        /// Player name.
        /// </summary>
        public virtual string Name { get; }
        /// <summary>
        /// Account UUID for Human Controlled Players.
        /// </summary>
        public virtual string AccountID { get; }
        /// <summary>
        /// Group that the player belongs to.
        /// </summary>
        public virtual int TeamID { get; } = -1;
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
        /// Player is a PMC Operator.
        /// </summary>
        public bool IsPmc => PlayerSide is Enums.EPlayerSide.Usec || PlayerSide is Enums.EPlayerSide.Bear;
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
        /// Player is hostile and alive/active.
        /// </summary>
        public bool IsHostileActive => IsHostile && IsActive && IsAlive;
        /// <summary>
        /// Player is human-controlled, hostile, and Active/Alive.
        /// </summary>
        public bool IsHumanActive => IsHuman && IsActive && IsAlive;
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
        public bool HasExfild => !IsActive && IsAlive;
        #endregion

        #region Methods


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
                        this.SetInactive();
                };
            }
        }

        /// <summary>
        /// Mark player as inactive (not dead / not on map).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetInactive()
        {
            Corpse = null;
            IsActive = false;
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
        /// Mark player as alive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetAlive()
        {
            Corpse = null;
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
        protected bool SetRotation(ref Vector2 rotation)
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
        /// Returns the Team ID of a player based on their Armband Color.
        /// </summary>
        /// <param name="inventoryController">Player's Inventory Controller.</param>
        /// <returns>Team ID. -1 if not found.</returns>
        protected static int GetTeamID(ulong inventoryController)
        {
            var inventory = Memory.ReadPtr(inventoryController + Offsets.InventoryController.Inventory);
            var equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment);
            var slots = Memory.ReadPtr(equipment + Offsets.Equipment.Slots);
            using var slotsArray = MemArray<ulong>.Get(slots);

            foreach (var slotPtr in slotsArray)
            {
                var slotNamePtr = Memory.ReadPtr(slotPtr + Offsets.Slot.ID);
                string name = Memory.ReadUnityString(slotNamePtr);
                if (name == "ArmBand")
                {
                    var containedItem = Memory.ReadPtr(slotPtr + Offsets.Slot.ContainedItem);
                    var itemTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    var idPtr = Memory.ReadValue<Types.MongoID>(itemTemplate + Offsets.ItemTemplate._id);
                    string id = Memory.ReadUnityString(idPtr.StringID);

                    if (id == "63615c104bc92641374a97c8")
                        return (int)Enums.ArmbandColorType.red;
                    else if (id == "63615bf35cb3825ded0db945")
                        return (int)Enums.ArmbandColorType.fuchsia;
                    else if (id == "63615c36e3114462cd79f7c1")
                        return (int)Enums.ArmbandColorType.yellow;
                    else if (id == "63615bfc5cb3825ded0db947")
                        return (int)Enums.ArmbandColorType.green;
                    else if (id == "63615bc6ff557272023d56ac")
                        return (int)Enums.ArmbandColorType.azure;
                    else if (id == "63615c225cb3825ded0db949")
                        return (int)Enums.ArmbandColorType.white;
                    else if (id == "63615be82e60050cb330ef2f")
                        return (int)Enums.ArmbandColorType.blue;
                    else
                        return -1;
                }
            }

            return -1;
        }
        /// <summary>
        /// Refresh Gear if Active Human Player. TODO
        /// </summary>
        //public void RefreshGear()
        //{
        //    try
        //    {
        //        Gear ??= new(this, IsPmc);
        //        Gear?.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoneLogging.WriteLine($"[GearManager] ERROR for Player {Name}: {ex}");
        //    }
        //}
        /// <summary>
        /// Refresh item in player's hands. TODO
        /// </summary>
        //public void RefreshHands()
        //{
        //    try
        //    {
        //        if (IsActive && IsAlive)
        //        {
        //            Hands ??= new HandsManager(this);
        //            Hands?.Refresh();
        //        }
        //    }
        //    catch { }
        //}

        /// <summary>
        /// Get the Transform Internal Chain for this Player.
        /// </summary>
        /// <param name="bone">Bone to lookup.</param>
        /// <returns>Array of offsets for transform internal chain.</returns>
        public virtual uint[] GetTransformInternalChain(Bones bone) =>
            throw new NotImplementedException();
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
            var pRendererContainersArray = Memory.ReadPtr(this.Body + Offsets.PlayerBody._bodyRenderers, false);
            using var rendererContainersArray = MemArray<Types.BodyRendererContainer>.Get(pRendererContainersArray, false);
            ArgumentOutOfRangeException.ThrowIfZero(rendererContainersArray.Count);

            foreach (var rendererContainer in rendererContainersArray)
            {
                using var renderersArray = MemArray<ulong>.Get(rendererContainer.Renderers, false);
                ArgumentOutOfRangeException.ThrowIfZero(renderersArray.Count);

                foreach (var skinnedMeshRenderer in renderersArray)
                {
                    // Cached ptr to Renderer
                    var renderer = Memory.ReadPtr(skinnedMeshRenderer + UnityOffsets.SkinnedMeshRenderer.Renderer, false);
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
            var slotViews = Memory.ReadValue<ulong>(this.Body + Offsets.PlayerBody.SlotViews, false);
            if (!Utils.IsValidVirtualAddress(slotViews))
                return;

            var pSlotViewsDict = Memory.ReadValue<ulong>(slotViews + Offsets.SlotViewsContainer.Dict, false);
            if (!Utils.IsValidVirtualAddress(pSlotViewsDict))
                return;

            using var slotViewsDict = MemDictionary<ulong, ulong>.Get(pSlotViewsDict, false);
            if (slotViewsDict.Count == 0)
                return;

            foreach (var slot in slotViewsDict)
            {
                if (!Utils.IsValidVirtualAddress(slot.Value))
                    continue;

                var pDressesArray = Memory.ReadValue<ulong>(slot.Value + Offsets.PlayerBodySubclass.Dresses, false);
                if (!Utils.IsValidVirtualAddress(pDressesArray))
                    continue;

                using var dressesArray = MemArray<ulong>.Get(pDressesArray, false);
                if (dressesArray.Count == 0)
                    continue;

                foreach (var dress in dressesArray)
                {
                    if (!Utils.IsValidVirtualAddress(dress))
                        continue;

                    var pRenderersArray = Memory.ReadValue<ulong>(dress + Offsets.Dress.Renderers, false);
                    if (!Utils.IsValidVirtualAddress(pRenderersArray))
                        continue;

                    using var renderersArray = MemArray<ulong>.Get(pRenderersArray, false);
                    if (renderersArray.Count == 0)
                        continue;

                    foreach (var renderer in renderersArray)
                    {
                        if (!Utils.IsValidVirtualAddress(renderer))
                            continue;

                        ulong rendererNative = Memory.ReadValue<ulong>(renderer + 0x10, false);
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
            int materialsCount = Memory.ReadValue<int>(renderer + UnityOffsets.Renderer.Count, false);
            ArgumentOutOfRangeException.ThrowIfLessThan(materialsCount, 0, nameof(materialsCount));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(materialsCount, 30, nameof(materialsCount));
            if (materialsCount == 0)
                return;
            var materialsArrayPtr = Memory.ReadPtr(renderer + UnityOffsets.Renderer.Materials, false);
            var materials = Enumerable.Repeat<int>(chamsMaterial, materialsCount).ToArray();
            writes.AddBufferEntry(materialsArrayPtr, materials.AsSpan());
        }

        #endregion

        #region Interfaces
        public Vector2 MouseoverPosition { get; set; }
        public ref Vector3 Position => ref this.Skeleton.Root.Position;

        public void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            try
            {
                var point = this.Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
                this.MouseoverPosition = new(point.X, point.Y);
                if (!this.IsAlive) // Player Dead -- Draw 'X' death marker and move on
                    DrawDeathMarker(canvas, point);
                else
                {
                    DrawPlayerMarker(canvas, localPlayer, point);
                    if (this == localPlayer)
                        return;
                    var height = this.Position.Y - localPlayer.Position.Y;
                    var dist = Vector3.Distance(localPlayer.Position, this.Position);
                    string[] lines = null;
                    if (!MainForm.Config.HideNames) // show full names & info
                    {
                        string name = null;
                        if (this.ErrorTimer.ElapsedMilliseconds > 100)
                            name = "ERROR"; // In case POS stops updating, let us know!
                        else
                            name = this.Name;
                        string health = null;
                        if (this is ArenaObservedPlayer observed)
                            health = observed.HealthStatus is Enums.ETagStatus.Healthy ?
                            null : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
                        lines = new string[2]
                        {
                        $"{name}{health}",
                        $"H: {(int)Math.Round(height)} D: {(int)Math.Round(dist)}"
                        };
                    }
                    else // just height, distance
                    {
                        lines = new string[1]
                        {
                        $"{(int)Math.Round(height)},{(int)Math.Round(dist)}"
                        };
                        if (this.ErrorTimer.ElapsedMilliseconds > 100)
                            lines[0] = "ERROR"; // In case POS stops updating, let us know!
                    }
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
        private void DrawPlayerMarker(SKCanvas canvas, IPlayer localPlayer, SKPoint point)
        {
            var radians = this.MapRotation.ToRadians();
            var paints = GetPaints();
            if (this != localPlayer && MainForm.MouseoverGroup is int grp && grp == this.TeamID)
                paints.Item1 = SKPaints.PaintMouseoverGroup;
            SKPaints.ShapeOutline.StrokeWidth = paints.Item1.StrokeWidth + (2f * MainForm.UIScale);

            var size = 6 * MainForm.UIScale;
            canvas.DrawCircle(point, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(point, size, paints.Item1); // draw LocalPlayer marker

            int aimlineLength = this == localPlayer ?
                MainForm.Config.AimLineLength : 15;
            if (!this.IsFriendly && this.IsFacingTarget(localPlayer)) // Hostile Player, check if aiming at a friendly (High Alert)
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
            float length = 6 * MainForm.UIScale;
            canvas.DrawLine(new SKPoint(point.X - length, point.Y + length), new SKPoint(point.X + length, point.Y - length), SKPaints.PaintDeathMarker);
            canvas.DrawLine(new SKPoint(point.X - length, point.Y - length), new SKPoint(point.X + length, point.Y + length), SKPaints.PaintDeathMarker);
        }
        /// <summary>
        /// Gets the point where the Aimline 'Line' ends. Applies UI Scaling internally.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SKPoint GetAimlineEndpoint(SKPoint start, float radians, float aimlineLength)
        {
            aimlineLength *= MainForm.UIScale;
            return new SKPoint((float)(start.X + MathF.Cos(radians) * aimlineLength), (float)(start.Y + Math.Sin(radians) * aimlineLength));
        }
        /// <summary>
        /// Draws Player Text on this location.
        /// </summary>
        private void DrawPlayerText(SKCanvas canvas, SKPoint point, string[] lines)
        {
            var paints = GetPaints();
            if (MainForm.MouseoverGroup is int grp && grp == this.TeamID)
                paints.Item2 = SKPaints.TextMouseoverGroup;
            float spacing = 3 * MainForm.UIScale;
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
            if (this.IsAimbotLocked)
                return new(SKPaints.PaintAimbotLocked, SKPaints.TextAimbotLocked);
            else if (this is LocalPlayer)
                return new(SKPaints.PaintLocalPlayer, null);
            else if (this.IsFocused)
                return new(SKPaints.PaintFocused, SKPaints.TextFocused);
            switch (this.Type)
            {
                case PlayerType.Teammate:
                    return new(SKPaints.PaintTeammate, SKPaints.TextTeammate);
                case PlayerType.Player:
                    return new(SKPaints.PaintPlayer, SKPaints.TextPlayer);
                case PlayerType.AI:
                    return new(SKPaints.PaintAI, SKPaints.TextAI);
                case PlayerType.Streamer:
                    return new(SKPaints.PaintStreamer, SKPaints.TextStreamer);
                default:
                    return new(SKPaints.PaintPlayer, SKPaints.TextPlayer);
            }
        }
        public void DrawMouseover(SKCanvas canvas, LoneMapParams mapParams, LocalPlayer localPlayer)
        {
            List<string> lines = new();
            string name = MainForm.Config.HideNames && this.IsHuman ? 
                "<Hidden>" : this.Name;
            if (this.IsStreaming) // Streamer Notice
                lines.Add("[LIVE TTV - Double Click]");
            if (this is ArenaObservedPlayer observed)
            {
                string health = observed.HealthStatus is Enums.ETagStatus.Healthy ?
                    null : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
                lines.Add($"{name}{health}");
                if (observed.TeamID != -1)
                {
                    lines.Add($" T:{observed.TeamID} ");
                }
                var equipment = observed.Gear?.Equipment;
                var hands = observed.Hands?.InHands;
                lines.Add($"Use:{(hands is null ? "--" : hands)}");
                if (equipment is not null)
                {
                    foreach (var item in equipment)
                        lines.Add($"{item.Key}: {item.Value}");
                }
            }
            else 
                return;
            this.Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, lines);
        }

        public void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (this == localPlayer ||
                    !this.IsActive || !this.IsAlive)
                return;
            bool showInfo = ESP.Config.PlayerRendering.ShowLabels;
            bool showDist = ESP.Config.PlayerRendering.ShowDist;
            bool showWep = ESP.Config.PlayerRendering.ShowWeapons;
            bool drawLabel = showInfo || showDist || showWep;
            var dist = Vector3.Distance(localPlayer.Position, this.Position);
            if (dist > LocalGameWorld.MAX_DIST)
                return;

            if (this.IsHostile && (ESP.Config.HighAlert && this.IsHuman)) // Check High Alert
            {
                if (this.IsFacingTarget(localPlayer))
                {
                    if (!this.HighAlertSw.IsRunning)
                        this.HighAlertSw.Start();
                    else if (this.HighAlertSw.Elapsed.TotalMilliseconds >= 500f) // Don't draw twice or more
                        HighAlert.DrawHighAlertESP(canvas, this);
                }
                else
                    this.HighAlertSw.Reset();
            }
            if (!CameraManagerBase.WorldToScreen(ref Position, out var baseScrPos))
                return;
            var paint = this.GetEspPlayerPaint();
            if (ESP.Config.PlayerRendering.RenderingMode is ESPPlayerRenderMode.Bones) // Draw Player Bones
            {
                if (!this.Skeleton.UpdateESPBuffer())
                    return;
                canvas.DrawPoints(SKPointMode.Lines, Skeleton.ESPBuffer, paint.Item1);
            }
            if (drawLabel && this is ArenaObservedPlayer observed)
            {
                var lines = new List<string>();
                if (showInfo)
                {
                    string health = observed.HealthStatus is Enums.ETagStatus.Healthy ?
                    null : $" ({observed.HealthStatus.GetDescription()})"; // Only display abnormal health status
                    lines.Add($"{observed.Name}{health}");
                }
                if (showWep)
                    lines.Add($"({observed.Hands?.InHands})");
                if (showDist)
                {
                    if (lines.Count == 0)
                        lines.Add($"{(int)dist}m");
                    else
                        lines[0] += $" ({(int)dist}m)";
                }
                var textPt = new SKPoint(baseScrPos.X,
                    baseScrPos.Y + (paint.Item2.TextSize * ESP.Config.FontScale));
                textPt.DrawESPText(canvas, observed, localPlayer, false, paint.Item2, lines.ToArray());
            }
            if (ESP.Config.ShowAimLock && this.IsAimbotLocked) // Show aim lock
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
        /// Gets Aimview drawing paintbrush based on Player Type.
        /// </summary>
        public ValueTuple<SKPaint, SKPaint> GetEspPlayerPaint()
        {
            if (this.IsAimbotLocked)
                return new(SKPaints.PaintAimbotLockedESP, SKPaints.TextAimbotLockedESP);
            else if (this.IsFocused)
                return new(SKPaints.PaintFocusedESP, SKPaints.TextFocusedESP);
            switch (this.Type)
            {
                case Player.PlayerType.Teammate:
                    return new(SKPaints.PaintTeammateESP, SKPaints.TextTeammateESP);
                case Player.PlayerType.Player:
                    return new(SKPaints.PaintPlayerESP, SKPaints.TextPlayerESP);
                case Player.PlayerType.AI:
                    return new(SKPaints.PaintAIESP, SKPaints.TextAIESP);
                case Player.PlayerType.Streamer:
                    return new(SKPaints.PaintStreamerESP, SKPaints.TextStreamerESP);
                default:
                    return new(SKPaints.PaintPlayerESP, SKPaints.TextPlayerESP);
            }
        }
        #endregion

        #region Focused Players
        private static readonly HashSet<string> _focusedPlayers = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Toggles this player's focused status.
        /// Only applies to Human Controlled Players.
        /// </summary>
        public void ToggleFocus()
        {
            if (this is not ArenaObservedPlayer ||
                !this.IsHumanActive)
                return;
            string id = this.AccountID?.Trim();
            if (string.IsNullOrEmpty(id))
                return;
            lock (_focusedPlayers)
            {
                bool isFocused = _focusedPlayers.Contains(id);
                if (isFocused)
                    _focusedPlayers.Remove(id);
                else
                    _focusedPlayers.Add(id);
                IsFocused = !isFocused;
            }
        }

        /// <summary>
        /// Check if this Player was focused prior (from a different round,etc.)
        /// </summary>
        /// <returns>True if focused, otherwise False.</returns>
        protected bool CheckIfFocused()
        {
            string id = this.AccountID?.Trim();
            if (string.IsNullOrEmpty(id))
                return false;
            lock (_focusedPlayers)
            {
                return _focusedPlayers.Contains(id);
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
            /// Bot/AI Player.
            /// </summary>
            [Description("AI")]
            AI,
            /// <summary>
            /// Human Controlled Player.
            /// </summary>
            [Description("Player")]
            Player,
            /// <summary>
            /// Human Controlled Hostile PMC/Scav that has a Twitch account name as their IGN.
            /// </summary>
            [Description("Streamer")]
            Streamer
        }

        #endregion
    }
}

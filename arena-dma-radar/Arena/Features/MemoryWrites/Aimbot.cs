using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.Arena.ArenaPlayer.Plugins;
using arena_dma_radar.Arena.GameWorld;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Ballistics;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.Features.MemoryWrites
{
    public sealed class Aimbot : MemWriteFeature<Aimbot>
    {
        #region Fields / Properties / Startup

        public static readonly Lock SyncRoot = new();
        public static bool Engaged = false;

        /// <summary>
        /// Aimbot Configuration.
        /// </summary>
        public static AimbotConfig Config { get; } = Program.Config.MemWrites.Aimbot;
        /// <summary>
        /// Aimbot Supported Bones.
        /// </summary>
        public static readonly IReadOnlySet<Bones> BoneNames = new HashSet<Bones>
        {
            Bones.HumanHead,
            Bones.HumanNeck,
            Bones.HumanSpine3,
            Bones.HumanPelvis,
            Bones.Legs
        };

        private bool _firstLock;
        private sbyte _lastShotIndex = -1;
        private Bones _lastRandomBone = Config.Bone;
        /// <summary>
        /// Aimbot Info.
        /// </summary>
        public AimbotCache Cache { get; private set; }

        public Aimbot()
        {
            new Thread(AimbotWorker)
            {
                IsBackground = true,
                Priority = ThreadPriority.Highest
            }.Start();
        }

        public override void OnGameStop()
        {
            _weaponDirectionGetter = null;
            _weaponDirectionPatched = default;
        }

        public override bool Enabled
        {
            get => Config.Enabled;
            set => Config.Enabled = value;
        }

        /// <summary>
        /// Managed Thread that does realtime Aimbot updates.
        /// </summary>
        private void AimbotWorker()
        {
            LoneLogging.WriteLine("Aimbot thread starting...");
            while (true)
            {
                try
                {
                    if (MemDMABase.WaitForRaid() && Enabled && MemWrites.Enabled && Memory.Game is LocalGameWorld game)
                    {
                        while (Enabled && MemWrites.Enabled && game.InRaid)
                        {
                            _weaponDirectionGetter ??= GetWeaponDirectionGetter();
                            SetAimbot(game);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoneLogging.WriteLine($"CRITICAL ERROR on Aimbot Thread: {ex}"); // Log CRITICAL error
                }
                finally
                {
                    try { ResetAimbot(); } catch { }
                    Thread.Sleep(200);
                }
            }
        }

        #endregion

        #region Aimbot Execution

        /// <summary>
        /// Executes Aimbot features on the AimbotDMAWorker Thread.
        /// </summary>
        private void SetAimbot(LocalGameWorld game)
        {
            try
            {
                if (Engaged && Memory.LocalPlayer is LocalPlayer localPlayer && ILocalPlayer.HandsController is ulong handsController && handsController.IsValidVirtualAddress())
                {
                    /// Check if the cache is still valid
                    /// This checks if the HandsController (FirearmController) address has changed for LocalPlayer
                    /// If it has changed we should re-init the Aimbot Cache
                    if (Cache != handsController) // Reset Aimbot Cache -> First Cycle
                    {
                        LoneLogging.WriteLine("[Aimbot] Reset!");
                        Cache?.ResetLock();
                        Cache = new AimbotCache(handsController);
                    }

                    /// If for some reason the cache is still null, do not continue
                    if (Cache is null)
                    {
                        Thread.Sleep(1);
                        return;
                    }
                    Cache.FireportTransform ??= GetFireport(handsController);

                    /// If already locked on, check if the target has died
                    if (Cache.AimbotLockedPlayer is not null)
                    {
                        ulong corpseAddr = Cache.AimbotLockedPlayer.CorpseAddr;
                        if (corpseAddr.IsValidVirtualAddress())
                        {
                            ulong corpse = Memory.ReadValue<ulong>(corpseAddr, false);
                            if (corpse.IsValidVirtualAddress()) // Dead
                            {
                                Cache.AimbotLockedPlayer.SetDead(corpse);
                                Cache.ResetLock();
                            }
                        }
                    }
                    /// If we do not have a target, acquire one
                    if (Cache.AimbotLockedPlayer is null)
                    {
                        const bool disableRelock = true;
                        if (_firstLock && disableRelock) // Disable re-locking if configured
                        {
                            ResetAimbot();
                            while (Engaged)
                                Thread.Sleep(1);
                            return;
                        }

                        Cache.AimbotLockedPlayer = GetBestAimbotTarget(game, localPlayer);
                    }
                    /// If we still do not have a target, Sleep and return
                    if (Cache.AimbotLockedPlayer is null)
                    {
                        Thread.Sleep(1);
                        return;
                    }
                    /// We have a valid target and Aimbot can continue
                    _firstLock = true;
                    BeginSilentAim(localPlayer);
                    Cache.AimbotLockedPlayer.IsAimbotLocked = true; // Locked On
                }
                else
                {
                    _firstLock = false;
                    ResetAimbot();
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Aimbot [FAIL] {ex}");
            }
        }

        /// <summary>
        /// Begin Silent Aim Aimbot.
        /// </summary>
        private void BeginSilentAim(LocalPlayer localPlayer)
        {
            try
            {
                var target = Cache.AimbotLockedPlayer;
                var bone = Config.Bone;

                if (Config.RandomBone.Enabled) // Random Bone
                {
                    var shotIndex = Memory.ReadValue<sbyte>(Cache + Offsets.ClientFirearmController.ShotIndex, false);
                    if (shotIndex != _lastShotIndex)
                    {
                        _lastRandomBone = Config.RandomBone.GetRandomBone();
                        _lastShotIndex = shotIndex;
                        LoneLogging.WriteLine($"New Random Bone {_lastRandomBone.GetDescription()} ({shotIndex})");
                    }
                    bone = _lastRandomBone;
                }
                else if (Config.SilentAim.AutoBone)
                {
                    var boneTargets = new List<PossibleAimbotTarget>();
                    foreach (var tr in target.Skeleton.Bones)
                    {
                        if (tr.Key is Bones.HumanBase)
                            continue;
                        if (CameraManagerBase.WorldToScreen(ref tr.Value.Position, out var scrPos, true))
                        {
                            boneTargets.Add(
                            new PossibleAimbotTarget()
                            {
                                Player = target,
                                FOV = CameraManagerBase.GetFovMagnitude(scrPos),
                                Bone = tr.Key
                            });
                        }
                    }
                    if (boneTargets.Count > 0)
                        bone = boneTargets.MinBy(x => x.FOV).Bone;
                }
                if (bone == Bones.Legs) // Pick a leg
                {
                    bool isLeft = Random.Shared.Next(0, 2) == 1;
                    if (isLeft)
                        bone = Bones.HumanLThigh2;
                    else
                        bone = Bones.HumanRThigh2;
                }

                /// Target Bone Position
                Vector3 bonePosition = target.Skeleton.Bones[bone].UpdatePosition();

                if (Config.SilentAim.SafeLock)
                {
                    if (IsSafeLockTripped()) // Unlock if target has left FOV
                    {
                        _firstLock = false; // Allow re-lock
                        ResetAimbot();
                        return;
                    }
                    bool IsSafeLockTripped()
                    {
                        foreach (var tr in target.Skeleton.Bones)
                        {
                            if (tr.Key is Bones.HumanBase)
                                continue;
                            if (CameraManagerBase.WorldToScreen(ref tr.Value.Position, out var scrPos, true) &&
                                CameraManagerBase.GetFovMagnitude(scrPos) is float fov && fov < Config.FOV)
                                return false; // At least one bone in FOV - exit early
                        }
                        return true;
                    }
                }

                /// Get Fireport Position & Run Prediction
                Vector3 fireportPosition;
                try
                {
                    fireportPosition = Cache.FireportTransform.UpdatePosition();
                }
                catch
                {
                    Cache.FireportTransform = null;
                    throw;
                }
                Vector3 newWeaponDirection = CalculateSilentAimTrajectory(target, ref fireportPosition, ref bonePosition);
                newWeaponDirection.ThrowIfAbnormal();

                Memory.WriteValue(localPlayer.PWA + Offsets.ProceduralWeaponAnimation.ShotNeedsFovAdjustments, false);
                PatchWeaponDirectionGetter(newWeaponDirection);
                Cache.LastFireportPos = fireportPosition;
                Cache.LastPlayerPos = bonePosition;
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Silent Aim [FAIL] {ex}");
                ResetSilentAim();
            }
        }

        #endregion

        #region Helper Methods

        private static Player GetBestAimbotTarget(LocalGameWorld game, Player localPlayer)
        {
            var players = game.Players?
                .Where(x => x.IsHostileActive);

            if (players is null || !players.Any())
                return null;

            // Calculate fov, distance and build Target Collection
            var targets = new List<PossibleAimbotTarget>();
            foreach (var player in players)
            {
                var distance = Vector3.Distance(localPlayer.Position, player.Position);
                if (distance > LocalGameWorld.MAX_DIST)
                    continue;
                foreach (var tr in player.Skeleton.Bones)
                {
                    if (tr.Key is Bones.HumanBase)
                        continue;
                    if (CameraManagerBase.WorldToScreen(ref tr.Value.Position, out var scrPos, true) &&
                        CameraManagerBase.GetFovMagnitude(scrPos) is float fov && fov < Config.FOV)
                    {
                        var target = new PossibleAimbotTarget()
                        {
                            Player = player,
                            FOV = fov,
                            Distance = Vector3.Distance(localPlayer.Position, tr.Value.Position)
                        };
                        targets.Add(target);
                    }
                }
            }

            if (targets.Count == 0)
                return null;
            switch (Config.TargetingMode)
            {
                case AimbotTargetingMode.FOV:
                    return targets.MinBy(x => x.FOV).Player;
                case AimbotTargetingMode.CQB:
                    return targets.MinBy(x => x.Distance).Player;
                default:
                    throw new NotImplementedException(nameof(Config.TargetingMode));
            }
        }

        /// <summary>
        /// Get LocalPlayer Fireport Transform.
        /// </summary>
        /// <param name="handsController"></param>
        /// <returns></returns>
        private static UnityTransform GetFireport(ulong handsController)
        {
            handsController.ThrowIfInvalidVirtualAddress();
            var ti = Memory.ReadPtrChain(handsController, Offsets.FirearmController.To_FirePortTransformInternal, false);
            return new UnityTransform(ti);
        }

        /// <summary>
        /// Recurses a given weapon for the total velocity on attachments.
        /// Used by Aimbot.
        /// </summary>
        /// <param name="lootItemBase">Item (Weapon) to recurse.</param>
        /// <param name="velocityModifier">Percentage to adjust the base velocity of a muzzle by.</param>
        private static void RecurseWeaponAttachVelocity(ulong lootItemBase, ref float velocityModifier)
        {
            try
            {
                var parentSlots = Memory.ReadPtr(lootItemBase + Offsets.LootItemMod.Slots);
                using var slots = MemArray<ulong>.Get(parentSlots);
                ArgumentOutOfRangeException.ThrowIfGreaterThan(slots.Count, 100, nameof(slots));

                foreach (var slot in slots)
                {
                    try
                    {
                        var containedItem = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem);
                        var itemTemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                        // Add this attachment's Velocity %
                        velocityModifier += Memory.ReadValue<float>(itemTemplate + Offsets.ModTemplate.Velocity);
                        RecurseWeaponAttachVelocity(containedItem, ref velocityModifier);
                    }
                    catch
                    {
                    } // Skip over empty slots
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"AIMBOT ERROR RecurseWeaponAttachVelocity() -> {ex}");
            }
        }

        /// <summary>
        /// Runs Aimbot Prediction between a source -> target.
        /// </summary>
        /// <param name="target">Target player.</param>
        /// <param name="sourcePosition">Source position.</param>
        /// <param name="targetPosition">Target position.</param>
        /// <returns>Weapon direction for the Source Position to aim towards the Target Position accounting for prediction results.</returns>
        private Vector3 CalculateSilentAimTrajectory(Player target, ref Vector3 sourcePosition, ref Vector3 targetPosition)
        {
            /// Get Current Ammo Details
            try
            {
                // Chambered bullet's velocity - this needs to be updated independently of the aimbot to improve performance

                int weaponVersion = Memory.ReadValue<int>(Cache.ItemBase + Offsets.LootItem.Version);
                if (Cache.LastWeaponVersion != weaponVersion) // New round in chamber
                {
                    var ammoTemplate = FirearmManager.MagazineManager.GetAmmoTemplateFromWeapon(Cache.ItemBase);
                    if (Cache.LoadedAmmo != ammoTemplate)
                    {
                        LoneLogging.WriteLine("[Aimbot] Ammo changed!");
                        Cache.Ballistics.BulletMassGrams = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletMassGram);
                        Cache.Ballistics.BulletDiameterMillimeters =
                            Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BulletDiameterMilimeters);
                        Cache.Ballistics.BallisticCoefficient =
                            Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.BallisticCoeficient);

                        /// Calculate Muzzle Velocity. There is a base value based on the Ammo Type,
                        /// however certain attachments/barrels will apply a % modifier to that base value.
                        /// These calculations will get the correct value.
                        float bulletSpeed = Memory.ReadValue<float>(ammoTemplate + Offsets.AmmoTemplate.InitialSpeed);
                        float velMod = 0f;
                        velMod += Memory.ReadValue<float>(Cache.ItemTemplate + Offsets.WeaponTemplate.Velocity);
                        RecurseWeaponAttachVelocity(Cache.ItemBase, ref velMod); // Expensive operation
                        velMod = 1f + (velMod / 100f); // Get percentage (the game will give us 15.00, we want to turn it into 1.15)
                        // Integrity check -> Should be between 0.01 and 1.99
                        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(velMod, 0d, nameof(velMod));
                        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(velMod, 2d, nameof(velMod));
                        bulletSpeed *= velMod;
                        // Calcs OK -> Cache Weapon/Ammo
                        Cache.Ballistics.BulletSpeed = bulletSpeed;
                        Cache.LoadedAmmo = ammoTemplate;
                    }
                    Cache.LastWeaponVersion = weaponVersion;
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Aimbot [WARNING] - Unable to set/update Ballistics: {ex}");
            }
            /// Target Velocity
            Vector3 targetVelocity;
            if (target is ArenaObservedPlayer)
                targetVelocity = Memory.ReadValue<Vector3>(target.MovementContext + Offsets.ObservedMovementController.Velocity, false);
            else
                targetVelocity = default; // No lateral prediction
            /// Run Prediction Simulation
            if (Cache.IsAmmoValid)
            {
                var sim = BallisticsSimulation.Run(ref sourcePosition, ref targetPosition, Cache.Ballistics);
                if (Math.Abs(targetVelocity.X) > 25f || Math.Abs(targetVelocity.Y) > 25f || Math.Abs(targetVelocity.Z) > 25f)
                {
                    LoneLogging.WriteLine("[AIMBOT] -> RunPrediction(): Invalid Target Velocity - Running without prediction.");
                }
                else
                {
                    targetVelocity *= sim.TravelTime;

                    targetPosition.X += targetVelocity.X;
                    targetPosition.Y += targetVelocity.Y + sim.DropCompensation;
                    targetPosition.Z += targetVelocity.Z;
                }
            }
            else
            {
                Cache.LoadedAmmo = default;
                Cache.LastWeaponVersion = default;
                LoneLogging.WriteLine("Aimbot [WARNING] - Invalid Ammo Ballistics! Running without prediction.");
            }

            return Vector3.Normalize(targetPosition - sourcePosition); // Return direction
        }

        /// <summary>
        /// Reset the Aimbot Lock and reset the aimbot back to default state.
        /// </summary>
        private void ResetAimbot()
        {
            Cache?.ResetLock();
            Cache = null;
            _lastShotIndex = -1;
            ResetSilentAim();
        }

        #endregion

        #region Silent Aim Internal

        private static ulong? _weaponDirectionGetter;
        private static bool _weaponDirectionPatched;

        private static readonly byte[] _weaponDirectionGetterOriginalBytes = new byte[] // The original bytes of the WeaponDirection getter method.
        {
            0x55,                    					// push rbp
            0x48, 0x8B, 0xEC,              				// mov rbp,rsp
            0x48, 0x81, 0xEC, 0x90, 0x00, 0x00, 0x00,   // sub rsp,00000090
            0x48, 0x89, 0x7D, 0xF8,           			// mov [rbp-08],rdi
            0x48, 0x89, 0x55, 0xF0,           			// mov [rbp-10],rdx
            0x48, 0x8B, 0xF9,              				// mov rdi,rcx
            0x49, 0xBB,                                 // mov r11
        };

        private const string _patchMask = "xx????xxx????xxx????xxxx";
        private static byte[] _weaponDirectionGetterPatchBytes = new byte[] // The silent aim bytes of the WeaponDirection getter method.
        {
            0xC7, 0x02, // mov [rdx], xBytes
            0x0, 0x0, 0x0, 0x0, // X

            0xC7, 0x42, 0x04, // mov [rdx+4], yBytes
            0x0, 0x0, 0x0, 0x0, // Y

            0xC7, 0x42, 0x08, // mov [rdx+8], zBytes
            0x0, 0x0, 0x0, 0x0, // Z

            0x48, 0x89, 0xD0, // mov rax, rdx

            0xC3 // ret
        };


        private static ulong GetWeaponDirectionGetter()
        {
            var fClass = MonoLib.MonoClass.Find("Assembly-CSharp", "EFT.Player+FirearmController", out _);
            ulong fMethod = fClass.FindJittedMethod("get_WeaponDirection");
            fMethod.ThrowIfInvalidVirtualAddress();
            var scan = new byte[32];
            Memory.ReadBuffer(fMethod, scan.AsSpan(), false, false);
            // Make sure the method signature matches by checking the first two bytes
            if (scan.FindSignatureOffset(_weaponDirectionGetterOriginalBytes) != -1 ||
                scan.FindSignatureOffset(_weaponDirectionGetterPatchBytes, _patchMask) != -1)
            {
                LoneLogging.WriteLine("[AIMBOT] WeaponDirectionGetter method found!");
                return fMethod;
            }
            throw new Exception("WeaponDirectionGetter method not found!");
        }

        private static void PatchWeaponDirectionGetter(Vector3 newWeaponDirection)
        {
            if (_weaponDirectionGetter is not ulong weaponDirectionGetter)
                throw new Exception("WeaponDirectionGetter is not set!");

            BinaryPrimitives.WriteSingleLittleEndian(_weaponDirectionGetterPatchBytes.AsSpan(2), newWeaponDirection.X);
            BinaryPrimitives.WriteSingleLittleEndian(_weaponDirectionGetterPatchBytes.AsSpan(9), newWeaponDirection.Y);
            BinaryPrimitives.WriteSingleLittleEndian(_weaponDirectionGetterPatchBytes.AsSpan(16), newWeaponDirection.Z);

            // Patch getter
            Memory.WriteBuffer(weaponDirectionGetter, _weaponDirectionGetterPatchBytes.AsSpan());
            _weaponDirectionPatched = true;
        }

        private static bool RestoreWeaponDirectionGetter()
        {
            try
            {
                if (_weaponDirectionGetter is not ulong weaponDirectionGetter)
                    return true; // Already unset

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        Memory.WriteBufferEnsure(weaponDirectionGetter, _weaponDirectionGetterOriginalBytes.AsSpan());
                        _weaponDirectionPatched = false;
                        return true;
                    }
                    catch { }
                }
                throw new Exception("Failed to restore Original Weapon Getter!");
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[AIMBOT] RestoreWeaponDirectionGetter(): {ex}");
            }

            return false;
        }

        /// <summary>
        /// Reset the Shot Direction (Silent Aim) back to default state.
        /// </summary>
        private static void ResetSilentAim()
        {
            if (_weaponDirectionPatched)
            {
                RestoreWeaponDirectionGetter();
                LoneLogging.WriteLine("Silent Aim [WEAPON GETTER RESET]");
            }
        }

        #endregion

        #region Types
        public enum AimbotTargetingMode : int
        {
            /// <summary>
            /// FOV based targeting.
            /// </summary>
            [Description(nameof(FOV))]
            FOV = 1,
            /// <summary>
            /// CQB (Distance) based targeting.
            /// </summary>
            [Description(nameof(CQB))]
            CQB = 2
        }
        /// <summary>
        /// Encapsulates Aimbot Targeting Results.
        /// </summary>
        private readonly struct PossibleAimbotTarget
        {
            /// <summary>
            /// Target Player that this result belongs to.
            /// </summary>
            public readonly Player Player { get; init; }
            /// <summary>
            /// LocalPlayer's FOV towards this Player.
            /// </summary>
            public readonly float FOV { get; init; }
            /// <summary>
            /// Target's Bone Type.
            /// </summary>
            public readonly Bones Bone { get; init; }
            /// <summary>
            /// LocalPlayer's Distance towards this Player.
            /// </summary>
            public readonly float Distance { get; init; }
        }

        /// <summary>
        /// Cached Values for the AimBot.
        /// Wraps the HandsController Base Address.
        /// </summary>
        public sealed class AimbotCache
        {
            public static implicit operator ulong(AimbotCache x) => x?.HandsBase ?? 0x0;
            /// <summary>
            /// Returns true if Ammo/Ballistics values are valid.
            /// </summary>
            public bool IsAmmoValid => Ballistics.IsAmmoValid;

            /// <summary>
            /// Address for Player.AbstractHandsController.
            /// Will change to a unique value each time a player changes what is in their hands (Weapon/Item/Grenade,etc.)
            /// </summary>
            private ulong HandsBase { get; }
            /// <summary>
            /// EFT.InventoryLogic.Item
            /// </summary>
            public ulong ItemBase { get; }
            /// <summary>
            /// EFT.InventoryLogic.ItemTemplate
            /// </summary>
            public ulong ItemTemplate { get; }
            /// <summary>
            /// Player that is currently 'locked on' to in Phase 1.
            /// </summary>
            public Player AimbotLockedPlayer { get; set; }
            /// <summary>
            /// Ammo Template of the ammo currently in the chamber.
            /// </summary>
            public ulong LoadedAmmo { get; set; }
            /// <summary>
            /// Ballistics Info.
            /// </summary>
            public BallisticsInfo Ballistics { get; } = new();
            /// <summary>
            /// Fireport Transform for LocalPlayer.
            /// </summary>
            public UnityTransform FireportTransform { get; set; }
            /// <summary>
            /// Last position of the Fireport from previous cycle.
            /// Null if first cycle.
            /// </summary>
            public Vector3? LastFireportPos { get; set; }
            /// <summary>
            /// Last position of the Last Player from previous cycle.
            /// Null if first cycle.
            /// </summary>
            public Vector3? LastPlayerPos { get; set; }
            /// <summary>
            /// Last weapon 'version', updates as shots are fired.
            /// </summary>
            public int LastWeaponVersion { get; set; } = -1;

            /// <param name="handsBase">Player.AbstractHandsController Address</param>
            public AimbotCache(ulong handsBase)
            {
                HandsBase = handsBase;
                ItemBase = Memory.ReadPtr(HandsBase + Offsets.ItemHandsController.Item, false);
                ItemTemplate = Memory.ReadPtr(ItemBase + Offsets.LootItem.Template, false);
            }

            /// <summary>
            /// Reset this Cache to a 'Non-Locked' state.
            /// </summary>
            public void ResetLock()
            {
                LastFireportPos = null;
                LastPlayerPos = null;
                if (AimbotLockedPlayer is not null)
                {
                    AimbotLockedPlayer.IsAimbotLocked = false;
                    AimbotLockedPlayer = null;
                }
            }
        }
        #endregion
    }
}
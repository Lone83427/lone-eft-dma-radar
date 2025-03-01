using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Misc.Pools;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;

namespace arena_dma_radar.Arena.ArenaPlayer.Plugins
{
    public sealed class FirearmManager
    {
        /// <summary>
        /// Program Configuration.
        /// </summary>
        private static Config Config { get; } = Program.Config;

        private readonly LocalPlayer _localPlayer;
        private CachedHandsInfo _hands;

        /// <summary>
        /// Magazine (if any) contained in this firearm.
        /// </summary>
        public MagazineManager Magazine { get; private set; }
        /// <summary>
        /// Current Firearm Fireport Transform.
        /// </summary>
        public UnityTransform FireportTransform { get; private set; }
        /// <summary>
        /// Last known Fireport Position.
        /// </summary>
        public Vector3? FireportPosition { get; private set; }
        /// <summary>
        /// Last known Fireport Rotation.
        /// </summary>
        public Quaternion? FireportRotation { get; private set; }

        public FirearmManager(LocalPlayer localPlayer)
        {
            _localPlayer = localPlayer;
            Magazine = new(localPlayer);
        }

        /// <summary>
        /// Realtime Loop for FirearmManager chained from LocalPlayer.
        /// </summary>
        /// <param name="index"></param>
        public void OnRealtimeLoop(ScatterReadIndex index)
        {
            if (FireportTransform is UnityTransform fireport)
            {
                index.AddEntry<SharedArray<UnityTransform.TrsX>>(-20, fireport.VerticesAddr,
                    (fireport.Index + 1) * SizeChecker<UnityTransform.TrsX>.Size); // Vertices
                index.Callbacks += x1 =>
                {
                    if (x1.TryGetResult<SharedArray<UnityTransform.TrsX>>(-20, out var vertices))
                        UpdateFireport(vertices);
                };
            }
        }

        /// <summary>
        /// Update Hands/Firearm/Magazine information for LocalPlayer.
        /// </summary>
        public void Update()
        {
            try
            {
                var hands = Memory.ReadPtr(_localPlayer.HandsControllerAddr, false);
                if (hands != _hands)
                {
                    _hands = null;
                    ResetFireport();
                    Magazine = new(_localPlayer);
                    _hands = GetHandsInfo(hands);
                }
                if (_hands.IsWeapon)
                {
                    if (CameraManagerBase.EspRunning && Config.ESP.ShowMagazine)
                    {
                        try
                        {
                            Magazine.Update(_hands);
                        }
                        catch (Exception ex)
                        {
                            LoneLogging.WriteLine($"[FirearmManager] ERROR Updating Magazine: {ex}");
                            Magazine = new(_localPlayer);
                        }
                    }
                    if (FireportTransform is UnityTransform fireportTransform) // Validate Fireport Transform
                    {
                        try
                        {
                            var v = Memory.ReadPtrChain(hands, Offsets.FirearmController.To_FirePortVertices);
                            if (fireportTransform.VerticesAddr != v)
                                ResetFireport();
                        }
                        catch (Exception ex)
                        {
                            LoneLogging.WriteLine($"[FirearmManager] ERROR Validating Fireport Transform: {ex}");
                            ResetFireport();
                        }
                    }
                    if (FireportTransform is null)
                    {
                        try
                        {
                            var t = Memory.ReadPtrChain(hands, Offsets.FirearmController.To_FirePortTransformInternal, false);
                            FireportTransform = new(t, false);
                            ArgumentOutOfRangeException.ThrowIfGreaterThan(
                                Vector3.Distance(FireportTransform.UpdatePosition(), _localPlayer.Position),
                                15f,
                                nameof(FireportTransform));
                        }
                        catch (Exception ex)
                        {
                            LoneLogging.WriteLine($"[FirearmManager] ERROR Getting Fireport Transform: {ex}");
                            ResetFireport();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[FirearmManager] ERROR: {ex}");
            }
        }

        /// <summary>
        /// Update cached fireport position/rotation (called from Main Loop).
        /// </summary>
        /// <param name="vertices">Fireport transform vertices.</param>
        private void UpdateFireport(SharedArray<UnityTransform.TrsX> vertices)
        {
            try
            {
                FireportPosition = FireportTransform?.UpdatePosition(vertices);
                FireportRotation = FireportTransform?.GetRotation(vertices);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[FirearmManager] ERROR Updating Fireport Data: {ex}");
                ResetFireport();
            }
        }

        /// <summary>
        /// Reset the Fireport Data.
        /// </summary>
        private void ResetFireport()
        {
            FireportTransform = null;
            FireportPosition = null;
            FireportRotation = null;
        }

        /// <summary>
        /// Get updated hands information.
        /// </summary>
        private static CachedHandsInfo GetHandsInfo(ulong handsController)
        {
            var itemBase = Memory.ReadPtr(handsController + Offsets.ItemHandsController.Item, false);
            var itemTemp = Memory.ReadPtr(itemBase + Offsets.LootItem.Template, false);
            var idPtr = Memory.ReadValue<Types.MongoID>(itemTemp + Offsets.ItemTemplate._id);
            string id = Memory.ReadUnityString(idPtr.StringID);
            ArgumentOutOfRangeException.ThrowIfNotEqual(id.Length, 24, nameof(id));
            if (!EftDataManager.AllItems.TryGetValue(id, out var heldItem))
                return new(handsController);
            return new(handsController, heldItem, itemBase);
        }

        #region Magazine Info

        /// <summary>
        /// Helper class to track a Player's Magazine Ammo Count.
        /// </summary>
        public sealed class MagazineManager
        {
            private readonly LocalPlayer _localPlayer;
            private string _fireType;
            private string _ammo;

            /// <summary>
            /// True if the MagazineManager is in a valid state for data output.
            /// </summary>
            public bool IsValid => MaxCount > 0;
            /// <summary>
            /// Current ammo count in Magazine.
            /// </summary>
            public int Count { get; private set; }
            /// <summary>
            /// Maximum ammo count in Magazine.
            /// </summary>
            public int MaxCount { get; private set; }
            /// <summary>
            /// Weapon Fire Mode & Ammo Type in a formatted string.
            /// </summary>
            public string WeaponInfo
            {
                get
                {
                    string result = "";
                    string ft = _fireType;
                    string ammo = _ammo;
                    if (ft is not null)
                        result += $"{ft}: ";
                    if (ammo is not null)
                        result += ammo;
                    if (string.IsNullOrEmpty(result))
                        return null;
                    return result.Trim().TrimEnd(':');
                }
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="player">Player to track magazine usage for.</param>
            public MagazineManager(LocalPlayer localPlayer)
            {
                _localPlayer = localPlayer;
            }

            /// <summary>
            /// Update Magazine Information for this instance.
            /// </summary>
            public void Update(CachedHandsInfo hands)
            {
                string ammoInChamber = null;
                string fireType = null;
                int maxCount = 0;
                int currentCount = 0;
                var fireModePtr = Memory.ReadValue<ulong>(hands.ItemAddr + Offsets.LootItemWeapon.FireMode);
                var chambersPtr = Memory.ReadValue<ulong>(hands.ItemAddr + Offsets.LootItemWeapon.Chambers);
                var magSlotPtr = Memory.ReadValue<ulong>(hands.ItemAddr + Offsets.LootItemWeapon._magSlotCache);
                if (fireModePtr != 0x0)
                {
                    var fireMode = (EFireMode)Memory.ReadValue<byte>(fireModePtr + Offsets.FireModeComponent.FireMode);
                    if (fireMode >= EFireMode.Auto && fireMode <= EFireMode.SemiAuto)
                        fireType = fireMode.GetDescription();
                }
                if (chambersPtr != 0x0) // Single chamber, or for some shotguns, multiple chambers
                {
                    using var chambers = MemArray<Chamber>.Get(chambersPtr);
                    currentCount += chambers.Count(x => x.HasBullet());
                    ammoInChamber = GetLoadedAmmoName(chambers.FirstOrDefault(x => x.HasBullet()));
                    maxCount += chambers.Count;
                }
                if (magSlotPtr != 0x0)
                {
                    var magItem = Memory.ReadValue<ulong>(magSlotPtr + Offsets.Slot.ContainedItem);
                    if (magItem != 0x0)
                    {
                        var magChambersPtr = Memory.ReadPtr(magItem + Offsets.LootItemMod.Slots);
                        using var magChambers = MemArray<Chamber>.Get(magChambersPtr);
                        if (magChambers.Count > 0) // Revolvers, etc.
                        {
                            maxCount += magChambers.Count;
                            currentCount += magChambers.Count(x => x.HasBullet());
                            ammoInChamber = GetLoadedAmmoName(magChambers.FirstOrDefault(x => x.HasBullet()));
                        }
                        else // Regular magazines
                        {
                            var cartridges = Memory.ReadPtr(magItem + Offsets.LootItemMagazine.Cartridges);
                            maxCount += Memory.ReadValue<int>(cartridges + Offsets.StackSlot.MaxCount);
                            var magStackPtr = Memory.ReadPtr(cartridges + Offsets.StackSlot._items);
                            using var magStack = MemList<ulong>.Get(magStackPtr);
                            foreach (var stack in magStack) // Each ammo type will be a separate stack
                            {
                                if (stack != 0x0)
                                    currentCount += Memory.ReadValue<int>(stack + Offsets.MagazineClass.StackObjectsCount, false);
                            }
                        }
                    }
                }
                _ammo = ammoInChamber;
                _fireType = fireType;
                Count = currentCount;
                MaxCount = maxCount;
            }

            /// <summary>
            /// Gets the name of the ammo round currently loaded in this chamber, otherwise NULL.
            /// </summary>
            /// <param name="chamber">Chamber to check.</param>
            /// <returns>Short name of ammo in chamber, or null if no round loaded.</returns>
            private static string GetLoadedAmmoName(Chamber chamber)
            {
                if (chamber != 0x0)
                {
                    var bulletItem = Memory.ReadValue<ulong>(chamber + Offsets.Slot.ContainedItem);
                    if (bulletItem != 0x0)
                    {
                        var bulletTemp = Memory.ReadPtr(bulletItem + Offsets.LootItem.Template);
                        var idPtr = Memory.ReadValue<Types.MongoID>(bulletTemp + Offsets.ItemTemplate._id);
                        string id = Memory.ReadUnityString(idPtr.StringID);
                        if (EftDataManager.AllItems.TryGetValue(id, out var bullet))
                            return bullet?.ShortName;
                    }
                }
                return null;
            }

            /// <summary>
            /// Returns the Ammo Template from a Weapon (First loaded round).
            /// </summary>
            /// <param name="lootItemBase">EFT.InventoryLogic.Weapon instance</param>
            /// <returns>Ammo Template Ptr</returns>
            public static ulong GetAmmoTemplateFromWeapon(ulong lootItemBase)
            {
                var chambersPtr = Memory.ReadValue<ulong>(lootItemBase + Offsets.LootItemWeapon.Chambers);
                ulong firstRound;
                MemArray<Chamber> chambers = null;
                MemArray<Chamber> magChambers = null;
                MemList<ulong> magStack = null;
                try
                {
                    if (chambersPtr != 0x0 && (chambers = MemArray<Chamber>.Get(chambersPtr)).Count > 0) // Single chamber, or for some shotguns, multiple chambers
                        firstRound = Memory.ReadPtr(chambers.First(x => x.HasBullet(true)) + Offsets.Slot.ContainedItem);
                    else
                    {
                        var magSlot = Memory.ReadPtr(lootItemBase + Offsets.LootItemWeapon._magSlotCache);
                        var magItemPtr = Memory.ReadPtr(magSlot + Offsets.Slot.ContainedItem);
                        var magChambersPtr = Memory.ReadPtr(magItemPtr + Offsets.LootItemMod.Slots);
                        magChambers = MemArray<Chamber>.Get(magChambersPtr);
                        if (magChambers.Count > 0) // Revolvers, etc.
                            firstRound = Memory.ReadPtr(magChambers.First(x => x.HasBullet(true)) + Offsets.Slot.ContainedItem);
                        else // Regular magazines
                        {
                            var cartridges = Memory.ReadPtr(magItemPtr + Offsets.LootItemMagazine.Cartridges);
                            var magStackPtr = Memory.ReadPtr(cartridges + Offsets.StackSlot._items);
                            magStack = MemList<ulong>.Get(magStackPtr);
                            firstRound = magStack[0];
                        }
                    }
                    return Memory.ReadPtr(firstRound + Offsets.LootItem.Template);
                }
                finally
                {
                    chambers?.Dispose();
                    magChambers?.Dispose();
                    magStack?.Dispose();
                }
            }

            /// <summary>
            /// Wrapper defining a Chamber Structure.
            /// </summary>
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private readonly struct Chamber
            {
                public static implicit operator ulong(Chamber x) => x._base;
                private readonly ulong _base;

                public readonly bool HasBullet(bool useCache = false)
                {
                    if (_base == 0x0)
                        return false;
                    return Memory.ReadValue<ulong>(_base + Offsets.Slot.ContainedItem, useCache) != 0x0;
                }
            }

            private enum EFireMode : byte
            {
                // Token: 0x0400B0EE RID: 45294
                [Description(nameof(Auto))]
                Auto = 0,
                // Token: 0x0400B0EF RID: 45295
                [Description(nameof(Single))]
                Single = 1,
                // Token: 0x0400B0F0 RID: 45296
                [Description(nameof(DbTap))]
                DbTap = 2,
                // Token: 0x0400B0F1 RID: 45297
                [Description(nameof(Burst))]
                Burst = 3,
                // Token: 0x0400B0F2 RID: 45298
                [Description(nameof(DbAction))]
                DbAction = 4,
                // Token: 0x0400B0F3 RID: 45299
                [Description(nameof(SemiAuto))]
                SemiAuto = 5
            }
        }

        #endregion

        #region Hands Cache

        public sealed class CachedHandsInfo
        {
            public static implicit operator ulong(CachedHandsInfo x) => x?._hands ?? 0x0;

            private readonly ulong _hands;
            private readonly TarkovMarketItem _item;
            /// <summary>
            /// Address of currently held item (if any).
            /// </summary>
            public ulong ItemAddr { get; }
            /// <summary>
            /// True if the Item being currently held (if any) is a weapon, otherwise False.
            /// </summary>
            public bool IsWeapon => _item?.IsWeapon ?? false;

            public CachedHandsInfo(ulong handsController)
            {
                _hands = handsController;
            }

            public CachedHandsInfo(ulong handsController, TarkovMarketItem item, ulong itemAddr)
            {
                _hands = handsController;
                _item = item;
                ItemAddr = itemAddr;
            }
        }

        #endregion
    }
}

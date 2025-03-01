using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Unity.Collections;

namespace arena_dma_radar.Arena.ArenaPlayer.Plugins
{
    public sealed class HandsManager
    {
        private readonly ArenaObservedPlayer _parent;

        private volatile string _ammo;
        private volatile string _thermal;
        private volatile TarkovMarketItem _cachedItem;
        private ulong _cached = 0x0;

        /// <summary>
        /// Entity currently in Player's Hands.
        /// </summary>
        public string InHands
        {
            get
            {
                var at = $"{_ammo} {_thermal}".Trim();
                var item = _cachedItem?.ShortName;
                if (item is null) return "--";
                if (at != string.Empty)
                    return $"{item} ({at})";
                else
                    return item;
            }
        }

        public HandsManager(ArenaObservedPlayer player)
        {
            _parent = player;
        }

        /// <summary>
        /// Refresh hands data.
        /// </summary>
        public void Refresh()
        {
            try
            {
                var handsController = Memory.ReadPtr(_parent.HandsControllerAddr); // or FirearmController
                var itemBase = Memory.ReadPtr(handsController + Offsets.ObservedHandsController.ItemInHands);
                if (itemBase != _cached)
                {
                    _cachedItem = null;
                    _ammo = null;
                    _thermal = null;
                    var itemTemplate = Memory.ReadPtr(itemBase + Offsets.LootItem.Template);
                    var itemIDPtr = Memory.ReadValue<Types.MongoID>(itemTemplate + Offsets.ItemTemplate._id);
                    var itemID = Memory.ReadUnityString(itemIDPtr.StringID);
                    if (EftDataManager.AllItems.TryGetValue(itemID, out var heldItem)) // Item exists in DB
                        _cachedItem = heldItem;
                    _cached = itemBase;
                }
                if (_cachedItem?.IsWeapon ?? false)
                {
                    try
                    {
                        var chambers = Memory.ReadPtr(itemBase + Offsets.LootItemWeapon.Chambers);
                        var slotPtr = Memory.ReadPtr(chambers + MemList<byte>.ArrStartOffset + 0 * 0x8); // One in the chamber ;)
                        var slotItem = Memory.ReadPtr(slotPtr + Offsets.Slot.ContainedItem);
                        var ammoTemplate = Memory.ReadPtr(slotItem + Offsets.LootItem.Template);
                        var idPtr = Memory.ReadValue<Types.MongoID>(ammoTemplate + Offsets.ItemTemplate._id);
                        string id = Memory.ReadUnityString(idPtr.StringID);
                        if (EftDataManager.AllItems.TryGetValue(id, out var ammo))
                            _ammo = ammo?.ShortName;
                    }
                    catch { }
                }
            }
            catch
            {
                _cached = 0x0;
            }
        }
    }
}

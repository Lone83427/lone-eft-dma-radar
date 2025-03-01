using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Unity.Collections;
using System.Collections.Frozen;

namespace arena_dma_radar.Arena.ArenaPlayer.Plugins
{
    public sealed class GearManager
    {
        private static readonly FrozenSet<string> _skipSlots = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "SecuredContainer", "Dogtag", "Compass", "Eyewear", "ArmBand", "Scabbard"
        }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// List of equipped items in Player Inventory Slots.
        /// </summary>
        public IReadOnlyDictionary<string, string> Equipment { get; private set; }

        public GearManager(ArenaObservedPlayer player)
        {
            var inventorycontroller = Memory.ReadPtr(player.InventoryControllerAddr);
            var inventory = Memory.ReadPtr(inventorycontroller + Offsets.InventoryController.Inventory);
            var equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment);
            var slots = Memory.ReadPtr(equipment + Offsets.Equipment.Slots);
            GetGear(slots);
        }
        private void GetGear(ulong slotsPtr)
        {
            using var slotsArray = MemArray<ulong>.Get(slotsPtr);
            var gearDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var slot in slotsArray)
            {
                try
                {
                    var namePtr = Memory.ReadPtr(slot + Offsets.Slot.ID);
                    var name = Memory.ReadUnityString(namePtr);
                    if (_skipSlots.Contains(name))
                        continue;
                    var containedItem = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem);
                    var inventorytemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    var idPtr = Memory.ReadValue<Types.MongoID>(inventorytemplate + Offsets.ItemTemplate._id);
                    string id = Memory.ReadUnityString(idPtr.StringID);
                    if (EftDataManager.AllItems.TryGetValue(id, out var entry))
                        gearDict.TryAdd(name, entry.Name);
                }
                catch { } // Skip over empty slots
            }
            Equipment = gearDict; // update readonly ref
        }
    }
}
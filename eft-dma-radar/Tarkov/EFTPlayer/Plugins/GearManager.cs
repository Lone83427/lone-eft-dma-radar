using System.Collections.Frozen;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Unity.Collections;

namespace eft_dma_radar.Tarkov.EFTPlayer.Plugins
{
    public sealed class GearManager
    {
        private static readonly FrozenSet<string> _skipSlots = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "SecuredContainer", "Dogtag", "Compass", "ArmBand"
        }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

        private readonly bool _isPMC;

        public GearManager(Player player, bool isPMC = false)
        {
            _isPMC = isPMC;
            var slotDict = new Dictionary<string, ulong>(StringComparer.OrdinalIgnoreCase);
            var inventorycontroller = Memory.ReadPtr(player.InventoryControllerAddr);
            var inventory = Memory.ReadPtr(inventorycontroller + Offsets.InventoryController.Inventory);
            var equipment = Memory.ReadPtr(inventory + Offsets.Inventory.Equipment);
            var slots = Memory.ReadPtr(equipment + Offsets.Equipment.Slots);
            using var slotsArray = MemArray<ulong>.Get(slots);

            foreach (var slotPtr in slotsArray)
            {
                var namePtr = Memory.ReadPtr(slotPtr + Offsets.Slot.ID);
                var name = Memory.ReadUnityString(namePtr);
                if (_skipSlots.Contains(name))
                    continue;
                slotDict.TryAdd(name, slotPtr);
            }

            Slots = slotDict;
            Refresh();
        }

        private IReadOnlyDictionary<string, ulong> Slots { get; }

        /// <summary>
        /// List of equipped items in Player Inventory Slots.
        /// </summary>
        public IReadOnlyDictionary<string, GearItem> Equipment { get; private set; }

        /// <summary>
        /// Player's contained gear/loot.
        /// </summary>
        public IReadOnlyList<LootItem> Loot { get; private set; }

        /// <summary>
        /// True if Quest Items are contained in this loot pool.
        /// </summary>
        public bool HasQuestItems => Loot?.Any(x => x.IsQuestCondition) ?? false;

        /// <summary>
        /// Value of this player's Gear/Loot.
        /// </summary>
        public int Value { get; private set; }

        public void Refresh()
        {
            var loot = new List<LootItem>();
            var gearDict = new Dictionary<string, GearItem>(StringComparer.OrdinalIgnoreCase);
            foreach (var slot in Slots)
                try
                {
                    if (_isPMC && slot.Key == "Scabbard")
                        continue; // skip pmc scabbard
                    var containedItem = Memory.ReadPtr(slot.Value + Offsets.Slot.ContainedItem);
                    var inventorytemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                    var idPtr = Memory.ReadValue<Types.MongoID>(inventorytemplate + Offsets.ItemTemplate._id);
                    var id = Memory.ReadUnityString(idPtr.StringID);
                    if (EftDataManager.AllItems.TryGetValue(id, out var entry1))
                        loot.Add(new LootItem(entry1));
                    try // Get all items on player
                    {
                        var grids = Memory.ReadValue<ulong>(containedItem + Offsets.LootItemMod.Grids);
                        LootManager.GetItemsInGrid(grids, loot);
                    }
                    catch
                    {
                    }

                    if (EftDataManager.AllItems.TryGetValue(id, out var entry2))
                    {
                        if (slot.Key == "FirstPrimaryWeapon" || slot.Key == "SecondPrimaryWeapon" ||
                            slot.Key == "Headwear") // Only interested in weapons / helmets
                            try
                            {
                                RecursePlayerGearSlots(containedItem, loot);
                            }
                            catch
                            {
                            }

                        var gear = new GearItem
                        {
                            Long = entry2.Name ?? "None",
                            Short = entry2.ShortName ?? "None"
                        };
                        gearDict.TryAdd(slot.Key, gear);
                    }
                }
                catch
                {
                } // Skip over empty slots

            Loot = loot.OrderLoot().ToList();
            Value = loot.Sum(x => x.Price); // Get value of player's loot/gear
            Equipment = gearDict; // update readonly ref
        }

        /// <summary>
        /// Checks a 'Primary' weapon for Ammo Type, and Thermal Scope.
        /// </summary>
        private static void RecursePlayerGearSlots(ulong lootItemBase, List<LootItem> loot)
        {
            try
            {
                var parentSlots = Memory.ReadPtr(lootItemBase + Offsets.LootItemMod.Slots);
                using var slotsArray = MemArray<ulong>.Get(parentSlots);
                var slotDict = new Dictionary<string, ulong>(StringComparer.OrdinalIgnoreCase);

                foreach (var slotPtr in slotsArray)
                {
                    var namePtr = Memory.ReadPtr(slotPtr + Offsets.Slot.ID);
                    var name = Memory.ReadUnityString(namePtr);
                    slotDict.TryAdd(name, slotPtr);
                }

                foreach (var slotName in slotDict.Keys)
                    try
                    {
                        if (slotDict.TryGetValue(slotName, out var slot))
                        {
                            var containedItem = Memory.ReadPtr(slot + Offsets.Slot.ContainedItem);
                            var inventorytemplate = Memory.ReadPtr(containedItem + Offsets.LootItem.Template);
                            var idPtr = Memory.ReadValue<Types.MongoID>(inventorytemplate + Offsets.ItemTemplate._id);
                            var id = Memory.ReadUnityString(idPtr.StringID);
                            if (EftDataManager.AllItems.TryGetValue(id, out var entry))
                                loot.Add(new LootItem(entry)); // Add to loot, get weapon attachment values
                            RecursePlayerGearSlots(containedItem, loot);
                        }
                    }
                    catch
                    {
                    } // Skip over empty slots
            }
            catch
            {
            }
        }
    }
}
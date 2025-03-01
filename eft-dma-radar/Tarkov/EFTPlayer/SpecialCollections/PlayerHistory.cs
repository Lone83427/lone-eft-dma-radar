using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.Radar;

namespace eft_dma_radar.Tarkov.EFTPlayer.SpecialCollections
{
    /// <summary>
    /// Wrapper class to manage Player History.
    /// Thread Safe.
    /// </summary>
    public sealed class PlayerHistory
    {
        private readonly HashSet<ulong> _logged = new();
        private readonly BindingList<PlayerHistoryEntry> _entries = new();

        /// <summary>
        /// Adds an entry to the Player History.
        /// </summary>
        /// <param name="player">Player to add/update.</param>
        public void AddOrUpdate(Player player)
        {
            try
            {
                if (player.IsHumanOther) /// Log to Player History
                {
                    var entry = new PlayerHistoryEntry(player);
                    MainForm.Window?.Invoke(() =>
                    {
                        if (_logged.Add(player)) // Add 
                        {
                            _entries.Insert(0, entry);
                            _entries.ResetBindings();
                        }
                        else // Update
                        {
                            var oldEntry = _entries.FirstOrDefault(x => (ulong)x.Player == (ulong)player);
                            if (oldEntry is not null)
                            {
                                int index = _entries.IndexOf(oldEntry);
                                _entries[index] = entry;
                                _entries.ResetBindings();
                            }
                        }
                    });
                }
            }
            catch { }
        }

        /// <summary>
        /// Resets the Player History state for a new raid.
        /// Does not clear existing entries, but clears the HashSet that prevents players from being added multiple times.
        /// </summary>
        public void Reset()
        {
            MainForm.Window?.Invoke(() =>
            {
                _logged.Clear();
            });
        }

        /// <summary>
        /// Get a reference to the backing collection.
        /// UNSAFE! Should only be done for binding purposes.
        /// </summary>
        /// <returns>List reference.</returns>
        public BindingList<PlayerHistoryEntry> GetReferenceUnsafe() => _entries;
    }
}

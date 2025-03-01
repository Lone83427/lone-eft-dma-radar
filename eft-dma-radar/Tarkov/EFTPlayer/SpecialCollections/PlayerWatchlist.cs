using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.Radar;

namespace eft_dma_radar.Tarkov.EFTPlayer.SpecialCollections
{
    /// <summary>
    /// Wrapper class to manage Player Watchlist.
    /// Thread Safe.
    /// </summary>
    public sealed class PlayerWatchlist
    {
        private readonly SortableBindingList<PlayerWatchlistEntry> _bindingList;

        /// <summary>
        /// Entries in this watchlist.
        /// </summary>
        public IReadOnlyDictionary<string, PlayerWatchlistEntry> Entries { get; private set; } = new Dictionary<string, PlayerWatchlistEntry>();

        /// <summary>
        /// Singleton Constructor.
        /// Only construct once!
        /// </summary>
        public PlayerWatchlist()
        {
            Program.Config.PlayerWatchlist = Program.Config.PlayerWatchlist
                .Where(x => x is not null && !string.IsNullOrWhiteSpace(x.AcctID))
                .OrderBy(x => x.Timestamp.Ticks)
                .DistinctBy(x => x.AcctID)
                .ToList(); // Sanitize List
            _bindingList = new(Program.Config.PlayerWatchlist);
            Update();
            _bindingList.ListChanged += Entries_ListChanged;
        }

        private void Entries_ListChanged(object sender, ListChangedEventArgs e) => Update();

        /// <summary>
        /// Update the backing dictionary to reflect the BindingList.
        /// </summary>
        private void Update()
        {
            this.Entries = _bindingList
                .DistinctBy(x => x.AcctID)
                .ToDictionary(
                    item => item.AcctID, // key
                    item => item); // value
        }

        /// <summary>
        /// Manually add an entry into the Player Watchlist.
        /// Will be invoked on the UI Thread.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        public void ManualAdd(PlayerWatchlistEntry entry)
        {
            MainForm.Window?.Invoke(() =>
            {
                _bindingList.Add(entry);
            });
        }

        /// <summary>
        /// Get a reference to the backing collection.
        /// UNSAFE! Should only be done for binding purposes.
        /// </summary>
        /// <returns>BindingList reference.</returns>
        internal SortableBindingList<PlayerWatchlistEntry> GetReferenceUnsafe() => _bindingList;
    }
}

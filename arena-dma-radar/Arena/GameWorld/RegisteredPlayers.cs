using arena_dma_radar.Arena.ArenaPlayer;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity.Collections;
using arena_dma_radar.Arena.Features;
using arena_dma_radar.Arena.Features.MemoryWrites;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.GameWorld
{
    public sealed class RegisteredPlayers : IReadOnlyCollection<Player>
    {
        public static implicit operator ulong(RegisteredPlayers x) => x.Base;
        private ulong Base { get; }
        private readonly LocalGameWorld _game;
        private readonly ConcurrentDictionary<ulong, Player> _players = new();

        /// <summary>
        /// LocalPlayer Instance.
        /// </summary>
        public LocalPlayer LocalPlayer { get; private set; }

        /// <summary>
        /// RegisteredPlayers List Constructor.
        /// </summary>
        public RegisteredPlayers(ulong baseAddr, LocalGameWorld game)
        {
            Base = baseAddr;
            _game = game;
            var mainPlayer = Memory.ReadPtr(game + Offsets.ClientLocalGameWorld.MainPlayer, false);
            var localPlayer = new LocalPlayer(mainPlayer);
            _players[localPlayer] = LocalPlayer = localPlayer;
        }

        /// <summary>
        /// Updates the ConcurrentDictionary of 'Players'
        /// </summary>
        public void Refresh()
        {
            try
            {
                var mainPlayer = Memory.ReadPtr(_game + Offsets.ClientLocalGameWorld.MainPlayer);
                if (mainPlayer != LocalPlayer)
                {
                    lock (MemWrites.SyncRoot) // Prevent race conditions with DMA Toolkit
                    lock (Aimbot.SyncRoot) // and Aimbot
                    {
                        LoneLogging.WriteLine("Re-Allocating LocalPlayer (Memory Address Changed)");
                        var localPlayer = new LocalPlayer(mainPlayer);
                        _players.Clear();
                        _players[localPlayer] = LocalPlayer = localPlayer;
                    }
                }
                ArgumentNullException.ThrowIfNull(LocalPlayer, nameof(LocalPlayer));
                using var playersList = MemList<ulong>.Get(this, false); // Realtime Read
                var registered = playersList.Where(x => x != 0x0).ToHashSet();
                /// Allocate New Players
                foreach (var playerBase in registered)
                {
                    if (playerBase == LocalPlayer) // Skip LocalPlayer, already allocated
                        continue;
                    if (_players.TryGetValue(playerBase, out var existingPlayer)) // Player already exists, check for problems
                    {
                        if (existingPlayer.ErrorTimer.ElapsedMilliseconds >= 1500) // Erroring out a lot? Re-Alloc
                        {
                            LoneLogging.WriteLine($"WARNING - Existing player '{existingPlayer.Name}' being re-allocated due to excessive errors...");
                            Player.Allocate(_players, playerBase, existingPlayer.Position);
                        }
                        // Nothing else needs to happen here
                    }
                    else // Add New Player
                        Player.Allocate(_players, playerBase);
                }
                /// Update Existing Players
                UpdateExistingPlayers(registered);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR - RegisteredPlayers Loop FAILED: {ex}");
            }
        }

        /// <summary>
        /// Returns the Player Count currently in the Registered Players List.
        /// </summary>
        /// <returns>Count of players.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetPlayerCount()
        {
            var count = Memory.ReadValue<int>(this + MemList<byte>.CountOffset, false);
            if (count < 0 || count > 128)
                throw new ArgumentOutOfRangeException(nameof(count));
            return count;
        }

        /// <summary>
        /// Scans the existing player list and updates Players as needed.
        /// </summary>
        private void UpdateExistingPlayers(IReadOnlySet<ulong> registered)
        {
            var allPlayers = _players.Values;
            if (allPlayers.Count == 0)
                return;
            using var map = ScatterReadMap.Get();
            var round1 = map.AddRound(false);
            int i = 0;
            foreach (var player in allPlayers)
            {
                player.OnRegRefresh(round1[i++], registered);
            }
            map.Execute();
        }

        #region IReadOnlyCollection
        public int Count => _players.Values.Count;
        public IEnumerator<Player> GetEnumerator() => _players.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
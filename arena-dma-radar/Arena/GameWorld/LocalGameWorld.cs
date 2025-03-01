using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.UI.Radar;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.DMA.ScatterAPI;
using eft_dma_shared.Common.Unity;
using arena_dma_radar.Arena.Features.MemoryWrites;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.GameWorld
{
    public sealed class LocalGameWorld : IDisposable
    {
        #region Fields/Properties/Constructor(s)

        public static implicit operator ulong(LocalGameWorld x) => x.Base;
        /// <summary>
        /// Maximum distance for rendering entities.
        /// </summary>
        public const int MAX_DIST = 500;

        /// <summary>
        /// LocalGameWorld Address.
        /// </summary>
        private ulong Base { get; }

        private static readonly WaitTimer _refreshWait = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly RegisteredPlayers _rgtPlayers;
        private readonly GrenadeManager _grenadeManager;
        private readonly Thread _t1;
        private readonly Thread _t2;
        private readonly Thread _t3;
        private readonly Thread _t4;

        /// <summary>
        /// Current Game Instance Mode.
        /// </summary>
        public static Enums.ERaidMode MatchMode { get; private set; }
        /// <summary>
        /// True if the current game mode has teams (armbands).
        /// </summary>
        public static bool MatchHasTeams
        {
            get
            {
                switch (MatchMode)
                {
                    case Enums.ERaidMode.LastHero:
                        return false;
                    default:
                        return true;
                }
            }
        }

        /// <summary>
        /// True if match instance is still active, and safe to Write Memory.
        /// </summary>
        public bool IsSafeToWriteMem
        {
            get
            {
                try
                {
                    if (MainForm.Window is null || !InRaid)
                        return false;
                    if (Memory.ReadValue<ulong>(LocalPlayer + Offsets.Player.Corpse, false) != 0x0)
                        return false;
                    return IsGameWorldActive();
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Map ID of Current Map.
        /// </summary>
        public string MapID { get; private set; }

        /// <summary>
        /// True if LocalPlayer is in Raid.
        /// </summary>
        public bool InRaid => !_disposed;

        public CameraManager CameraManager { get; }
        public IReadOnlyCollection<Player> Players => _rgtPlayers;
        public LocalPlayer LocalPlayer => _rgtPlayers?.LocalPlayer;
        public IReadOnlyCollection<Grenade> Grenades => _grenadeManager;

        /// <summary>
        /// Game Constructor.
        /// Only called internally.
        /// </summary>
        private LocalGameWorld(ulong localGameWorld, string mapID)
        {
            var ct = _cts.Token;
            Base = localGameWorld;
            MapID = mapID;
            _t1 = new Thread(() => { RealtimeWorker(ct); })
            {
                IsBackground = true
            };
            _t2 = new Thread(() => { MiscWorker(ct); })
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            _t3 = new Thread(() => { GrenadesWorker(ct); })
            {
                IsBackground = true
            };
            _t4 = new Thread(() => { FastWorker(ct); })
            {
                IsBackground = true
            };
            Player.Reset();
            var rgtPlayersAddr = Memory.ReadPtr(localGameWorld + Offsets.ClientLocalGameWorld.RegisteredPlayers, false);
            _rgtPlayers = new RegisteredPlayers(rgtPlayersAddr, this);
            if (_rgtPlayers.GetPlayerCount() < 1)
                throw new ArgumentOutOfRangeException(nameof(_rgtPlayers));
            CameraManager = new();
            _grenadeManager = new(localGameWorld);
        }

        /// <summary>
        /// Start all Game Threads.
        /// </summary>
        public void Start()
        {
            _t1.Start();
            _t2.Start();
            _t3.Start();
            _t4.Start();
        }

        /// <summary>
        /// Blocks until a LocalGameWorld Singleton Instance can be instantiated.
        /// </summary>
        /// <param name="unityBase">Module Base of UnityPlayer.dll</param>
        public static LocalGameWorld CreateGameInstance(ulong unityBase)
        {
            while (true)
            {
                ResourceJanitor.Run();
                Memory.ThrowIfNotInGame();
                try
                {
                    var instance = GetLocalGameWorld(unityBase);
                    LoneLogging.WriteLine("Match has started!");
                    return instance;
                }
                catch (Exception ex)
                {
                    LoneLogging.WriteLine($"ERROR Instantiating Game Instance: {ex}");
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a Raid has started.
        /// Loads Local Game World resources.
        /// </summary>
        /// <returns>True if Raid has started, otherwise False.</returns>
        private static LocalGameWorld GetLocalGameWorld(ulong unityBase)
        {
            try
            {
                /// Get LocalGameWorld
                var localGameWorld = Memory.ReadPtr(MonoLib.GameWorldField, false); // Game world >> Local Game World
                /// Get Selected Map
                var mapPtr = Memory.ReadValue<ulong>(localGameWorld + Offsets.GameWorld.Location, false);
                var map = Memory.ReadUnityString(mapPtr, 64, false);
                LoneLogging.WriteLine("Detected Map " + map);
                if (!GameData.MapNames.ContainsKey(map))
                    throw new Exception("Invalid Map ID!");
                /// Get Raid Instance / Players List
                var inMatch = Memory.ReadValue<bool>(localGameWorld + Offsets.ClientLocalGameWorld.IsInRaid, false);
                if (!inMatch)
                    throw new Exception("Invalid Match Instance (Hideout?)");
                var networkGame = Memory.ReadPtr(MonoLib.AbstractGameField, false);
                var networkGameData = Memory.ReadPtr(networkGame + Offsets.NetworkGame.NetworkGameData, false);
                var raidMode = Memory.ReadValue<int>(networkGameData + Offsets.NetworkGameData.raidMode, false);
                if (raidMode is < 0 or > 20)
                    throw new ArgumentOutOfRangeException(nameof(raidMode));
                MatchMode = (Enums.ERaidMode)raidMode;
                return new LocalGameWorld(localGameWorld, map);
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Getting LocalGameWorld", ex);
            }
        }

        /// <summary>
        /// Main Game Loop executed by Memory Worker Thread. Updates Player List and performs Player Allocations.
        /// </summary>
        public void Refresh() // primary memory thread (t0)
        {
            try
            {
                ThrowIfMatchEnded();
                _rgtPlayers.Refresh();
            }
            catch (RaidEnded)
            {
                LoneLogging.WriteLine("Match has ended!");
                Dispose();
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR - Match ended due to unhandled exception: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Throws an exception if the current match instance has ended.
        /// </summary>
        /// <exception cref="RaidEnded"></exception>
        private void ThrowIfMatchEnded()
        {
            for (int i = 0; i < 5; i++) // Re-attempt if read fails -- 5 times
            {
                try
                {
                    if (_rgtPlayers.GetPlayerCount() < 1)
                        throw new Exception("Not in match!");
                    return;
                }
                catch { Thread.Sleep(10); } // short delay between read attempts
            }
            throw new RaidEnded(); // Still not valid? Match must have ended.
        }

        /// <summary>
        /// Checks if the Current Raid is Active, and LocalPlayer is alive/active.
        /// </summary>
        /// <returns>True if raid is active, otherwise False.</returns>
        private bool IsGameWorldActive()
        {
            try
            {
                var localGameWorld = Memory.ReadPtr(MonoLib.GameWorldField, false);
                ArgumentOutOfRangeException.ThrowIfNotEqual(localGameWorld, this, nameof(localGameWorld));
                var mainPlayer = Memory.ReadPtr(localGameWorld + Offsets.ClientLocalGameWorld.MainPlayer, false);
                ArgumentOutOfRangeException.ThrowIfNotEqual(mainPlayer, _rgtPlayers.LocalPlayer, nameof(mainPlayer));
                return _rgtPlayers.GetPlayerCount() > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Realtime Thread T1

        /// <summary>
        /// Managed Thread that does realtime player position/info updates.
        /// </summary>
        private void RealtimeWorker(CancellationToken ct) // t1
        {
            if (_disposed) return;
            try
            {
                LoneLogging.WriteLine("Realtime thread starting...");
                while (InRaid)
                {
                    if (Program.Config.RatelimitRealtimeReads || !CameraManagerBase.EspRunning || (MemWriteFeature<Aimbot>.Instance.Enabled && Aimbot.Engaged))
                        _refreshWait.AutoWait(TimeSpan.FromMilliseconds(1), 1000);
                    ct.ThrowIfCancellationRequested();
                    RealtimeLoop(); // Realtime update loop (player positions, etc.)
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR on Realtime Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                LoneLogging.WriteLine("Realtime thread stopping...");
            }
        }

        /// <summary>
        /// Updates all Realtime Values (View Matrix, player positions, etc.)
        /// </summary>
        public void RealtimeLoop()
        {
            try
            {
                var players = _rgtPlayers
                    .Where(x => x.IsActive && x.IsAlive);
                var localPlayer = LocalPlayer;
                if (!players.Any()) // No players - Throttle
                {
                    Thread.Sleep(1);
                    return;
                }

                using var scatterMap = ScatterReadMap.Get();
                var round1 = scatterMap.AddRound(false);
                if (CameraManager is CameraManager cm)
                {
                    cm.OnRealtimeLoop(round1[-1], localPlayer);
                }
                int i = 0;
                foreach (var player in players)
                {
                    player.OnRealtimeLoop(round1[i++]);
                }

                scatterMap.Execute(); // Execute scatter read
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR - UpdatePlayers Loop FAILED: {ex}");
            }
        }

        #endregion

        #region Misc Thread T2

        /// <summary>
        /// Managed Thread that does Misc. Local Game World Updates.
        /// </summary>
        private void MiscWorker(CancellationToken ct) // t2
        {
            if (_disposed) return;
            try
            {
                LoneLogging.WriteLine("Misc thread starting...");
                while (InRaid)
                {
                    ct.ThrowIfCancellationRequested();
                    UpdateMisc();
                    Thread.Sleep(250);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR on Misc Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                LoneLogging.WriteLine("Misc thread stopping...");
            }
        }

        /// <summary>
        /// Validates Player Transforms -> Refresh Gear
        /// </summary>
        private void UpdateMisc()
        {
            try
            {
                ValidatePlayerTransforms(); // Check for transform anomalies
                GetGear();
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[QuestManager] CRITICAL ERROR: {ex}");
            }
        }

        public void ValidatePlayerTransforms()
        {
            try
            {
                var players = _rgtPlayers
                    .Where(x => x.IsActive && x.IsAlive);
                if (players.Any()) // at least 1 player
                {
                    using var scatterMap = ScatterReadMap.Get();
                    var round1 = scatterMap.AddRound();
                    var round2 = scatterMap.AddRound();
                    int i = 0;
                    foreach (var player in players)
                    {
                        player.OnValidateTransforms(round1[i], round2[i]);
                        i++;
                    }
                    scatterMap.Execute(); // execute scatter read
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR - ValidatePlayerTransforms Loop FAILED: {ex}");
            }
        }

        // <summary>
        // Execute Gear Manager
        // </summary>
        private void GetGear()
        {
            try
            {
                var players = _rgtPlayers
                    .Where(x => x.IsHostileActive);
                if (players is not null && players.Any())
                    foreach (var player in players)
                        if (player is ArenaObservedPlayer observed)
                            observed.GetGear();
            }
            catch
            {
            }
        }

        #endregion

        #region Grenades Thread T3

        /// <summary>
        /// Managed Thread that does Grenade/Throwable updates.
        /// </summary>
        private void GrenadesWorker(CancellationToken ct) // t3
        {
            if (_disposed) return;
            try
            {
                LoneLogging.WriteLine("Grenades thread starting...");
                while (InRaid)
                {
                    ct.ThrowIfCancellationRequested();
                    _grenadeManager.Refresh();
                    Thread.Sleep(10);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR on Grenades Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                LoneLogging.WriteLine("Grenades thread stopping...");
            }
        }

        #endregion

        #region Fast Thread T4

        /// <summary>
        /// Managed Thread that does Rapid / DMA Toolkit updates.
        /// No long operations on this thread.
        /// </summary>
        private void FastWorker(CancellationToken ct) // t4
        {
            if (_disposed) return;
            try
            {
                LoneLogging.WriteLine("FastWorker thread starting...");
                while (InRaid)
                {
                    ct.ThrowIfCancellationRequested();
                    CameraManager.Refresh();
                    RefreshFast();
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"CRITICAL ERROR on FastWorker Thread: {ex}"); // Log CRITICAL error
                Dispose(); // Game object is in a corrupted state --> Dispose
            }
            finally
            {
                LoneLogging.WriteLine("FastWorker thread stopping...");
            }
        }

        // <summary>
        // Refresh various player items via Fast Worker Thread.
        // </summary>
        private void RefreshFast()
        {
            try
            {
                var players = _rgtPlayers
                    .Where(x => x.IsActive && x.IsAlive);
                if (players is not null && players.Any())
                    foreach (var player in players)
                        if (player is ArenaObservedPlayer observed)
                            observed.RefreshHands();
                        else if (player is LocalPlayer localPlayer)
                            localPlayer.Firearm.Update();
            }
            catch
            {
            }
        }

        #endregion

        #region IDisposable

        private bool _disposed = false;

        public void Dispose()
        {
            bool disposed = Interlocked.Exchange(ref _disposed, true);
            if (!disposed)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        #endregion

        #region Types

        public sealed class RaidEnded : Exception
        {
            public RaidEnded()
            {
            }

            public RaidEnded(string message)
                : base(message)
            {
            }

            public RaidEnded(string message, Exception inner)
                : base(message, inner)
            {
            }
        }

        #endregion
    }
}
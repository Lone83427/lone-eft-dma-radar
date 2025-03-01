using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Unity.Collections;

namespace eft_dma_radar.Tarkov.GameWorld.Explosives
{
    public sealed class ExplosivesManager : IReadOnlyCollection<IExplosiveItem>
    {
        private static readonly uint[] _toSyncObjects = new uint[] { Offsets.ClientLocalGameWorld.SynchronizableObjectLogicProcessor, Offsets.SynchronizableObjectLogicProcessor.SynchronizableObjects };
        private readonly ulong _localGameWorld;
        private readonly ConcurrentDictionary<ulong, IExplosiveItem> _explosives = new();
        private ulong _grenadesBase;

        public ExplosivesManager(ulong localGameWorld)
        {
            _localGameWorld = localGameWorld;
        }

        private void Init()
        {
            var grenadesPtr = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.Grenades, false);
            _grenadesBase = Memory.ReadPtr(grenadesPtr + 0x18, false);
        }

        /// <summary>
        /// Check for "hot" explosives in LocalGameWorld if due.
        /// </summary>
        public void Refresh()
        {
            foreach (var explosive in _explosives.Values)
            {
                try
                {
                    explosive.Refresh();
                }
                catch (Exception ex)
                {
                    LoneLogging.WriteLine($"Error Refreshing Explosive @ 0x{explosive.Addr.ToString("X")}: {ex}");
                }
            }
            GetGrenades();
            GetTripwires();
            GetMortarProjectiles();
        }

        private void GetGrenades()
        {
            try
            {
                if (_grenadesBase == 0x0)
                {
                    Init();
                }
                using var allGrenades = MemList<ulong>.Get(_grenadesBase, false);
                foreach (var grenadeAddr in allGrenades)
                {
                    try
                    {
                        if (!_explosives.ContainsKey(grenadeAddr))
                        {
                            var grenade = new Grenade(grenadeAddr, _explosives);
                            _explosives[grenade] = grenade;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoneLogging.WriteLine($"Error Processing Grenade @ 0x{grenadeAddr.ToString("X")}: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                _grenadesBase = 0x0;
                LoneLogging.WriteLine($"Grenades Error: {ex}");
            }
        }

        private void GetTripwires()
        {
            try
            {
                var syncObjectsPtr = Memory.ReadPtrChain(_localGameWorld, _toSyncObjects);
                using var syncObjects = MemList<ulong>.Get(syncObjectsPtr);
                foreach (var syncObject in syncObjects)
                {
                    try
                    {
                        var type = (Enums.SynchronizableObjectType)Memory.ReadValue<int>(syncObject + Offsets.SynchronizableObject.Type);
                        if (type is not Enums.SynchronizableObjectType.Tripwire)
                            continue;
                        if (!_explosives.ContainsKey(syncObject))
                        {
                            var tripwire = new Tripwire(syncObject);
                            _explosives[tripwire] = tripwire;
                        }
                    }
                    catch (Exception ex)
                    {
                        LoneLogging.WriteLine($"Error Processing SyncObject @ 0x{syncObject.ToString("X")}: {ex}");
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Sync Objects Error: {ex}");
            }
        }

        private void GetMortarProjectiles()
        {
            try
            {
                var clientShellingController = Memory.ReadValue<ulong>(_localGameWorld + Offsets.ClientLocalGameWorld.ClientShellingController);
                if (clientShellingController != 0x0)
                {
                    var activeProjectilesPtr = Memory.ReadValue<ulong>(clientShellingController + Offsets.ClientShellingController.ActiveClientProjectiles);
                    if (activeProjectilesPtr != 0x0)
                    {
                        using var activeProjectiles = MemDictionary<int, ulong>.Get(activeProjectilesPtr);
                        foreach (var activeProjectile in activeProjectiles)
                        {
                            if (activeProjectile.Value == 0x0)
                                continue;
                            try
                            {
                                if (!_explosives.ContainsKey(activeProjectile.Value))
                                {
                                    var mortarProjectile = new MortarProjectile(activeProjectile.Value, _explosives);
                                    _explosives[mortarProjectile] = mortarProjectile;
                                }
                            }
                            catch (Exception ex)
                            {
                                LoneLogging.WriteLine($"Error Processing Mortar Projectile @ 0x{activeProjectile.Value.ToString("X")}: {ex}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Mortar Projectiles Error: {ex}");
            }
        }

        #region IReadOnlyCollection

        public int Count => _explosives.Values.Count;
        public IEnumerator<IExplosiveItem> GetEnumerator() => _explosives.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
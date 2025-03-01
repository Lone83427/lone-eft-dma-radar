using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Radar;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.Collections;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.Arena.GameWorld
{
    public sealed class GrenadeManager : IReadOnlyCollection<Grenade>
    {
        private readonly ulong _localGameWorld;
        private readonly ConcurrentDictionary<ulong, Grenade> _grenades = new();

        public GrenadeManager(ulong localGameWorld)
        {
            _localGameWorld = localGameWorld;
        }

        /// <summary>
        /// Check for "hot" grenades in LocalGameWorld if due.
        /// </summary>
        public void Refresh()
        {
            try
            {
                var grenadesPtr = Memory.ReadPtr(_localGameWorld + Offsets.ClientLocalGameWorld.Grenades);
                var listBase = Memory.ReadPtr(grenadesPtr + 0x18);
                using var allGrenades = MemList<ulong>.Get(listBase, false);
                foreach (var grenadeAddr in allGrenades)
                    if (_grenades.TryGetValue(grenadeAddr, out var existing))
                    {
                        existing.UpdatePos();
                    }
                    else
                    {
                        var grenade = new Grenade(grenadeAddr);
                        _grenades.TryAdd(grenade, grenade);
                    }

                foreach (var grenade in _grenades.Values)
                    try
                    {
                        var isDetonated = Memory.ReadValue<bool>(grenade + Offsets.Grenade.IsDestroyed, false);
                        if (isDetonated)
                            _grenades.Remove(grenade, out var removed); // Doesn't work on smokes though
                    }
                    catch
                    {
                    }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"Grenade Manager ERROR: {ex}");
            }
        }

        #region IReadOnlyCollection

        public int Count => _grenades.Values.Count;
        public IEnumerator<Grenade> GetEnumerator() => _grenades.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }

    /// <summary>
    /// Represents a 'Hot' grenade in Local Game World.
    /// </summary>
    public sealed class Grenade : IWorldEntity, IMapEntity, IESPEntity
    {
        private static readonly uint[] _toPosChain =
            ObjectClass.To_GameObject.Concat(new uint[] { GameObject.ComponentsOffset, 0x8, 0x38 }).ToArray();

        private readonly Stopwatch _sw = new();

        public Grenade(ulong baseAddr)
        {
            Base = baseAddr;
            PosAddr = Memory.ReadPtrChain(baseAddr, _toPosChain, false);
            UpdatePos();
            _sw.Start();
        }

        /// <summary>
        /// Base Address of Grenade Object.
        /// </summary>
        private ulong Base { get; }

        /// <summary>
        /// Position Pointer for the Vector3 location of this object.
        /// </summary>
        private ulong PosAddr { get; }

        /// <summary>
        /// True if grenade is currently active.
        /// </summary>
        public bool IsActive => _sw.ElapsedMilliseconds < 12000;

        public static implicit operator ulong(Grenade x) => x.Base;

        /// <summary>
        /// Get the updated Position of this Grenade.
        /// </summary>
        public void UpdatePos()
        {
            if (!IsActive)
                return;
            Position = Memory.ReadValue<Vector3>(PosAddr + 0x90, false);
        }

        #region Interfaces

        private Vector3 _position;
        public ref Vector3 Position => ref _position;

        public void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            var circlePosition = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            var size = 5 * MainForm.UIScale;
            SKPaints.ShapeOutline.StrokeWidth = SKPaints.PaintExplosives.StrokeWidth + 2f * MainForm.UIScale;
            canvas.DrawCircle(circlePosition, size, SKPaints.ShapeOutline); // Draw outline
            canvas.DrawCircle(circlePosition, size, SKPaints.PaintExplosives); // draw LocalPlayer marker
        }

        public void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (!IsActive)
                return;
            if (Vector3.Distance(localPlayer.Position, _position) > ESP.Config.GrenadeDrawDistance)
                return;
            if (!CameraManagerBase.WorldToScreen(ref _position, out var scrPos))
                return;
            var circleRadius = 8f * ESP.Config.FontScale;
            canvas.DrawCircle(scrPos, circleRadius, SKPaints.PaintGrenadeESP);
        }

        #endregion
    }
}
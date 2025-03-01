using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.Tarkov.Loot
{
    public sealed class StaticLootContainer : LootContainer
    {
        private static readonly IReadOnlyList<LootItem> _defaultLoot = new List<LootItem>(1);

        public override string Name { get; } = "Container";
        public override string ID { get; }

        /// <summary>
        /// True if the container has been searched by LocalPlayer or another Networked Entity.
        /// </summary>
        public bool Searched { get; }

        public StaticLootContainer(string containerId, bool opened) : base(_defaultLoot)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(containerId, nameof(containerId));
            this.ID = containerId;
            this.Searched = opened;
            if (EftDataManager.AllContainers.TryGetValue(containerId, out var container))
            {
                this.Name = container.ShortName ?? "Container";
            }
        }

        public override void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            var dist = Vector3.Distance(localPlayer.Position, Position);

            if (dist > MainForm.Config.ContainerDrawDistance)
                return;
            var heightDiff = Position.Y - localPlayer.Position.Y;
            var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            MouseoverPosition = new Vector2(point.X, point.Y);
            SKPaints.ShapeOutline.StrokeWidth = 2f;
            if (heightDiff > 1.45) // loot is above player
            {
                using var path = point.GetUpArrow(4);
                canvas.DrawPath(path, SKPaints.ShapeOutline);
                canvas.DrawPath(path, SKPaints.PaintContainerLoot);
            }
            else if (heightDiff < -1.45) // loot is below player
            {
                using var path = point.GetDownArrow(4);
                canvas.DrawPath(path, SKPaints.ShapeOutline);
                canvas.DrawPath(path, SKPaints.PaintContainerLoot);
            }
            else // loot is level with player
            {
                var size = 4 * MainForm.UIScale;
                canvas.DrawCircle(point, size, SKPaints.ShapeOutline);
                canvas.DrawCircle(point, size, SKPaints.PaintContainerLoot);
            }
        }

        public override void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var dist = Vector3.Distance(localPlayer.Position, Position);

            if (dist > ESP.Config.ContainerDrawDistance)
                return;
            if (!CameraManagerBase.WorldToScreen(ref Position, out var scrPos))
                return;
            var boxHalf = 2.75f * ESP.Config.FontScale; // was 3.5f
            var showDist = ESP.Config.ShowDistances || dist <= 10f;
            var boxPt = new SKRect(scrPos.X - boxHalf, scrPos.Y + boxHalf,
                scrPos.X + boxHalf, scrPos.Y - boxHalf);
            canvas.DrawRect(boxPt, SKPaints.PaintContainerLootESP);
            var textPt = new SKPoint(scrPos.X,
                scrPos.Y + 16f * ESP.Config.FontScale);
            textPt.DrawESPText(canvas, this, localPlayer, showDist, SKPaints.TextContainerLootESP, this.Name);
        }

        public override void DrawMouseover(SKCanvas canvas, LoneMapParams mapParams, LocalPlayer localPlayer)
        {
            var lines = new List<string>()
            {
                this.Name
            };
            Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, lines);
        }
    }
}

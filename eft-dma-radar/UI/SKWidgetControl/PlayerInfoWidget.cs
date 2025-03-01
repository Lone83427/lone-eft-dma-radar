using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Data;

namespace eft_dma_radar.UI.SKWidgetControl
{
    public sealed class PlayerInfoWidget : SKWidget
    {
        /// <summary>
        /// Constructs a Player Info Overlay.
        /// </summary>
        public PlayerInfoWidget(SKGLControl parent, SKRect location, bool minimized, float scale)
            : base(parent, "Player Info", new SKPoint(location.Left, location.Top),
                new SKSize(location.Width, location.Height), scale, false)
        {
            Minimized = minimized;
            SetScaleFactor(scale);
        }

        internal static SKPaint TextPlayersOverlay { get; } = new()
        {
            SubpixelText = true,
            Color = SKColors.White,
            IsStroke = false,
            TextSize = 12,
            TextEncoding = SKTextEncoding.Utf8,
            IsAntialias = true,
            Typeface = SKTypeface.FromFamilyName("Consolas"), // Do NOT change this font
            FilterQuality = SKFilterQuality.High
        };

        public void Draw(SKCanvas canvas, Player localPlayer, IEnumerable<Player> players)
        {
            if (Minimized)
            {
                Draw(canvas);
                return;
            }

            var localPlayerPos = localPlayer.Position;
            var hostileCount = players.Count(x => x.IsHostileActive);
            var filteredPlayers = players.Where(x => x.IsHumanHostileActive)
                .OrderBy(x => Vector3.Distance(localPlayerPos, x.Position));
            var sb = new StringBuilder();
            sb.AppendFormat("{0,-21}", "Fac / Lvl / Name")
                .AppendFormat("{0,-5}", "Acct")
                .AppendFormat("{0,-7}", "K/D")
                .AppendFormat("{0,-7}", "Hours")
                .AppendFormat("{0,-6}", "Raids")
                .AppendFormat("{0,-6}", "S/R%")
                .AppendFormat("{0,-5}", "Grp")
                .AppendFormat("{0,-8}", "Value")
                .AppendFormat("{0,-30}", "In Hands")
                .AppendFormat("{0,-5}", "Dist")
                .AppendLine();
            foreach (var player in filteredPlayers)
            {
                var name = MainForm.Config.HideNames && player.IsHuman ? "<Hidden>" : player.Name;
                var faction = player.PlayerSide.GetDescription()[0];
                var hands = player.Hands?.CurrentItem;
                var inHands = hands is not null ? hands : "--";
                string edition = "--";
                string level = "0";
                string kd = "--";
                string raidCount = "--";
                string survivePercent = "--";
                string hours = "--";
                if (player is ObservedPlayer observed)
                {
                    edition = observed.Profile?.Acct;
                    if (observed.Profile?.Level is int levelResult)
                        level = levelResult.ToString();
                    if (observed.Profile?.Overall_KD is float kdResult)
                        kd = kdResult.ToString("n2");
                    if (observed.Profile?.RaidCount is int raidCountResult)
                        raidCount = raidCountResult.ToString();
                    if (observed.Profile?.SurvivedRate is float survivedResult)
                        survivePercent = survivedResult.ToString("n1");
                    if (observed.Profile?.Hours is int hoursResult)
                        hours = hoursResult.ToString();
                }
                var grp = player.GroupID != -1 ? player.GroupID.ToString() : "--";
                var focused = player.IsFocused ? "*" : null;
                sb.AppendFormat("{0,-21}", $"{focused}{faction}{level}:{name}");
                sb.AppendFormat("{0,-5}", edition)
                    .AppendFormat("{0,-7}", kd)
                    .AppendFormat("{0,-7}", hours)
                    .AppendFormat("{0,-6}", raidCount)
                    .AppendFormat("{0,-6}", survivePercent)
                    .AppendFormat("{0,-5}", grp)
                    .AppendFormat("{0,-8}", $"{TarkovMarketItem.FormatPrice(player.Gear?.Value ?? 0)}")
                    .AppendFormat("{0,-30}", $"{inHands}")
                    .AppendFormat("{0,-5}", $"{(int)Math.Round(Vector3.Distance(localPlayerPos, player.Position))}")
                    .AppendLine();
            }

            var data = sb.ToString().Split(Environment.NewLine);
            var lineSpacing = TextPlayersOverlay.FontSpacing;
            var maxLength = data.Max(x => TextPlayersOverlay.MeasureText(x));
            var pad = 2.5f * ScaleFactor;
            Size = new SKSize(maxLength + pad, data.Length * lineSpacing + pad);
            Location = Location; // Bounds check
            Draw(canvas); // Draw backer
            var drawPt = new SKPoint(ClientRectangle.Left + pad, ClientRectangle.Top + lineSpacing / 2 + pad);
            canvas.DrawText($"Hostile Count: {hostileCount}", drawPt, TextPlayersOverlay); // draw line text
            drawPt.Y += lineSpacing;
            foreach (var line in data) // Draw tooltip text
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;
                canvas.DrawText(line, drawPt, TextPlayersOverlay); // draw line text
                drawPt.Y += lineSpacing;
            }
        }

        public override void SetScaleFactor(float newScale)
        {
            base.SetScaleFactor(newScale);
            TextPlayersOverlay.TextSize = 12 * newScale;
        }
    }
}
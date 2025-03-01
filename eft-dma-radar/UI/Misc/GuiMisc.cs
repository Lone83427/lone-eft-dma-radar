using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.UI.Misc
{
    /// <summary>
    /// Contains long/short names for player gear.
    /// </summary>
    public sealed class GearItem
    {
        public string Long { get; init; }
        public string Short { get; init; }
    }

    /// <summary>
    /// Represents a PMC in the PMC History log.
    /// </summary>
    public sealed class PlayerHistoryEntry
    {
        private readonly Player _player;

        /// <summary>
        /// The Player Object that this entry is bound to.
        /// </summary>
        public Player Player => _player;

        public string Name => _player.Name;
        public string ID => _player.AccountID;
        public string Acct
        {
            get
            {
                if (_player is ObservedPlayer observed)
                    return observed.Profile?.Acct;
                return "--";
            }
        }
        public string Type =>
            $"{_player.Type.GetDescription()} {_player.PlayerSide.GetDescription()}";
        public string KD
        {
            get
            {
                if (_player is ObservedPlayer observed && observed.Profile?.Overall_KD is float kd)
                    return kd.ToString("n2");
                return "--";
            }
        }
        public string Hours
        {
            get
            {
                if (_player is ObservedPlayer observed && observed.Profile?.Hours is int hours)
                    return hours.ToString();
                return "--";
            }
        }
        public string Raids
        {
            get
            {
                if (_player is ObservedPlayer observed && observed.Profile?.RaidCount is int raidCount)
                    return raidCount.ToString();
                return "--";
            }
        }
        public string SR
        {
            get
            {
                if (_player is ObservedPlayer observed && observed.Profile?.SurvivedRate is float sr)
                    return sr.ToString("n1");
                return "--";
            }
        }
        public string Group => _player.GroupID != -1 ? _player.GroupID.ToString() : "--";
        public string Alerts => _player.Alerts;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="player">Player Object to bind to.</param>
        public PlayerHistoryEntry(Player player)
        {
            ArgumentNullException.ThrowIfNull(player, nameof(player));
            _player = player;
        }
    }

    /// <summary>
    /// JSON Wrapper for Player Watchlist.
    /// </summary>
    public sealed class PlayerWatchlistEntry
    {
        /// <summary>
        /// Player's Account ID as obtained from Player History.
        /// </summary>
        [JsonPropertyName("acctID")]
        public string AcctID { get; set; } = string.Empty;
        /// <summary>
        /// Reason for adding player to Watchlist (ex: Cheater, streamer name,etc.)
        /// </summary>
        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp when the entry was originally added.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; init; } = DateTime.Now;
    }

    public sealed class ScreenEntry
    {
        private readonly int _screenNumber;

        /// <summary>
        /// Screen Index Number.
        /// </summary>
        public int ScreenNumber => _screenNumber;

        public ScreenEntry(int screenNumber)
        {
            _screenNumber = screenNumber;
        }

        public override string ToString() => $"Screen {_screenNumber}";
    }

    public enum HotkeyMode : int
    {
        [Description("Hold")]
        /// <summary>
        /// Continuous Hold the hotkey.
        /// </summary>
        Hold = 1,
        [Description("Toggle")]
        /// <summary>
        /// Toggle hotkey on/off.
        /// </summary>
        Toggle = 2
    }

    public sealed class BonesListItem
    {
        public string Name { get; }
        public Bones Bone { get; }
        public BonesListItem(Bones bone)
        {
            Name = bone.GetDescription();
            Bone = bone;
        }
        public override string ToString() => Name;
    }

    public sealed class HotkeyModeListItem
    {
        public string Name { get; }
        public HotkeyMode Mode { get; }
        public HotkeyModeListItem(HotkeyMode mode)
        {
            Name = mode.GetDescription();
            Mode = mode;
        }
        public override string ToString() => Name;
    }

    public sealed class QuestListItem
    {
        public string Id { get; }
        public string Name { get; }
        public QuestListItem(string id)
        {
            Id = id;
            if (EftDataManager.TaskData.TryGetValue(id, out var task))
            {
                Name = task.Name ?? id;
            }
            else
            {
                Name = id;
            }
        }
        public override string ToString() => Name;
    }

    public enum LootPriceMode : int
    {
        /// <summary>
        /// Optimal Flea Price.
        /// </summary>
        FleaMarket = 0,
        /// <summary>
        /// Highest Trader Price.
        /// </summary>
        Trader = 1
    }

    /// <summary>
    /// Serializable RectF Structure.
    /// </summary>
    public struct RectFSer
    {
        [JsonPropertyName("left")] public float Left { get; set; }
        [JsonPropertyName("top")] public float Top { get; set; }
        [JsonPropertyName("right")] public float Right { get; set; }
        [JsonPropertyName("bottom")] public float Bottom { get; set; }

        public RectFSer(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    public static class GuiExtensions
    {
        #region GUI Extensions
        /// <summary>
        /// Convert Unity Position (X,Y,Z) to an unzoomed Map Position..
        /// </summary>
        /// <param name="vector">Unity Vector3</param>
        /// <param name="map">Current Map</param>
        /// <returns>Unzoomed 2D Map Position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ToMapPos(this System.Numerics.Vector3 vector, LoneMapConfig map) =>
            new()
            {
                X = (map.X * map.SvgScale) + (vector.X * (map.Scale * map.SvgScale)),
                Y = (map.Y * map.SvgScale) - (vector.Z * (map.Scale * map.SvgScale))
            };

        /// <summary>
        /// Convert an Unzoomed Map Position to a Zoomed Map Position ready for 2D Drawing.
        /// </summary>
        /// <param name="mapPos">Unzoomed Map Position.</param>
        /// <param name="mapParams">Current Map Parameters.</param>
        /// <returns>Zoomed 2D Map Position.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SKPoint ToZoomedPos(this Vector2 mapPos, LoneMapParams mapParams) =>
            new SKPoint()
            {
                X = (mapPos.X - mapParams.Bounds.Left) * mapParams.XScale,
                Y = (mapPos.Y - mapParams.Bounds.Top) * mapParams.YScale
            };

        /// <summary>
        /// Gets a drawable 'Up Arrow'. IDisposable. Applies UI Scaling internally.
        /// </summary>
        public static SKPath GetUpArrow(this SKPoint point, float size = 6, float offsetX = 0, float offsetY = 0)
        {
            float x = point.X + offsetX;
            float y = point.Y + offsetY;

            size *= MainForm.UIScale;
            var path = new SKPath();
            path.MoveTo(x, y);
            path.LineTo(x - size, y + size);
            path.LineTo(x + size, y + size);
            path.Close();

            return path;
        }

        /// <summary>
        /// Gets a drawable 'Down Arrow'. IDisposable. Applies UI Scaling internally.
        /// </summary>
        public static SKPath GetDownArrow(this SKPoint point, float size = 6, float offsetX = 0, float offsetY = 0)
        {
            float x = point.X + offsetX;
            float y = point.Y + offsetY;

            size *= MainForm.UIScale;
            var path = new SKPath();
            path.MoveTo(x, y);
            path.LineTo(x - size, y - size);
            path.LineTo(x + size, y - size);
            path.Close();

            return path;
        }

        /// <summary>
        /// Draws a Mine/Explosive Marker on this zoomed location.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DrawMineMarker(this SKPoint zoomedMapPos, SKCanvas canvas)
        {
            float length = 3.5f * MainForm.UIScale;
            canvas.DrawLine(new SKPoint(zoomedMapPos.X - length, zoomedMapPos.Y + length), new SKPoint(zoomedMapPos.X + length, zoomedMapPos.Y - length), SKPaints.PaintExplosives);
            canvas.DrawLine(new SKPoint(zoomedMapPos.X - length, zoomedMapPos.Y - length), new SKPoint(zoomedMapPos.X + length, zoomedMapPos.Y + length), SKPaints.PaintExplosives);
        }

        /// <summary>
        /// Draws Mouseover Text (with backer) on this zoomed location.
        /// </summary>
        public static void DrawMouseoverText(this SKPoint zoomedMapPos, SKCanvas canvas, IEnumerable<string> lines)
        {
            float maxLength = 0;
            foreach (var line in lines)
            {
                var length = SKPaints.TextBoss.MeasureText(line);
                if (length > maxLength)
                    maxLength = length;
            }
            var backer = new SKRect()
            {
                Bottom = zoomedMapPos.Y + ((lines.Count() * 12f) - 2) * MainForm.UIScale,
                Left = zoomedMapPos.X + (9 * MainForm.UIScale),
                Top = zoomedMapPos.Y - (9 * MainForm.UIScale),
                Right = zoomedMapPos.X + (9 * MainForm.UIScale) + maxLength + (6 * MainForm.UIScale)
            };
            canvas.DrawRect(backer, SKPaints.PaintTransparentBacker); // Draw tooltip backer
            zoomedMapPos.Offset(11 * MainForm.UIScale, 3 * MainForm.UIScale);
            foreach (var line in lines) // Draw tooltip text
            {
                if (string.IsNullOrEmpty(line?.Trim()))
                    continue;
                canvas.DrawText(line, zoomedMapPos, SKPaints.TextMouseover); // draw line text
                zoomedMapPos.Offset(0, 12f * MainForm.UIScale);
            }
        }

        /// <summary>
        /// Draw an ESP Text Label on an Entity.
        /// </summary>
        public static void DrawESPText(this SKPoint screenPos, SKCanvas canvas, IESPEntity entity, LocalPlayer localPlayer, bool printDist, SKPaint paint, params string[] lines)
        {
            if (printDist && lines.Length > 0)
            {
                var dist = Vector3.Distance(entity.Position, localPlayer.Position);
                string distStr;
                if (entity is LootItem && dist < 10f)
                {
                    distStr = $" {dist.ToString("n1")}m";
                }
                else
                {
                    distStr = $" {(int)dist}m";
                }
                lines[0] += distStr;
            }
            foreach (var x in lines)
            {
                if (string.IsNullOrEmpty(x?.Trim()))
                    continue;
                canvas.DrawText(x, screenPos, paint);
                screenPos.Y += paint.TextSize;
            }
        }
        #endregion
    }
}

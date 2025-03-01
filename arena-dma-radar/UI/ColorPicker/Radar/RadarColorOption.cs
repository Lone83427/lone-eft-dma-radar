using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Misc;

namespace arena_dma_radar.UI.ColorPicker.Radar
{
    [Obfuscation(Exclude = true, Feature = "renaming")]
    public enum RadarColorOption
    {
        LocalPlayer,
        FriendlyPlayer,
        AIPlayer,
        EnemyPlayer,
        StreamerPlayer,
        AimbotLocked,
        FocusedPlayer,
        DeathMarker,
        Explosives
    }

    internal static class RadarColorOptions
    {
        #region Static Interfaces
        /// <summary>
        /// Load all ESP Color Config. Run once at start of application.
        /// </summary>
        internal static void LoadColors(Config config)
        {
            config.Colors ??= new();
            foreach (var defaultColor in GetDefaultColors())
                config.Colors.TryAdd(defaultColor.Key, defaultColor.Value);
            SetColors(config.Colors);
        }
        /// <summary>
        /// Returns all default color combinations for Radar.
        /// </summary>
        internal static Dictionary<RadarColorOption, string> GetDefaultColors() =>
        new()
        {
            [RadarColorOption.LocalPlayer] = SKColors.Green.ToString(),
            [RadarColorOption.FriendlyPlayer] = SKColors.LimeGreen.ToString(),
            [RadarColorOption.EnemyPlayer] = SKColors.Red.ToString(),
            [RadarColorOption.AIPlayer] = SKColors.Yellow.ToString(),
            [RadarColorOption.StreamerPlayer] = SKColors.MediumPurple.ToString(),
            [RadarColorOption.DeathMarker] = SKColors.Black.ToString(),
            [RadarColorOption.Explosives] = SKColors.OrangeRed.ToString(),
            [RadarColorOption.AimbotLocked] = SKColors.Blue.ToString(),
            [RadarColorOption.FocusedPlayer] = SKColors.Coral.ToString()
        };
        /// <summary>
        /// Save all ESP Color Changes.
        /// </summary>
        internal static void SetColors(IReadOnlyDictionary<RadarColorOption, string> colors)
        {
            try
            {
                foreach (var color in colors)
                {
                    if (!SKColor.TryParse(color.Value, out var skColor))
                        throw new Exception($"Invalid Color Value for {color.Key}!");
                    switch (color.Key)
                    {
                        case RadarColorOption.LocalPlayer:
                            SKPaints.PaintLocalPlayer.Color = skColor;
                            EspWidget.PaintAimviewLocalPlayer.Color = skColor;
                            break;
                        case RadarColorOption.FriendlyPlayer:
                            SKPaints.PaintTeammate.Color = skColor;
                            SKPaints.TextTeammate.Color = skColor;
                            EspWidget.PaintAimviewTeammate.Color = skColor;
                            break;
                        case RadarColorOption.AIPlayer:
                            SKPaints.PaintAI.Color = skColor;
                            SKPaints.TextAI.Color = skColor;
                            EspWidget.PaintAimviewAI.Color = skColor;
                            break;
                        case RadarColorOption.EnemyPlayer:
                            SKPaints.PaintPlayer.Color = skColor;
                            SKPaints.TextPlayer.Color = skColor;
                            EspWidget.PaintAimviewPlayer.Color = skColor;
                            break;
                        case RadarColorOption.StreamerPlayer:
                            SKPaints.PaintStreamer.Color = skColor;
                            SKPaints.TextStreamer.Color = skColor;
                            EspWidget.PaintAimviewStreamer.Color = skColor;
                            break;
                        case RadarColorOption.DeathMarker:
                            SKPaints.PaintDeathMarker.Color = skColor;
                            break;
                        case RadarColorOption.Explosives:
                            SKPaints.PaintExplosives.Color = skColor;
                            break;
                        case RadarColorOption.AimbotLocked:
                            SKPaints.PaintAimbotLocked.Color = skColor;
                            SKPaints.TextAimbotLocked.Color = skColor;
                            EspWidget.PaintAimviewAimbotLocked.Color = skColor;
                            break;
                        case RadarColorOption.FocusedPlayer:
                            SKPaints.PaintFocused.Color = skColor;
                            SKPaints.TextFocused.Color = skColor;
                            EspWidget.PaintAimviewFocused.Color = skColor;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Setting Radar Colors", ex);
            }
        }
        #endregion
    }
}

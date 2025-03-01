using arena_dma_radar.UI.Misc;

namespace arena_dma_radar.UI.ColorPicker.ESP
{
    [Obfuscation(Exclude = true, Feature = "renaming")]
    public enum EspColorOption
    {
        FriendlyPlayer,
        EnemyPlayer,
        AIPlayer,
        StreamerPlayer,
        AimbotLockedPlayer,
        FocusedPlayer,
        Grenade
    }

    internal static class EspColorOptions
    {
        #region Static Interfaces
        /// <summary>
        /// Load all ESP Color Config. Run once at start of application.
        /// </summary>
        internal static void LoadColors(Config config)
        {
            config.ESP.Colors ??= new();
            foreach (var defaultColor in GetDefaultColors())
                config.ESP.Colors.TryAdd(defaultColor.Key, defaultColor.Value);
            SetColors(config.ESP.Colors);
        }
        /// <summary>
        /// Returns all default color combinations for ESP.
        /// </summary>
        internal static Dictionary<EspColorOption, string> GetDefaultColors() =>
        new()
        {
            [EspColorOption.Grenade] = SKColors.Orange.ToString(),
            [EspColorOption.FriendlyPlayer] = SKColors.LimeGreen.ToString(),
            [EspColorOption.EnemyPlayer] = SKColors.Red.ToString(),
            [EspColorOption.AIPlayer] = SKColors.Yellow.ToString(),
            [EspColorOption.StreamerPlayer] = SKColors.MediumPurple.ToString(),
            [EspColorOption.AimbotLockedPlayer] = SKColor.Parse("4654ff").ToString(), // Blue doesnt work on some fusers
            [EspColorOption.FocusedPlayer] = SKColors.Coral.ToString()
        };
        /// <summary>
        /// Save all ESP Color Changes.
        /// </summary>
        internal static void SetColors(IReadOnlyDictionary<EspColorOption, string> colors)
        {
            try
            {
                foreach (var color in colors)
                {
                    if (!SKColor.TryParse(color.Value, out var skColor))
                        throw new Exception($"Invalid Color Value for {color.Key}!");
                    switch (color.Key)
                    {
                        case EspColorOption.FriendlyPlayer:
                            SKPaints.PaintTeammateESP.Color = skColor;
                            SKPaints.TextTeammateESP.Color = skColor;
                            break;
                        case EspColorOption.EnemyPlayer:
                            SKPaints.PaintPlayerESP.Color = skColor;
                            SKPaints.TextPlayerESP.Color = skColor;
                            break;
                        case EspColorOption.AIPlayer:
                            SKPaints.PaintAIESP.Color = skColor;
                            SKPaints.TextAIESP.Color = skColor;
                            break;
                        case EspColorOption.StreamerPlayer:
                            SKPaints.PaintStreamerESP.Color = skColor;
                            SKPaints.TextStreamerESP.Color = skColor;
                            break;
                        case EspColorOption.Grenade:
                            SKPaints.PaintGrenadeESP.Color = skColor;
                            break;
                        case EspColorOption.AimbotLockedPlayer:
                            SKPaints.PaintAimbotLockedESP.Color = skColor;
                            SKPaints.TextAimbotLockedESP.Color = skColor;
                            break;
                        case EspColorOption.FocusedPlayer:
                            SKPaints.PaintFocusedESP.Color = skColor;
                            SKPaints.TextFocusedESP.Color = skColor;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Setting ESP Colors", ex);
            }
        }
        #endregion
    }
}

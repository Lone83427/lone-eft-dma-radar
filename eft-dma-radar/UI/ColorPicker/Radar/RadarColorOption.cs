using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Misc;

namespace eft_dma_radar.UI.ColorPicker.Radar
{
    [Obfuscation(Exclude = true, Feature = "renaming")]
    public enum RadarColorOption
    {
        LocalPlayer,
        FriendlyPlayer,
        PMCPlayer,
        WatchlistPlayer,
        StreamerPlayer,
        AimbotLockedPlayer,
        HumanScavPlayer,
        ScavPlayer,
        RaiderPlayer,
        BossPlayer,
        FocusedPlayer,
        DeathMarker,
        RegularLoot,
        ValuableLoot,
        WishlistLoot,
        ContainerLoot,
        MedsFilterLoot,
        FoodFilterLoot,
        BackpacksFilterLoot,
        QuestLoot,
        StaticQuestItemsAndZones,
        Corpse,
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
            config.Colors ??= new Dictionary<RadarColorOption, string>();
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
                [RadarColorOption.PMCPlayer] = SKColors.Red.ToString(),
                [RadarColorOption.WatchlistPlayer] = SKColors.HotPink.ToString(),
                [RadarColorOption.StreamerPlayer] = SKColors.MediumPurple.ToString(),
                [RadarColorOption.AimbotLockedPlayer] = SKColors.Blue.ToString(),
                [RadarColorOption.HumanScavPlayer] = SKColors.White.ToString(),
                [RadarColorOption.ScavPlayer] = SKColors.Yellow.ToString(),
                [RadarColorOption.RaiderPlayer] = SKColor.Parse("ffc70f").ToString(),
                [RadarColorOption.BossPlayer] = SKColors.Fuchsia.ToString(),
                [RadarColorOption.FocusedPlayer] = SKColors.Coral.ToString(),
                [RadarColorOption.DeathMarker] = SKColors.Black.ToString(),
                [RadarColorOption.RegularLoot] = SKColors.WhiteSmoke.ToString(),
                [RadarColorOption.ValuableLoot] = SKColors.Turquoise.ToString(),
                [RadarColorOption.WishlistLoot] = SKColors.Red.ToString(),
                [RadarColorOption.ContainerLoot] = SKColor.Parse("FFFFCC").ToString(),
                [RadarColorOption.QuestLoot] = SKColors.YellowGreen.ToString(),
                [RadarColorOption.StaticQuestItemsAndZones] = SKColors.DeepPink.ToString(),
                [RadarColorOption.Corpse] = SKColors.Silver.ToString(),
                [RadarColorOption.MedsFilterLoot] = SKColors.LightSalmon.ToString(),
                [RadarColorOption.FoodFilterLoot] = SKColors.CornflowerBlue.ToString(),
                [RadarColorOption.BackpacksFilterLoot] = SKColor.Parse("00b02c").ToString(),
                [RadarColorOption.Explosives] = SKColors.OrangeRed.ToString(),
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
                            SKPaints.TextLocalPlayer.Color = skColor;
                            EspWidget.PaintESPWidgetLocalPlayer.Color = skColor;
                            break;
                        case RadarColorOption.FriendlyPlayer:
                            SKPaints.PaintTeammate.Color = skColor;
                            SKPaints.TextTeammate.Color = skColor;
                            EspWidget.PaintESPWidgetTeammate.Color = skColor;
                            break;
                        case RadarColorOption.PMCPlayer:
                            SKPaints.PaintPMC.Color = skColor;
                            SKPaints.TextPMC.Color = skColor;
                            EspWidget.PaintESPWidgetPMC.Color = skColor;
                            break;
                        case RadarColorOption.WatchlistPlayer:
                            SKPaints.PaintWatchlist.Color = skColor;
                            SKPaints.TextWatchlist.Color = skColor;
                            EspWidget.PaintESPWidgetWatchlist.Color = skColor;
                            break;
                        case RadarColorOption.StreamerPlayer:
                            SKPaints.PaintStreamer.Color = skColor;
                            SKPaints.TextStreamer.Color = skColor;
                            EspWidget.PaintESPWidgetStreamer.Color = skColor;
                            break;
                        case RadarColorOption.AimbotLockedPlayer:
                            SKPaints.PaintAimbotLocked.Color = skColor;
                            SKPaints.TextAimbotLocked.Color = skColor;
                            EspWidget.PaintESPWidgetAimbotLocked.Color = skColor;
                            break;
                        case RadarColorOption.HumanScavPlayer:
                            SKPaints.PaintPScav.Color = skColor;
                            SKPaints.TextPScav.Color = skColor;
                            EspWidget.PaintESPWidgetPScav.Color = skColor;
                            break;
                        case RadarColorOption.ScavPlayer:
                            SKPaints.PaintScav.Color = skColor;
                            SKPaints.TextScav.Color = skColor;
                            EspWidget.PaintESPWidgetScav.Color = skColor;
                            break;
                        case RadarColorOption.RaiderPlayer:
                            SKPaints.PaintRaider.Color = skColor;
                            SKPaints.TextRaider.Color = skColor;
                            EspWidget.PaintESPWidgetRaider.Color = skColor;
                            break;
                        case RadarColorOption.BossPlayer:
                            SKPaints.PaintBoss.Color = skColor;
                            SKPaints.TextBoss.Color = skColor;
                            EspWidget.PaintESPWidgetBoss.Color = skColor;
                            break;
                        case RadarColorOption.FocusedPlayer:
                            SKPaints.PaintFocused.Color = skColor;
                            SKPaints.TextFocused.Color = skColor;
                            EspWidget.PaintESPWidgetFocused.Color = skColor;
                            break;
                        case RadarColorOption.DeathMarker:
                            SKPaints.PaintDeathMarker.Color = skColor;
                            break;
                        case RadarColorOption.RegularLoot:
                            SKPaints.PaintLoot.Color = skColor;
                            SKPaints.TextLoot.Color = skColor;
                            EspWidget.PaintESPWidgetLoot.Color = skColor;
                            EspWidget.TextESPWidgetLoot.Color = skColor;
                            break;
                        case RadarColorOption.ValuableLoot:
                            SKPaints.PaintImportantLoot.Color = skColor;
                            SKPaints.TextImportantLoot.Color = skColor;
                            break;
                        case RadarColorOption.WishlistLoot:
                            SKPaints.PaintWishlistItem.Color = skColor;
                            SKPaints.TextWishlistItem.Color = skColor;
                            break;
                        case RadarColorOption.QuestLoot:
                            SKPaints.PaintQuestItem.Color = skColor;
                            SKPaints.TextQuestItem.Color = skColor;
                            break;
                        case RadarColorOption.StaticQuestItemsAndZones:
                            SKPaints.QuestHelperPaint.Color = skColor;
                            SKPaints.QuestHelperText.Color = skColor;
                            break;
                        case RadarColorOption.Corpse:
                            SKPaints.PaintCorpse.Color = skColor;
                            SKPaints.TextCorpse.Color = skColor;
                            break;
                        case RadarColorOption.MedsFilterLoot:
                            SKPaints.PaintMeds.Color = skColor;
                            SKPaints.TextMeds.Color = skColor;
                            break;
                        case RadarColorOption.FoodFilterLoot:
                            SKPaints.PaintFood.Color = skColor;
                            SKPaints.TextFood.Color = skColor;
                            break;
                        case RadarColorOption.BackpacksFilterLoot:
                            SKPaints.PaintBackpacks.Color = skColor;
                            SKPaints.TextBackpacks.Color = skColor;
                            break;
                        case RadarColorOption.Explosives:
                            SKPaints.PaintExplosives.Color = skColor;
                            break;
                        case RadarColorOption.ContainerLoot:
                            SKPaints.PaintContainerLoot.Color = skColor;
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
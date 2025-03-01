using eft_dma_radar.UI.Misc;

namespace eft_dma_radar.UI.ColorPicker.ESP
{
    [Obfuscation(Exclude = true, Feature = "renaming")]
    public enum EspColorOption
    {
        FriendlyPlayer,
        ScavPlayer,
        HumanScavPlayer,
        RaiderPlayer,
        BossPlayer,
        PMCPlayer,
        WatchlistPlayer,
        StreamerPlayer,
        AimbotLockedPlayer,
        FocusedPlayer,
        RegularLoot,
        ValuableLoot,
        WishlistLoot,
        ContainerLoot,
        MedsFilterLoot,
        FoodFilterLoot,
        BackpackFilterLoot,
        QuestLoot,
        StaticQuestItemsAndZones,
        Grenade,
        Exfil,
        Corpse
    }

    internal static class EspColorOptions
    {
        #region Static Interfaces

        /// <summary>
        /// Load all ESP Color Config. Run once at start of application.
        /// </summary>
        internal static void LoadColors(Config config)
        {
            config.ESP.Colors ??= new Dictionary<EspColorOption, string>();
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
                [EspColorOption.QuestLoot] = SKColors.YellowGreen.ToString(),
                [EspColorOption.StaticQuestItemsAndZones] = SKColors.DeepPink.ToString(),
                [EspColorOption.Grenade] = SKColors.Orange.ToString(),
                [EspColorOption.FriendlyPlayer] = SKColors.LimeGreen.ToString(),
                [EspColorOption.PMCPlayer] = SKColors.Red.ToString(),
                [EspColorOption.WatchlistPlayer] = SKColors.HotPink.ToString(),
                [EspColorOption.StreamerPlayer] = SKColors.MediumPurple.ToString(),
                [EspColorOption.AimbotLockedPlayer] = SKColor.Parse("4654ff").ToString(), // Blue doesnt work on some fusers
                [EspColorOption.FocusedPlayer] = SKColors.Coral.ToString(),
                [EspColorOption.HumanScavPlayer] = SKColors.White.ToString(),
                [EspColorOption.BossPlayer] = SKColors.Fuchsia.ToString(),
                [EspColorOption.RaiderPlayer] = SKColor.Parse("ffc70f").ToString(),
                [EspColorOption.ScavPlayer] = SKColors.Yellow.ToString(),
                [EspColorOption.RegularLoot] = SKColors.White.ToString(),
                [EspColorOption.ValuableLoot] = SKColors.Turquoise.ToString(),
                [EspColorOption.WishlistLoot] = SKColors.Red.ToString(),
                [EspColorOption.ContainerLoot] = SKColor.Parse("FFFFCC").ToString(),
                [EspColorOption.MedsFilterLoot] = SKColors.LightSalmon.ToString(),
                [EspColorOption.FoodFilterLoot] = SKColors.CornflowerBlue.ToString(),
                [EspColorOption.BackpackFilterLoot] = SKColor.Parse("00b02c").ToString(),
                [EspColorOption.Exfil] = SKColors.MediumSeaGreen.ToString(),
                [EspColorOption.Corpse] = SKColors.Silver.ToString()
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
                            SKPaints.PaintFriendlyESP.Color = skColor;
                            SKPaints.TextFriendlyESP.Color = skColor;
                            break;
                        case EspColorOption.PMCPlayer:
                            SKPaints.PaintPMCESP.Color = skColor;
                            SKPaints.TextPMCESP.Color = skColor;
                            break;
                        case EspColorOption.ScavPlayer:
                            SKPaints.PaintScavESP.Color = skColor;
                            SKPaints.TextScavESP.Color = skColor;
                            break;
                        case EspColorOption.HumanScavPlayer:
                            SKPaints.PaintPlayerScavESP.Color = skColor;
                            SKPaints.TextPlayerScavESP.Color = skColor;
                            break;
                        case EspColorOption.BossPlayer:
                            SKPaints.PaintBossESP.Color = skColor;
                            SKPaints.TextBossESP.Color = skColor;
                            break;
                        case EspColorOption.WatchlistPlayer:
                            SKPaints.PaintWatchlistESP.Color = skColor;
                            SKPaints.TextWatchlistESP.Color = skColor;
                            break;
                        case EspColorOption.StreamerPlayer:
                            SKPaints.PaintStreamerESP.Color = skColor;
                            SKPaints.TextStreamerESP.Color = skColor;
                            break;
                        case EspColorOption.AimbotLockedPlayer:
                            SKPaints.PaintAimbotLockedESP.Color = skColor;
                            SKPaints.TextAimbotLockedESP.Color = skColor;
                            break;
                        case EspColorOption.FocusedPlayer:
                            SKPaints.PaintFocusedESP.Color = skColor;
                            SKPaints.TextFocusedESP.Color = skColor;
                            break;
                        case EspColorOption.RaiderPlayer:
                            SKPaints.PaintRaiderESP.Color = skColor;
                            SKPaints.TextRaiderESP.Color = skColor;
                            break;
                        case EspColorOption.RegularLoot:
                            SKPaints.PaintLootESP.Color = skColor;
                            SKPaints.TextLootESP.Color = skColor;
                            break;
                        case EspColorOption.ValuableLoot:
                            SKPaints.PaintImpLootESP.Color = skColor;
                            SKPaints.TextImpLootESP.Color = skColor;
                            break;
                        case EspColorOption.WishlistLoot:
                            SKPaints.PaintWishlistItemESP.Color = skColor;
                            SKPaints.TextWishlistItemESP.Color = skColor;
                            break;
                        case EspColorOption.FoodFilterLoot:
                            SKPaints.PaintFoodESP.Color = skColor;
                            SKPaints.TextFoodESP.Color = skColor;
                            break;
                        case EspColorOption.MedsFilterLoot:
                            SKPaints.PaintMedsESP.Color = skColor;
                            SKPaints.TextMedsESP.Color = skColor;
                            break;
                        case EspColorOption.BackpackFilterLoot:
                            SKPaints.PaintBackpackESP.Color = skColor;
                            SKPaints.TextBackpackESP.Color = skColor;
                            break;
                        case EspColorOption.QuestLoot:
                            SKPaints.PaintQuestItemESP.Color = skColor;
                            SKPaints.TextQuestItemESP.Color = skColor;
                            break;
                        case EspColorOption.StaticQuestItemsAndZones:
                            SKPaints.PaintQuestHelperESP.Color = skColor;
                            SKPaints.TextQuestHelperESP.Color = skColor;
                            break;
                        case EspColorOption.Grenade:
                            SKPaints.PaintGrenadeESP.Color = skColor;
                            break;
                        case EspColorOption.Exfil:
                            SKPaints.TextExfilESP.Color = skColor;
                            break;
                        case EspColorOption.Corpse:
                            SKPaints.PaintCorpseESP.Color = skColor;
                            SKPaints.TextCorpseESP.Color = skColor;
                            break;
                        case EspColorOption.ContainerLoot:
                            SKPaints.PaintContainerLootESP.Color = skColor;
                            SKPaints.TextContainerLootESP.Color = skColor;
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
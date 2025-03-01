using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.LootFilters;
using eft_dma_radar.UI.Radar;
using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Misc.Data;

namespace eft_dma_radar.Tarkov.Loot
{
    public class LootItem : IMouseoverEntity, IMapEntity, IWorldEntity, IESPEntity
    {
        private static Config Config { get; } = Program.Config;
        private readonly TarkovMarketItem _item;

        public LootItem(TarkovMarketItem item)
        {
            ArgumentNullException.ThrowIfNull(item, nameof(item));
            _item = item;
        }

        public LootItem(string id, string name)
        {
            ArgumentNullException.ThrowIfNull(id, nameof(id));
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            _item = new TarkovMarketItem
            {
                Name = name,
                ShortName = name,
                FleaPrice = -1,
                TraderPrice = -1,
                BsgId = id
            };
        }

        /// <summary>
        /// Item's BSG ID.
        /// </summary>
        public virtual string ID => _item.BsgId;

        /// <summary>
        /// Item's Long Name.
        /// </summary>
        public virtual string Name => _item.Name;

        /// <summary>
        /// Item's Short Name.
        /// </summary>
        public string ShortName => _item.ShortName;

        /// <summary>
        /// Item's Price (In roubles).
        /// </summary>
        public int Price
        {
            get
            {
                long price;
                if (Config.LootPPS)
                {
                    if (Config.LootPriceMode is LootPriceMode.FleaMarket)
                        price = (long)((float)_item.FleaPrice / GridCount);
                    else
                        price = (long)((float)_item.TraderPrice / GridCount);
                }
                else
                {
                    if (Config.LootPriceMode is LootPriceMode.FleaMarket)
                        price = _item.FleaPrice;
                    else
                        price = _item.TraderPrice;
                }
                if (price <= 0)
                    price = Math.Max(_item.FleaPrice, _item.TraderPrice);
                return (int)price;
            }
        }

        /// <summary>
        /// Number of grid spaces this item takes up.
        /// </summary>
        public int GridCount => _item.Slots == 0 ? 1 : _item.Slots;

        /// <summary>
        /// Custom filter for this item (if set).
        /// </summary>
        public LootFilterEntry CustomFilter => _item.CustomFilter;

        /// <summary>
        /// True if the item is important via the UI.
        /// </summary>
        public bool Important => CustomFilter?.Important ?? false;

        /// <summary>
        /// True if this item is wishlisted.
        /// </summary>
        public bool IsWishlisted => Config.LootWishlist && LocalPlayer.WishlistItems.Contains(ID);

        /// <summary>
        /// True if the item is blacklisted via the UI.
        /// </summary>
        public bool Blacklisted => CustomFilter?.Blacklisted ?? false;

        public bool IsMeds
        {
            get
            {
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsMeds);
                }
                return _item.IsMed;
            }
        }
        public bool IsFood
        {
            get
            {
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsFood);
                }
                return _item.IsFood;
            }
        }
        public bool IsBackpack
        {
            get
            {
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsBackpack);
                }
                return _item.IsBackpack;
            }
        }
        public bool IsWeapon => _item.IsWeapon;
        public bool IsCurrency => _item.IsCurrency;

        /// <summary>
        /// Checks if an item exceeds regular loot price threshold.
        /// </summary>
        public bool IsRegularLoot
        {
            get
            {
                if (Blacklisted)
                    return false;
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsRegularLoot);
                }
                return Price >= Program.Config.MinLootValue;
            }
        }

        /// <summary>
        /// Checks if an item exceeds valuable loot price threshold.
        /// </summary>
        public bool IsValuableLoot
        {
            get
            {
                if (Blacklisted)
                    return false;
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsValuableLoot);
                }
                return Price >= Program.Config.MinValuableLootValue;
            }
        }

        /// <summary>
        /// Checks if an item/container is important.
        /// </summary>
        public bool IsImportant
        {
            get
            {
                if (Blacklisted)
                    return false;
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsImportant);
                }
                return _item.Important || IsWishlisted;
            }
        }

        /// <summary>
        /// True if a condition for a quest.
        /// </summary>
        public bool IsQuestCondition
        {
            get
            {
                if (Blacklisted)
                    return false;
                if (IsCurrency) // Don't show currencies
                    return false;
                if (this is LootContainer container)
                {
                    return container.Loot.Any(x => x.IsQuestCondition);
                }
                return Memory.QuestManager?.ItemConditions?.Contains(ID) ?? false;
            }
        }

        /// <summary>
        /// True if this item contains the specified Search Predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>True if search matches, otherwise False.</returns>
        public bool ContainsSearchPredicate(Predicate<LootItem> predicate)
        {
            if (this is LootContainer container)
            {
                return container.Loot.Any(x => x.ContainsSearchPredicate(predicate));
            }
            return predicate(this);
        }

        public virtual void DrawESP(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var dist = Vector3.Distance(localPlayer.Position, Position);
            if (this is QuestItem)
            {
               if (dist > ESP.Config.QuestHelperDrawDistance)
                    return;
            }
            else if (this is not QuestItem && (IsImportant || IsValuableLoot))
            {
                if (dist > ESP.Config.ImpLootDrawDistance)
                    return;
            }
            else if (dist > ESP.Config.LootDrawDistance)
            {
                return;
            }

            if (this is LootCorpse && Config.HideCorpses)
                return;
            if (!CameraManagerBase.WorldToScreen(ref _position, out var scrPos))
                return;
            var boxHalf = 3.5f * ESP.Config.FontScale;
            var label = GetUILabel(MainForm.Config.QuestHelper.Enabled);
            var showDist = ESP.Config.ShowDistances || dist <= 10f;
            var boxPt = new SKRect(scrPos.X - boxHalf, scrPos.Y + boxHalf,
                scrPos.X + boxHalf, scrPos.Y - boxHalf);
            var paints = GetESPPaints();
            var textPt = new SKPoint(scrPos.X,
                scrPos.Y + 16f * ESP.Config.FontScale);
            canvas.DrawRect(boxPt, paints.Item1);
            textPt.DrawESPText(canvas, this, localPlayer, showDist, paints.Item2, label);
        }

        private Vector3 _position;
        public ref Vector3 Position => ref _position;
        public Vector2 MouseoverPosition { get; set; }

        public virtual void Draw(SKCanvas canvas, LoneMapParams mapParams, ILocalPlayer localPlayer)
        {
            var label = GetUILabel(MainForm.Config.QuestHelper.Enabled);
            var paints = GetPaints();
            var heightDiff = Position.Y - localPlayer.Position.Y;
            var point = Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams);
            MouseoverPosition = new Vector2(point.X, point.Y);
            SKPaints.ShapeOutline.StrokeWidth = 2f;
            if (heightDiff > 1.45) // loot is above player
            {
                using var path = point.GetUpArrow(5);
                canvas.DrawPath(path, SKPaints.ShapeOutline);
                canvas.DrawPath(path, paints.Item1);
            }
            else if (heightDiff < -1.45) // loot is below player
            {
                using var path = point.GetDownArrow(5);
                canvas.DrawPath(path, SKPaints.ShapeOutline);
                canvas.DrawPath(path, paints.Item1);
            }
            else // loot is level with player
            {
                var size = 5 * MainForm.UIScale;
                canvas.DrawCircle(point, size, SKPaints.ShapeOutline);
                canvas.DrawCircle(point, size, paints.Item1);
            }

            point.Offset(7 * MainForm.UIScale, 3 * MainForm.UIScale);
            canvas.DrawText(label, point, SKPaints.TextOutline); // Draw outline
            canvas.DrawText(label, point, paints.Item2);
        }

        public virtual void DrawMouseover(SKCanvas canvas, LoneMapParams mapParams, LocalPlayer localPlayer)
        {
            if (this is LootContainer container)
            {
                var lines = new List<string>();
                var loot = container.FilteredLoot;
                if (container is LootCorpse corpse) // Draw corpse loot
                {
                    var corpseLoot = corpse.Loot?.OrderLoot();
                    var sumPrice = corpseLoot?.Sum(x => x.Price) ?? 0;
                    var corpseValue = TarkovMarketItem.FormatPrice(sumPrice);
                    var playerObj = corpse.PlayerObject;
                    if (playerObj is not null)
                    {
                        var name = MainForm.Config.HideNames && playerObj.IsHuman ? "<Hidden>" : playerObj.Name;
                        lines.Add($"{playerObj.Type.GetDescription()}:{name}");
                        string g = null;
                        if (playerObj.GroupID != -1) g = $"G:{playerObj.GroupID} ";
                        if (g is not null) lines.Add(g);
                        lines.Add($"Value: {corpseValue}");
                    }
                    else
                    {
                        lines.Add($"{corpse.Name} (Value:{corpseValue})");
                    }

                    if (corpseLoot?.Any() == true)
                        foreach (var item in corpseLoot)
                            lines.Add(item.GetUILabel(MainForm.Config.QuestHelper.Enabled));
                    else lines.Add("Empty");
                }
                else if (loot is not null && loot.Count() > 1) // draw regular container loot
                {
                    foreach (var item in loot)
                        lines.Add(item.GetUILabel(MainForm.Config.QuestHelper.Enabled));
                }
                else
                {
                    return; // Don't draw single items
                }

                Position.ToMapPos(mapParams.Map).ToZoomedPos(mapParams).DrawMouseoverText(canvas, lines);
            }
        }

        /// <summary>
        /// Gets a UI Friendly Label.
        /// </summary>
        /// <param name="showPrice">Show price in label.</param>
        /// <param name="showImportant">Show Important !! in label.</param>
        /// <param name="showQuest">Show Quest tag in label.</param>
        /// <returns>Item Label string cleaned up for UI usage.</returns>
        public string GetUILabel(bool showQuest = false)
        {
            var label = "";
            if (this is LootContainer container)
            {
                var important = container.Loot.Any(x => x.IsImportant);
                var loot = container.FilteredLoot;
                if (this is not LootCorpse && loot.Count() == 1)
                {
                    var firstItem = loot.First();
                    label = firstItem.ShortName;
                }
                else
                {
                    label = container.Name;
                }

                if (important)
                    label = $"!!{label}";
            }
            else
            {
                if (IsImportant)
                    label += "!!";
                else if (Price > 0)
                    label += $"[{TarkovMarketItem.FormatPrice(Price)}] ";
                label += ShortName;
                if (showQuest && IsQuestCondition)
                    label += " (Quest)";
            }

            if (string.IsNullOrEmpty(label))
                label = "Item";
            return label;
        }

        private ValueTuple<SKPaint, SKPaint> GetPaints()
        {
            if (IsWishlisted)
                return new(SKPaints.PaintWishlistItem, SKPaints.TextWishlistItem);
            else if (this is QuestItem)
                return new(SKPaints.QuestHelperPaint, SKPaints.QuestHelperText);
            else if (MainForm.Config.QuestHelper.Enabled && IsQuestCondition)
                return new (SKPaints.PaintQuestItem, SKPaints.TextQuestItem);
            if (LootFilter.ShowBackpacks && IsBackpack)
                return new(SKPaints.PaintBackpacks, SKPaints.TextBackpacks);
            if (LootFilter.ShowMeds && IsMeds)
                return new (SKPaints.PaintMeds, SKPaints.TextMeds);
            if (LootFilter.ShowFood && IsFood)
                return new (SKPaints.PaintFood, SKPaints.TextFood);
            string filterColor = null;
            if (this is LootContainer ctr)
            {
                filterColor = ctr.Loot?.FirstOrDefault(x => x.Important)?.CustomFilter?.Color;
                if (filterColor is null && this is LootCorpse)
                    return new (SKPaints.PaintCorpse, SKPaints.TextCorpse);
            }
            else
            {
                filterColor = CustomFilter?.Color;
            }

            if (!string.IsNullOrEmpty(filterColor))
            {
                var filterPaints = GetFilterPaints(filterColor);
                return new (filterPaints.Item1, filterPaints.Item2);
            }
            if (IsValuableLoot || this is LootAirdrop)
                return new (SKPaints.PaintImportantLoot, SKPaints.TextImportantLoot);
            return new (SKPaints.PaintLoot, SKPaints.TextLoot);
        }

        public ValueTuple<SKPaint, SKPaint> GetESPPaints()
        {
            if (this is LootCorpse)
                return new(SKPaints.PaintCorpseESP, SKPaints.TextCorpseESP);
            if (IsWishlisted)
                return new(SKPaints.PaintWishlistItemESP, SKPaints.TextWishlistItemESP);
            else if (this is QuestItem)
                return new(SKPaints.PaintQuestHelperESP, SKPaints.TextQuestHelperESP);
            else if (MainForm.Config.QuestHelper.Enabled && IsQuestCondition)
                return new (SKPaints.PaintQuestItemESP, SKPaints.TextQuestItemESP);
            if (LootFilter.ShowBackpacks && IsBackpack)
                return new(SKPaints.PaintBackpackESP, SKPaints.TextBackpackESP);
            if (LootFilter.ShowMeds && IsMeds)
                return new(SKPaints.PaintMedsESP, SKPaints.TextMedsESP);
            if (LootFilter.ShowFood && IsFood)
                return new(SKPaints.PaintFoodESP, SKPaints.TextFoodESP);
            string filterColor = null;
            if (this is LootContainer ctr)
                filterColor = ctr.Loot?.FirstOrDefault(x => x.Important)?.CustomFilter?.Color;
            else
                filterColor = CustomFilter?.Color;
            if (!string.IsNullOrEmpty(filterColor))
            {
                var filterPaints = GetFilterPaints(filterColor);
                return new(filterPaints.Item3, filterPaints.Item4);
            }
            return IsImportant || IsValuableLoot ? 
                new(SKPaints.PaintImpLootESP, SKPaints.TextImpLootESP) : new(SKPaints.PaintLootESP, SKPaints.TextLootESP);
        }

        #region Custom Loot Paints
        private static readonly ConcurrentDictionary<string, Tuple<SKPaint, SKPaint, SKPaint, SKPaint>> _paints = new();

        /// <summary>
        /// Returns the Paints for this color value.
        /// </summary>
        /// <param name="color">Color rgba hex string.</param>
        /// <returns>Tuple of paints. Item1 = Paint, Item2 = Text. Item3 = ESP Paint, Item4 = ESP Text</returns>
        private static Tuple<SKPaint, SKPaint, SKPaint, SKPaint> GetFilterPaints(string color)
        {
            if (!SKColor.TryParse(color, out var skColor))
                return new Tuple<SKPaint, SKPaint, SKPaint, SKPaint>(SKPaints.PaintLoot, SKPaints.TextLoot, SKPaints.PaintLootESP, SKPaints.TextBasicESP);
            var result = _paints.AddOrUpdate(color,
                key =>
                {
                    var paint = new SKPaint
                    {
                        Color = skColor,
                        StrokeWidth = 3f * MainForm.UIScale,
                        Style = SKPaintStyle.Fill,
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High
                    };
                    var text = new SKPaint
                    {
                        SubpixelText = true,
                        Color = skColor,
                        IsStroke = false,
                        TextSize = 12f * MainForm.UIScale,
                        TextEncoding = SKTextEncoding.Utf8,
                        IsAntialias = true,
                        Typeface = CustomFonts.SKFontFamilyRegular,
                        FilterQuality = SKFilterQuality.High
                    };
                    var espPaint = new SKPaint()
                    {
                        Color = skColor,
                        StrokeWidth = 0.25f,
                        Style = SKPaintStyle.Fill,
                        IsAntialias = true,
                        FilterQuality = SKFilterQuality.High
                    };
                    var espText = new SKPaint()
                    {
                        SubpixelText = true,
                        Color = skColor,
                        IsStroke = false,
                        TextSize = 12f,
                        TextAlign = SKTextAlign.Center,
                        TextEncoding = SKTextEncoding.Utf8,
                        IsAntialias = true,
                        Typeface = CustomFonts.SKFontFamilyMedium,
                        FilterQuality = SKFilterQuality.High
                    };
                    return new Tuple<SKPaint, SKPaint, SKPaint, SKPaint>(paint, text, espPaint, espText);
                },
                (key, existingValue) =>
                {
                    existingValue.Item1.StrokeWidth = 3f * MainForm.UIScale;
                    existingValue.Item2.TextSize = 12f * MainForm.UIScale;
                    existingValue.Item4.TextSize = 12f * ESP.Config.FontScale;
                    return existingValue;
                });
            return result;
        }
        #endregion
    }

    public static class LootItemExtensions
    {
        /// <summary>
        /// Order loot (important first, then by price).
        /// </summary>
        /// <param name="loot"></param>
        /// <returns>Ordered loot.</returns>
        public static IEnumerable<LootItem> OrderLoot(this IEnumerable<LootItem> loot)
        {
            return loot
                .OrderByDescending(x => x.IsImportant || (MainForm.Config.QuestHelper.Enabled && x.IsQuestCondition))
                .ThenByDescending(x => x.Price);
        }
    }
}
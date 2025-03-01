using DarkModeForms;
using eft_dma_radar.Features.MemoryWrites.UI;
using eft_dma_radar.Tarkov.API;
using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.EFTPlayer.Plugins;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.Tarkov.Features.MemoryWrites;
using eft_dma_radar.Tarkov.Features.MemoryWrites.Patches;
using eft_dma_radar.Tarkov.GameWorld;
using eft_dma_radar.Tarkov.GameWorld.Exits;
using eft_dma_radar.Tarkov.GameWorld.Explosives;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.Tarkov.WebRadar;
using eft_dma_radar.UI.ColorPicker;
using eft_dma_radar.UI.ColorPicker.ESP;
using eft_dma_radar.UI.ColorPicker.Radar;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.Hotkeys;
using eft_dma_radar.UI.LootFilters;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.SKWidgetControl;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Maps;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel;
using System.Security.Authentication.ExtendedProtection;
using System.Timers;
using static eft_dma_radar.UI.Hotkeys.HotkeyManager;
using static eft_dma_radar.UI.Hotkeys.HotkeyManager.HotkeyActionController;
using Timer = System.Timers.Timer;

namespace eft_dma_radar.UI.Radar
{
    public sealed partial class MainForm : Form
    {
        #region Fields / Properties

        private readonly DarkModeCS _darkmode;
        private readonly Stopwatch _fpsSw = new();
        private readonly PrecisionTimer _renderTimer;
        private readonly Timer _lootMenuTimer = new()
        {
            Interval = 250,
            AutoReset = false
        };

        private IMouseoverEntity _mouseOverItem;
        private bool _mouseDown;
        private Point _lastMousePosition;
        private int _zoom = 100;
        private int _fps;
        private Vector2 _mapPanPosition;
        private EspWidget _aimview;
        private PlayerInfoWidget _playerInfo;

        /// <summary>
        /// Main UI/Application Config.
        /// </summary>
        public static Config Config { get; } = Program.Config;

        /// <summary>
        /// Singleton Instance of MainForm.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal static MainForm Window { get; private set; }

        /// <summary>
        /// Current UI Scale Value for Primary Application Window.
        /// </summary>
        public static float UIScale => Config.UIScale;

        /// <summary>
        /// Currently 'Moused Over' Group.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static int? MouseoverGroup { get; private set; }

        /// <summary>
        /// Map Identifier of Current Map.
        /// </summary>
        private static string MapID
        {
            get
            {
                var id = Memory.MapID;
                id ??= "null";
                return id;
            }
        }

        /// <summary>
        /// Item Search Filter has been set/applied.
        /// </summary>
        private static bool FilterIsSet =>
            !string.IsNullOrEmpty(LootFilter.SearchString);

        /// <summary>
        /// True if corpses are visible as loot.
        /// </summary>
        private bool LootCorpsesVisible => checkBox_Loot.Checked && !checkBox_HideCorpses.Checked && !FilterIsSet;

        /// <summary>
        /// Game has started and Radar is starting up...
        /// </summary>
        private static bool Starting => Memory.Starting;

        /// <summary>
        /// Radar has found Escape From Tarkov process and is ready.
        /// </summary>
        private static bool Ready => Memory.Ready;

        /// <summary>
        /// Radar has found Local Game World, and a Raid Instance is active.
        /// </summary>
        private static bool InRaid => Memory.InRaid;

        /// <summary>
        /// LocalPlayer (who is running Radar) 'Player' object.
        /// Returns the player the Current Window belongs to.
        /// </summary>
        private static LocalPlayer LocalPlayer => Memory.LocalPlayer;

        /// <summary>
        /// All Filtered Loot on the map.
        /// </summary>
        private static IEnumerable<LootItem> Loot => Memory.Loot?.FilteredLoot;

        /// <summary>
        /// All Static Containers on the map.
        /// </summary>
        private static IEnumerable<StaticLootContainer> Containers => Memory.Loot?.StaticLootContainers;

        /// <summary>
        /// All Players in Local Game World (including dead/exfil'd) 'Player' collection.
        /// </summary>
        private static IReadOnlyCollection<Player> AllPlayers => Memory.Players;

        /// <summary>
        /// Contains all 'Hot' grenades in Local Game World, and their position(s).
        /// </summary>
        private static IReadOnlyCollection<IExplosiveItem> Explosives => Memory.Explosives;

        /// <summary>
        /// Contains all 'Exfils' in Local Game World, and their status/position(s).
        /// </summary>
        private static IReadOnlyCollection<IExitPoint> Exits => Memory.Exits;

        /// <summary>
        /// Contains all 'mouse-overable' items.
        /// </summary>
        private IEnumerable<IMouseoverEntity> MouseOverItems
        {
            get
            {
                var players = AllPlayers
                                  .Where(x => x is not Tarkov.EFTPlayer.LocalPlayer
                                              && !x.HasExfild && (LootCorpsesVisible ? x.IsAlive : true))
                              ?? Enumerable.Empty<Player>();

                var loot = Loot ?? Enumerable.Empty<IMouseoverEntity>();
                var containers = Containers ?? Enumerable.Empty<IMouseoverEntity>();
                var exits = Exits ?? Enumerable.Empty<IMouseoverEntity>();
                var questZones = Memory.QuestManager?.LocationConditions ?? Enumerable.Empty<IMouseoverEntity>();

                if (FilterIsSet && !checkBox_HideCorpses.Checked) // Item Search
                    players = players.Where(x =>
                        x.LootObject is null || !loot.Contains(x.LootObject)); // Don't show both corpse objects

                var result = loot.Concat(containers).Concat(players).Concat(exits).Concat(questZones);
                return result.Any() ? result : null;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// GUI Constructor.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Size = Config.WindowSize;
            if (Config.WindowMaximized)
                this.WindowState = FormWindowState.Maximized;
            SetDarkMode(ref _darkmode);
            SetControlTooltips();
            RadarColorOptions.LoadColors(Config);
            EspColorOptions.LoadColors(Config);
            SetUiEventHandlers();
            PopulateComboBoxes();
            LoadHotkeyManager();
            SetupDataGrids();
            SetMemWriteFeatures();
            SetUiValues();
            var interval = TimeSpan.FromMilliseconds(1000d / Config.RadarTargetFPS);
            _renderTimer = new(interval);
            Shown += MainForm_Shown;
        }

        private void TrackBar_ContainerDist_ValueChanged(object sender, EventArgs e)
        {
            int amt = trackBar_ContainerDist.Value;
            label_ContainerDist.Text = $"Container Dist: {amt}";
            Config.ContainerDrawDistance = amt;
        }

        #endregion

        #region Render

        /// <summary>
        /// Purge SkiaSharp Resources.
        /// </summary>
        public void PurgeSKResources()
        {
            this.Invoke(() =>
            {
                skglControl_Radar?.GRContext?.PurgeResources();
            });
        }

        private void RenderTimer_Elapsed(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                skglControl_Radar.Invalidate();
            });
        }


        /// <summary>
        /// Main Render Event.
        /// </summary>
        private void Radar_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var isStarting = Starting;
            var isReady = Ready; // cache bool
            var inRaid = InRaid; // cache bool
            var localPlayer = LocalPlayer; // cache ref to current player
            var canvas = e.Surface.Canvas; // get Canvas reference to draw on
            try
            {
                SetFPS(inRaid);
                SetMapName();
                /// Check for map switch
                var mapID = MapID; // Cache ref
                if (!mapID.Equals(LoneMapManager.Map?.ID, StringComparison.OrdinalIgnoreCase)) // Map changed
                {
                    LoneMapManager.LoadMap(mapID);
                }
                canvas.Clear(); // Clear canvas
                if (inRaid && localPlayer is not null) // LocalPlayer is in a raid -> Begin Drawing...
                {
                    var map = LoneMapManager.Map; // Cache ref
                    ArgumentNullException.ThrowIfNull(map, nameof(map));
                    var closestToMouse = _mouseOverItem; // cache ref
                    var mouseOverGrp = MouseoverGroup; // cache value for entire render
                                                       // Get LocalPlayer location
                    var localPlayerPos = localPlayer.Position;
                    var localPlayerMapPos = localPlayerPos.ToMapPos(map.Config);
                    if (groupBox_MapSetup.Visible) // Print coordinates for Map Setup Helper (if visible)
                        label_Pos.Text = $"Unity X,Y,Z: {localPlayerPos.X},{localPlayerPos.Y},{localPlayerPos.Z}";
                    // Prepare to draw Game Map
                    LoneMapParams mapParams; // Drawing Source
                    if (checkBox_MapFree.Checked) // Map fixed location, click to pan map
                        mapParams = map.GetParameters(skglControl_Radar, _zoom, ref _mapPanPosition);
                    else
                        mapParams = map.GetParameters(skglControl_Radar, _zoom, ref localPlayerMapPos); // Map auto follow LocalPlayer
                    var mapCanvasBounds = new SKRect() // Drawing Destination
                    {
                        Left = skglControl_Radar.Left,
                        Right = skglControl_Radar.Right,
                        Top = skglControl_Radar.Top,
                        Bottom = skglControl_Radar.Bottom
                    };
                    // Draw Map
                    map.Draw(canvas, localPlayer.Position.Y, mapParams.Bounds, mapCanvasBounds);
                    // Draw LocalPlayer
                    localPlayer.Draw(canvas, mapParams, localPlayer);
                    // Draw other players
                    var allPlayers = AllPlayers?
                        .Where(x => !x.HasExfild); // Skip exfil'd players
                    if (Config.ShowLoot) // Draw loot (if enabled)
                    {
                        var loot = Loot?.Reverse(); // Draw important loot last (on top)
                        if (loot is not null)
                        {
                            foreach (var item in loot)
                            {
                                if (checkBox_HideCorpses.Checked && item is LootCorpse)
                                    continue;
                                item.Draw(canvas, mapParams, localPlayer);
                            }
                        }
                        if (Config.Containers.Show) // Draw Containers
                        {
                            var containers = Containers;
                            if (containers is not null)
                            {
                                foreach (var container in containers)
                                {
                                    if (ContainerIsTracked(container.ID ?? "NULL"))
                                    {
                                        if (Config.Containers.HideSearched && container.Searched)
                                        {
                                            continue;
                                        }
                                        container.Draw(canvas, mapParams, localPlayer);
                                    }
                                }
                            }
                        } // end containers
                    } // end loot

                    if (Config.QuestHelper.Enabled)
                    {
                        var questItems = Loot?.Where(x => x is QuestItem);
                        if (questItems is not null)
                            foreach (var item in questItems)
                                item.Draw(canvas, mapParams, localPlayer);
                        var questLocations = Memory.QuestManager?.LocationConditions;
                        if (questLocations is not null)
                            foreach (var loc in questLocations)
                                loc.Draw(canvas, mapParams, localPlayer);
                    } // End QuestHelper

                    if (checkBox_ShowMines.Checked &&
                    GameData.Mines.TryGetValue(mapID, out var mines)) // Draw Mines
                    {
                        foreach (ref var mine in mines.Span)
                        {
                            var mineZoomedPos = mine.ToMapPos(map.Config).ToZoomedPos(mapParams);
                            mineZoomedPos.DrawMineMarker(canvas);
                        }
                    }

                    var explosives = Explosives; // cache ref
                    if (explosives is not null) // Draw grenades
                    {
                        foreach (var explosive in explosives)
                        {
                            explosive.Draw(canvas, mapParams, localPlayer);
                        }
                    } // end grenades

                    var exits = Exits; // cache ref
                    if (exits is not null)
                    {
                        foreach (var exit in exits)
                        {
                            if (exit is Exfil exfil && !localPlayer.IsPmc && exfil.Status is Exfil.EStatus.Closed)
                            {
                                continue; // Only draw available SCAV Exfils
                            }
                            exit.Draw(canvas, mapParams, localPlayer);
                        } // end exfils
                    }

                    if (allPlayers is not null)
                        foreach (var player in allPlayers) // Draw PMCs
                        {
                            if (player == localPlayer)
                                continue; // Already drawn local player, move on
                            player.Draw(canvas, mapParams, localPlayer);
                        } // end ForEach (allPlayers)

                    // End allPlayers not null
                    if (checkBox_GrpConnect.Checked) // Connect Groups together
                    {
                        var groupedPlayers = allPlayers?
                            .Where(x => x.IsHumanHostileActive && x.GroupID != -1);
                        if (groupedPlayers is not null)
                        {
                            var groups = groupedPlayers.Select(x => x.GroupID).ToHashSet();
                            foreach (var grp in groups)
                            {
                                var grpMembers = groupedPlayers.Where(x => x.GroupID == grp);
                                if (grpMembers is not null && grpMembers.Any())
                                {
                                    var combinations = grpMembers
                                        .SelectMany(x => grpMembers, (x, y) =>
                                            Tuple.Create(
                                                x.Position.ToMapPos(map.Config).ToZoomedPos(mapParams),
                                                y.Position.ToMapPos(map.Config).ToZoomedPos(mapParams)));
                                    foreach (var pair in combinations)
                                        canvas.DrawLine(pair.Item1.X, pair.Item1.Y,
                                            pair.Item2.X, pair.Item2.Y, SKPaints.PaintConnectorGroup);
                                }
                            }
                        }
                    } // End Grp Connect

                    if (allPlayers is not null &&
                        checkBox_ShowInfoTab.Checked) // Players Overlay
                        _playerInfo?.Draw(canvas, localPlayer, allPlayers);
                    closestToMouse?.DrawMouseover(canvas, mapParams, localPlayer);// draw tooltip for object the mouse is closest to

                    if (Config.ESPWidgetEnabled)
                        _aimview?.Draw(canvas);
                }
                else // LocalPlayer is *not* in a Raid -> Display Reason
                {
                    if (!isStarting)
                        GameNotRunningStatus(canvas);
                    else if (isStarting && !isReady)
                        StartingUpStatus(canvas);
                    else if (!inRaid)
                        WaitingForRaidStatus(canvas);
                }
                SetStatusText(canvas);
                canvas.Flush(); // commit frame to GPU
            }
            catch (Exception ex) // Log rendering errors
            {
                LoneLogging.WriteLine($"CRITICAL RENDER ERROR: {ex}");
            }
        }

        private readonly Stopwatch _statusSw = Stopwatch.StartNew();
        private int _statusOrder = 1;

        private void IncrementStatus()
        {
            if (_statusSw.Elapsed.TotalSeconds >= 1d)
            {
                if (_statusOrder == 3)
                    _statusOrder = 1;
                else
                    _statusOrder++;
                _statusSw.Restart();
            }
        }

        private void GameNotRunningStatus(SKCanvas canvas)
        {
            const string notRunning = "Game Process Not Running!";
            float textWidth = SKPaints.TextRadarStatus.MeasureText(notRunning);
            canvas.DrawText(notRunning, (skglControl_Radar.Width / 2) - textWidth / 2f, skglControl_Radar.Height / 2,
                SKPaints.TextRadarStatus);
            IncrementStatus();
        }
        private void StartingUpStatus(SKCanvas canvas)
        {
            const string startingUp1 = "Starting Up.";
            const string startingUp2 = "Starting Up..";
            const string startingUp3 = "Starting Up...";
            string status = _statusOrder == 1 ?
                startingUp1 : _statusOrder == 2 ?
                startingUp2 : startingUp3;
            float textWidth = SKPaints.TextRadarStatus.MeasureText(startingUp1);
            canvas.DrawText(status, (skglControl_Radar.Width / 2) - textWidth / 2f, skglControl_Radar.Height / 2,
                SKPaints.TextRadarStatus);
            IncrementStatus();
        }
        private void WaitingForRaidStatus(SKCanvas canvas)
        {
            const string waitingFor1 = "Waiting for Raid Start.";
            const string waitingFor2 = "Waiting for Raid Start..";
            const string waitingFor3 = "Waiting for Raid Start...";
            string status = _statusOrder == 1 ?
                waitingFor1 : _statusOrder == 2 ?
                waitingFor2 : waitingFor3;
            float textWidth = SKPaints.TextRadarStatus.MeasureText(waitingFor1);
            canvas.DrawText(status, (skglControl_Radar.Width / 2) - textWidth / 2f, skglControl_Radar.Height / 2,
                SKPaints.TextRadarStatus);
            IncrementStatus();
        }

        #endregion

        #region Event

        private void CheckedListBox_QuestHelper_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                if (checkedListBox_QuestHelper.Items[e.Index] is QuestListItem item)
                {
                    Config.QuestHelper.BlacklistedQuests.Remove(item.Id.ToLower());
                }
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                if (checkedListBox_QuestHelper.Items[e.Index] is QuestListItem item)
                {
                    Config.QuestHelper.BlacklistedQuests.Add(item.Id.ToLower());
                }
            }
        }

        private void checkBox_LootWishlist_CheckedChanged(object sender, EventArgs e)
        {
            Config.LootWishlist = checkBox_LootWishlist.Checked;
        }

        private void checkBox_AIAimlines_CheckedChanged(object sender, EventArgs e)
        {
            Config.AIAimlines = checkBox_AIAimlines.Checked;
        }

        private void checkBox_AntiPage_CheckedChanged(object sender, EventArgs e)
        {
            Program.Config.MemWrites.AntiPage = checkBox_AntiPage.Checked;
        }

        private void radioButton_Loot_FleaPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Loot_FleaPrice.Checked)
                Config.LootPriceMode = LootPriceMode.FleaMarket;
        }

        private void radioButton_Loot_VendorPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Loot_VendorPrice.Checked)
                Config.LootPriceMode = LootPriceMode.Trader;
        }

        private void textBox_VischeckVisColor_TextChanged(object sender, EventArgs e)
        {
            Chams.Config.VisibleColor = textBox_VischeckVisColor.Text;
        }

        private void textBox_VischeckInvisColor_TextChanged(object sender, EventArgs e)
        {
            Chams.Config.InvisibleColor = textBox_VischeckInvisColor.Text;
        }

        private void button_VischeckVisColorPick_Click(object sender, EventArgs e)
        {
            if (colorPicker1.ShowDialog() is DialogResult.OK)
            {
                textBox_VischeckVisColor.Text = colorPicker1.Color.ToSKColor().ToString();
            }
        }

        private void button_VischeckInvisColorPick_Click(object sender, EventArgs e)
        {
            if (colorPicker1.ShowDialog() is DialogResult.OK)
            {
                textBox_VischeckInvisColor.Text = colorPicker1.Color.ToSKColor().ToString();
            }
        }

        private void checkBox_FastLoadUnload_CheckedChanged(object sender, EventArgs e)
        {
            MemPatchFeature<FastLoadUnload>.Instance.Enabled = checkBox_FastLoadUnload.Checked;
        }
        private void checkBox_FastWeaponOps_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<FastWeaponOps>.Instance.Enabled = checkBox_FastWeaponOps.Checked;
        }

        private void checkBox_AdvancedMemWrites_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_AdvancedMemWrites.Checked;
            ToggleAdvMemwriteFeatures(enabled);
            if (enabled) // Enable Memory Writing
            {
                var dlg = MessageBox.Show(
                    "Are you sure you want to enable Advanced Memory Writing? This uses a riskier injection technique than regular Mem Write Features.",
                    "Enable Advanced Mem Writes?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg is DialogResult.Yes)
                {
                    MemWrites.Config.AdvancedMemWrites = enabled;
                }
                else
                    checkBox_AdvancedMemWrites.Checked = false;
            }
            else // Disable Memory Writing
            {
                MemWrites.Config.AdvancedMemWrites = false;
            }
        }

        private void ToggleAdvMemwriteFeatures(bool enabled)
        {
            radioButton_Chams_Vischeck.Enabled = enabled;
            radioButton_Chams_Visible.Enabled = enabled;
            checkBox_AntiPage.Enabled = enabled;
        }
        private void checkBox_FullBright_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<FullBright>.Instance.Enabled = checkBox_FullBright.Checked;
        }
        private async void button_GymHack_Click(object sender, EventArgs e)
        {
            string original = button_GymHack.Text;
            button_GymHack.Text = "Please Wait...";
            button_GymHack.Enabled = false;
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                await MemPatchFeature<GymPatch>.Instance.Apply(cts.Token);
                MessageBox.Show("Gym Hack is Set! This will remain set until the game closes.",
                    Program.Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR Setting Gym Hack! Make sure you started a workout in the 15 second window, otherwise your memory may be paged out. " +
                    "Reopen your game and try again.\n\n" +
                    $"{ex}",
                    Program.Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_GymHack.Text = original;
                button_GymHack.Enabled = true;
            }
        }
        private void checkBox_Containers_HideSearched_CheckedChanged(object sender, EventArgs e)
        {
            Config.Containers.HideSearched = checkBox_Containers_HideSearched.Checked;
        }

        private void checkBox_ShowContainers_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_ShowContainers.Checked;
            Config.Containers.Show = enabled;
            flowLayoutPanel_Loot_Containers.Enabled = enabled;
        }
        private void TrackBar_LTWAmount_ValueChanged(object sender, EventArgs e)
        {
            int value = trackBar_LTWAmount.Value;
            MemWrites.Config.LootThroughWalls.ZoomAmount = value;
            float scaledAmt = value * 0.01f;
            label_LTWAmount.Text = $"Zoom Amount {scaledAmt.ToString("0.00")}";
        }

        private void checkBox_LTW_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_LTW.Checked;
            flowLayoutPanel_LTW.Enabled = enabled;
            MemWriteFeature<LootThroughWalls>.Instance.Enabled = enabled;
        }

        private void checkBox_MoveSpeed_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_MoveSpeed.Checked;
            MemWriteFeature<MoveSpeed>.Instance.Enabled = enabled;
        }

        private void TrackBar_WideLeanAmt_ValueChanged(object sender, EventArgs e)
        {
            int value = trackBar_WideLeanAmt.Value;
            MemWrites.Config.WideLean.Amount = value;
            float scaledAmt = value * 0.01f;
            label_WideLeanAmt.Text = $"Amount {scaledAmt.ToString("0.00")}";
        }
        private void comboBox_WideLeanMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_WideLeanMode.SelectedItem is HotkeyModeListItem item)
            {
                MemWrites.Config.WideLean.Mode = item.Mode;
            }
        }

        private void checkBox_NoWepMalf_CheckedChanged(object sender, EventArgs e)
        {
            MemPatchFeature<NoWepMalfPatch>.Instance.Enabled = checkBox_NoWepMalf.Checked;
        }
        private async void button_AntiAfk_Click(object sender, EventArgs e)
        {
            button_AntiAfk.Text = "Please Wait...";
            button_AntiAfk.Enabled = false;
            try
            {
                await Task.Run(() => // Run on non ui thread
                {
                    MemWriteFeature<AntiAfk>.Instance.Set();
                });
                MessageBox.Show("Anti-AFK is Set!\n\n" +
                    "NOTE: If you leave the Main Menu, you may need to re-set this.",
                    Program.Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR Setting Anti-AFK! Your memory may be paged out, try close and re-open the game and try again.\n\n" +
                    $"{ex}",
                    Program.Name,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_AntiAfk.Text = "Anti-AFK";
                button_AntiAfk.Enabled = true;
            }
        }
        private void checkBox_RageMode_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<RageMode>.Instance.Enabled = checkBox_RageMode.Checked;
        }
        private void checkBox_AimRandomBone_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_AimRandomBone.Checked;
            Aimbot.Config.RandomBone.Enabled = enabled;
            button_RandomBoneCfg.Enabled = enabled;
            comboBox_AimbotTarget.Enabled = !enabled;
        }

        private void button_RandomBoneCfg_Click(object sender, EventArgs e)
        {
            using var form = new AimbotRandomBoneForm();
            var dlg = form.ShowDialog();
            if (!Aimbot.Config.RandomBone.Is100Percent)
                Aimbot.Config.RandomBone.ResetDefaults();
        }
        private void checkBox_SA_AutoBone_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.SilentAim.AutoBone = checkBox_SA_AutoBone.Checked;
        }

        private void checkBox_HeadAI_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.HeadshotAI = checkBox_AimHeadAI.Checked;
        }

        private void checkBox_SA_SafeLock_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.SilentAim.SafeLock = checkBox_SA_SafeLock.Checked;
        }

        private void checkBox_TeammateAimlines_CheckedChanged(object sender, EventArgs e)
        {
            Config.TeammateAimlines = checkBox_TeammateAimlines.Checked;
        }
        private void PlayerWatchlist_Validating(object sender, CancelEventArgs e)
        {
            Config.Save();
            e.Cancel = false;
        }

        private void checkBox_HideNames_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_HideNames.Checked;
            Config.HideNames = enabled;
        }

        private void TrackBar_AimlineLength_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_AimlineLength.Value;
            Config.AimLineLength = value;
        }

        private void radioButton_Chams_Normal_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = radioButton_Chams_Basic.Checked;
            if (enabled)
                Chams.Config.Mode = ChamsManager.ChamsMode.Basic;
        }

        private void radioButton_Chams_Vischeck_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = radioButton_Chams_Vischeck.Checked;
            if (enabled)
                Chams.Config.Mode = ChamsManager.ChamsMode.VisCheck;
            flowLayoutPanel_AdvancedChams.Enabled = enabled;
        }

        private void radioButton_Chams_Visible_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = radioButton_Chams_Visible.Checked;
            if (enabled)
                Chams.Config.Mode = ChamsManager.ChamsMode.Visible;
            flowLayoutPanel_AdvancedChams.Enabled = enabled;
        }

        private void button_BackupConfig_Click(object sender, EventArgs e)
        {
            button_BackupConfig.Enabled = false;
            try
            {
                const string backupFile = Config.Filename + ".bak";
                if (File.Exists(backupFile))
                {
                    var prompt = MessageBox.Show(this, "A Config Backup already exists, would you like to overwrite it?",
                        Program.Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (prompt is not DialogResult.Yes)
                        return;
                }

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var json = JsonSerializer.Serialize<Config>(Config, options);
                File.WriteAllText(backupFile, json);
                MessageBox.Show(this, $"Config backed up successfully to {backupFile}!", Program.Name, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"ERROR Backing up Config!\n\n{ex.Message}", Program.Name, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_BackupConfig.Enabled = true;
            }
        }

        private void checkBox_AimbotDisableReLock_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.DisableReLock = checkBox_AimbotDisableReLock.Checked;
        }

        /// <summary>
        /// Waits for GR Context to be set.
        /// </summary>
        private async void MainForm_Shown(object sender, EventArgs e)
        {
            while (!this.IsHandleCreated)
                await Task.Delay(25);
            Window ??= this;
            _renderTimer.Start();
            _fpsSw.Start(); // start render stopwatch
            while (skglControl_Radar.GRContext is null)
                await Task.Delay(25);
            skglControl_Radar.GRContext.SetResourceCacheLimit(536870912); // 512 MB
            SetupWidgets();
            /// Begin Render
            skglControl_Radar.PaintSurface += Radar_PaintSurface;
            _renderTimer.Elapsed += RenderTimer_Elapsed;
        }

        /// <summary>
        /// Event fires when switching Tab Pages.
        /// </summary>
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Validation for Data Sources
            tabControl1.TabPages[tabControl1.SelectedIndex].Focus();
            tabControl1.TabPages[tabControl1.SelectedIndex].CausesValidation = true;
            if (tabControl1.SelectedIndex == 1) // Settings
            {
                RefreshQuestHelper();
            }
            else if (tabControl1.SelectedIndex == 2) // Player Loadouts Tab
            {
                richTextBox_PlayersInfo.Clear();
                var enemyPlayers = AllPlayers?
                    .Where(x => x.IsHostilePmc && x.IsActive && x.IsAlive)
                    .OrderBy(x => x.Name);
                if (InRaid && enemyPlayers is not null)
                {
                    var sb = new StringBuilder();
                    sb.Append(@"{\rtf1\ansi");
                    foreach (var player in enemyPlayers)
                    {
                        var name = checkBox_HideNames.Checked && player.IsHuman ? "<Hidden>" : player.Name;
                        var faction = player.PlayerSide.ToString();
                        var title = $"*** {name} ({player.Type.GetDescription()} {faction})";
                        if (player.GroupID != -1) title += $" G:{player.GroupID}";
                        sb.Append(@$"\b {title} \b0 ");
                        sb.Append(@" \line ");
                        var gear = player.Gear; // cache ref
                        if (gear is not null)
                        {
                            var playerValue = TarkovMarketItem.FormatPrice(gear.Value);
                            sb.Append(@"\b Value: \b0 ");
                            sb.Append(playerValue); // print player loot/gear value
                            sb.Append(@" \line ");
                            var inHands = player.Hands?.CurrentItem;
                            if (inHands is not null)
                            {
                                sb.Append(@"\b In Hands: \b0 ");
                                sb.Append(inHands); // print item in hands
                                sb.Append(@" \line ");
                            }

                            foreach (var slot in gear.Equipment)
                            {
                                sb.Append(@$"\b {slot.Key}: \b0 ");
                                sb.Append(slot.Value.Long); // Use long item name
                                sb.Append(@" \line ");
                            }
                        }
                        else
                        {
                            sb.Append(@" ERROR retrieving gear \line");
                        }

                        sb.Append(@" \line ");
                    }

                    sb.Append(@"}");
                    richTextBox_PlayersInfo.Rtf = sb.ToString();
                }
            }
            Config.Save();
        }

        /// <summary>
        /// Fired when loot is toggled.
        /// </summary>
        private void checkBox_Loot_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_Loot.Checked;
            Config.ShowLoot = enabled;
            button_Loot.Visible = enabled;
            flowLayoutPanel_Loot.Visible = false;
            button_Loot.Enabled = true;
        }

        /// <summary>
        /// Fired when UI Scale Trackbar is Adjusted
        /// </summary>
        private void TrackBar_UIScale_ValueChanged(object sender, EventArgs e)
        {
            var newScale = .01f * trackBar_UIScale.Value;
            Config.UIScale = newScale;
            label_UIScale.Text = $"UI Scale {newScale.ToString("n2")}";
            // Update Widgets
            _aimview?.SetScaleFactor(newScale);
            _playerInfo?.SetScaleFactor(newScale);

            #region UpdatePaints

            /// Outlines
            SKPaints.TextOutline.TextSize = 12f * newScale;
            SKPaints.TextOutline.StrokeWidth = 2f * newScale;
            // Shape Outline is computed before usage due to different stroke widths

            SKPaints.PaintConnectorGroup.StrokeWidth = 2.25f * newScale;
            SKPaints.PaintMouseoverGroup.StrokeWidth = 3 * newScale;
            SKPaints.TextMouseoverGroup.TextSize = 12 * newScale;
            SKPaints.PaintLocalPlayer.StrokeWidth = 3 * newScale;
            SKPaints.TextLocalPlayer.TextSize = 12 * newScale;
            SKPaints.PaintTeammate.StrokeWidth = 3 * newScale;
            SKPaints.TextTeammate.TextSize = 12 * newScale;
            SKPaints.PaintPMC.StrokeWidth = 3 * newScale;
            SKPaints.TextPMC.TextSize = 12 * newScale;
            SKPaints.PaintWatchlist.StrokeWidth = 3 * newScale;
            SKPaints.TextWatchlist.TextSize = 12 * newScale;
            SKPaints.PaintStreamer.StrokeWidth = 3 * newScale;
            SKPaints.TextStreamer.TextSize = 12 * newScale;
            SKPaints.PaintAimbotLocked.StrokeWidth = 3 * newScale;
            SKPaints.TextAimbotLocked.TextSize = 12 * newScale;
            SKPaints.PaintAimbotLocked.StrokeWidth = 3 * newScale;
            SKPaints.TextAimbotLocked.TextSize = 12 * newScale;
            SKPaints.PaintScav.StrokeWidth = 3 * newScale;
            SKPaints.TextScav.TextSize = 12 * newScale;
            SKPaints.PaintRaider.StrokeWidth = 3 * newScale;
            SKPaints.TextRaider.TextSize = 12 * newScale;
            SKPaints.PaintBoss.StrokeWidth = 3 * newScale;
            SKPaints.TextBoss.TextSize = 12 * newScale;
            SKPaints.PaintFocused.StrokeWidth = 3 * newScale;
            SKPaints.TextFocused.TextSize = 12 * newScale;
            SKPaints.PaintPScav.StrokeWidth = 3 * newScale;
            SKPaints.TextPScav.TextSize = 12 * newScale;
            SKPaints.TextMouseover.TextSize = 12 * newScale;
            SKPaints.PaintCorpse.StrokeWidth = 3 * newScale;
            SKPaints.TextCorpse.TextSize = 12 * newScale;
            SKPaints.PaintMeds.StrokeWidth = 3 * newScale;
            SKPaints.TextMeds.TextSize = 12 * newScale;
            SKPaints.PaintFood.StrokeWidth = 3 * newScale;
            SKPaints.TextFood.TextSize = 12 * newScale;
            SKPaints.PaintBackpacks.StrokeWidth = 3 * newScale;
            SKPaints.TextBackpacks.TextSize = 12 * newScale;
            SKPaints.PaintQuestItem.StrokeWidth = 3 * newScale;
            SKPaints.TextQuestItem.TextSize = 12 * newScale;
            SKPaints.PaintWishlistItem.StrokeWidth = 3 * newScale;
            SKPaints.TextWishlistItem.TextSize = 12 * newScale;
            SKPaints.QuestHelperPaint.StrokeWidth = 3 * newScale;
            SKPaints.QuestHelperText.TextSize = 12 * newScale;
            SKPaints.PaintDeathMarker.StrokeWidth = 3 * newScale;
            SKPaints.PaintLoot.StrokeWidth = 3 * newScale;
            SKPaints.PaintImportantLoot.StrokeWidth = 3 * newScale;
            SKPaints.PaintContainerLoot.StrokeWidth = 3 * newScale;
            SKPaints.TextLoot.TextSize = 12 * newScale;
            SKPaints.TextImportantLoot.TextSize = 12 * newScale;
            SKPaints.PaintTransparentBacker.StrokeWidth = 1 * newScale;
            SKPaints.TextRadarStatus.TextSize = 48 * newScale;
            SKPaints.TextStatusSmall.TextSize = 13 * newScale;
            SKPaints.PaintExplosives.StrokeWidth = 3 * newScale;
            SKPaints.PaintExfilOpen.StrokeWidth = 1 * newScale;
            SKPaints.PaintExfilTransit.StrokeWidth = 1 * newScale;
            SKPaints.PaintExfilPending.StrokeWidth = 1 * newScale;
            SKPaints.PaintExfilClosed.StrokeWidth = 1 * newScale;
            SKPaints.PaintExfilInactive.StrokeWidth = 1 * newScale;

            #endregion
        }

        /// <summary>
        /// Event fires when the "Map Free" or "Map Follow" checkbox (button) is clicked on the Main Window.
        /// </summary>
        private void checkBox_MapFree_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MapFree.Checked)
            {
                checkBox_MapFree.Text = "Map Follow";
                var localPlayer = LocalPlayer;
                if (localPlayer is not null)
                {
                    var localPlayerMapPos = localPlayer.Position.ToMapPos(LoneMapManager.Map.Config);
                    _mapPanPosition = new Vector2
                    {
                        X = localPlayerMapPos.X,
                        Y = localPlayerMapPos.Y
                    };
                }
            }
            else
            {
                checkBox_MapFree.Text = "Map Free";
            }
        }

        /// <summary>
        /// Handles mouse movement on Map Canvas, specifically checks if mouse moves close to a 'Player' position.
        /// </summary>
        private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown && checkBox_MapFree.Checked)
            {
                var deltaX = -(e.X - _lastMousePosition.X);
                var deltaY = -(e.Y - _lastMousePosition.Y);

                _mapPanPosition.X += deltaX;
                _mapPanPosition.Y += deltaY;
                _lastMousePosition = e.Location; // Store the current mouse position for the next move event
            }
            else
            {
                if (!InRaid)
                {
                    ClearRefs();
                    return;
                }

                var items = MouseOverItems;
                if (items?.Any() != true)
                {
                    ClearRefs();
                    return;
                }

                var mouse = new Vector2(e.X, e.Y); // Get current mouse position in control
                var closest = items.Aggregate(
                    (x1, x2) => Vector2.Distance(x1.MouseoverPosition, mouse)
                                < Vector2.Distance(x2.MouseoverPosition, mouse)
                        ? x1
                        : x2); // Get object 'closest' to mouse position
                if (Vector2.Distance(closest.MouseoverPosition, mouse) >= 12)
                {
                    ClearRefs();
                    return;
                }

                switch (closest)
                {
                    case Player player:
                        _mouseOverItem = player;
                        if (player.IsHumanHostile
                            && player.GroupID != -1)
                            MouseoverGroup = player.GroupID; // Set group ID for closest player(s)
                        else
                            MouseoverGroup = null; // Clear Group ID
                        break;
                    case LootCorpse corpseObj:
                        _mouseOverItem = corpseObj;
                        var corpse = corpseObj.PlayerObject;
                        if (corpse is not null)
                        {
                            if (corpse.IsHumanHostile && corpse.GroupID != -1)
                                MouseoverGroup = corpse.GroupID; // Set group ID for closest player(s)
                        }
                        else
                        {
                            MouseoverGroup = null;
                        }

                        break;
                    case LootContainer ctr:
                        _mouseOverItem = ctr;
                        break;
                    case IExitPoint exit:
                        _mouseOverItem = exit;
                        MouseoverGroup = null;
                        break;
                    case QuestLocation quest:
                        _mouseOverItem = quest;
                        MouseoverGroup = null;
                        break;
                    default:
                        ClearRefs();
                        break;
                }

                void ClearRefs()
                {
                    _mouseOverItem = null;
                    MouseoverGroup = null;
                }
            }
        }

        /// <summary>
        /// Handle loading streamers on dbl click.
        /// </summary>
        private void MapCanvas_MouseDblClick(object sender, MouseEventArgs e)
        {
            if (InRaid && _mouseOverItem is Player player && player.IsStreaming) // Must be in-raid
                try
                {
                    Process.Start(new ProcessStartInfo(player.TwitchChannelURL) { UseShellExecute = true });
                }
                catch
                {
                    MessageBox.Show("Unable to open this player's Twitch. Do you have a default browser set?");
                }
        }

        /// <summary>
        /// Event fires when Map Setup box is checked/unchecked.
        /// </summary>
        private void checkBox_MapSetup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MapSetup.Checked)
            {
                groupBox_MapSetup.Visible = true;
                var currentMap = LoneMapManager.Map?.Config;
                if (currentMap is not null)
                {
                    textBox_mapX.Text = currentMap.X.ToString();
                    textBox_mapY.Text = currentMap.Y.ToString();
                    textBox_mapScale.Text = currentMap.Scale.ToString();
                }
            }
            else
            {
                groupBox_MapSetup.Visible = false;
            }
        }

        /// <summary>
        /// Event fires when Restart Game button is clicked in Settings.
        /// </summary>
        private void button_Restart_Click(object sender, EventArgs e) => Memory.RestartRadar = true;

        /// <summary>
        /// Event fires when Apply button is clicked in the "Map Setup Groupbox".
        /// </summary>
        private void button_MapSetupApply_Click(object sender, EventArgs e)
        {
            if (float.TryParse(textBox_mapX.Text, out var x) &&
                float.TryParse(textBox_mapY.Text, out var y) &&
                float.TryParse(textBox_mapScale.Text, out var scale))
            {
                var currentMap = LoneMapManager.Map?.Config;
                if (currentMap is not null)
                {
                    currentMap.X = x;
                    currentMap.Y = y;
                    currentMap.Scale = scale;
                }
                else
                {
                    MessageBox.Show(this, "No Map Loaded! Unable to apply.");
                }
            }
            else
            {
                throw new Exception("INVALID float values in Map Setup.");
            }
        }

        private void MapCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            _mouseDown = false;
        }

        private void MapCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button is MouseButtons.Left)
            {
                _lastMousePosition = e.Location;
                _mouseDown = true;
            }
        }

        /// <summary>
        /// Allows panning the map when in "Free" mode.
        /// </summary>
        private void MapCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button is MouseButtons.Right &&
                _mouseOverItem is Player player &&
                player.IsHostileActive)
                player.IsFocused = !player.IsFocused; // Toggle Focus
            if (flowLayoutPanel_Loot.Visible) // Close loot window
            {
                flowLayoutPanel_Loot.Visible = false;
                button_Loot.Enabled = true;
            }
        }

        /// <summary>
        /// Copies Player "BSG ID" to Clipboard upon double clicking History Entry.
        /// </summary>
        private void dataGridView_PlayerHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var info = dataGridView_PlayerHistory.HitTest(e.X, e.Y);
            var rowIndex = info.RowIndex;
            if (rowIndex < 0)
                return;
            var item = (PlayerHistoryEntry)dataGridView_PlayerHistory.Rows[rowIndex]?.DataBoundItem;
            if (item is not null)
            {
                var acctId = item.ID;
                if (!string.IsNullOrEmpty(acctId))
                {
                    var dlg1 = MessageBox.Show(this, $"Add {item.Name} ({acctId}) to the watchlist?", Program.Name,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dlg1 is DialogResult.Yes)
                    {
                        if (Player.PlayerWatchlist.Entries.ContainsKey(acctId))
                        {
                            MessageBox.Show(this, "Player is already in the watchlist!", Program.Name, MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            return;
                        }

                        using var input = new InputBox("Enter reason", "Enter watchlist reason below:");
                        input.ShowDialog();
                        var watchlistEntry = new PlayerWatchlistEntry
                        {
                            AcctID = acctId.Trim(),
                            Reason = input.Result
                        };
                        Player.PlayerWatchlist.ManualAdd(watchlistEntry);
                        item.Player.UpdateAlerts(input.Result);
                        dataGridView_PlayerHistory.Refresh();
                        dataGridView_PlayerHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        MessageBox.Show(this, "Entry Added!", Program.Name, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
        }

        /// <summary>
        /// Fired when 'Loot' button is pressed in main radar window.
        /// </summary>
        private void button_LootFilter_Click(object sender, EventArgs e)
        {
            button_Loot.Enabled = false;
            flowLayoutPanel_Loot.Visible = true;
        }

        /// <summary>
        /// Fired when 'Regular' Loot Value is changed.
        /// </summary>
        private void textBox_LootRegValue_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox_LootRegValue.Text, out var n))
                textBox_LootRegValue.Text = "0";
            Config.MinLootValue = int.Parse(textBox_LootRegValue.Text.Trim());
            _lootMenuTimer.Restart();
        }

        /// <summary>
        /// Fired when 'Important' Loot Value is changed.
        /// </summary>
        private void textBox_LootImpValue_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox_LootImpValue.Text, out var n))
                textBox_LootImpValue.Text = "0";
            Config.MinValuableLootValue = int.Parse(textBox_LootImpValue.Text.Trim());
            _lootMenuTimer.Restart();
        }

        /// <summary>
        /// Fired when textBox_LootFilterByName is changed.
        /// </summary>
        private void textBox_LootFilterByName_TextChanged(object sender, EventArgs e)
        {
            _lootMenuTimer.Restart();
        }

        private void checkBox_ShowMeds_CheckedChanged(object sender, EventArgs e)
        {
            LootFilter.ShowMeds = checkBox_ShowMeds.Checked;
            _lootMenuTimer.Restart();
        }

        private void checkBox_ShowFood_CheckedChanged(object sender, EventArgs e)
        {
            LootFilter.ShowFood = checkBox_ShowFood.Checked;
            _lootMenuTimer.Restart();
        }
        private void checkBox_ShowBackpacks_CheckedChanged(object sender, EventArgs e)
        {
            LootFilter.ShowBackpacks = checkBox_ShowBackpacks.Checked;
            _lootMenuTimer.Restart();
        }
        private void button_Loot_Close_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel_Loot.Visible) // Close loot window
            {
                flowLayoutPanel_Loot.Visible = false;
                button_Loot.Enabled = true;
            }
        }

        private void lootMenuTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Invoke(() =>
            {
                LootApply();
            });
        }

        private void checkBox_NoRecoil_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_NoRecoilSway.Checked;
            MemWriteFeature<NoRecoil>.Instance.Enabled = enabled;
            flowLayoutPanel_NoRecoil.Enabled = enabled;
        }

        private void checkBox_InfStamina_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<InfStamina>.Instance.Enabled = checkBox_InfStamina.Checked;
        }

        private void TrackBar_NoSway_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_NoSway.Value;
            label_Sway.Text = $"Sway {value}";
            MemWrites.Config.NoSwayAmount = value;
        }

        private void TrackBar_NoRecoil_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_NoRecoil.Value;
            label_Recoil.Text = $"Recoil {value}";
            MemWrites.Config.NoRecoilAmount = value;
        }

        private void checkBox_NoVisor_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<NoVisor>.Instance.Enabled = checkBox_NoVisor.Checked;
        }

        private void checkBox_AimBot_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_AimBotEnabled.Checked;
            MemWriteFeature<Aimbot>.Instance.Enabled = enabled;
            flowLayoutPanel_Aimbot.Enabled = enabled;
        }
        private void checkBox_EnableMemWrite_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_EnableMemWrite.Checked;
            if (enabled) // Enable Memory Writing
            {
                var dlg = MessageBox.Show(
                    "Are you sure you want to enable Memory Writing? This is riskier than using Read-Only radar features.",
                    "Enable Mem Writes?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg is DialogResult.Yes)
                {
                    MemWrites.Enabled = enabled;
                    flowLayoutPanel_MemWrites.Enabled = enabled;
                }
                else
                    checkBox_EnableMemWrite.Checked = false;
            }
            else // Disable Memory Writing
            {
                MemWrites.Enabled = false;
                flowLayoutPanel_MemWrites.Enabled = false;
            }
        }

        /// <summary>
        /// Loads the Hotkey Manager GUI.
        /// </summary>
        private void button_HotkeyManager_Click(object sender, EventArgs e)
        {
            button_HotkeyManager.Enabled = false;
            try
            {
                using var hkMgr = new HotkeyManager();
                hkMgr.ShowDialog();
                Config.Save();
            }
            finally
            {
                button_HotkeyManager.Enabled = true;
            }
        }

        private void comboBox_AimbotTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_AimbotTarget.SelectedItem is BonesListItem entry) Aimbot.Config.Bone = entry.Bone;
        }

        private void TrackBar_MaxDist_ValueChanged(object sender, EventArgs e)
        {
            Config.MaxDistance = trackBar_MaxDist.Value;
            label_MaxDist.Text = $"Max Dist {trackBar_MaxDist.Value}";
        }

        private void checkBox_QuestHelper_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_QuestHelper_Enabled.Checked;
            Config.QuestHelper.Enabled = enabled;
            checkedListBox_QuestHelper.Enabled = enabled;
            RefreshQuestHelper();
        }

        private void checkBox_Chams_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_Chams.Checked;
            MemWriteFeature<Chams>.Instance.Enabled = enabled;
            flowLayoutPanel_Chams.Enabled = enabled;
        }

        private void checkBox_AlwaysDay_CheckedChanged(object sender, EventArgs e)
        {
            MemWriteFeature<AlwaysDaySunny>.Instance.Enabled = checkBox_AlwaysDaySunny.Checked;
        }

        private void radioButton_AimbotDefaultMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AimTarget_FOV.Checked)
                Aimbot.Config.TargetingMode = Aimbot.AimbotTargetingMode.FOV;
        }

        private void radioButton_AimbotCQBMode_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AimTarget_CQB.Checked)
                Aimbot.Config.TargetingMode = Aimbot.AimbotTargetingMode.CQB;
        }

        private void TrackBar_AimFOV_ValueChanged(object sender, EventArgs e)
        {
            float fov = trackBar_AimFOV.Value; // Cache value
            Aimbot.Config.FOV = fov; // Set Global
            label_AimFOV.Text = $"FOV {(int)fov}";
        }

        private void checkBox_LootPPS_CheckedChanged(object sender, EventArgs e)
        {
            Config.LootPPS = checkBox_LootPPS.Checked;
            _lootMenuTimer.Restart();
        }

        private void checkBox_HideCorpses_CheckedChanged(object sender, EventArgs e)
        {
            Config.HideCorpses = checkBox_HideCorpses.Checked;
            _lootMenuTimer.Restart();
        }

        private void checkBox_Aimview_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESPWidgetEnabled = checkBox_Aimview.Checked;
        }

        private void button_Radar_ColorPicker_Click(object sender, EventArgs e)
        {
            button_Radar_ColorPicker.Enabled = false;
            try
            {
                using var cp =
                    new ColorPicker<RadarColorOption, ColorItem<RadarColorOption>>("Radar Color Picker", Config.Colors);
                cp.ShowDialog();
                if (cp.DialogResult is DialogResult.OK && cp.Result is not null)
                {
                    RadarColorOptions.SetColors(cp.Result);
                    Config.Colors = cp.Result;
                    Config.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR Updating Colors! {ex.Message}", "Radar Color Picker", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_Radar_ColorPicker.Enabled = true;
            }
        }

        private void checkBox_WideLean_CheckedChanged(object sender, EventArgs e)
        {
            bool enabled = checkBox_WideLean.Checked;
            MemWriteFeature<WideLean>.Instance.Enabled = enabled;
            flowLayoutPanel_WideLean.Enabled = enabled;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh quest helper (if enabled).
        /// </summary>
        private void RefreshQuestHelper()
        {
            if (Config.QuestHelper.Enabled && Memory.InRaid && Memory.QuestManager is QuestManager quests)
            {
                var currentList = checkedListBox_QuestHelper.Items.Cast<QuestListItem>().ToArray();
                foreach (var quest in quests.CurrentQuests)
                {
                    if (!currentList.Any(x => x.Id.Equals(quest, StringComparison.OrdinalIgnoreCase)))
                    {
                        bool enabled = !Config.QuestHelper.BlacklistedQuests.Contains(quest, StringComparer.OrdinalIgnoreCase);
                        checkedListBox_QuestHelper.Items.Add(new QuestListItem(quest), enabled);
                    }
                }
                foreach (var existing in currentList)
                {
                    if (!quests.CurrentQuests.Contains(existing.Id))
                        checkedListBox_QuestHelper.Items.Remove(existing);
                }
            }
        }

        /// <summary>
        /// Set Dark Mode on startup.
        /// </summary>
        /// <param name="darkmode"></param>
        private void SetDarkMode(ref DarkModeCS darkmode)
        {
            darkmode = new DarkModeCS(this);
            if (darkmode.IsDarkMode)
            {
                SharedPaints.PaintBitmap.ColorFilter = SharedPaints.GetDarkModeColorFilter(0.7f);
                SharedPaints.PaintBitmapAlpha.ColorFilter = SharedPaints.GetDarkModeColorFilter(0.7f);
            }
        }

        /// <summary>
        /// Set Event Handlers on Startup.
        /// </summary>
        private void SetUiEventHandlers()
        {
            checkedListBox_QuestHelper.ItemCheck += CheckedListBox_QuestHelper_ItemCheck;
            trackBar_AimlineLength.ValueChanged += TrackBar_AimlineLength_ValueChanged;
            trackBar_UIScale.ValueChanged += TrackBar_UIScale_ValueChanged;
            trackBar_MaxDist.ValueChanged += TrackBar_MaxDist_ValueChanged;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            skglControl_Radar.MouseMove += MapCanvas_MouseMove;
            skglControl_Radar.MouseClick += MapCanvas_MouseClick;
            skglControl_Radar.MouseDown += MapCanvas_MouseDown;
            skglControl_Radar.MouseUp += MapCanvas_MouseUp;
            skglControl_Radar.MouseDoubleClick += MapCanvas_MouseDblClick;
            dataGridView_PlayerHistory.MouseDoubleClick += dataGridView_PlayerHistory_MouseDoubleClick;
            trackBar_AimFOV.ValueChanged += TrackBar_AimFOV_ValueChanged;
            trackBar_NoRecoil.ValueChanged += TrackBar_NoRecoil_ValueChanged;
            trackBar_NoSway.ValueChanged += TrackBar_NoSway_ValueChanged;
            trackBar_WideLeanAmt.ValueChanged += TrackBar_WideLeanAmt_ValueChanged;
            trackBar_LTWAmount.ValueChanged += TrackBar_LTWAmount_ValueChanged;
            _lootFiltersItemSearchTimer.Elapsed += impLootSearchTimer_Elapsed;
            _lootMenuTimer.Elapsed += lootMenuTimer_Elapsed;
        }

        /// <summary>
        /// Sets Mouseover Tooltips for Winforms Controls.
        /// </summary>
        private void SetControlTooltips()
        {
            toolTip1.SetToolTip(textBox_VischeckVisColor, "Set the VISIBLE color of the Vischeck Chams. Must be set before chams are injected.");
            toolTip1.SetToolTip(textBox_VischeckInvisColor, "Set the INVISIBLE color of the Vischeck Chams. Must be set before chams are injected.");
            toolTip1.SetToolTip(button_VischeckVisColorPick, "Set the VISIBLE color of the Vischeck Chams. Must be set before chams are injected.");
            toolTip1.SetToolTip(button_VischeckInvisColorPick, "Set the INVISIBLE color of the Vischeck Chams. Must be set before chams are injected.");
            toolTip1.SetToolTip(checkBox_FastLoadUnload, "Allows you to pack/unpack magazines super fast.");
            toolTip1.SetToolTip(checkBox_FastWeaponOps, "Makes weapon operations (instant ADS, reloading mag,etc.) faster for your player.\n" +
                "NOTE: Trying to heal or do other actions while reloading a mag can cause the 'hands busy' bug.");
            toolTip1.SetToolTip(checkBox_FullBright, "Enables the Full Bright Feature. This will make the game world brighter.");
            toolTip1.SetToolTip(checkBox_NoWepMalf, "Enables the No Weapons Malfunction feature. This prevents your gun from failing to fire due to misfires/overheating/etc.\n" +
                "Once enabled this feature will remain enabled until you restart your game.\n" +
                "Stream Safe!");
            toolTip1.SetToolTip(button_AntiAfk, "Enables the Anti-AFK Feature. Prevents the game from closing due to inactivity.\n" +
                "NOTE: Set this *right before* you go AFK while you are on the Tarkov Main Menu.\n" +
                "NOTE: If you leave the Main Menu, you may need to re-set this.\n" +
                "NOTE: If you have trouble setting this, your memory may be paged out. Try close/reopen the game.");
            toolTip1.SetToolTip(checkBox_RageMode,
                "Enables the Rage Mode feature. While enabled sets Recoil/Sway to 0% and Aimbot Bone is overriden to 'Head' for all targets.\nThis setting does not save on program exit.\n" +
                "WARNING: This is marked as a RISKY feature since it sets your recoil to 0% and you will always headshot, other players could notice.");
            toolTip1.SetToolTip(checkBox_TeammateAimlines, "When enabled makes teammate aimlines the same length as the main player.");
            toolTip1.SetToolTip(button_Radar_ColorPicker, "Allows customizing entity colors on the Radar UI.");
            toolTip1.SetToolTip(button_EspColorPicker, "Allows customizing entity colors on the Fuser ESP.");
            toolTip1.SetToolTip(button_BackupConfig, "Backs up your configuration (Recommended).");
            toolTip1.SetToolTip(checkBox_ESP_ShowMag, "Shows your currently loaded Magazine Ammo Count/Type.");
            toolTip1.SetToolTip(checkBox_ESPRender_Labels, "Display entity label/name.");
            toolTip1.SetToolTip(checkBox_ESPRender_Dist, "Display entity distance from LocalPlayer.");
            toolTip1.SetToolTip(checkBox_ESPRender_Weapons, "Display entity's held weapon/ammo.");
            toolTip1.SetToolTip(checkBox_ESPAIRender_Labels, "Display entity label/name.");
            toolTip1.SetToolTip(checkBox_ESPAIRender_Dist, "Display entity distance from LocalPlayer.");
            toolTip1.SetToolTip(checkBox_ESPAIRender_Weapons, "Display entity's held weapon/ammo.");
            toolTip1.SetToolTip(radioButton_ESPRender_None, "Do not render this entity at all.");
            toolTip1.SetToolTip(radioButton_ESPRender_Bones, "Render full entity skeletal bones.");
            toolTip1.SetToolTip(radioButton_ESPRender_Box, "Render a 'box' around this entity location.");
            toolTip1.SetToolTip(radioButton_ESPRender_Presence, "Render a 'dot' showing this entity's presence (does not scale with distance).");
            toolTip1.SetToolTip(radioButton_ESPAIRender_None, "Do not render this entity at all.");
            toolTip1.SetToolTip(radioButton_ESPAIRender_Bones, "Render full entity skeletal bones.");
            toolTip1.SetToolTip(radioButton_ESPAIRender_Box, "Render a 'box' around this entity location.");
            toolTip1.SetToolTip(radioButton_ESPAIRender_Presence, "Render a 'dot' showing this entity's presence (does not scale with distance).");
            toolTip1.SetToolTip(checkBox_MapSetup,
                "Toggles the 'Map Setup Helper' to assist with getting Map Bounds/Scaling");
            toolTip1.SetToolTip(button_Restart, "Restarts the Radar for the current raid instance.");
            toolTip1.SetToolTip(checkBox_Loot, "Toggles displaying Loot on the Radar/ESP.");
            toolTip1.SetToolTip(checkBox_Aimview,
                "Toggles the ESP 'Widget' that gives you a Mini ESP in the radar window. Can be moved.");
            toolTip1.SetToolTip(checkBox_HideNames,
                "Hides human player names from displaying in the Radar or Player Info Widget. Instead will show the (Height,Distance).");
            toolTip1.SetToolTip(checkBox_QuestHelper_Enabled,
                "Toggles the Quest Helper feature. This will display Items and Zones that you need to pickup/visit for quests that you currently have active.");
            toolTip1.SetToolTip(checkBox_ShowInfoTab,
                "Toggles the Player Info 'Widget' that gives you information about the players/bosses in your raid. Can be moved.");
            toolTip1.SetToolTip(checkBox_GrpConnect,
                "Connects players that are grouped up via semi-transparent green lines. Does not apply to your own party.");
            toolTip1.SetToolTip(trackBar_AimlineLength, "Sets the Aimline Length for Local Player/Teammates");
            toolTip1.SetToolTip(trackBar_UIScale,
                "Sets the scaling factor for the Radar/User Interface. For high resolution monitors you may want to increase this.");
            toolTip1.SetToolTip(trackBar_MaxDist,
                "Sets the 'Maximum Distance' for the Radar and many of it's features. This will affect Hostile Aimlines, Aimview, ESP, and Aimbot.\nIn most cases you don't need to set this over 500.");
            toolTip1.SetToolTip(button_HotkeyManager,
                "Opens the Hotkey Manager. This is used to create Custom Hotkeys that can be pressed IN GAME to control the Radar Application.");
            toolTip1.SetToolTip(checkBox_EnableMemWrite,
                "Enables/Disables the ability to use Memory Write Features. When this is disabled, it prevents any Memory Writing from occurring in the application.\n\n" +
                "Regarding 'Risk'\n" +
                "- The majority of risk stems from the fact that most of these features increase your power greatly, making other players more likely to report you.\n" +
                "- Player reports are the #1 risk to getting banned.\n" +
                "- None of these features are currently 'detected', but there is a VERY small risk that they could be in the future.");
            toolTip1.SetToolTip(checkBox_NoRecoilSway,
                "Enables the No Recoil/Sway Write Feature. Mouseover the Recoil/Sway sliders for more info.\n" +
                "WARNING: This is marked as a RISKY feature since it reduces your recoil/sway, other players could notice your abnormal spray patterns.");
            toolTip1.SetToolTip(trackBar_NoRecoil,
                "Sets the percentage of normal recoil to apply (ex: 0 = 0% or no recoil). This affects the up/down motion of a gun while firing.");
            toolTip1.SetToolTip(trackBar_NoSway,
                "Sets the percentage of scope sway to apply (ex: 0 = 0% or no sway). This affects the swaying motion when looking down your sights/scope.");
            toolTip1.SetToolTip(checkBox_NoVisor,
                "Enables the No Visor feature. This removes the view obstruction from certain faceshields (like the Altyn/Killa Helmet) and gives you a clear view.");
            toolTip1.SetToolTip(checkBox_Chams,
                "Enables the Chams feature. This will enable Chams on ALL players except yourself and teammates.");
            toolTip1.SetToolTip(radioButton_Chams_Basic,
                "These basic chams will only show when a target is VISIBLE. Cannot change color (always White).");
            toolTip1.SetToolTip(radioButton_Chams_Visible,
                "These advanced chams will only show when a target is VISIBLE. You can change the color(s).");
            toolTip1.SetToolTip(radioButton_Chams_Vischeck,
                "These advanced chams (vischeck) will show different colors when a target is VISIBLE/INVISIBLE. You can change the color(s).");
            toolTip1.SetToolTip(checkBox_AlwaysDaySunny,
                "Enables the Always Day/Sunny feature. This sets the In-Raid time to always 12 Noon (day), and sets the weather to sunny/clear.");
            toolTip1.SetToolTip(checkBox_AimBotEnabled,
                "Enables the Aimbot (Silent Aim) Feature. We employ Aim Prediction (Online Raids Only) with our Aimbot to compensate for bullet drop/ballistics and target movement.\n" +
                "WARNING: This is marked as a RISKY feature since it makes it more likely for other players to report you. Use with care.");
            toolTip1.SetToolTip(trackBar_AimFOV,
                "Sets the FOV for Aimbot Targeting. Increase/Lower this to your preference. Please note when you ADS/Scope in, the FOV field becomes narrower.");
            toolTip1.SetToolTip(radioButton_AimTarget_FOV,
                "Enables the FOV (Field of View) Targeting Mode for Aimbot. This will prefer the target closest to the center of your screen within your FOV.");
            toolTip1.SetToolTip(radioButton_AimTarget_CQB,
                "Enables the CQB (Close Quarters Battle) Targeting Mode for Aimbot.\nThis will prefer the target closest to your player *within your FOV*.");
            toolTip1.SetToolTip(comboBox_AimbotTarget, "Sets the Bone to target with the Aimbot.");
            toolTip1.SetToolTip(label_Width,
                "The resolution Width of your Game PC Monitor that Tarkov runs on. This must be correctly set for Aimview/Aimbot/ESP to function properly.");
            toolTip1.SetToolTip(label_Height,
                "The resolution Height of your Game PC Monitor that Tarkov runs on. This must be correctly set for Aimview/Aimbot/ESP to function properly.");
            toolTip1.SetToolTip(button_DetectRes,
                "Automatically detects the resolution of your Game PC Monitor that Tarkov runs on, and sets the Width/Height fields. Game must be running.");
            toolTip1.SetToolTip(button_StartESP,
                "Starts the ESP Window. This will render ESP over a black background. Move this window to the screen that is being fused, and double click to go Fullscreen.");
            toolTip1.SetToolTip(label_ESPFPSCap,
                "Sets an FPS Cap for the ESP Window. Generally this can be the refresh rate of your Game PC Monitor. This also helps reduce resource usage on your Radar PC.\nSetting this to 0 disables it entirely.");
            toolTip1.SetToolTip(button_EspColorPicker,
                "Opens the 'ESP Color Picker' that will allow you to specify custom colors for different ESP Elements.");
            toolTip1.SetToolTip(flowLayoutPanel_ESP_PlayerRender,
                "Sets the ESP Rendering Options for Human Players in Fuser ESP.");
            toolTip1.SetToolTip(flowLayoutPanel_ESP_AIRender, "Sets the ESP Rendering Options for AI Bots in Fuser ESP.");
            toolTip1.SetToolTip(checkBox_ESP_Exfils, "Enables the rendering of Exfil Points in the ESP Window.");
            toolTip1.SetToolTip(checkBox_ESP_Explosives, "Enables the rendering of Grenades in the ESP Window.");
            toolTip1.SetToolTip(checkBox_ESP_AimFov,
                "Enables the rendering of an 'Aim FOV Circle' in the center of your ESP Window. This is used for Aimbot Targeting.");
            toolTip1.SetToolTip(checkBox_ESP_FPS,
                "Enables the display of the ESP Rendering Rate (FPS) in the Top Left Corner of your ESP Window.");
            toolTip1.SetToolTip(checkBox_ESP_Dist,
                "Enables the rendering of 'Distance' below ESP Entities. This is the In-Game distance from yourself and the entity.");
            toolTip1.SetToolTip(checkBox_ESP_AimLock,
                "Enables the rendering of a line between your Fireport and your currently locked Aimbot Target.");
            toolTip1.SetToolTip(trackBar_EspLootDist,
                "Sets the Maximum Distance from LocalPlayer for regular loot to be rendered.");
            toolTip1.SetToolTip(trackBar_EspImpLootDist,
                "Sets the Maximum Distance from LocalPlayer for important loot to be rendered.");
            toolTip1.SetToolTip(trackBar_EspQuestHelperDist,
                "Sets the Maximum Distance from LocalPlayer for Static Quest Items/Locations to be rendered. Quest Helper must be on.");
            toolTip1.SetToolTip(trackBar_EspGrenadeDist,
                "Sets the Maximum Distance from LocalPlayer for grenades to be rendered.");
            toolTip1.SetToolTip(trackBar_EspFontScale,
                "Sets the font scaling factor for the ESP Window.\nIf you are rendering at a really high resolution, you may want to increase this.");
            toolTip1.SetToolTip(trackBar_EspLineScale,
    "Sets the lines scaling factor for the ESP Window.\nIf you are rendering at a really high resolution, you may want to increase this.");
            toolTip1.SetToolTip(checkBox_ESP_AutoFS,
                "Sets 'Auto Fullscreen' for the ESP Window.\nWhen set this will automatically go into full screen mode on the selected screen when the application starts, and when hitting the Start ESP button.");
            toolTip1.SetToolTip(comboBox_ESPAutoFS, "Sets the screen for 'Auto Fullscreen'.");
            toolTip1.SetToolTip(checkBox_AimbotDisableReLock,
                "Disables 're-locking' onto a new target with aimbot when the current target dies/is no longer valid.\n Prevents accidentally killing multiple targets in quick succession before you can react.");
            toolTip1.SetToolTip(label_ESP_HighAlert,
                "Enables the 'High Alert' ESP Feature. This will activate when you are being aimed at for longer than 0.5 seconds.\nTargets in your FOV (in front of you) will draw an aimline towards your character.\nTargets outside your FOV will draw the border of your screen red.");
            toolTip1.SetToolTip(comboBox_ESP_HighAlert,
                "None = Feature Disabled\nAllPlayers = Enabled for both players and bots (AI)\nHumansOnly = Enabled only for human-controlled players.");
            toolTip1.SetToolTip(checkBox_ESP_RaidStats,
                "Displays Raid Stats (Player counts, etc.) in top right corner of ESP window.");
            toolTip1.SetToolTip(checkBox_SA_AutoBone, "Automatically selects best bone target based on where you are aiming.");
            toolTip1.SetToolTip(checkBox_AimHeadAI, "Always headshot AI Targets regardless of other settings.");
            toolTip1.SetToolTip(checkBox_SA_SafeLock, "Unlocks the aimbot if your target leaves your FOV Radius.\n" +
                "NOTE: It is possible to 're-lock' another target (or the same target) after unlocking.");
            toolTip1.SetToolTip(checkBox_AimRandomBone, "Will select a random aimbot bone after each shot. You can set custom percentage values for body zones.\nNOTE: This will supersede silent aim 'auto bone'.");
            toolTip1.SetToolTip(button_RandomBoneCfg, "Set random bone percentages (must add up to 100%).");
            toolTip1.SetToolTip(checkBox_WebRadarUPNP, "Attempts to automatically Port Map using UPnP. If disabled, you will need to forward port(s) manually on your Router.");
            toolTip1.SetToolTip(button_WebRadarStart, "Starts the Web Radar Service.");
            toolTip1.SetToolTip(label_WebRadarHost, "Sets the IP Address/Hostname for the Web Radar Service to be bound to. Usually your LAN IP.");
            toolTip1.SetToolTip(label_WebRadarPort, "Sets the Port (TCP) for the Web Radar Service to be bound to. Recommend using a random port between 50000-60000.");
            toolTip1.SetToolTip(label_WebRadarPassword, "Randomized password for the Web Radar Service. This is used to authenticate with the Web Radar Service.");
            toolTip1.SetToolTip(label_WebRadarTickRate, "Sets the Server Tickrate for the Web Radar Service. This is how often (per second) the server updates the client with new data.");
            toolTip1.SetToolTip(linkLabel_WebRadarLink, "Web Radar Link to access the Web Radar Service. You can share this with your friend(s). Click to copy to clipboard.");
            toolTip1.SetToolTip(comboBox_WideLeanMode, "Sets the Wide Lean Mode for the Wide Lean feature.\nHold = Must press and hold the hotkey to remain leaned.\nToggle = Must press the hotkey once to toggle on/off.");
            toolTip1.SetToolTip(trackBar_WideLeanAmt, "Sets the amount of lean to apply when using the Wide Lean feature. You may need to lower this if shots fail.");
            toolTip1.SetToolTip(label_WideLean, "Wide Lean allows you to move your weapon left/right/up. This can be useful for peeking corners with silent aim. Set your hotkey(s) in Hotkey Manager.");
            toolTip1.SetToolTip(checkBox_WideLean, "Enables/Disables Wide Lean Globally. You still need to set hotkeys in Hotkey Manager.\nWARNING: This is overall a riskier write feature.");
            toolTip1.SetToolTip(checkBox_InfStamina,
                "Enables the Infinite Stamina feature. Prevents you from running out of stamina/breath, and bypasses the Fatigue debuff. Due to safety reasons you can only disable this after the raid has ended.\n" +
                "NOTE: Your footsteps will be silent, this is normal.\n" +
                "NOTE: You will not gain endurance/strength xp with this on.\n" +
                "NOTE: At higher weights you may get server desync. You can try disabling 1.2 Move Speed, or reducing your weight. MULE stims help here too.\n" +
                "WARNING: This is marked as a RISKY feature since other players can see you 'gliding' instead of running and is visually noticeable.");
            toolTip1.SetToolTip(checkBox_MoveSpeed,
                "Enables/Disables 1.2x Move Speed Feature. This causes your player to move 1.2 times faster.\n" +
                "NOTE: When used in conjunction with Infinite Stamina this can contribute to Server Desync at higher carry weights. Turn this off to reduce desync.\n" +
                "WARNING: This is marked as a RISKY feature since other players can see you moving faster than normal.");
            toolTip1.SetToolTip(checkBox_ESP_FireportAim, "Shows the base fireport trajectory on screen so you can see where bullets will go. Disappears when ADS.");
            toolTip1.SetToolTip(checkBox_ESP_StatusText, "Displays status text in the top center of the screen (Aimbot Status, Wide Lean, etc.)");
            toolTip1.SetToolTip(checkBox_LTW, "Enables Loot Through Walls Feature. This allows you to loot items through walls.\n" +
                "* You can loot most quest items / container items normally up to 3.8m.\n" +
                "* You can loot loose loot up to ~1m (may not always work either).\n" +
                "* To loot loose loot, some items you will need to 'ADS' with your firearm (Use the 'Toggle LTW Zoom' Hotkey), and it will zoom the camera through the wall. Find your item and loot it.\n" +
                "WARNING: Due to the complex nature of this feature, and the presence of Server-Side checks, it is marked as Risky.");
            toolTip1.SetToolTip(trackBar_LTWAmount, "Sets the Zoom Amount for Loot Through Walls. This is how far the camera will zoom through the wall.");
            toolTip1.SetToolTip(checkBox_ShowContainers, "Shows static containers on the map. Due to recent Tarkov Anti-Cheat Measures, you cannot see what the contents are however.");
            toolTip1.SetToolTip(checkBox_Containers_SelectAll, "Selects all container types for display on the map.");
            toolTip1.SetToolTip(checkBox_Containers_HideSearched, "Hides containers that have already been searched by a networked entity (usually ONLY yourself).");
            toolTip1.SetToolTip(button_GymHack, "Enables the Gym Hack Feature which causes your workouts always to succeed.\n" +
                "NOTE: After enabling this feature you must start a workout within 15 seconds for the hack to be applied. Complete your first rep normally, and then it should activate for following reps.\n" +
                "NOTE: You must still 'left click' on each repetition.");
            toolTip1.SetToolTip(checkBox_AdvancedMemWrites, "Enables Advanced Memory Writing Features. Includes (but not limited to):\n" +
                "- AntiPage Feature.\n" +
                "- Advanced Chams Options.\n" +
                "- Show proper AI Enemy Types (Passive).\n" +
                "- Enhanced reliability of some features (Passive)." +
                "\n\nWARNING: These features use a riskier injection technique. Use at your own risk.");
            toolTip1.SetToolTip(radioButton_Loot_FleaPrice, "Loot prices use the optimal flea market price for the item based on ~realtime market value for displayed loot items.");
            toolTip1.SetToolTip(radioButton_Loot_VendorPrice, "Loot prices use the highest trader price for displayed loot items.");
            toolTip1.SetToolTip(checkBox_AntiPage, "Attempts to prevent memory paging out. This can help if you are experiencing 'paging out' (see the FAQ in Discord).\n" +
                "For best results start the Radar Client BEFORE opening the Game.");
            toolTip1.SetToolTip(checkBox_AIAimlines, "Enables dynamic aimlines for AI Players. When you are being aimed at the aimlines will extend.");
            toolTip1.SetToolTip(checkBox_LootWishlist, "Tracks loot on your account's Loot Wishlist (Manual Adds Only, does not work for Automatically Added Items).");
            toolTip1.SetToolTip(checkedListBox_QuestHelper, "Active Quest List (populates once you are in raid). Uncheck a quest to untrack it.");
        }

        /// <summary>
        /// Setup Widgets after GL Context is fully setup and window loaded to proper size.
        /// </summary>
        private void SetupWidgets()
        {
            if (Config.Widgets.AimviewLocation == default)
            {
                var cr = skglControl_Radar.ClientRectangle;
                Config.Widgets.AimviewLocation = new SKRect(cr.Left, cr.Bottom - 200, cr.Left + 200, cr.Bottom);
            }

            if (Config.Widgets.PlayerInfoLocation == default)
            {
                var cr = skglControl_Radar.ClientRectangle;
                Config.Widgets.PlayerInfoLocation = new SKRect(cr.Right - 1, cr.Top, cr.Right, cr.Top + 1);
            }

            _aimview = new EspWidget(skglControl_Radar, this, Config.Widgets.AimviewLocation, Config.Widgets.AimviewMinimized,
                UIScale);
            _playerInfo = new PlayerInfoWidget(skglControl_Radar, Config.Widgets.PlayerInfoLocation,
                Config.Widgets.PlayerInfoMinimized, UIScale);
        }

        private void SetMemWriteFeatures()
        {
            /// Setup Memwrites
            checkBox_EnableMemWrite.Checked = MemWrites.Enabled;
            flowLayoutPanel_MemWrites.Enabled = MemWrites.Enabled;
            checkBox_AdvancedMemWrites.Checked = MemWrites.Config.AdvancedMemWrites;
            ToggleAdvMemwriteFeatures(MemWrites.Config.AdvancedMemWrites);
            checkBox_EnableMemWrite.CheckedChanged += checkBox_EnableMemWrite_CheckedChanged;
            checkBox_AdvancedMemWrites.CheckedChanged += checkBox_AdvancedMemWrites_CheckedChanged;
            /// Set Features
            checkBox_AntiPage.Checked = Config.MemWrites.AntiPage;
            checkBox_EnableMemWrite.Checked = MemWrites.Enabled;
            checkBox_NoRecoilSway.Checked = MemWriteFeature<NoRecoil>.Instance.Enabled;
            trackBar_NoRecoil.Value = MemWrites.Config.NoRecoilAmount;
            trackBar_NoSway.Value = MemWrites.Config.NoSwayAmount;
            trackBar_WideLeanAmt.Value = MemWrites.Config.WideLean.Amount;
            trackBar_LTWAmount.Value = MemWrites.Config.LootThroughWalls.ZoomAmount;

            checkBox_LTW.Checked = MemWriteFeature<LootThroughWalls>.Instance.Enabled;
            checkBox_MoveSpeed.Checked = MemWriteFeature<MoveSpeed>.Instance.Enabled;
            checkBox_WideLean.Checked = MemWriteFeature<WideLean>.Instance.Enabled;
            checkBox_AimBotEnabled.Checked = MemWriteFeature<Aimbot>.Instance.Enabled;
            checkBox_AimbotDisableReLock.Checked = Aimbot.Config.DisableReLock;
            comboBox_AimbotTarget.SelectedIndex =
                comboBox_AimbotTarget.FindStringExact(Aimbot.Config.Bone.GetDescription());
            comboBox_AimbotTarget.SelectedIndexChanged += comboBox_AimbotTarget_SelectedIndexChanged;
            comboBox_WideLeanMode.SelectedIndex =
                comboBox_WideLeanMode.FindStringExact(MemWrites.Config.WideLean.Mode.GetDescription());
            comboBox_WideLeanMode.SelectedIndexChanged += comboBox_WideLeanMode_SelectedIndexChanged;
            checkBox_SA_AutoBone.Checked = Aimbot.Config.SilentAim.AutoBone;
            checkBox_AimHeadAI.Checked = Aimbot.Config.HeadshotAI;
            checkBox_SA_SafeLock.Checked = Aimbot.Config.SilentAim.SafeLock;
            checkBox_AimRandomBone.Checked = Aimbot.Config.RandomBone.Enabled;

            checkBox_NoVisor.Checked = MemWriteFeature<NoVisor>.Instance.Enabled;
            checkBox_InfStamina.Checked = MemWriteFeature<InfStamina>.Instance.Enabled;
            checkBox_Chams.Checked = MemWriteFeature<Chams>.Instance.Enabled;
            checkBox_AlwaysDaySunny.Checked = MemWriteFeature<AlwaysDaySunny>.Instance.Enabled;
            checkBox_NoWepMalf.Checked = MemPatchFeature<NoWepMalfPatch>.Instance.Enabled;
            checkBox_FullBright.Checked = MemWriteFeature<FullBright>.Instance.Enabled;
            checkBox_FastWeaponOps.Checked = MemWriteFeature<FastWeaponOps>.Instance.Enabled;
            checkBox_FastLoadUnload.Checked = MemPatchFeature<FastLoadUnload>.Instance.Enabled;

            switch (Aimbot.Config.TargetingMode)
            {
                case Aimbot.AimbotTargetingMode.FOV:
                    radioButton_AimTarget_FOV.Checked = true;
                    break;
                case Aimbot.AimbotTargetingMode.CQB:
                    radioButton_AimTarget_CQB.Checked = true;
                    break;
            }
            switch (Chams.Config.Mode)
            {
                case ChamsManager.ChamsMode.Basic:
                    radioButton_Chams_Basic.Checked = true;
                    break;
                case ChamsManager.ChamsMode.VisCheck:
                    radioButton_Chams_Vischeck.Checked = true;
                    break;
                case ChamsManager.ChamsMode.Visible:
                    radioButton_Chams_Visible.Checked = true;
                    break;
            }
        }

        private void ToggleFullscreen(bool toFullscreen)
        {
            var screen = Screen.FromControl(this);

            if (toFullscreen)
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                Width = screen.Bounds.Width;
                Height = screen.Bounds.Height;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                Width = 1280;
                Height = 720;
                CenterToScreen();
            }
        }

        /// <summary>
        /// Set the Map Name on Radar Tab.
        /// </summary>
        private void SetMapName()
        {
            var map = LoneMapManager.Map?.Config?.Name;
            var name = map is null ? "Radar" : $"Radar ({map})";
            if (tabPage1.Text != name)
                tabPage1.Text = name;
        }

        /// <summary>
        /// Set the FPS Counter.
        /// </summary>
        private void SetFPS(bool inRaid)
        {
            if (_fpsSw.ElapsedMilliseconds >= 1000)
            {
                var fps = Interlocked.Exchange(ref _fps, 0); // Get FPS -> Reset FPS counter
                var title = Program.Name;
                if (inRaid) title += $" ({fps} fps)";
                Text = title; // Set new window title
                _fpsSw.Restart();
            }
            else
            {
                _fps++; // Increment FPS counter
            }
        }

        /// <summary>
        /// Set the status text in the top middle of the radar window.
        /// </summary>
        /// <param name="canvas"></param>
        private void SetStatusText(SKCanvas canvas)
        {
            try
            {
                bool aimEnabled = checkBox_AimBotEnabled.Enabled && checkBox_AimBotEnabled.Checked;

                var mode = Aimbot.Config.TargetingMode;
                string label = null;
                if (checkBox_RageMode.Checked)
                    label = MemWriteFeature<Aimbot>.Instance.Enabled ? $"{mode.GetDescription()}: RAGE MODE" : "RAGE MODE";
                else if (aimEnabled)
                {
                    if (Aimbot.Config.RandomBone.Enabled)
                        label = $"{mode.GetDescription()}: Random Bone";
                    else if (Aimbot.Config.SilentAim.AutoBone)
                        label = $"{mode.GetDescription()}: Auto Bone";
                    else
                    {
                        var defaultBone = (BonesListItem)comboBox_AimbotTarget.SelectedItem;
                        label = $"{mode.GetDescription()}: {defaultBone!.Name}";
                    }
                }
                if (MemWrites.Enabled)
                {
                    if (MemWriteFeature<WideLean>.Instance.Enabled)
                    {
                        if (label is null)
                            label = "Lean";
                        else
                            label += " (Lean)";
                    }
                    if (MemWriteFeature<LootThroughWalls>.Instance.Enabled && LootThroughWalls.ZoomEngaged)
                    {
                        if (label is null)
                            label = "LTW";
                        else
                            label += " (LTW)";
                    }
                    else if (MemWriteFeature<MoveSpeed>.Instance.Enabled)
                    {
                        if (label is null)
                            label = "MOVE";
                        else
                            label += " (MOVE)";
                    }
                }
                if (label is null)
                    return;
                var clientArea = skglControl_Radar.ClientRectangle;
                var labelWidth = SKPaints.TextStatusSmall.MeasureText(label);
                var spacing = 1f * UIScale;
                var top = clientArea.Top + spacing;
                var labelHeight = SKPaints.TextStatusSmall.FontSpacing;
                var bgRect = new SKRect(
                    clientArea.Width / 2 - labelWidth / 2,
                    top,
                    clientArea.Width / 2 + labelWidth / 2,
                    top + labelHeight + spacing);
                canvas.DrawRect(bgRect, SKPaints.PaintTransparentBacker);
                var textLoc = new SKPoint(clientArea.Width / 2, top + labelHeight);
                canvas.DrawText(label, textLoc, SKPaints.TextStatusSmall);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Setting Aim UI Text: {ex}");
            }
        }

        /// <summary>
        /// Load previously set GUI Config values. Run at startup.
        /// </summary>
        private void SetUiValues()
        {
            comboBox_LootFiltersItem_Items.Items
                .AddRange(EftDataManager.AllItems.Values
                .OrderBy(x => x.Name)
                .ToArray());
            trackBar_AimlineLength.Value = Config.AimLineLength;
            checkBox_Loot.Checked = Config.ShowLoot;
            checkBox_LootPPS.Checked = Config.LootPPS;
            if (Config.LootPriceMode is LootPriceMode.FleaMarket)
                radioButton_Loot_FleaPrice.Checked = true;
            else if (Config.LootPriceMode is LootPriceMode.Trader)
                radioButton_Loot_VendorPrice.Checked = true;
            checkBox_LootWishlist.Checked = Config.LootWishlist;
            checkBox_QuestHelper_Enabled.Checked = Config.QuestHelper.Enabled;
            checkBox_ShowInfoTab.Checked = Config.ShowInfoTab;
            checkBox_HideCorpses.Checked = Config.HideCorpses;
            checkBox_ShowMines.Checked = Config.ShowMines;
            checkBox_TeammateAimlines.Checked = Config.TeammateAimlines;
            checkBox_AIAimlines.Checked = Config.AIAimlines;
            checkBox_WebRadarUPNP.Checked = Config.WebRadar.UPnP;
            textBox_WebRadarBindIP.Text = Config.WebRadar.IP;
            textBox_WebRadarPort.Text = Config.WebRadar.Port;
            textBox_WebRadarTickRate.Text = Config.WebRadar.TickRate;
            textBox_WebRadarPassword.Text = WebRadarServer.Password;
            checkBox_Aimview.Checked = Config.ESPWidgetEnabled;
            trackBar_UIScale.Value = (int)Math.Round(Config.UIScale * 100);
            trackBar_MaxDist.Value = (int)Config.MaxDistance;
            trackBar_AimFOV.Value = (int)Math.Round(Aimbot.Config.FOV);
            textBox_ResWidth.Text = Config.MonitorWidth.ToString();
            textBox_ResHeight.Text = Config.MonitorHeight.ToString();
            textBox_VischeckVisColor.Text = Chams.Config.VisibleColor;
            textBox_VischeckInvisColor.Text = Chams.Config.InvisibleColor;
            CameraManagerBase.UpdateViewportRes();
            LoadESPConfig();
            InitializeContainers();

            checkBox_HideNames.Checked = Config.HideNames;
            checkBox_GrpConnect.Checked = Config.ConnectGroups;
            _zoom = Config.Zoom;
            textBox_LootRegValue.Text = Config.MinLootValue.ToString();
            textBox_LootImpValue.Text = Config.MinValuableLootValue.ToString();
        }

        private void PopulateComboBoxes()
        {
            /// Aimbot Bones
            var bones = new List<BonesListItem>();
            foreach (var bone in Aimbot.BoneNames)
                bones.Add(new BonesListItem(bone));
            comboBox_AimbotTarget.Items.AddRange(bones.ToArray());
            comboBox_AimbotTarget.SelectedIndex = comboBox_AimbotTarget.FindStringExact(Bones.HumanSpine3.GetDescription());
            /// Wide Lean
            var wideLeanModes = new List<HotkeyModeListItem>();
            foreach (var wlMode in Enum.GetValues(typeof(HotkeyMode)).Cast<HotkeyMode>())
                wideLeanModes.Add(new HotkeyModeListItem(wlMode));
            comboBox_WideLeanMode.Items.AddRange(wideLeanModes.ToArray());
            comboBox_WideLeanMode.SelectedIndex = comboBox_WideLeanMode.FindStringExact(HotkeyMode.Hold.GetDescription());
        }



        /// <summary>
        /// Zooms the bitmap 'in'.
        /// </summary>
        private void ZoomIn(int amt)
        {
            if (_zoom - amt >= 1) _zoom -= amt;
            else _zoom = 1;
        }

        /// <summary>
        /// Zooms the bitmap 'out'.
        /// </summary>
        private void ZoomOut(int amt)
        {
            if (_zoom + amt <= 200) _zoom += amt;
            else _zoom = 200;
        }

        /// <summary>
        /// Runs/Updates Loot Filter.
        /// </summary>
        private void LootApply()
        {
            LootFilter.SearchString = textBox_LootFilterByName.Text?.Trim();
            Memory.Loot?.RefreshFilter();
        }

        /// <summary>
        /// Setup Player Watchlist / Important Loot.
        /// Primary Player Only!
        /// </summary>
        private void SetupDataGrids()
        {
            /// Player History
            dataGridView_PlayerHistory.AutoGenerateColumns = false;
            dataGridView_PlayerHistory.DataSource = Player.PlayerHistory.GetReferenceUnsafe();
            /// Player Watchlist
            dataGridView_Watchlist.AutoGenerateColumns = false;
            dataGridView_Watchlist.DataSource = Player.PlayerWatchlist.GetReferenceUnsafe();
            /// Loot Filters
            dataGridView_Loot.AutoGenerateColumns = false;
            Column_LootType.DataSource = LootFilterEntry.Types;
            Column_LootType.ValueMember = "Id";
            Column_LootType.DisplayMember = "Name";
            Column_LootType.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            string filterName = Config.LootFilters.Selected;
            EnsureDefaultLootFilter();
            ResetLootFilterBinding();
            comboBox_LootFilters.SelectedIndex = comboBox_LootFilters.FindStringExact(filterName);
            RefreshLootFilter();

            dataGridView_Loot.CellClick += DataGridView_Loot_CellClick;
            tabPage5.Validating += PlayerWatchlist_Validating;
            tabPage6.Validating += LootFilters_Validating;
        }

        /// <summary>
        /// Toggles the currently selected Aimbot Bone.
        /// </summary>
        private void ToggleAimbotBone()
        {
            var maxIndex = comboBox_AimbotTarget.Items.Count - 1;
            var newIndex = comboBox_AimbotTarget.SelectedIndex + 1;
            if (newIndex > maxIndex)
                comboBox_AimbotTarget.SelectedIndex = 0;
            else
                comboBox_AimbotTarget.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Toggles the currently selected Aimbot Mode.
        /// </summary>
        private void ToggleAimbotMode()
        {
            if (radioButton_AimTarget_FOV.Checked)
                radioButton_AimTarget_CQB.Checked = true;
            else if (radioButton_AimTarget_CQB.Checked)
                radioButton_AimTarget_FOV.Checked = true;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Form closing event.
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _renderTimer.Dispose();
                /// Main Program Shutdown
                Window = null;
                Config.WindowSize = Size;
                Config.WindowMaximized = WindowState is FormWindowState.Maximized;
                Config.Widgets.AimviewLocation = _aimview.ClientRectangle;
                Config.Widgets.AimviewMinimized = _aimview.Minimized;
                Config.Widgets.PlayerInfoLocation = _playerInfo.Rectangle;
                Config.Widgets.PlayerInfoMinimized = _playerInfo.Minimized;
                Config.AimLineLength = trackBar_AimlineLength.Value;
                Config.ShowInfoTab = checkBox_ShowInfoTab.Checked;
                Config.HideNames = checkBox_HideNames.Checked;
                Config.ShowMines = checkBox_ShowMines.Checked;
                Config.ConnectGroups = checkBox_GrpConnect.Checked;
                Config.Containers.Selected = TrackedContainers
                    .Where(x => x.Value is true)
                    .Select(x => x.Key)
                    .ToList();

                Config.Zoom = _zoom;
                Config.MaxDistance = trackBar_MaxDist.Value;
                Memory.CloseFPGA(); // Close FPGA
            }
            finally
            {
                base.OnFormClosing(e); // Proceed with closing
            }
        }

        /// <summary>
        /// Process hotkey presses.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F1)
            {
                ZoomIn(5);
                return true;
            }

            if (keyData == Keys.F2)
            {
                ZoomOut(5);
                return true;
            }

            if (keyData == Keys.F3)
            {
                checkBox_Loot.Checked = !checkBox_Loot.Checked; // Toggle loot
                return true;
            }

            if (keyData == Keys.F11)
            {
                var toFullscreen = FormBorderStyle is FormBorderStyle.Sizable;
                ToggleFullscreen(toFullscreen);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Process mousewheel events.
        /// </summary>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) // Main Radar Tab should be open
            {
                if (e.Delta > 0) // mouse wheel up (zoom in)
                {
                    var amt = e.Delta / SystemInformation.MouseWheelScrollDelta *
                              5; // Calculate zoom amount based on number of deltas
                    ZoomIn(amt);
                    return;
                }

                if (e.Delta < 0) // mouse wheel down (zoom out)
                {
                    var amt = e.Delta / -SystemInformation.MouseWheelScrollDelta *
                              5; // Calculate zoom amount based on number of deltas
                    ZoomOut(amt);
                    return;
                }
            }

            base.OnMouseWheel(e);
        }

        #endregion

        #region Hotkey Manager

        /// <summary>
        /// Loads Hotkey Manager resources.
        /// Only call from Primary Thread/Window (ONCE!)
        /// </summary>
        private void LoadHotkeyManager()
        {
            SetHotkeyEvents();
            HotkeyManager.Initialize(this);
        }

        private const int HK_ZoomTickAmt = 5; // amt to zoom
        private const int HK_ZoomTickDelay = 120; // ms

        private void SetHotkeyEvents()
        {
            var zoomIn = new HotkeyActionController("Zoom In");
            zoomIn.Delay = HK_ZoomTickDelay;
            zoomIn.HotkeyDelayElapsed += ZoomIn_HotkeyDelayElapsed;
            var zoomOut = new HotkeyActionController("Zoom Out");
            zoomOut.Delay = HK_ZoomTickDelay;
            zoomOut.HotkeyDelayElapsed += ZoomOut_HotkeyDelayElapsed;
            var toggleLoot = new HotkeyActionController("Toggle Loot");
            toggleLoot.HotkeyStateChanged += ToggleLoot_HotkeyStateChanged;
            var toggleESPWidget = new HotkeyActionController("Toggle ESP Widget");
            toggleESPWidget.HotkeyStateChanged += ToggleESPWidget_HotkeyStateChanged;
            var toggleNames = new HotkeyActionController("Toggle Player Names");
            toggleNames.HotkeyStateChanged += ToggleNames_HotkeyStateChanged;
            var toggleInfo = new HotkeyActionController("Toggle Game Info Tab");
            toggleInfo.HotkeyStateChanged += ToggleInfo_HotkeyStateChanged;
            var engageAimbot = new HotkeyActionController("Engage Aimbot");
            engageAimbot.HotkeyStateChanged += EngageAimbot_HotkeyStateChanged;
            var toggleAimbotBone = new HotkeyActionController("Toggle Aimbot Bone");
            toggleAimbotBone.HotkeyStateChanged += ToggleAimbotBone_HotkeyStateChanged;
            var toggleAimbotMode = new HotkeyActionController("Toggle Aimbot Mode");
            toggleAimbotMode.HotkeyStateChanged += ToggleAimbotMode_HotkeyStateChanged;
            var toggleQuestHelper = new HotkeyActionController("Toggle Quest Helper");
            toggleQuestHelper.HotkeyStateChanged += ToggleQuestHelper_HotkeyStateChanged;
            var toggleShowFood = new HotkeyActionController("Toggle Show Food");
            toggleShowFood.HotkeyStateChanged += ToggleShowFood_HotkeyStateChanged;
            var toggleShowMeds = new HotkeyActionController("Toggle Show Meds");
            toggleShowMeds.HotkeyStateChanged += ToggleShowMeds_HotkeyStateChanged;
            var toggleNoRecoil = new HotkeyActionController("Toggle No Recoil/Sway");
            toggleNoRecoil.HotkeyStateChanged += ToggleNoRecoil_HotkeyStateChanged;
            var toggleRageMode = new HotkeyActionController("Toggle Rage Mode");
            toggleRageMode.HotkeyStateChanged += ToggleRageMode_HotkeyStateChanged;
            var toggleEsp = new HotkeyActionController("Toggle ESP");
            toggleEsp.HotkeyStateChanged += ToggleEsp_HotkeyStateChanged;
            var toggleAutoBone = new HotkeyActionController("Toggle Auto Bone (Silent Aim)");
            toggleAutoBone.HotkeyStateChanged += ToggleAutoBone_HotkeyStateChanged;
            var toggleRandomBone = new HotkeyActionController("Toggle Random Bone (Aimbot)");
            toggleRandomBone.HotkeyStateChanged += ToggleRandomBone_HotkeyStateChanged;
            var toggleSafeLock = new HotkeyActionController("Toggle Safe Lock (Silent Aim)");
            toggleSafeLock.HotkeyStateChanged += ToggleSafeLock_HotkeyStateChanged;
            var wideLeanLeft = new HotkeyActionController("Wide Lean Left");
            wideLeanLeft.HotkeyStateChanged += WideLeanLeft_HotkeyStateChanged;
            var wideLeanRight = new HotkeyActionController("Wide Lean Right");
            wideLeanRight.HotkeyStateChanged += WideLeanRight_HotkeyStateChanged;
            var wideLeanUp = new HotkeyActionController("Wide Lean Up");
            wideLeanUp.HotkeyStateChanged += WideLeanUp_HotkeyStateChanged;
            var toggleWideLean = new HotkeyActionController("Toggle Wide Lean");
            toggleWideLean.HotkeyStateChanged += ToggleWideLean_HotkeyStateChanged;
            var toggleLTW = new HotkeyActionController("Toggle LTW Zoom");
            toggleLTW.HotkeyStateChanged += ToggleLTW_HotkeyStateChanged;
            var toggleMoveSpeed = new HotkeyActionController("Toggle Move Speed");
            toggleMoveSpeed.HotkeyStateChanged += ToggleMoveSpeed_HotkeyStateChanged;
            var toggleFullBright = new HotkeyActionController("Toggle Full Bright");
            toggleFullBright.HotkeyStateChanged += ToggleFullBright_HotkeyStateChanged;
            var toggleFastWeaponOps = new HotkeyActionController("Toggle Fast Weapon Ops");
            toggleFastWeaponOps.HotkeyStateChanged += ToggleFastWeaponOps_HotkeyStateChanged;
            // Add to Static Collection:
            HotkeyManager.RegisterActionController(zoomIn);
            HotkeyManager.RegisterActionController(zoomOut);
            HotkeyManager.RegisterActionController(toggleLoot);
            HotkeyManager.RegisterActionController(toggleESPWidget);
            HotkeyManager.RegisterActionController(toggleNames);
            HotkeyManager.RegisterActionController(toggleInfo);
            HotkeyManager.RegisterActionController(engageAimbot);
            HotkeyManager.RegisterActionController(toggleAimbotBone);
            HotkeyManager.RegisterActionController(toggleAimbotMode);
            HotkeyManager.RegisterActionController(toggleQuestHelper);
            HotkeyManager.RegisterActionController(toggleShowFood);
            HotkeyManager.RegisterActionController(toggleShowMeds);
            HotkeyManager.RegisterActionController(toggleNoRecoil);
            HotkeyManager.RegisterActionController(toggleRageMode);
            HotkeyManager.RegisterActionController(toggleEsp);
            HotkeyManager.RegisterActionController(toggleAutoBone);
            HotkeyManager.RegisterActionController(toggleRandomBone);
            HotkeyManager.RegisterActionController(toggleSafeLock);
            HotkeyManager.RegisterActionController(wideLeanLeft);
            HotkeyManager.RegisterActionController(wideLeanRight);
            HotkeyManager.RegisterActionController(wideLeanUp);
            HotkeyManager.RegisterActionController(toggleWideLean);
            HotkeyManager.RegisterActionController(toggleLTW);
            HotkeyManager.RegisterActionController(toggleMoveSpeed);
            HotkeyManager.RegisterActionController(toggleFullBright);
            HotkeyManager.RegisterActionController(toggleFastWeaponOps);
        }

        private void ToggleFastWeaponOps_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_FastWeaponOps.Checked = !checkBox_FastWeaponOps.Checked;
            }
        }

        private void ToggleFullBright_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_FullBright.Checked = !checkBox_FullBright.Checked;
            }
        }

        private void ToggleMoveSpeed_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_MoveSpeed.Checked = !checkBox_MoveSpeed.Checked;
            }
        }

        private void ToggleLTW_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                LootThroughWalls.ZoomEngaged = !LootThroughWalls.ZoomEngaged;
            }
        }

        private void ToggleWideLean_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_WideLean.Checked = !checkBox_WideLean.Checked;
                if (!checkBox_WideLean.Checked)
                    WideLean.Direction = WideLean.EWideLeanDirection.Off;
            }
        }

        private void WideLeanUp_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (!checkBox_WideLean.Checked)
            {
                WideLean.Direction = WideLean.EWideLeanDirection.Off;
                return;
            }
            bool isValid = WideLean.Direction is WideLean.EWideLeanDirection.Off ||
                    WideLean.Direction is WideLean.EWideLeanDirection.Up;
            if (MemWrites.Config.WideLean.Mode is HotkeyMode.Hold)
            {
                if (isValid)
                {
                    WideLean.Direction = e.State ? WideLean.EWideLeanDirection.Up : WideLean.EWideLeanDirection.Off;
                }
            }
            else
            {
                if (e.State)
                {
                    if (isValid)
                        WideLean.Direction = WideLean.Direction is WideLean.EWideLeanDirection.Off ?
                            WideLean.EWideLeanDirection.Up
                            : WideLean.EWideLeanDirection.Off;
                    else
                        WideLean.Direction = WideLean.EWideLeanDirection.Off;
                }
            }
        }

        private void WideLeanRight_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (!checkBox_WideLean.Checked)
            {
                WideLean.Direction = WideLean.EWideLeanDirection.Off;
                return;
            }
            bool isValid = WideLean.Direction is WideLean.EWideLeanDirection.Off ||
                    WideLean.Direction is WideLean.EWideLeanDirection.Right;
            if (MemWrites.Config.WideLean.Mode is HotkeyMode.Hold)
            {
                if (isValid)
                {
                    WideLean.Direction = e.State ? WideLean.EWideLeanDirection.Right : WideLean.EWideLeanDirection.Off;
                }
            }
            else
            {
                if (e.State)
                {
                    if (isValid)
                        WideLean.Direction = WideLean.Direction is WideLean.EWideLeanDirection.Off ?
                            WideLean.EWideLeanDirection.Right
                            : WideLean.EWideLeanDirection.Off;
                    else
                        WideLean.Direction = WideLean.EWideLeanDirection.Off;
                }
            }
        }

        private void WideLeanLeft_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (!checkBox_WideLean.Checked)
            {
                WideLean.Direction = WideLean.EWideLeanDirection.Off;
                return;
            }
            bool isValid = WideLean.Direction is WideLean.EWideLeanDirection.Off ||
                    WideLean.Direction is WideLean.EWideLeanDirection.Left;
            if (MemWrites.Config.WideLean.Mode is HotkeyMode.Hold)
            {
                if (isValid)
                {
                    WideLean.Direction = e.State ? WideLean.EWideLeanDirection.Left : WideLean.EWideLeanDirection.Off;
                }
            }
            else
            {
                if (e.State)
                {
                    if (isValid)
                        WideLean.Direction = WideLean.Direction is WideLean.EWideLeanDirection.Off ?
                            WideLean.EWideLeanDirection.Left
                            : WideLean.EWideLeanDirection.Off;
                    else
                        WideLean.Direction = WideLean.EWideLeanDirection.Off;
                }
            }
        }

        private void ToggleRageMode_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && checkBox_RageMode.Enabled)
                checkBox_RageMode.Checked = !checkBox_RageMode.Checked;
        }

        private void ToggleSafeLock_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_SA_SafeLock.Checked = !checkBox_SA_SafeLock.Checked;
            }
        }

        private void ToggleRandomBone_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && flowLayoutPanel_Aimbot.Enabled)
                checkBox_AimRandomBone.Checked = !checkBox_AimRandomBone.Checked;
        }

        private void ToggleAutoBone_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_SA_AutoBone.Checked = !checkBox_SA_AutoBone.Checked;
            }
        }

        private void ToggleEsp_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && CameraManagerBase.EspRunning)
                EspForm.ShowESP = !EspForm.ShowESP;
        }

        private void ToggleNoRecoil_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && checkBox_NoRecoilSway.Enabled)
                checkBox_NoRecoilSway.Checked = !checkBox_NoRecoilSway.Checked;
        }

        private void ToggleShowMeds_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_ShowMeds.Checked = !checkBox_ShowMeds.Checked;
                _lootMenuTimer.Stop();
                Memory.Loot?.RefreshFilter(); // Force immediate refresh
            }
        }

        private void ToggleShowFood_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
            {
                checkBox_ShowFood.Checked = !checkBox_ShowFood.Checked;
                _lootMenuTimer.Stop();
                Memory.Loot?.RefreshFilter(); // Force immediate refresh
            }
        }

        private void ToggleAimbotMode_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && checkBox_AimBotEnabled.Checked)
                ToggleAimbotMode();
        }

        private void ToggleQuestHelper_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State) // Only if enabled & Primary Window
                Config.QuestHelper.Enabled = !Config.QuestHelper.Enabled;
        }

        private void ToggleAimbotBone_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && comboBox_AimbotTarget.Enabled)
                ToggleAimbotBone();
        }

        private void EngageAimbot_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            Aimbot.Engaged = e.State;
        }

        private void ToggleInfo_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_ShowInfoTab.Checked = !checkBox_ShowInfoTab.Checked;
        }

        private void ToggleNames_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_HideNames.Checked = !checkBox_HideNames.Checked;
        }

        private void ToggleESPWidget_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_Aimview.Checked = !checkBox_Aimview.Checked;
        }

        private void ToggleLoot_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_Loot.Checked = !checkBox_Loot.Checked;
        }

        private void ZoomOut_HotkeyDelayElapsed(object sender, EventArgs e)
        {
            ZoomOut(HK_ZoomTickAmt);
        }

        private void ZoomIn_HotkeyDelayElapsed(object sender, EventArgs e)
        {
            ZoomIn(HK_ZoomTickAmt);
        }

        #endregion

        #region ESP Controls

        /// <summary>
        /// Load ESP Configuration.
        /// </summary>
        private void LoadESPConfig()
        {
            trackBar_EspLootDist.ValueChanged += TrackBar_EspLootDist_ValueChanged;
            trackBar_EspImpLootDist.ValueChanged += TrackBar_EspImpLootDist_ValueChanged;
            trackBar_EspQuestHelperDist.ValueChanged += TrackBar_EspQuestHelperDist_ValueChanged;
            trackBar_EspGrenadeDist.ValueChanged += TrackBar_EspGrenadeDist_ValueChanged;
            trackBar_EspFontScale.ValueChanged += TrackBar_EspFontScale_ValueChanged;
            trackBar_EspLineScale.ValueChanged += TrackBar_EspLineScale_ValueChanged;
            trackBar_ESPContainerDist.ValueChanged += TrackBar_ESPContainerDist_ValueChanged;
            Config.ESP.PlayerRendering ??= new ESPPlayerRenderOptions();
            Config.ESP.AIRendering ??= new ESPPlayerRenderOptions();
            switch (Config.ESP.PlayerRendering.RenderingMode)
            {
                case ESPPlayerRenderMode.None:
                    radioButton_ESPRender_None.Checked = true;
                    break;
                case ESPPlayerRenderMode.Bones:
                    radioButton_ESPRender_Bones.Checked = true;
                    break;
                case ESPPlayerRenderMode.Box:
                    radioButton_ESPRender_Box.Checked = true;
                    break;
                case ESPPlayerRenderMode.Presence:
                    radioButton_ESPRender_Presence.Checked = true;
                    break;
            }

            switch (Config.ESP.AIRendering.RenderingMode)
            {
                case ESPPlayerRenderMode.None:
                    radioButton_ESPAIRender_None.Checked = true;
                    break;
                case ESPPlayerRenderMode.Bones:
                    radioButton_ESPAIRender_Bones.Checked = true;
                    break;
                case ESPPlayerRenderMode.Box:
                    radioButton_ESPAIRender_Box.Checked = true;
                    break;
                case ESPPlayerRenderMode.Presence:
                    radioButton_ESPAIRender_Presence.Checked = true;
                    break;
            }

            checkBox_ESPRender_Labels.Checked = Config.ESP.PlayerRendering.ShowLabels;
            checkBox_ESPRender_Weapons.Checked = Config.ESP.PlayerRendering.ShowWeapons;
            checkBox_ESPRender_Dist.Checked = Config.ESP.PlayerRendering.ShowDist;
            checkBox_ESPAIRender_Labels.Checked = Config.ESP.AIRendering.ShowLabels;
            checkBox_ESPAIRender_Weapons.Checked = Config.ESP.AIRendering.ShowWeapons;
            checkBox_ESPAIRender_Dist.Checked = Config.ESP.AIRendering.ShowDist;
            textBox_EspFpsCap.Text = Config.ESP.FPSCap.ToString();
            checkBox_ESP_Exfils.Checked = Config.ESP.ShowExfils;
            checkBox_ESP_Loot.Checked = Config.ESP.ShowLoot;
            checkBox_ESP_Explosives.Checked = Config.ESP.ShowExplosives;
            checkBox_ESP_AimFov.Checked = Config.ESP.ShowAimFOV;
            checkBox_ESP_Dist.Checked = Config.ESP.ShowDistances;
            checkBox_ESP_AimLock.Checked = Config.ESP.ShowAimLock;
            checkBox_ESP_FireportAim.Checked = Config.ESP.ShowFireportAim;
            checkBox_ESP_ShowMines.Checked = Config.ESP.ShowMines;
            checkBox_ESP_ShowMag.Checked = Config.ESP.ShowMagazine;
            checkBox_ESP_RaidStats.Checked = Config.ESP.ShowRaidStats;
            checkBox_ESP_StatusText.Checked = Config.ESP.ShowStatusText;
            checkBox_ESP_FPS.Checked = Config.ESP.ShowFPS;
            trackBar_EspLootDist.Value = (int)Config.ESP.LootDrawDistance;
            trackBar_EspImpLootDist.Value = (int)Config.ESP.ImpLootDrawDistance;
            trackBar_EspQuestHelperDist.Value = (int)Config.ESP.QuestHelperDrawDistance;
            trackBar_EspGrenadeDist.Value = (int)Config.ESP.GrenadeDrawDistance;
            trackBar_EspFontScale.Value = (int)Math.Round(Config.ESP.FontScale * 100f);
            trackBar_EspLineScale.Value = (int)Math.Round(Config.ESP.LineScale * 100f);
            trackBar_ESPContainerDist.Value = (int)Math.Round(Config.ESP.ContainerDrawDistance);
            checkBox_ESP_AutoFS.Checked = Config.ESP.AutoFullscreen;
            /// High Alert Combobox
            foreach (var mode in Enum.GetValues(typeof(HighAlertMode)).Cast<HighAlertMode>())
            {
                var entry = new HighAlertModeEntry(mode);
                comboBox_ESP_HighAlert.Items.Add(entry);
            }

            comboBox_ESP_HighAlert.SelectedIndex =
                comboBox_ESP_HighAlert.FindStringExact(Config.ESP.HighAlertMode.GetDescription());
            /// ESP Screens ComboBox
            var allScreens = Screen.AllScreens;
            if (Config.ESP.SelectedScreen > allScreens.Length - 1)
            {
                Config.ESP.SelectedScreen = 0;
                checkBox_ESP_AutoFS.Checked = false;
            }

            for (var i = 0; i < allScreens.Length; i++)
            {
                var entry = new ScreenEntry(i);
                comboBox_ESPAutoFS.Items.Add(entry);
            }

            comboBox_ESPAutoFS.SelectedIndex = Config.ESP.SelectedScreen;
            if (checkBox_ESP_AutoFS.Checked)
                StartESP();
        }

        private void TrackBar_ESPContainerDist_ValueChanged(object sender, EventArgs e)
        {
            int amt = trackBar_ESPContainerDist.Value;
            label_ESPContainerDist.Text = $"Container Dist {amt}";
            Config.ESP.ContainerDrawDistance = amt;
        }

        private void checkBox_ESP_AutoFS_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_ESP_AutoFS.Checked;
            comboBox_ESPAutoFS.Enabled = enabled;
            Config.ESP.AutoFullscreen = enabled;
        }

        private void comboBox_ESPAutoFS_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_ESPAutoFS.SelectedItem is ScreenEntry entry)
                Config.ESP.SelectedScreen = entry.ScreenNumber;
        }

        private void button_StartESP_Click(object sender, EventArgs e) =>
            StartESP();

        private void StartESP()
        {
            button_StartESP.Text = "Running...";
            flowLayoutPanel_ESPSettings.Enabled = false;
            flowLayoutPanel_MonitorSettings.Enabled = false;
            var t = new Thread(() =>
            {
                try
                {
                    EspForm.ShowESP = true;
                    Application.Run(new EspForm());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ESP Critical Runtime Error! " + ex, Program.Name, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                finally
                {
                    Invoke(() =>
                    {
                        button_StartESP.Text = "Start ESP";
                        flowLayoutPanel_ESPSettings.Enabled = true;
                        flowLayoutPanel_MonitorSettings.Enabled = true;
                    });
                }
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            tabControl1.SelectedIndex = 0; // Switch back to Radar
        }

        private void textBox_EspFpsCap_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox_EspFpsCap.Text, out var value))
                Config.ESP.FPSCap = value;
            else
                textBox_EspFpsCap.Text = Config.ESP.FPSCap.ToString();
        }

        private void TrackBar_EspGrenadeDist_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_EspGrenadeDist.Value;
            label_EspExplosivesDist.Text = $"Explosives Dist {value}";
            Config.ESP.GrenadeDrawDistance = value;
        }

        private void TrackBar_EspLootDist_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_EspLootDist.Value;
            label_EspLootDist.Text = $"Loot Dist {value}";
            Config.ESP.LootDrawDistance = value;
        }

        private void TrackBar_EspImpLootDist_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_EspImpLootDist.Value;
            label_EspImpLootDist.Text = $"Imp Loot Dist {value}";
            Config.ESP.ImpLootDrawDistance = value;
        }

        private void TrackBar_EspQuestHelperDist_ValueChanged(object sender, EventArgs e)
        {
            var value = trackBar_EspQuestHelperDist.Value;
            label_EspQuestHelperDist.Text = $"Quest Dist {value}";
            Config.ESP.QuestHelperDrawDistance = value;
        }

        private void TrackBar_EspFontScale_ValueChanged(object sender, EventArgs e)
        {
            float value = .01f * trackBar_EspFontScale.Value;
            label_EspFontScale.Text = $"Font Scale {value.ToString("n2")}";
            Config.ESP.FontScale = value;
            ScaleESPPaints();
        }

        private void TrackBar_EspLineScale_ValueChanged(object sender, EventArgs e)
        {
            float value = .01f * trackBar_EspLineScale.Value;
            label_EspLineScale.Text = $"Line Scale {value.ToString("n2")}";
            Config.ESP.LineScale = value;
            ScaleESPPaints();
        }

        private void checkBox_ESP_Exfils_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowExfils = checkBox_ESP_Exfils.Checked;
        }

        private void checkBox_ESP_Explosives_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowExplosives = checkBox_ESP_Explosives.Checked;
        }

        private void checkBox_ESP_FPS_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowFPS = checkBox_ESP_FPS.Checked;
        }

        private void checkBox_ESP_AimFov_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowAimFOV = checkBox_ESP_AimFov.Checked;
        }

        private void checkBox_ESP_Dist_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowDistances = checkBox_ESP_Dist.Checked;
        }

        private void checkBox_ESP_AimLock_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowAimLock = checkBox_ESP_AimLock.Checked;
        }

        private void checkBox_ESP_Loot_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowLoot = checkBox_ESP_Loot.Checked;
        }

        private void checkBox_ESP_ShowMines_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowMines = checkBox_ESP_ShowMines.Checked;
        }

        private void checkBox_ESP_ShowMag_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowMagazine = checkBox_ESP_ShowMag.Checked;
        }

        private void comboBox_ESP_HighAlert_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_ESP_HighAlert.SelectedItem is HighAlertModeEntry entry)
                Config.ESP.HighAlertMode = entry.Value;
        }

        private static void ScaleESPPaints()
        {
            float fontScale = Config.ESP.FontScale;
            float lineScale = Config.ESP.LineScale;
            SKPaints.PaintPMCESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextPMCESP.TextSize = 12f * fontScale;
            SKPaints.PaintScavESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextScavESP.TextSize = 12f * fontScale;
            SKPaints.PaintFriendlyESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextFriendlyESP.TextSize = 12f * fontScale;
            SKPaints.PaintPlayerScavESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextPlayerScavESP.TextSize = 12f * fontScale;
            SKPaints.PaintBossESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextBossESP.TextSize = 12f * fontScale;
            SKPaints.PaintRaiderESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextRaiderESP.TextSize = 12f * fontScale;
            SKPaints.PaintWatchlistESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextWatchlistESP.TextSize = 12f * fontScale;
            SKPaints.PaintStreamerESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextStreamerESP.TextSize = 12f * fontScale;
            SKPaints.PaintAimbotLockedESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextAimbotLockedESP.TextSize = 12f * fontScale;
            SKPaints.PaintFocusedESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextFocusedESP.TextSize = 12f * fontScale;
            SKPaints.PaintCrosshairESP.StrokeWidth = 1.75f * lineScale;
            SKPaints.PaintBasicESP.StrokeWidth = 1f * lineScale;
            SKPaints.PaintHighAlertAimlineESP.StrokeWidth = 1f * lineScale;
            SKPaints.PaintHighAlertBorderESP.StrokeWidth = 3f * lineScale;
            SKPaints.PaintLootESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextLootESP.TextSize = 12f * fontScale;
            SKPaints.PaintCorpseESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextCorpseESP.TextSize = 12f * fontScale;
            SKPaints.PaintImpLootESP.StrokeWidth = 1f * lineScale;
            SKPaints.PaintContainerLootESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextImpLootESP.TextSize = 12f * fontScale;
            SKPaints.TextContainerLootESP.TextSize = 11f * fontScale;
            SKPaints.PaintFoodESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextFoodESP.TextSize = 12f * fontScale;
            SKPaints.PaintMedsESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextMedsESP.TextSize = 12f * fontScale;
            SKPaints.PaintBackpackESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextBackpackESP.TextSize = 12f * fontScale;
            SKPaints.PaintQuestItemESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextQuestItemESP.TextSize = 12f * fontScale;
            SKPaints.PaintWishlistItemESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextWishlistItemESP.TextSize = 12f * fontScale;
            SKPaints.PaintQuestHelperESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextQuestHelperESP.TextSize = 12f * fontScale;
            SKPaints.PaintGrenadeESP.StrokeWidth = 1f * lineScale;
            SKPaints.TextExfilESP.TextSize = 12f * fontScale;
            SKPaints.TextMagazineESP.TextSize = 42f * fontScale;
            SKPaints.TextMagazineInfoESP.TextSize = 16f * fontScale;
            SKPaints.TextBasicESP.TextSize = 12f * fontScale;
            SKPaints.TextBasicESPLeftAligned.TextSize = 12f * fontScale;
            SKPaints.TextBasicESPRightAligned.TextSize = 12f * fontScale;
            SKPaints.TextStatusSmallEsp.TextSize = 13 * fontScale;
        }

        private void button_EspColorPicker_Click(object sender, EventArgs e)
        {
            button_EspColorPicker.Enabled = false;
            try
            {
                using var cp =
                    new ColorPicker<EspColorOption, ColorItem<EspColorOption>>("ESP Color Picker", Config.ESP.Colors);
                cp.ShowDialog();
                if (cp.DialogResult is DialogResult.OK && cp.Result is not null)
                {
                    EspColorOptions.SetColors(cp.Result);
                    Config.ESP.Colors = cp.Result;
                    Config.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ERROR Updating Colors! {ex.Message}", "ESP Color Picker", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_EspColorPicker.Enabled = true;
            }
        }

        private void radioButton_ESPRender_None_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPRender_None.Checked)
                Config.ESP.PlayerRendering.RenderingMode = ESPPlayerRenderMode.None;
        }

        private void radioButton_ESPRender_Bones_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPRender_Bones.Checked)
                Config.ESP.PlayerRendering.RenderingMode = ESPPlayerRenderMode.Bones;
        }

        private void radioButton_ESPRender_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPRender_Box.Checked)
                Config.ESP.PlayerRendering.RenderingMode = ESPPlayerRenderMode.Box;
        }

        private void radioButton_ESPRender_Presence_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPRender_Presence.Checked)
                Config.ESP.PlayerRendering.RenderingMode = ESPPlayerRenderMode.Presence;
        }

        private void radioButton_ESPAIRender_None_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPAIRender_None.Checked)
                Config.ESP.AIRendering.RenderingMode = ESPPlayerRenderMode.None;
        }

        private void radioButton_ESPAIRender_Bones_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPAIRender_Bones.Checked)
                Config.ESP.AIRendering.RenderingMode = ESPPlayerRenderMode.Bones;
        }

        private void radioButton_ESPAIRender_Box_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPAIRender_Box.Checked)
                Config.ESP.AIRendering.RenderingMode = ESPPlayerRenderMode.Box;
        }

        private void radioButton_ESPAIRender_Presence_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_ESPAIRender_Presence.Checked)
                Config.ESP.AIRendering.RenderingMode = ESPPlayerRenderMode.Presence;
        }

        private void checkBox_ESPRender_Labels_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.PlayerRendering.ShowLabels = checkBox_ESPRender_Labels.Checked;
        }

        private void checkBox_ESPRender_Weapons_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.PlayerRendering.ShowWeapons = checkBox_ESPRender_Weapons.Checked;
        }

        private void checkBox_ESPRender_Dist_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.PlayerRendering.ShowDist = checkBox_ESPRender_Dist.Checked;
        }

        private void checkBox_ESPAIRender_Labels_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.AIRendering.ShowLabels = checkBox_ESPAIRender_Labels.Checked;
        }

        private void checkBox_ESPAIRender_Weapons_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.AIRendering.ShowWeapons = checkBox_ESPAIRender_Weapons.Checked;
        }

        private void checkBox_ESPAIRender_Dist_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.AIRendering.ShowDist = checkBox_ESPAIRender_Dist.Checked;
        }

        private void checkBox_ESP_RaidStats_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowRaidStats = checkBox_ESP_RaidStats.Checked;
        }

        private void checkBox_ESP_StatusText_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowStatusText = checkBox_ESP_StatusText.Checked;
        }

        private void checkBox_ESP_FireportAim_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowFireportAim = checkBox_ESP_FireportAim.Checked;
        }

        #endregion

        #region Monitor Resolution Code

        private void textBox_ResWidth_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox_ResWidth.Text, out var w))
            {
                Config.MonitorWidth = w;
                CameraManagerBase.UpdateViewportRes();
            }
            else
            {
                textBox_ResWidth.Text = "1920";
            }
        }

        private void textBox_ResHeight_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox_ResHeight.Text, out var h))
            {
                Config.MonitorHeight = h;
                CameraManagerBase.UpdateViewportRes();
            }
            else
            {
                textBox_ResHeight.Text = "1080";
            }
        }

        private async void button_DetectRes_Click(object sender, EventArgs e)
        {
            string original = button_DetectRes.Text;
            button_DetectRes.Enabled = false;
            button_DetectRes.Text = "Working...";
            try
            {
                if (Memory.Ready)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            var res = Memory.GetMonitorRes();
                            this.Invoke(() =>
                            {
                                textBox_ResWidth.Text = res.Width.ToString();
                                textBox_ResHeight.Text = res.Height.ToString();
                                MessageBox.Show(this,
                                    $"Detected {res.Width}x{res.Height} Resolution.",
                                    Program.Name,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this,
                                $"ERROR Detecting Resolution! Please try again.\n{ex.Message}",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    });
                }
                else
                    MessageBox.Show(this,
                        "Game is not running! Make sure the EFT Process is started, and try again.",
                        Program.Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
            finally
            {
                button_DetectRes.Enabled = true;
                button_DetectRes.Text = original;
            }
        }

        #endregion

        #region Loot Filtering

        private readonly Timer _lootFiltersItemSearchTimer = new()
        {
            AutoReset = false,
            Interval = 250
        };
        /// <summary>
        /// Current selected Tarkov Market Item in the Loot Filters UI.
        /// </summary>
        private TarkovMarketItem CurrentLootFilterItem
        {
            get
            {
                if (comboBox_LootFiltersItem_Items.SelectedItem is TarkovMarketItem entry)
                    return entry;
                return null;
            }
        }
        /// <summary>
        /// Current selected Loot Filter in the Loot Filters UI.
        /// </summary>
        private Tuple<string, UserLootFilter> CurrentLootFilter
        {
            get
            {
                if (comboBox_LootFilters.SelectedItem is KeyValuePair<string, UserLootFilter> kvp)
                    return new(kvp.Key, kvp.Value);
                return null;
            }
        }
        /// <summary>
        /// Binding List for the currently selected loot filter (in the datagrid).
        /// </summary>
        private SortableBindingList<LootFilterEntry> LootDatagridBindingList
        {
            get
            {
                if (dataGridView_Loot.DataSource is SortableBindingList<LootFilterEntry> list)
                    return list;
                return null;
            }
        }

        /// <summary>
        /// Refreshes the Loot Filter.
        /// Should be called at startup and during validation.
        /// </summary>
        private static void RefreshLootFilter()
        {
            /// Remove old filters (if any)
            foreach (var item in EftDataManager.AllItems.Values)
                item.SetFilter(null);
            /// Set new filters
            var currentFilters = Config.LootFilters.Filters
                .Values
                .Where(x => x.Enabled)
                .SelectMany(x => x.Entries);
            if (!currentFilters.Any())
                return;
            foreach (var filter in currentFilters)
            {
                if (string.IsNullOrEmpty(filter.ItemID))
                    continue;
                if (EftDataManager.AllItems.TryGetValue(filter.ItemID, out var item))
                    item.SetFilter(filter);
            }
        }

        /// <summary>
        /// Adds a new loot filter to the Filters Dictionary.
        /// </summary>
        /// <param name="filterName">Filter name to add.</param>
        private void AddLootFilter(string filterName)
        {
            try
            {
                if (!Config.LootFilters.Filters.TryAdd(filterName, new()
                {
                    Enabled = true,
                    Entries = new()
                }))
                    throw new Exception("Filter already exists!");
                ResetLootFilterBinding();
                comboBox_LootFilters.SelectedIndex = comboBox_LootFilters.FindStringExact(filterName);
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR Adding Filter '{filterName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Removes a loot filter from the Filters Dictionary.
        /// </summary>
        /// <param name="filterName">Filter name to remove.</param>
        private void RemoveLootFilter(string filterName)
        {
            try
            {
                if (!Config.LootFilters.Filters.Remove(filterName, out var removed))
                    throw new Exception($"Unable to remove filter {filterName}");
                EnsureDefaultLootFilter();
                ResetLootFilterBinding();
                comboBox_LootFilters.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR Removing Filter '{filterName}': {ex.Message}");
            }
        }

        /// <summary>
        /// Renames a loot filter from the old name to the new name.
        /// </summary>
        /// <param name="oldName">Existing loot filter name.</param>
        /// <param name="newName">New loot filter name.</param>
        private void RenameLootFilter(string oldName, string newName)
        {
            try
            {
                if (Config.LootFilters.Filters.TryGetValue(oldName, out var filter) &&
                    Config.LootFilters.Filters.TryAdd(newName, filter) &&
                    Config.LootFilters.Filters.Remove(oldName))
                {
                    ResetLootFilterBinding();
                    comboBox_LootFilters.SelectedIndex = comboBox_LootFilters.FindStringExact(newName);
                }
                else
                    throw new Exception($"ERROR Renaming Loot Filter {oldName}");
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR Renaming Loot Filter", ex);
            }
        }

        /// <summary>
        /// Ensures at least one filter exists.
        /// IMPORTANT: The binding should be reset afterwards, and index set.
        /// </summary>
        private static void EnsureDefaultLootFilter()
        {
            const string defaultName = "default";
            if (Config.LootFilters.Filters.Count == 0)
            {
                var filter = new List<LootFilterEntry>();
                if (Config.LootFilters.Filters.TryAdd(defaultName, new()
                {
                    Enabled = true,
                    Entries = filter
                }))
                {
                    Config.LootFilters.Selected = defaultName;
                }
            }
        }

        /// <summary>
        /// Resets the Loot Filters Dictionary binding to the Loot Filter Dropdown Menu.
        /// This should be called when the collection is modified.
        /// </summary>
        private void ResetLootFilterBinding()
        {
            if (comboBox_LootFilters.DataSource is BindingSource existing)
                existing?.Dispose();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            comboBox_LootFilters.DataSource = new BindingSource(Config.LootFilters.Filters, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            comboBox_LootFilters.DisplayMember = "Key";
            comboBox_LootFilters.ValueMember = "Value";
        }

        #region Loot Filter Event Handlers
        private void LootFilters_Validating(object sender, CancelEventArgs e)
        {
            RefreshLootFilter();

            Config.Save();
            e.Cancel = false;
        }
        private void comboBox_LootFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentFilter = CurrentLootFilter;
            if (currentFilter is not null)
            {
                dataGridView_Loot.DataSource = new SortableBindingList<LootFilterEntry>(currentFilter.Item2.Entries);
                checkBox_CurrentLootFilter_Enabled.Checked = currentFilter.Item2.Enabled;
                Config.LootFilters.Selected = currentFilter.Item1;
            }
            checkBox_CurrentLootFilter_Enabled.Enabled = comboBox_LootFilters.SelectedIndex != -1;
        }
        private void button_LootFilters_Add_Click(object sender, EventArgs e)
        {
            button_LootFilters_Add.Enabled = false;
            try
            {
                using var input = new InputBox("Loot Filter", "Enter the name of the new loot filter:");
                if (input.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(input.Result))
                    AddLootFilter(input.Result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"ERROR Adding Filter: {ex.Message}", "Loot Filter", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button_LootFilters_Add.Enabled = true;
            }
        }

        private void button_LootFilters_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                var currentFilter = CurrentLootFilter;
                if (currentFilter is null)
                {
                    MessageBox.Show(this, "No loot filter selected!");
                    return;
                }
                string name = currentFilter.Item1;

                var dlg = MessageBox.Show(this, $"Are you sure you want to delete the filter {name}?",
                    "Loot Filter", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dlg is DialogResult.Yes)
                    RemoveLootFilter(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"ERROR Deleting Filter: {ex.Message}", "Loot Filter", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void button_LootFilters_Rename_Click(object sender, EventArgs e)
        {
            try
            {
                var filter = CurrentLootFilter;
                if (filter is not null)
                {
                    using var input = new InputBox($"Rename {filter.Item1}", "Enter the new filter name:");
                    if (input.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(input.Result))
                        RenameLootFilter(filter.Item1, input.Result);
                }
                else
                    throw new Exception("No Filter Selected!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"ERROR Renaming Filter: {ex.Message}", "Loot Filter", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void checkBox_CurrentLootFilter_Enabled_CheckedChanged(object sender, EventArgs e)
        {
            var selectedFilter = CurrentLootFilter?.Item2;
            if (selectedFilter is not null)
                selectedFilter.Enabled = checkBox_CurrentLootFilter_Enabled.Checked;
        }

        private void DataGridView_Loot_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the button column
            var row = e.RowIndex;
            if (e.ColumnIndex == Column_LootColorPicker.Index && row >= 0)
            {
                var item = (LootFilterEntry)dataGridView_Loot.Rows[row].DataBoundItem;
                var cp = colorPicker1.ShowDialog(); // Prompt user for color
                if (cp is DialogResult.OK)
                {
                    var skColor = colorPicker1.Color.ToSKColor();
                    item!.Color = skColor.ToString();
                    dataGridView_Loot.Refresh();
                }
            }
        }
        private void textBox_ImpLoot_Search_TextChanged(object sender, EventArgs e)
        {
            _lootFiltersItemSearchTimer.Restart();
        }

        private void impLootSearchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Invoke(() =>
            {
                TarkovMarketItem[] searchItems;
                if (textBox_LootFiltersItem_Search.Text is null
                    || textBox_LootFiltersItem_Search.Text.Trim() == string.Empty) // Show All Items
                    searchItems = EftDataManager.AllItems.Values.OrderBy(x => x.Name)
                        .ToArray();
                else // Query
                    searchItems = EftDataManager
                        .AllItems.Values
                        .Where(x => x.Name
                            .Contains(textBox_LootFiltersItem_Search.Text, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(x => x.Name)
                        .ToArray();
                comboBox_LootFiltersItem_Items.Items.Clear();
                comboBox_LootFiltersItem_Items.Items.AddRange(searchItems);
                if (comboBox_LootFiltersItem_Items.Items.Count > 0)
                    comboBox_LootFiltersItem_Items.SelectedIndex = 0;
            });
        }

        private void button_ImpLoot_Add_Click(object sender, EventArgs e)
        {
            var filter = CurrentLootFilter?.Item2;
            if (filter is null)
            {
                MessageBox.Show(this, "No filter selected!");
                return;
            }

            var item = CurrentLootFilterItem; // cache ref
            if (item is not null && item.BsgId is not null)
            {
                if (filter
                    .Entries
                    .Any(x => x.ItemID?
                        .Equals(item.BsgId, StringComparison.OrdinalIgnoreCase) ?? false))
                {
                    MessageBox.Show(this, "This Item ID is already in this Filter!");
                    return;
                }

                var entry = new LootFilterEntry
                {
                    Enabled = true,
                    ItemID = item.BsgId
                };
                LootDatagridBindingList?.Insert(0, entry);
            }

            comboBox_LootFiltersItem_Items.SelectedIndex = -1;
        }
        #endregion

        #endregion

        #region Web Radar

        /// <summary>
        /// Startup Web Radar. Can only be done once for the program lifetime.
        /// </summary>
        private async void StartWebRadar()
        {
            button_WebRadarStart.Enabled = false;
            checkBox_WebRadarUPNP.Enabled = false;
            textBox_WebRadarTickRate.Enabled = false;
            textBox_WebRadarBindIP.Enabled = false;
            textBox_WebRadarPort.Enabled = false;
            button_WebRadarStart.Text = "Starting...";
            try
            {
                var tickRate = TimeSpan.FromMilliseconds(1000d / int.Parse(textBox_WebRadarTickRate.Text.Trim()));
                string bindIP = textBox_WebRadarBindIP.Text.Trim();
                int port = int.Parse(textBox_WebRadarPort.Text.Trim());
                var externalIP = await WebRadarServer.GetExternalIPAsync();
                await WebRadarServer.StartAsync(bindIP, port, tickRate, checkBox_WebRadarUPNP.Checked);
                button_WebRadarStart.Text = "Running...";
                linkLabel_WebRadarLink.Text = $"http://d3ibl7ms0iloq6.cloudfront.net/?host={externalIP}&port={port}&password={textBox_WebRadarPassword.Text}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, $"ERROR Starting Web Radar Server: {ex.Message}", Program.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                button_WebRadarStart.Text = "Start";
                button_WebRadarStart.Enabled = true;
                checkBox_WebRadarUPNP.Enabled = true;
                textBox_WebRadarTickRate.Enabled = true;
                textBox_WebRadarBindIP.Enabled = true;
                textBox_WebRadarPort.Enabled = true;
            }
        }

        private void button_WebRadarStart_Click(object sender, EventArgs e) =>
            StartWebRadar();

        private void checkBox_WebRadarUPNP_CheckedChanged(object sender, EventArgs e)
        {
            Config.WebRadar.UPnP = checkBox_WebRadarUPNP.Checked;
        }

        private void textBox_WebRadarHost_TextChanged(object sender, EventArgs e)
        {
            Config.WebRadar.IP = textBox_WebRadarBindIP.Text;
        }

        private void textBox_WebRadarPort_TextChanged(object sender, EventArgs e)
        {
            Config.WebRadar.Port = textBox_WebRadarPort.Text;
        }
        private void textBox_WebRadarTickRate_TextChanged(object sender, EventArgs e)
        {
            Config.WebRadar.TickRate = textBox_WebRadarTickRate.Text;
        }
        private void linkLabel_WebRadarLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel_WebRadarLink.Text);
            MessageBox.Show(this, "Copied to clipboard!");
        }
        #endregion

        #region Containers
        /// <summary>
        /// Tracked Containers Dictionary.
        /// TRUE if the container should be displayed.
        /// </summary>
        internal static ConcurrentDictionary<string, bool> TrackedContainers { get; } =
            new(EftDataManager.AllContainers.ToDictionary(x => x.Key, x => false));

        /// <summary>
        /// Checks if a container is being tracked by it's Item ID.
        /// </summary>
        /// <param name="id">Container Item ID</param>
        /// <returns>True if being tracked, otherwise False.</returns>
        internal static bool ContainerIsTracked(string id)
        {
            return TrackedContainers.TryGetValue(id, out bool tracked) && tracked;
        }

        private void InitializeContainers()
        {
            checkedListBox_Containers.ItemCheck += CheckedListBox_Containers_ItemCheck;
            trackBar_ContainerDist.ValueChanged += TrackBar_ContainerDist_ValueChanged;
            var entries = EftDataManager.AllContainers.Values
                .OrderBy(x => x.Name)
                .Select(x => new ContainerListItem(x)).ToArray();
            checkedListBox_Containers.Items.AddRange(entries);
            checkBox_ShowContainers.Checked = Config.Containers.Show;
            trackBar_ContainerDist.Value = (int)Math.Round(Config.ContainerDrawDistance);
            checkBox_Containers_SelectAll.Checked = Config.Containers.SelectAll;
            checkBox_Containers_HideSearched.Checked = Config.Containers.HideSearched;
            if (Config.Containers.SelectAll)
            {
                ContainersSelectAll();
            }
            else
            {
                foreach (var selected in Config.Containers.Selected)
                {
                    int index = checkedListBox_Containers.Items.IndexOf(selected);
                    if (index != -1)
                    {
                        checkedListBox_Containers.SetItemChecked(index, true);
                    }
                }
            }
            checkBox_Containers_SelectAll.CheckedChanged += checkBox_Containers_SelectAll_CheckedChanged;
        }

        private void CheckedListBox_Containers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var item = checkedListBox_Containers.Items[e.Index] as ContainerListItem;
            if (item is not null)
            {
                bool tracked = e.NewValue == CheckState.Checked;
                TrackedContainers[item.Id] = tracked;
                if (!tracked && checkBox_Containers_SelectAll.Checked)
                {
                    checkBox_Containers_SelectAll.Checked = false;
                }
            }
        }

        private void ContainersSelectAll()
        {
            for (int i = 0; i < checkedListBox_Containers.Items.Count; i++)
            {
                checkedListBox_Containers.SetItemChecked(i, true);
            }
        }

        private void checkBox_Containers_SelectAll_CheckedChanged(object sender, EventArgs e)
        {
            bool selectAll = checkBox_Containers_SelectAll.Checked;
            Config.Containers.SelectAll = selectAll;
            if (selectAll)
            {
                ContainersSelectAll();
            }
        }

        public sealed class ContainerListItem
        {
            public string Name { get; }
            public string Id { get; }
            public ContainerListItem(TarkovMarketItem container)
            {
                this.Name = container.ShortName;
                this.Id = container.BsgId;
            }

            public override string ToString() => Name;

            public override bool Equals(object obj)
            {
                if (obj is ContainerListItem other)
                {
                    return string.Equals(Id, other.Id, StringComparison.OrdinalIgnoreCase);
                }
                if (obj is string id)
                {
                    return string.Equals(Id, id, StringComparison.OrdinalIgnoreCase);
                }

                return false;
            }

            public override int GetHashCode()
            {
                return StringComparer.OrdinalIgnoreCase.GetHashCode(Id);
            }
        }

        #endregion

        private void linkLabel_CheckForUpdates_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            const string updatesUrl = "https://lone-eft.com/ongoingsupport";
            Process.Start(new ProcessStartInfo()
            {
                FileName = updatesUrl,
                UseShellExecute = true
            });
        }
    }
}
using DarkModeForms;
using arena_dma_radar.Features.MemoryWrites.UI;
using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.Arena.GameWorld;
using arena_dma_radar.UI.ColorPicker;
using arena_dma_radar.UI.ColorPicker.ESP;
using arena_dma_radar.UI.ColorPicker.Radar;
using arena_dma_radar.UI.ESP;
using arena_dma_radar.UI.Hotkeys;
using arena_dma_radar.UI.Misc;
using static arena_dma_radar.UI.Hotkeys.HotkeyManager;
using static arena_dma_radar.UI.Hotkeys.HotkeyManager.HotkeyActionController;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel;
using eft_dma_shared.Common.Maps;
using arena_dma_radar.Arena.Features;
using arena_dma_radar.Arena.Features.MemoryWrites;
using arena_dma_radar.Arena.Features.MemoryWrites.Patches;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.UI.Radar
{
    public sealed partial class MainForm : Form
    {
        #region Fields/Properties/Constructor(s)

        private readonly Stopwatch _fpsSw = new();
        private readonly PrecisionTimer _renderTimer;
        private readonly DarkModeCS _darkmode;

        private IMouseoverEntity _mouseOverItem;
        private bool _mouseDown;
        private Point _lastMousePosition;
        private int _zoom = 100;
        private int _fps;
        private Vector2 _mapPanPosition;
        private EspWidget _espWidget;

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
        /// Current Map ID
        /// </summary>
        private static string MapID =>
            Memory.MapID ?? "null";

        /// <summary>
        /// Game has started and Radar is starting up...
        /// </summary>
        private static bool Starting => Memory.Starting;

        /// <summary>
        /// Radar has found Escape From Tarkov process and is ready.
        /// </summary>
        private static bool Ready => Memory.Ready;

        /// <summary>
        /// Radar has found Local Game World, and a Raid/Match is active.
        /// </summary>
        private static bool InRaid => Memory.InRaid;

        /// <summary>
        /// LocalPlayer (who is running Radar) 'Player' object.
        /// </summary>
        private static LocalPlayer LocalPlayer =>
            Memory.LocalPlayer;

        /// <summary>
        /// All Players in Local Game World (including dead/exfil'd) 'Player' collection.
        /// </summary>
        private static IReadOnlyCollection<Player> AllPlayers => Memory.Players;

        /// <summary>
        /// Contains all 'Hot' grenades in Local Game World, and their position(s).
        /// </summary>
        private static IReadOnlyCollection<Grenade> Grenades => Memory.Grenades;

        /// <summary>
        /// Contains all 'mouse-overable' items.
        /// </summary>
        private IEnumerable<IMouseoverEntity> MouseOverItems
        {
            get
            {
                var players = AllPlayers?
                                  .Where(x => x is not Arena.ArenaPlayer.LocalPlayer)
                              ?? Enumerable.Empty<Player>();

                return players.Any() ? players : null;
            }
        }

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
            LoadHotkeyManager();
            SetMemWriteFeatures();
            SetUiValues();
            var interval = TimeSpan.FromMilliseconds(1000d / Config.RadarTargetFPS);
            _renderTimer = new(interval);
            Shown += MainForm_Shown;
        }

        private async void MainForm_Shown(object sender, EventArgs e)
        {
            while (!this.IsHandleCreated)
                await Task.Delay(25);
            Window = this;
            _renderTimer.Start();
            _fpsSw.Start();
            while (skglControl_Radar.GRContext is null)
                await Task.Delay(25);
            skglControl_Radar.GRContext.SetResourceCacheLimit(536870912); // 512 MB
            SetupWidgets();
            /// Begin render
            skglControl_Radar.PaintSurface += Radar_PaintSurface;
            _renderTimer.Elapsed += RenderTimer_Elapsed;
        }

        #endregion

        #region Render Loop

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

        private void Radar_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var isStarting = Starting;
            var isReady = Ready; // cache bool
            var inMatch = InRaid; // cache bool
            var localPlayer = LocalPlayer; // cache ref to current player
            var canvas = e.Surface.Canvas; // get Canvas reference to draw on
            try
            {
                SetFPS(inMatch);
                SetMapName();
                /// Check for map switch
                var mapID = MapID; // Cache ref
                if (!mapID.Equals(LoneMapManager.Map?.ID, StringComparison.OrdinalIgnoreCase)) // Map changed
                {
                    LoneMapManager.LoadMap(mapID);
                }
                canvas.Clear(); // Clear canvas
                if (inMatch && localPlayer is not null) // LocalPlayer is in a raid -> Begin Drawing...
                {
                    var map = LoneMapManager.Map; // cache ref
                    ArgumentNullException.ThrowIfNull(map, nameof(map));
                    var closestToMouse = _mouseOverItem; // cache ref
                    var mouseOverGrp = MouseoverGroup; // cache value for entire render
                                                       // Get LocalPlayer location
                    var localPlayerPos = localPlayer?.Position ?? new Vector3();
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
                    map.Draw(canvas, localPlayer?.Position.Y ?? 0f, mapParams.Bounds, mapCanvasBounds);
                    if (localPlayer is not null && localPlayer.IsAlive)
                        // Draw LocalPlayer
                        localPlayer.Draw(canvas, mapParams, localPlayer);
                    var grenades = Grenades; // cache ref
                    if (grenades is not null) // Draw grenades
                        foreach (var grenade in grenades)
                            if (grenade.IsActive)
                                grenade.Draw(canvas, mapParams, localPlayer); // end grenades

                    // Draw other players
                    var allPlayers = AllPlayers?
                        .Where(x => !x.HasExfild); // Skip exfil'd players
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
                            .Where(x => x.IsHumanHostileActive && x.TeamID != -1);
                        if (groupedPlayers is not null)
                        {
                            var groups = groupedPlayers.Select(x => x.TeamID).ToHashSet();
                            foreach (var grp in groups)
                            {
                                var grpMembers = groupedPlayers.Where(x => x.TeamID == grp);
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

                    // Mouseover
                    closestToMouse?.DrawMouseover(canvas, mapParams, localPlayer);
                    // ESP Widget
                    if (checkBox_Aimview.Checked)
                        _espWidget?.Draw(canvas);
                }
                else // LocalPlayer is *not* in a Raid -> Display Reason
                {
                    if (!isStarting)
                        GameNotRunningStatus(canvas);
                    else if (isStarting && !isReady)
                        StartingUpStatus(canvas);
                    else if (!inMatch)
                        WaitingForMatchStatus(canvas);
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
        private void WaitingForMatchStatus(SKCanvas canvas)
        {
            const string waitingFor1 = "Waiting for Match Start.";
            const string waitingFor2 = "Waiting for Match Start..";
            const string waitingFor3 = "Waiting for Match Start...";
            string status = _statusOrder == 1 ?
                waitingFor1 : _statusOrder == 2 ?
                waitingFor2 : waitingFor3;
            float textWidth = SKPaints.TextRadarStatus.MeasureText(waitingFor1);
            canvas.DrawText(status, (skglControl_Radar.Width / 2) - textWidth / 2f, skglControl_Radar.Height / 2,
                SKPaints.TextRadarStatus);
            IncrementStatus();
        }

        #endregion

        #region Methods
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

        private void SetUiEventHandlers()
        {
            trackBar_UIScale.ValueChanged += TrackBar_UIScale_ValueChanged;
            trackBar_AimlineLength.ValueChanged += TrackBar_AimlineLength_ValueChanged;
            skglControl_Radar.MouseMove += MapCanvas_MouseMove;
            skglControl_Radar.MouseClick += MapCanvas_MouseClick;
            skglControl_Radar.MouseDown += MapCanvas_MouseDown;
            skglControl_Radar.MouseUp += MapCanvas_MouseUp;
            skglControl_Radar.MouseDoubleClick += MapCanvas_MouseDblClick;
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
            trackBar_NoRecoil.ValueChanged += TrackBar_NoRecoil_ValueChanged;
            trackBar_NoSway.ValueChanged += TrackBar_NoSway_ValueChanged;
            trackBar_AimFOV.ValueChanged += TrackBar_AimFOV_ValueChanged;
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
            toolTip1.SetToolTip(checkBox_NoWepMalf, "Enables the No Weapons Malfunction feature. This prevents your gun from failing to fire due to misfires/overheating/etc.\n" +
                "Once enabled this feature will remain enabled until you restart your game.\n" +
                "Stream Safe!");
            toolTip1.SetToolTip(checkBox_ESP_ShowMag, "Shows your currently loaded Magazine Ammo Count/Type.");
            toolTip1.SetToolTip(flowLayoutPanel_ESP_PlayerRender, "Controls ESP Rendering for All Players.");
            toolTip1.SetToolTip(checkBox_ESPRender_Weapons, "Display entity's held weapon/ammo.");
            toolTip1.SetToolTip(checkBox_ESPRender_Labels, "Display entity label/name.");
            toolTip1.SetToolTip(checkBox_ESPRender_Dist, "Display entity distance from LocalPlayer.");
            toolTip1.SetToolTip(radioButton_ESPRender_None, "Do not render this entity at all.");
            toolTip1.SetToolTip(radioButton_ESPRender_Bones, "Render full entity skeletal bones.");
            toolTip1.SetToolTip(checkBox_ESP_HighAlert,
                "Enables the 'High Alert' ESP Feature. This will activate when you are being aimed at for longer than 0.5 seconds.\nTargets in your FOV (in front of you) will draw an aimline towards your character.\nTargets outside your FOV will draw the border of your screen red.");
            toolTip1.SetToolTip(button_Radar_ColorPicker, "Allows customizing entity colors on the Radar UI.");
            toolTip1.SetToolTip(button_EspColorPicker, "Allows customizing entity colors on the Fuser ESP.");
            toolTip1.SetToolTip(button_BackupConfig, "Backs up your configuration (Recommended).");
            toolTip1.SetToolTip(checkBox_AimBotEnabled,
                "Enables the Aimbot (Silent Aim) Feature. We employ Aim Prediction with our Aimbot to compensate for bullet drop/ballistics and target movement.\n" +
                "WARNING: This is marked as a RISKY feature since it makes it more likely for other players to report you. Use with care.");
            toolTip1.SetToolTip(trackBar_AimFOV,
                "Sets the FOV for Aimbot Targeting. Increase/Lower this to your preference. Please note when you ADS/Scope in, the FOV field becomes narrower.");
            toolTip1.SetToolTip(comboBox_AimbotTarget, "Sets the Bone to target with the Aimbot.");
            toolTip1.SetToolTip(checkBox_NoVisor,
                "Enables the No Visor feature. This removes the view obstruction from certain faceshields (like the Altyn/Killa Helmet) and gives you a clear view.");
            toolTip1.SetToolTip(checkBox_MapSetup,
                "Toggles the 'Map Setup Helper' to assist with getting Map Bounds/Scaling");
            toolTip1.SetToolTip(button_Restart, "Restarts the Radar for the current raid instance.");
            toolTip1.SetToolTip(checkBox_Aimview,
                "Toggles the ESP 'Widget' that gives you a Mini ESP in the radar window. Can be moved.");
            toolTip1.SetToolTip(checkBox_HideNames,
                "Hides human player names from displaying in the Radar or Player Info Widget. Instead will show the (Height,Distance).");
            toolTip1.SetToolTip(checkBox_GrpConnect,
                "Connects players that are grouped up via semi-transparent green lines. Does not apply to your own party.");
            toolTip1.SetToolTip(trackBar_AimlineLength, "Sets your Aimline Length.");
            toolTip1.SetToolTip(trackBar_UIScale,
                "Sets the scaling factor for the Radar/User Interface. For high resolution monitors you may want to increase this.");
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
                "WARNING: This is marked as a RISKY feature since it reduces your recoil/sway, other players could notice your abnormal spray patterns.\n" +
                "EXTRA WARNING: Arena players could notice this on the Killcam/Replay.");
            toolTip1.SetToolTip(label_Recoil, "Recoil affects the up/down motion of a gun while firing.\n" +
                "WARNING: Setting recoil to zero is VERY RISKY, and can usually be distinguished on replay/killcam.");
            toolTip1.SetToolTip(trackBar_NoRecoil, "Recoil affects the up/down motion of a gun while firing.\n" +
                "WARNING: Setting recoil to zero is VERY RISKY, and can usually be distinguished on replay/killcam.");
            toolTip1.SetToolTip(checkBox_Chams,
                "Enables the Chams feature. This will enable Chams/Glow Effect on ALL players except yourself and teammates.");
            toolTip1.SetToolTip(radioButton_Chams_Basic,
                "These basic chams will only show when a target is VISIBLE. Cannot change color (always White).");
            toolTip1.SetToolTip(radioButton_Chams_Visible,
                "These advanced chams will only show when a target is VISIBLE. You can change the color(s).");
            toolTip1.SetToolTip(radioButton_Chams_Vischeck,
                "These advanced chams (vischeck) will show different colors when a target is VISIBLE/INVISIBLE. You can change the color(s).");
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
            toolTip1.SetToolTip(checkBox_ESP_Grenades, "Enables the rendering of Grenades in the ESP Window.");
            toolTip1.SetToolTip(checkBox_ESP_FPS,
                "Enables the display of the ESP Rendering Rate (FPS) in the Top Left Corner of your ESP Window.");
            toolTip1.SetToolTip(trackBar_EspFontScale,
                "Sets the font scaling factor for the ESP Window.\nIf you are rendering at a really high resolution, you may want to increase this.");
            toolTip1.SetToolTip(trackBar_EspLineScale,
    "Sets the lines scaling factor for the ESP Window.\nIf you are rendering at a really high resolution, you may want to increase this.");
            toolTip1.SetToolTip(checkBox_ESP_AutoFS,
                "Sets 'Auto Fullscreen' for the ESP Window.\nWhen set this will automatically go into full screen mode on the selected screen when the application starts, and when hitting the Start ESP button.");
            toolTip1.SetToolTip(comboBox_ESPAutoFS, "Sets the screen for 'Auto Fullscreen'.");
            toolTip1.SetToolTip(checkBox_ESP_AimFov,
                "Enables the rendering of an 'Aim FOV Circle' in the center of your ESP Window. This is used for Aimbot Targeting.");
            toolTip1.SetToolTip(checkBox_ESP_AimLock,
                "Enables the rendering of a line between your Fireport and your currently locked Aimbot Target.");
            toolTip1.SetToolTip(radioButton_AimTarget_FOV,
                "Enables the FOV (Field of View) Targeting Mode for Aimbot. This will prefer the target closest to the center of your screen within your FOV.");
            toolTip1.SetToolTip(radioButton_AimTarget_CQB,
                "Enables the CQB (Close Quarters Battle) Targeting Mode for Aimbot.\nThis will prefer the target closest to your player within your FOV.");
            toolTip1.SetToolTip(checkBox_SA_AutoBone, "Automatically selects best bone target based on where you are aiming.");
            toolTip1.SetToolTip(checkBox_SA_SafeLock, "Unlocks the aimbot if your target leaves your FOV Radius.\n" +
                "NOTE: It is possible to 're-lock' another target (or the same target) after unlocking.");
            toolTip1.SetToolTip(checkBox_AimRandomBone, "Will select a random aimbot bone after each shot. You can set custom percentage values for body zones.\nNOTE: This will supersede silent aim 'auto bone'.");
            toolTip1.SetToolTip(button_RandomBoneCfg, "Set random bone percentages (must add up to 100%).");
            toolTip1.SetToolTip(checkBox_ESP_FireportAim, "Shows the base fireport trajectory on screen so you can see where bullets will go. Disappears when ADS.");
            toolTip1.SetToolTip(checkBox_ESP_StatusText, "Displays status text in the top center of the screen (Aimbot Status, Wide Lean, etc.)");
            toolTip1.SetToolTip(checkBox_AdvancedMemWrites, "Enables Advanced Memory Writing Features. These features use a riskier injection technique. Use at your own risk. Includes (but not limited to):\n" +
                "- Advanced Chams Options.\n" +
                "- Enhanced reliability of some features (Passive).");
        }

        /// <summary>
        /// Setup Widgets after GL Context is fully setup and window loaded to proper size.
        /// </summary>
        private void SetupWidgets()
        {
            if (Config.Widgets.ESPWidgetLocation == default)
            {
                var cr = skglControl_Radar.ClientRectangle;
                Config.Widgets.ESPWidgetLocation = new SKRect(cr.Left, cr.Bottom - 200, cr.Left + 200, cr.Bottom);
            }

            _espWidget = new EspWidget(skglControl_Radar, Config.Widgets.ESPWidgetLocation, Config.Widgets.ESPWidgetMinimized,
                UIScale);
        }

        /// <summary>
        /// Load previously set GUI Config values. Run at startup.
        /// </summary>
        private void SetUiValues()
        {
            trackBar_AimlineLength.Value = Config.AimLineLength;
            checkBox_Aimview.Checked = Config.ShowESPWidget;
            trackBar_UIScale.Value = (int)Math.Round(Config.UIScale * 100);
            textBox_ResWidth.Text = Config.MonitorWidth.ToString();
            textBox_ResHeight.Text = Config.MonitorHeight.ToString();
            textBox_VischeckVisColor.Text = Chams.Config.VisibleColor;
            textBox_VischeckInvisColor.Text = Chams.Config.InvisibleColor;
            CameraManagerBase.UpdateViewportRes();
            LoadESPConfig();
            checkBox_HideNames.Checked = Config.HideNames;
            checkBox_GrpConnect.Checked = Config.ConnectGroups;
            _zoom = Config.LastZoom;
        }

        /// <summary>
        /// Zooms the bitmap 'in'.
        /// </summary>
        private void ZoomIn(int amt)
        {
            if (_zoom - amt >= 1) _zoom -= amt;
            else _zoom = 1;
            Config.LastZoom = _zoom;
        }

        /// <summary>
        /// Zooms the bitmap 'out'.
        /// </summary>
        private void ZoomOut(int amt)
        {
            if (_zoom + amt <= 200) _zoom += amt;
            else _zoom = 200;
            Config.LastZoom = _zoom;
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
                Text = title;
                _fpsSw.Restart();
            }
            else
            {
                _fps++; // Increment FPS counter
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
            checkBox_NoRecoilSway.Checked = MemWriteFeature<NoRecoil>.Instance.Enabled;
            checkBox_Chams.Checked = MemWriteFeature<Chams>.Instance.Enabled;
            checkBox_NoVisor.Checked = MemWriteFeature<NoVisor>.Instance.Enabled;
            trackBar_NoRecoil.Value = Config.MemWrites.NoRecoilAmount;
            trackBar_NoSway.Value = Config.MemWrites.NoSwayAmount;
            checkBox_NoWepMalf.Checked = MemPatchFeature<NoWepMalfPatch>.Instance.Enabled;

            /// Aimbot Bones
            var bones = new List<BonesListItem>();
            foreach (var bone in Aimbot.BoneNames)
                bones.Add(new BonesListItem(bone));
            comboBox_AimbotTarget.Items.AddRange(bones.ToArray());
            comboBox_AimbotTarget.SelectedIndex = comboBox_AimbotTarget.FindStringExact(Bones.HumanSpine3.GetDescription());
            comboBox_AimbotTarget.SelectedIndexChanged += comboBox_AimbotTarget_SelectedIndexChanged;

            checkBox_AimBotEnabled.Checked = MemWriteFeature<Aimbot>.Instance.Enabled;
            comboBox_AimbotTarget.SelectedIndex =
                comboBox_AimbotTarget.FindStringExact(Aimbot.Config.Bone.GetDescription());
            trackBar_AimFOV.Value = (int)Math.Round(Aimbot.Config.FOV);
            checkBox_SA_AutoBone.Checked = Aimbot.Config.SilentAim.AutoBone;
            checkBox_SA_SafeLock.Checked = Aimbot.Config.SilentAim.SafeLock;
            checkBox_AimRandomBone.Checked = Aimbot.Config.RandomBone.Enabled;


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

        /// <summary>
        /// Set status text in top center of screen.
        /// </summary>
        /// <param name="canvas"></param>
        private void SetStatusText(SKCanvas canvas)
        {
            try
            {
                var aimEnabled = checkBox_AimBotEnabled.Enabled && checkBox_AimBotEnabled.Checked;
                string label;
                if (aimEnabled)
                {
                    var mode = Aimbot.Config.TargetingMode;
                    if (Aimbot.Config.RandomBone.Enabled)
                        label = $"{mode.GetDescription()}: Random Bone";
                    else if (Aimbot.Config.SilentAim.AutoBone)
                        label = $"{mode.GetDescription()}: Auto Bone";
                    else
                    {
                        var bone = (BonesListItem)comboBox_AimbotTarget.SelectedItem;
                        label = $"{mode.GetDescription()}: {bone!.Name}";
                    }
                }
                else if (MemWriteFeature<NoRecoil>.Instance.Enabled)
                    label = "No Recoil";
                else
                    return;
                var clientArea = skglControl_Radar.ClientRectangle;
                var spacing = 1f * UIScale;
                canvas.DrawStatusText(clientArea, SKPaints.TextStatusSmall, spacing, label);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Setting Status Text: {ex}");
            }
        }

        #endregion

        #region Events
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
            if (colorDialog1.ShowDialog() is DialogResult.OK)
            {
                textBox_VischeckVisColor.Text = colorDialog1.Color.ToSKColor().ToString();
            }
        }

        private void button_VischeckInvisColorPick_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() is DialogResult.OK)
            {
                textBox_VischeckInvisColor.Text = colorDialog1.Color.ToSKColor().ToString();
            }
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
        }
        private void checkBox_ESP_FireportAim_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowFireportAim = checkBox_ESP_FireportAim.Checked;
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
        /// <summary>
        /// Handles mouse clicks on the Map Canvas.
        /// </summary>
        private void MapCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button is MouseButtons.Right &&
                _mouseOverItem is Player player &&
                player.IsHumanActive)
                player.ToggleFocus();
        }
        private void checkBox_SA_AutoBone_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.SilentAim.AutoBone = checkBox_SA_AutoBone.Checked;
        }

        private void checkBox_SA_SafeLock_CheckedChanged(object sender, EventArgs e)
        {
            Aimbot.Config.SilentAim.SafeLock = checkBox_SA_SafeLock.Checked;
        }
        private void TrackBar_AimlineLength_ValueChanged(object sender, EventArgs e)
        {
            Config.AimLineLength = trackBar_AimlineLength.Value;
        }

        private void checkBox_GrpConnect_CheckedChanged(object sender, EventArgs e)
        {
            Config.ConnectGroups = checkBox_GrpConnect.Checked;
        }

        private void checkBox_HideNames_CheckedChanged(object sender, EventArgs e)
        {
            Config.HideNames = checkBox_HideNames.Checked;
        }

        private void checkBox_Aimview_CheckedChanged(object sender, EventArgs e)
        {
            Config.ShowESPWidget = checkBox_Aimview.Checked;
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

        private void checkBox_Chams_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_Chams.Checked;
            flowLayoutPanel_Chams.Enabled = enabled;
            MemWriteFeature<Chams>.Instance.Enabled = enabled;
            Chams.Config.Enabled = enabled;
        }

        private void radioButton_Chams_Basic_CheckedChanged(object sender, EventArgs e)
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
            flowLayoutPanel_Vischeck.Enabled = enabled;
        }

        private void radioButton_Chams_Visible_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = radioButton_Chams_Visible.Checked;
            if (enabled)
                Chams.Config.Mode = ChamsManager.ChamsMode.Visible;
            flowLayoutPanel_Vischeck.Enabled = enabled;
        }

        private void checkBox_NoRecoilSway_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_NoRecoilSway.Checked;
            flowLayoutPanel_NoRecoil.Enabled = enabled;
            MemWriteFeature<NoRecoil>.Instance.Enabled = enabled;
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

        private void checkBox_NoWepMalf_CheckedChanged(object sender, EventArgs e)
        {
            MemPatchFeature<NoWepMalfPatch>.Instance.Enabled = checkBox_NoWepMalf.Checked;
        }

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

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Config.Save();
            }
            catch
            {
            }
        }

        private void checkBox_MapSetup_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_MapSetup.Checked)
            {
                groupBox_MapSetup.Visible = true;
                var currentMap = LoneMapManager.Map.Config;
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

        /// <summary>
        /// Fired when UI Scale Trackbar is Adjusted
        /// </summary>
        private void TrackBar_UIScale_ValueChanged(object sender, EventArgs e)
        {
            var newScale = .01f * trackBar_UIScale.Value;
            Config.UIScale = newScale;
            label_UIScale.Text = $"UI Scale {newScale.ToString("n2")}";
            // Update Widgets
            _espWidget?.SetScaleFactor(newScale);

            #region UpdatePaints

            /// Outlines
            SKPaints.TextOutline.TextSize = 12f * newScale;
            SKPaints.TextOutline.StrokeWidth = 2f * newScale;
            SKPaints.ShapeOutline.StrokeWidth = 3f * newScale;

            SKPaints.PaintConnectorGroup.StrokeWidth = 2.25f * newScale;
            SKPaints.PaintMouseoverGroup.StrokeWidth = 3 * newScale;
            SKPaints.TextMouseoverGroup.TextSize = 12 * newScale;
            SKPaints.PaintLocalPlayer.StrokeWidth = 3 * newScale;
            SKPaints.PaintTeammate.StrokeWidth = 3 * newScale;
            SKPaints.TextTeammate.TextSize = 12 * newScale;
            SKPaints.PaintAI.StrokeWidth = 3 * newScale;
            SKPaints.TextAI.TextSize = 12 * newScale;
            SKPaints.PaintPlayer.StrokeWidth = 3 * newScale;
            SKPaints.TextPlayer.TextSize = 12 * newScale;
            SKPaints.PaintStreamer.StrokeWidth = 3 * newScale;
            SKPaints.TextStreamer.TextSize = 12 * newScale;
            SKPaints.PaintAimbotLocked.StrokeWidth = 3 * newScale;
            SKPaints.TextAimbotLocked.TextSize = 12 * newScale;
            SKPaints.PaintFocused.StrokeWidth = 3 * newScale;
            SKPaints.TextFocused.TextSize = 12 * newScale;
            SKPaints.TextMouseover.TextSize = 12 * newScale;
            SKPaints.PaintDeathMarker.StrokeWidth = 3 * newScale;
            SKPaints.PaintTransparentBacker.StrokeWidth = 1 * newScale;
            SKPaints.TextRadarStatus.TextSize = 48 * newScale;
            SKPaints.PaintExplosives.StrokeWidth = 3 * newScale;
            SKPaints.TextStatusSmall.TextSize = 13 * newScale;

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

                // Update both X and Y positions
                _mapPanPosition = new Vector2
                {
                    X = _mapPanPosition.X + deltaX,
                    Y = _mapPanPosition.Y + deltaY
                };
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
                            && player.TeamID != -1)
                            MouseoverGroup = player.TeamID; // Set group ID for closest player(s)
                        else
                            MouseoverGroup = null; // Clear Group ID
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
        /// Event fires when Restart Game button is clicked in Settings.
        /// </summary>
        private void button_Restart_Click(object sender, EventArgs e) => Memory.RestartRadar = true;

        /// <summary>
        /// Automatically detect Game Resolution.
        /// </summary>
        private async void button_DetectRes_Click(object sender, EventArgs e)
        {
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
                        "Game is not running! Make sure the EFT Arena Process is started, and try again.",
                        Program.Name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            }
            finally
            {
                button_DetectRes.Enabled = true;
                button_DetectRes.Text = "Auto-Detect";
            }
        }

        /// <summary>
        /// Launch ColorPicker for Radar Colors.
        /// </summary>
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

        /// <summary>
        /// Backup the Config File.
        /// </summary>
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

        private void checkBox_AimBotEnabled_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = checkBox_AimBotEnabled.Checked;
            Aimbot.Config.Enabled = enabled;
            flowLayoutPanel_Aimbot.Enabled = enabled;
        }

        private void comboBox_AimbotTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_AimbotTarget.SelectedItem is not null &&
                comboBox_AimbotTarget.SelectedItem is BonesListItem entry)
            {
                var bone = entry.Bone;
                Aimbot.Config.Bone = bone;
            }
        }

        private void radioButton_AimTarget_FOV_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AimTarget_FOV.Checked)
                Aimbot.Config.TargetingMode = Aimbot.AimbotTargetingMode.FOV;
        }

        private void radioButton_AimTarget_CQB_CheckedChanged(object sender, EventArgs e)
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

        private void checkBox_ESP_AimFov_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowAimFOV = checkBox_ESP_AimFov.Checked;
        }

        private void checkBox_ESP_AimLock_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowAimLock = checkBox_ESP_AimLock.Checked;
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
                Window = null;
                if (_espWidget is not null)
                {
                    Config.Widgets.ESPWidgetLocation = _espWidget.ClientRectangle;
                    Config.Widgets.ESPWidgetMinimized = _espWidget.Minimized;
                }

                Config.WindowSize = Size;
                Config.WindowMaximized = WindowState is FormWindowState.Maximized;
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
        /// Loads the Hotkey Manager GUI.
        /// </summary>
        private void button_HotkeyManager_Click(object sender, EventArgs e)
        {
            button_HotkeyManager.Enabled = false;
            try
            {
                var hkMgr = new HotkeyManager();
                hkMgr.ShowDialog();
                Config.Save();
            }
            finally
            {
                button_HotkeyManager.Enabled = true;
            }
        }

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
            var toggleNames = new HotkeyActionController("Toggle Player Names");
            toggleNames.HotkeyStateChanged += ToggleNames_HotkeyStateChanged;
            var toggleEspWidget = new HotkeyActionController("Toggle ESP Widget");
            toggleEspWidget.HotkeyStateChanged += ToggleAimview_HotkeyStateChanged;
            var toggleFuserEsp = new HotkeyActionController("Toggle Fuser ESP");
            toggleFuserEsp.HotkeyStateChanged += ToggleEsp_HotkeyStateChanged;
            var engageAimbot = new HotkeyActionController("Engage Aimbot");
            engageAimbot.HotkeyStateChanged += EngageAimbot_HotkeyStateChanged;
            var toggleAimbotBone = new HotkeyActionController("Toggle Aimbot Bone");
            toggleAimbotBone.HotkeyStateChanged += ToggleAimbotBone_HotkeyStateChanged;
            var toggleAimbotMode = new HotkeyActionController("Toggle Aimbot Mode");
            toggleAimbotMode.HotkeyStateChanged += ToggleAimbotMode_HotkeyStateChanged;
            var toggleAutoBone = new HotkeyActionController("Toggle Auto Bone (Silent Aim)");
            toggleAutoBone.HotkeyStateChanged += ToggleAutoBone_HotkeyStateChanged;
            var toggleRandomBone = new HotkeyActionController("Toggle Random Bone (Aimbot)");
            toggleRandomBone.HotkeyStateChanged += ToggleRandomBone_HotkeyStateChanged;
            var toggleSafeLock = new HotkeyActionController("Toggle Safe Lock (Silent Aim)");
            toggleSafeLock.HotkeyStateChanged += ToggleSafeLock_HotkeyStateChanged;
            // Add to Static Collection:
            HotkeyManager.RegisterActionController(zoomIn);
            HotkeyManager.RegisterActionController(zoomOut);
            HotkeyManager.RegisterActionController(toggleNames);
            HotkeyManager.RegisterActionController(toggleEspWidget);
            HotkeyManager.RegisterActionController(toggleFuserEsp);
            HotkeyManager.RegisterActionController(engageAimbot);
            HotkeyManager.RegisterActionController(toggleAimbotBone);
            HotkeyManager.RegisterActionController(toggleAimbotMode);
            HotkeyManager.RegisterActionController(toggleAutoBone);
            HotkeyManager.RegisterActionController(toggleRandomBone);
            HotkeyManager.RegisterActionController(toggleSafeLock);
        }

        private void ToggleSafeLock_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_SA_SafeLock.Checked = !checkBox_SA_SafeLock.Checked;
        }

        private void ToggleRandomBone_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && flowLayoutPanel_Aimbot.Enabled)
                checkBox_AimRandomBone.Checked = !checkBox_AimRandomBone.Checked;
        }

        private void ToggleAutoBone_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_SA_AutoBone.Checked = !checkBox_SA_AutoBone.Checked;
        }

        private void ToggleAimbotMode_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && checkBox_AimBotEnabled.Checked)
                ToggleAimbotMode();
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

        private void ToggleNames_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_HideNames.Checked = !checkBox_HideNames.Checked;
        }

        private void ZoomOut_HotkeyDelayElapsed(object sender, EventArgs e)
        {
            ZoomOut(HK_ZoomTickAmt);
        }

        private void ZoomIn_HotkeyDelayElapsed(object sender, EventArgs e)
        {
            ZoomIn(HK_ZoomTickAmt);
        }

        private void ToggleAimview_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State)
                checkBox_Aimview.Checked = !checkBox_Aimview.Checked;
        }

        private void ToggleEsp_HotkeyStateChanged(object sender, HotkeyEventArgs e)
        {
            if (e.State && CameraManagerBase.EspRunning)
                EspForm.ShowESP = !EspForm.ShowESP;
        }

        #endregion

        #region ESP Controls

        /// <summary>
        /// Load ESP Configuration.
        /// </summary>
        private void LoadESPConfig()
        {
            trackBar_EspFontScale.ValueChanged += TrackBar_EspFontScale_ValueChanged;
            trackBar_EspLineScale.ValueChanged += TrackBar_EspLineScale_ValueChanged;
            textBox_EspFpsCap.Text = Config.ESP.FPSCap.ToString();
            checkBox_ESP_Grenades.Checked = Config.ESP.ShowGrenades;
            checkBox_ESP_FireportAim.Checked = Config.ESP.ShowFireportAim;
            checkBox_ESP_ShowMag.Checked = Config.ESP.ShowMagazine;
            checkBox_ESP_FPS.Checked = Config.ESP.ShowFPS;
            trackBar_EspFontScale.Value = (int)Math.Round(Config.ESP.FontScale * 100f);
            trackBar_EspLineScale.Value = (int)Math.Round(Config.ESP.LineScale * 100f);
            checkBox_ESP_AutoFS.Checked = Config.ESP.AutoFullscreen;
            checkBox_ESP_HighAlert.Checked = Config.ESP.HighAlert;
            checkBox_ESP_AimLock.Checked = Config.ESP.ShowAimLock;
            checkBox_ESP_AimFov.Checked = Config.ESP.ShowAimFOV;
            checkBox_ESP_StatusText.Checked = Config.ESP.ShowStatusText;
            Config.ESP.PlayerRendering ??= new ESPPlayerRenderOptions();
            switch (Config.ESP.PlayerRendering.RenderingMode)
            {
                case ESPPlayerRenderMode.None:
                    radioButton_ESPRender_None.Checked = true;
                    break;
                case ESPPlayerRenderMode.Bones:
                    radioButton_ESPRender_Bones.Checked = true;
                    break;
            }

            checkBox_ESPRender_Labels.Checked = Config.ESP.PlayerRendering.ShowLabels;
            checkBox_ESPRender_Weapons.Checked = Config.ESP.PlayerRendering.ShowWeapons;
            checkBox_ESPRender_Dist.Checked = Config.ESP.PlayerRendering.ShowDist;
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

        private void checkBox_ESP_Grenades_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowGrenades = checkBox_ESP_Grenades.Checked;
        }

        private void checkBox_ESP_FPS_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowFPS = checkBox_ESP_FPS.Checked;
        }

        private void checkBox_ESP_ShowMag_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowMagazine = checkBox_ESP_ShowMag.Checked;
        }

        private void checkBox_ESP_HighAlert_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.HighAlert = checkBox_ESP_HighAlert.Checked;
        }

        private void checkBox_ESP_StatusText_CheckedChanged(object sender, EventArgs e)
        {
            Config.ESP.ShowStatusText = checkBox_ESP_StatusText.Checked;
        }

        private static void ScaleESPPaints()
        {
            float fontScale = Config.ESP.FontScale;
            float lineScale = Config.ESP.LineScale;
            SKPaints.PaintPlayerESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextPlayerESP.TextSize = 12f * fontScale;
            SKPaints.PaintAIESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextAIESP.TextSize = 12f * fontScale;
            SKPaints.PaintTeammateESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextTeammateESP.TextSize = 12f * fontScale;
            SKPaints.PaintStreamerESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextStreamerESP.TextSize = 12f * fontScale;
            SKPaints.PaintAimbotLockedESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextAimbotLockedESP.TextSize = 12f * fontScale;
            SKPaints.PaintFocusedESP.StrokeWidth = 1.5f * lineScale;
            SKPaints.TextFocusedESP.TextSize = 12f * fontScale;
            SKPaints.PaintCrosshairESP.StrokeWidth = 1.75f * lineScale;
            SKPaints.PaintBasicESP.StrokeWidth = 1f * lineScale;
            SKPaints.PaintHighAlertAimlineESP.StrokeWidth = 1f * lineScale;
            SKPaints.PaintHighAlertBorderESP.StrokeWidth = 3f * lineScale;
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
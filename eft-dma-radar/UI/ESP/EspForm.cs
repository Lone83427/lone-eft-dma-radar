using eft_dma_radar.Tarkov;
using eft_dma_radar.Tarkov.EFTPlayer;
using eft_dma_radar.Tarkov.Features;
using eft_dma_radar.Tarkov.Features.MemoryWrites;
using eft_dma_radar.Tarkov.GameWorld.Exits;
using eft_dma_radar.Tarkov.GameWorld.Explosives;
using eft_dma_radar.Tarkov.Loot;
using eft_dma_radar.UI.Misc;
using eft_dma_radar.UI.Radar;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Misc.Commercial;
using eft_dma_shared.Common.Misc.Data;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;

namespace eft_dma_radar.UI.ESP
{
    public partial class EspForm : Form
    {
        #region Fields/Properties/Constructor

        public static bool ShowESP = true;
        private readonly Stopwatch _fpsSw = new();
        private readonly PrecisionTimer _renderTimer;
        private int _fpsCounter;
        private int _fps;

        /// <summary>
        /// Singleton Instance of EspForm.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal static EspForm Window { get; private set; }

        /// <summary>
        ///  App Config.
        /// </summary>
        public static Config Config { get; } = Program.Config;

        /// <summary>
        /// True if ESP Window is Fullscreen.
        /// </summary>
        public bool IsFullscreen =>
            FormBorderStyle is FormBorderStyle.None;

        /// <summary>
        /// Map Identifier of Current Map.
        /// </summary>
        private static string MapID
        {
            get
            {
                var id = Memory.MapID;
                id ??= "MAPDEFAULT";
                return id;
            }
        }

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
        private static IReadOnlyCollection<IExplosiveItem> Explosives => Memory.Explosives;

        /// <summary>
        /// Contains all 'Exfils' in Local Game World, and their status/position(s).
        /// </summary>
        private static IReadOnlyCollection<IExitPoint> Exits => Memory.Exits;

        /// <summary>
        /// Contains all filtered loot in Local Game World.
        /// </summary>
        private static IEnumerable<LootItem> Loot => Memory.Loot?.FilteredLoot;

        /// <summary>
        /// Contains all static containers in Local Game World.
        /// </summary>
        private static IEnumerable<StaticLootContainer> Containers => Memory.Loot?.StaticLootContainers;

        public EspForm()
        {
            InitializeComponent();
            CenterToScreen();
            skglControl_ESP.DoubleClick += ESP_DoubleClick;
            _fpsSw.Start();

            var allScreens = Screen.AllScreens;
            if (Config.ESP.AutoFullscreen && Config.ESP.SelectedScreen < allScreens.Length)
            {
                var screen = allScreens[Config.ESP.SelectedScreen];
                var bounds = screen.Bounds;
                FormBorderStyle = FormBorderStyle.None;
                Location = new Point(bounds.Left, bounds.Top);
                Size = CameraManagerBase.Viewport.Size;
            }

            var interval = Config.ESP.FPSCap == 0 ?
                TimeSpan.Zero : TimeSpan.FromMilliseconds(1000d / Config.ESP.FPSCap);
            _renderTimer = new PrecisionTimer(interval);
            this.Shown += EspForm_Shown;
        }

        private async void EspForm_Shown(object sender, EventArgs e)
        {
            while (!this.IsHandleCreated)
                await Task.Delay(25);
            Window ??= this;
            CameraManagerBase.EspRunning = true;
            _renderTimer.Start();
            /// Begin Render
            skglControl_ESP.PaintSurface += ESP_PaintSurface;
            _renderTimer.Elapsed += RenderTimer_Elapsed;
        }

        private void RenderTimer_Elapsed(object sender, EventArgs e)
        {
            this.Invoke(() =>
            {
                skglControl_ESP.Invalidate();
            });
        }

        #endregion

        #region Form Methods

        /// <summary>
        /// Purge SkiaSharp Resources.
        /// </summary>
        public void PurgeSKResources()
        {
            this.Invoke(() =>
            {
                skglControl_ESP?.GRContext?.PurgeResources();
            });
        }

        /// <summary>
        /// Toggles Full Screen mode for ESP Window.
        /// </summary>
        private void SetFullscreen(bool toFullscreen)
        {
            const int minWidth = 640;
            const int minHeight = 480;
            var screen = Screen.FromControl(this);
            Rectangle view;
            if (toFullscreen)
            {
                FormBorderStyle = FormBorderStyle.None;
                view = CameraManagerBase.Viewport;
                /// Set Minimum Size if ViewPort is default
                if (view.Width < minWidth)
                    view.Width = minWidth;
                if (view.Height < minHeight)
                    view.Height = minHeight;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.FixedSingle;
                view = new Rectangle(screen.Bounds.X, screen.Bounds.Y, minWidth, minHeight);
            }

            /// Move & Resize Window
            WindowState = FormWindowState.Normal;
            Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
            Width = view.Width;
            Height = view.Height;
            if (!toFullscreen)
                CenterToScreen();
        }

        /// <summary>
        /// Record the Rendering FPS.
        /// </summary>
        private void SetFPS()
        {
            if (_fpsSw.ElapsedMilliseconds >= 1000)
            {
                _fps = Interlocked.Exchange(ref _fpsCounter, 0); // Get FPS -> Reset FPS counter
                _fpsSw.Restart();
            }
            else
            {
                _fpsCounter++;
            }
        }

        /// <summary>
        /// Handle double click even on ESP Window (toggles fullscreen).
        /// </summary>
        private void ESP_DoubleClick(object sender, EventArgs e) =>
            SetFullscreen(!IsFullscreen);

        /// <summary>
        /// Main ESP Render Event.
        /// </summary>
        private void ESP_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            SetFPS();
            canvas.Clear();
            try
            {
                var localPlayer = LocalPlayer; // Cache ref
                var allPlayers = AllPlayers; // Cache ref
                if (localPlayer is not null && allPlayers is not null)
                {
                    if (!ShowESP)
                    {
                        DrawNotShown(canvas);
                    }
                    else
                    {
                        if (Config.ESP.ShowLoot && Config.ShowLoot)
                            DrawLoot(canvas, localPlayer);
                        if (MainForm.Config.QuestHelper.Enabled)
                            DrawQuests(canvas, localPlayer);
                        if (Config.ESP.ShowMines && GameData.Mines.TryGetValue(MapID, out var mines))
                            DrawMines(canvas, localPlayer, mines);
                        if (Config.ESP.ShowExfils)
                            DrawExfils(canvas, localPlayer);
                        if (Config.ESP.ShowExplosives)
                            DrawExplosives(canvas, localPlayer);
                        foreach (var player in allPlayers)
                            player.DrawESP(canvas, localPlayer);
                        if (Config.ESP.ShowRaidStats)
                            DrawRaidStats(canvas, allPlayers);
                        if (Config.ESP.ShowAimFOV && MemWriteFeature<Aimbot>.Instance.Enabled)
                            DrawAimFOV(canvas);
                        if (Config.ESP.ShowFPS)
                            DrawFPS(canvas);
                        if (Config.ESP.ShowMagazine)
                            DrawMagazine(canvas, localPlayer);
                        if (Config.ESP.ShowFireportAim &&
                            !CameraManagerBase.IsADS &&
                            !(ESP.Config.ShowAimLock && MemWriteFeature<Aimbot>.Instance.Cache?.AimbotLockedPlayer is not null))
                            DrawFireportAim(canvas, localPlayer);
                        if (Config.ESP.ShowStatusText)
                            DrawStatusText(canvas);
                    }
                }
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ESP RENDER CRITICAL ERROR: {ex}");
            }
            canvas.Flush();
        }

        /// <summary>
        /// Draw status text on ESP Window (top middle of screen).
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawStatusText(SKCanvas canvas)
        {
            try
            {
                bool aimEnabled = MemWriteFeature<Aimbot>.Instance.Enabled;

                var mode = Aimbot.Config.TargetingMode;
                string label = null;
                if (MemWriteFeature<RageMode>.Instance.Enabled)
                    label = MemWriteFeature<Aimbot>.Instance.Enabled ? $"{mode.GetDescription()}: RAGE MODE" : "RAGE MODE";
                else if (aimEnabled)
                {
                    if (Aimbot.Config.RandomBone.Enabled)
                        label = $"{mode.GetDescription()}: Random Bone";
                    else if (Aimbot.Config.SilentAim.AutoBone)
                        label = $"{mode.GetDescription()}: Auto Bone";
                    else
                    {
                        var defaultBone = Aimbot.Config.Bone;
                        label = $"{mode.GetDescription()}: {defaultBone.GetDescription()}";
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
                var clientArea = skglControl_ESP.ClientRectangle;
                var labelWidth = SKPaints.TextStatusSmallEsp.MeasureText(label);
                var spacing = 1f * Config.ESP.FontScale;
                var top = clientArea.Top + spacing;
                var labelHeight = SKPaints.TextStatusSmallEsp.FontSpacing;
                var bgRect = new SKRect(
                    clientArea.Width / 2 - labelWidth / 2,
                    top,
                    clientArea.Width / 2 + labelWidth / 2,
                    top + labelHeight + spacing);
                canvas.DrawRect(bgRect, SKPaints.PaintTransparentBacker);
                var textLoc = new SKPoint(clientArea.Width / 2, top + labelHeight);
                canvas.DrawText(label, textLoc, SKPaints.TextStatusSmallEsp);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Setting ESP Status Text: {ex}");
            }
        }

        /// <summary>
        /// Draw fireport aim in front of player.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawFireportAim(SKCanvas canvas, LocalPlayer localPlayer)
        {
            if (localPlayer.Firearm.FireportPosition is not Vector3 fireportPos)
                return;
            if (localPlayer.Firearm.FireportRotation is not Quaternion fireportRot)
                return;
            if (!CameraManagerBase.WorldToScreen(ref fireportPos, out var fireportPosScr))
                return;
            var forward = fireportRot.Down();
            var targetPos = fireportPos += forward * 1000f;
            if (!CameraManagerBase.WorldToScreen(ref targetPos, out var targetScr))
                return;

            canvas.DrawLine(fireportPosScr, targetScr, SKPaints.PaintBasicESP);
        }

        /// <summary>
        /// Draw player's Magazine/Ammo Count on ESP.
        /// </summary>
        private void DrawMagazine(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var mag = localPlayer.Firearm.Magazine;
            string counter;
            if (mag.IsValid)
                counter = $"{mag.Count} / {mag.MaxCount}";
            else
                counter = "-- / --";
            var textWidth = SKPaints.TextMagazineESP.MeasureText(counter);
            var wepInfo = mag.WeaponInfo;
            if (wepInfo is not null)
                textWidth = Math.Max(textWidth, SKPaints.TextMagazineInfoESP.MeasureText(wepInfo));
            var textHeight = SKPaints.TextMagazineESP.FontSpacing + SKPaints.TextMagazineInfoESP.FontSpacing;
            var x = CameraManagerBase.Viewport.Width - textWidth - 15f * Config.ESP.FontScale;
            var y = CameraManagerBase.Viewport.Height - CameraManagerBase.Viewport.Height * 0.10f - textHeight + 4f * Config.ESP.FontScale;
            if (wepInfo is not null)
                canvas.DrawText(wepInfo, x, y, SKPaints.TextMagazineInfoESP); // Draw Weapon Info
            canvas.DrawText(counter, x,
                y + (SKPaints.TextMagazineESP.FontSpacing - SKPaints.TextMagazineInfoESP.FontSpacing +
                     6f * Config.ESP.FontScale), SKPaints.TextMagazineESP); // Draw Counter
        }

        /// <summary>
        /// Draw Mines/Claymores on ESP.
        /// </summary>
        private static void DrawMines(SKCanvas canvas, LocalPlayer localPlayer, Memory<Vector3> mines)
        {
            foreach (ref var mine in mines.Span)
            {
                if (Vector3.Distance(localPlayer.Position, mine) > Config.ESP.GrenadeDrawDistance)
                    continue;
                if (!CameraManagerBase.WorldToScreen(ref mine, out var mineScr))
                    continue;
                canvas.DrawText("*DANGER* Mine", mineScr, SKPaints.TextBasicESP);
            }
        }

        /// <summary>
        /// Draw 'ESP Hidden' notification.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawNotShown(SKCanvas canvas)
        {
            var textPt = new SKPoint(CameraManagerBase.Viewport.Left + 4.5f * Config.ESP.FontScale,
                CameraManagerBase.Viewport.Top + 14f * Config.ESP.FontScale);
            canvas.DrawText("ESP Hidden", textPt, SKPaints.TextBasicESPLeftAligned);
        }

        /// <summary>
        /// Draw FPS Counter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawFPS(SKCanvas canvas)
        {
            var textPt = new SKPoint(CameraManagerBase.Viewport.Left + 4.5f * Config.ESP.FontScale,
                CameraManagerBase.Viewport.Top + 14f * Config.ESP.FontScale);
            canvas.DrawText($"{_fps}fps", textPt, SKPaints.TextBasicESPLeftAligned);
        }

        /// <summary>
        /// Draw the Aim FOV Circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawAimFOV(SKCanvas canvas) =>
            canvas.DrawCircle(CameraManagerBase.ViewportCenter, Aimbot.Config.FOV, SKPaints.PaintBasicESP);

        /// <summary>
        /// Draw all filtered Loot Items within range.
        /// </summary>
        private static void DrawLoot(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var loot = Loot?.Where(x => x is not QuestItem);
            if (loot is not null)
            {
                foreach (var item in loot)
                {
                    item.DrawESP(canvas, localPlayer);
                }
            }
            if (Config.Containers.Show)
            {
                var containers = Containers;
                if (containers is not null)
                {
                    foreach (var container in containers)
                    {
                        if (MainForm.ContainerIsTracked(container.ID ?? "NULL"))
                        {
                            if (Config.Containers.HideSearched && container.Searched)
                            {
                                continue;
                            }
                            container.DrawESP(canvas, localPlayer);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw all Open/Pending exfils.
        /// </summary>
        private static void DrawExfils(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var exits = Exits;
            if (exits is not null)
                foreach (var exit in exits)
                    exit.DrawESP(canvas, localPlayer);
        }

        /// <summary>
        /// Draw all grenades within range.
        /// </summary>
        private static void DrawExplosives(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var explosives = Explosives;
            if (explosives is not null)
                foreach (var explosive in explosives)
                    explosive.DrawESP(canvas, localPlayer);
        }

        /// <summary>
        /// Draw all quest locations within range.
        /// </summary>
        private static void DrawQuests(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var questItems = Loot?.Where(x => x is QuestItem);
            if (questItems is not null)
                foreach (var item in questItems)
                    item.DrawESP(canvas, localPlayer);
            var questLocations = Memory.QuestManager?.LocationConditions;
            if (questLocations is not null)
                foreach (var loc in questLocations)
                    loc.DrawESP(canvas, localPlayer);
        }

        /// <summary>
        /// Draw Raid Stats in top right corner.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawRaidStats(SKCanvas canvas, IReadOnlyCollection<Player> players)
        {
            var hostiles = players
                .Where(x => x.IsHostileActive)
                .ToArray();
            var pmcCount = hostiles.Count(x => x.IsPmc);
            var pscavCount = hostiles.Count(x => x.Type is Player.PlayerType.PScav);
            var aiCount = hostiles.Count(x => x.IsAI);
            var bossCount = hostiles.Count(x => x.Type is Player.PlayerType.AIBoss);
            var lines = new string[]
            {
                $"PMC: {pmcCount}",
                $"PScav: {pscavCount}",
                $"AI: {aiCount}",
                $"Boss: {bossCount}"
            };
            var x = CameraManagerBase.Viewport.Right - 3f * Config.ESP.FontScale;
            var y = CameraManagerBase.Viewport.Top + SKPaints.TextBasicESPRightAligned.TextSize +
                    CameraManagerBase.Viewport.Height * 0.0575f * Config.ESP.FontScale;
            foreach (var line in lines)
            {
                canvas.DrawText(line, x, y, SKPaints.TextBasicESPRightAligned);
                y += SKPaints.TextBasicESPRightAligned.TextSize;
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F11)
            {
                SetFullscreen(!IsFullscreen);
                return true;
            }

            if (keyData == Keys.Escape && IsFullscreen)
            {
                SetFullscreen(false);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (WindowState is FormWindowState.Maximized)
                SetFullscreen(true);
            else
                base.OnSizeChanged(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                CameraManagerBase.EspRunning = false;
                Window = null;
                _renderTimer.Dispose();
            }
            finally
            {
                base.OnFormClosing(e);
            }
        }

        #endregion
    }
}
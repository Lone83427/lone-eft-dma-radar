using arena_dma_radar.Features;
using arena_dma_radar.Arena.ArenaPlayer;
using arena_dma_radar.Arena.GameWorld;
using arena_dma_radar.UI.Misc;
using eft_dma_shared.Common.Features;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;
using arena_dma_radar.Arena.Features.MemoryWrites;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Misc.Commercial;

namespace arena_dma_radar.UI.ESP
{
    public partial class EspForm : Form
    {
        #region Fields/Properties/Constructor

        public static volatile bool ShowESP = true;
        private readonly Stopwatch _fpsSw = new();
        private readonly PrecisionTimer _renderTimer;
        private int _fpsCounter = 0;
        private int _fps = 0;

        /// <summary>
        /// Singleton Instance of EspForm.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal static EspForm Window { get; private set; }

        /// <summary>
        ///  App Config.
        /// </summary>
        public Config Config { get; } = Program.Config;

        /// <summary>
        /// True if ESP Window is Fullscreen.
        /// </summary>
        public bool IsFullscreen =>
            this.FormBorderStyle is FormBorderStyle.None;

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

        public EspForm()
        {
            InitializeComponent();
            this.CenterToScreen();
            skglControl_ESP.DoubleClick += ESP_DoubleClick;
            _fpsSw.Start();
            var allScreens = Screen.AllScreens;
            if (Config.ESP.AutoFullscreen && Config.ESP.SelectedScreen < allScreens.Length)
            {
                var screen = allScreens[Config.ESP.SelectedScreen];
                var bounds = screen.Bounds;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Location = new(bounds.Left, bounds.Top);
                this.Size = CameraManagerBase.Viewport.Size;
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
                this.FormBorderStyle = FormBorderStyle.None;
                view = CameraManagerBase.Viewport;
                /// Set Minimum Size if ViewPort is default
                if (view.Width < minWidth)
                    view.Width = minWidth;
                if (view.Height < minHeight)
                    view.Height = minHeight;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                view = new(screen.Bounds.X, screen.Bounds.Y, minWidth, minHeight);
            }
            /// Move & Resize Window
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
            this.Width = view.Width;
            this.Height = view.Height;
            if (!toFullscreen)
                this.CenterToScreen();
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
                _fpsCounter++;
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
                        DrawNotShown(canvas);
                    else
                    {
                        if (Config.ESP.ShowGrenades)
                            DrawGrenades(canvas, localPlayer);
                        foreach (var player in allPlayers)
                            player.DrawESP(canvas, localPlayer);
                        if (Config.ESP.ShowAimFOV && MemWriteFeature<Aimbot>.Instance.Enabled && localPlayer.IsAlive)
                            DrawAimFOV(canvas);
                        if (Config.ESP.ShowFPS)
                            DrawFPS(canvas);
                        if (Config.ESP.ShowMagazine && localPlayer.IsAlive)
                            DrawMagazine(canvas, localPlayer);
                        if (Config.ESP.ShowFireportAim &&
                            localPlayer.IsAlive &&
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
                var aimEnabled = Aimbot.Config.Enabled;
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
                        var bone = Aimbot.Config.Bone;
                        label = $"{mode.GetDescription()}: {bone.GetDescription()}";
                    }
                }
                else if (MemWriteFeature<NoRecoil>.Instance.Enabled)
                    label = "No Recoil";
                else
                    return;
                var clientArea = skglControl_ESP.ClientRectangle;
                var spacing = 1f * Config.ESP.FontScale;
                canvas.DrawStatusText(clientArea, SKPaints.TextStatusSmallEsp, spacing, label);
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"ERROR Setting ESP Status Text: {ex}");
            }
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
            float textWidth = SKPaints.TextMagazineESP.MeasureText(counter);
            string wepInfo = mag.WeaponInfo;
            if (wepInfo is not null)
                textWidth = Math.Max(textWidth, SKPaints.TextMagazineInfoESP.MeasureText(wepInfo));
            float textHeight = SKPaints.TextMagazineESP.FontSpacing + SKPaints.TextMagazineInfoESP.FontSpacing;
            float x = CameraManagerBase.Viewport.Width - textWidth - (15f * Config.ESP.FontScale);
            float y = CameraManagerBase.Viewport.Height - (CameraManagerBase.Viewport.Height * 0.10f) - textHeight + (4f * Config.ESP.FontScale);
            if (wepInfo is not null)
                canvas.DrawText(wepInfo, x, y, SKPaints.TextMagazineInfoESP); // Draw Weapon Info
            canvas.DrawText(counter, x, y + ((SKPaints.TextMagazineESP.FontSpacing - SKPaints.TextMagazineInfoESP.FontSpacing) + 6f * Config.ESP.FontScale), SKPaints.TextMagazineESP); // Draw Counter
        }

        /// <summary>
        /// Draw 'ESP Hidden' notification.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawNotShown(SKCanvas canvas)
        {
            var textPt = new SKPoint(CameraManagerBase.Viewport.Left + (4.5f * Config.ESP.FontScale),
                CameraManagerBase.Viewport.Top + (14f * Config.ESP.FontScale));
            canvas.DrawText($"ESP Hidden", textPt, SKPaints.TextBasicESPLeftAligned);
        }

        /// <summary>
        /// Draw FPS Counter.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawFPS(SKCanvas canvas)
        {
            var textPt = new SKPoint(CameraManagerBase.Viewport.Left + (4.5f * Config.ESP.FontScale),
                CameraManagerBase.Viewport.Top + (14f * Config.ESP.FontScale));
            canvas.DrawText($"{_fps}fps", textPt, SKPaints.TextBasicESPLeftAligned);
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
        /// Draw the Aim FOV Circle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void DrawAimFOV(SKCanvas canvas) =>
            canvas.DrawCircle(CameraManagerBase.ViewportCenter, Aimbot.Config.FOV, SKPaints.PaintBasicESP);

        /// <summary>
        /// Draw all grenades within range.
        /// </summary>
        private void DrawGrenades(SKCanvas canvas, LocalPlayer localPlayer)
        {
            var grenades = Grenades;
            if (grenades is not null)
            {
                float circleRadius = 8f * Config.ESP.FontScale;
                foreach (var grenade in grenades)
                {
                    if (!grenade.IsActive)
                        continue;
                    if (Vector3.Distance(localPlayer.Position, grenade.Position) > Config.ESP.GrenadeDrawDistance)
                        continue;
                    if (!CameraManagerBase.WorldToScreen(ref grenade.Position, out var scrPos))
                        continue;
                    canvas.DrawCircle(scrPos, circleRadius, SKPaints.PaintGrenadeESP);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F11))
            {
                SetFullscreen(!IsFullscreen);
                return true;
            }
            else if (keyData == (Keys.Escape) && IsFullscreen)
            {
                SetFullscreen(false);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (this.WindowState is FormWindowState.Maximized)
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

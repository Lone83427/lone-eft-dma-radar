using arena_dma_radar.UI.ColorPicker.ESP;
using arena_dma_radar.UI.ColorPicker.Radar;
using arena_dma_radar.UI.ESP;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel;
using arena_dma_radar.Arena.Features.MemoryWrites;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Misc.Config;

namespace arena_dma_radar.UI.Misc
{
    /// <summary>
    /// Global Program Configuration (Config.json)
    /// </summary>
    public sealed class Config : IConfig
    {
        #region ISharedConfig

        public bool MemWritesEnabled => MemWrites.MemWritesEnabled;
        public LowLevelCache LowLevelCache => this.Cache.LowLevel;

        public ChamsConfig ChamsConfig => this.MemWrites.Chams;

        public bool AdvancedMemWrites => this.MemWrites.AdvancedMemWrites;

        #endregion

        /// <summary>
        /// Target FPS for the 2D Radar.
        /// </summary>
        [JsonPropertyName("radarTargetFPS")]
        public int RadarTargetFPS { get; set; } = 60;

        /// <summary>
        /// True if there should be a mandatory delay between memory reads on the Realtime Thread.
        /// </summary>
        [JsonPropertyName("ratelimitRealtimeReads")]
        public bool RatelimitRealtimeReads { get; set; } = false;

        /// <summary>
        /// UI Scale Value (0.5 - 2.0 , default: 1.0)
        /// </summary>
        [JsonPropertyName("uiScale2")]
        public float UIScale { get; set; } = 1.0f;

        /// <summary>
        /// Size of the Radar Window.
        /// </summary>
        [JsonPropertyName("windowSize")]
        public Size WindowSize { get; set; } = new(1280, 720);

        /// <summary>
        /// Window is maximized.
        /// </summary>
        [JsonPropertyName("windowMaximized")]
        public bool WindowMaximized { get; set; }

        /// <summary>
        /// Player/Teammates Aimline Length (Max: 1500)
        /// </summary>
        [JsonPropertyName("aimLineLength")]
        public int AimLineLength { get; set; } = 1500;

        /// <summary>
        /// Enables 'Aimlines' for Teammates (uses LocalPlayer aimline Length).
        /// Also enables 'Dynamic Aimlines' for hostile players aiming at Teammates.
        /// </summary>
        [JsonPropertyName("teammateAimlinesEnabled")]
        public bool TeammateAimlinesEnabled { get; set; }

        /// <summary>
        /// Last used 'Zoom' level.
        /// </summary>
        [JsonPropertyName("lastZoom")]
        public int LastZoom { get; set; } = 100;

        /// <summary>
        /// Enables ESP Widget in Main Window.
        /// </summary>
        [JsonPropertyName("showESPWidget")]
        public bool ShowESPWidget { get; set; } = true;

        /// <summary>
        /// Hides player names & extended player info in Radar GUI.
        /// </summary>
        [JsonPropertyName("hideNames")]
        public bool HideNames { get; set; }

        /// <summary>
        /// Connects grouped players together via a semi-transparent line.
        /// </summary>
        [JsonPropertyName("connectGroups")]
        public bool ConnectGroups { get; set; }

        /// <summary>
        /// FPGA Read Algorithm
        /// </summary>
        [JsonPropertyName("fpgaAlgo")]
        public FpgaAlgo FpgaAlgo { get; set; } = FpgaAlgo.Auto;

        /// <summary>
        /// Use a Memory Map for FPGA DMA Connection.
        /// </summary>
        [JsonPropertyName("enableMemMap")]
        public bool MemMapEnabled { get; set; } = true;

        /// <summary>
        /// Game PC Monitor Resolution Width
        /// </summary>
        [JsonPropertyName("monitorWidth")]
        public int MonitorWidth { get; set; } = 1920;

        /// <summary>
        /// Game PC Monitor Resolution Height
        /// </summary>
        [JsonPropertyName("monitorHeight")]
        public int MonitorHeight { get; set; } = 1080;

        /// <summary>
        /// Hotkeys Dictionary for Radar.
        /// </summary>
        [JsonPropertyName("hotkeys")]
        public Dictionary<int, string> Hotkeys { get; set; } = new() // Default entries
        {
            { 306, "Engage Aimbot" }
        };

        /// <summary>
        /// All defined Radar Colors.
        /// </summary>
        [JsonPropertyName("radarColors")]
        public Dictionary<RadarColorOption, string> Colors { get; set; } = RadarColorOptions.GetDefaultColors();

        /// <summary>
        /// DMA Toolkit (Write Features) Config.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("dmaToolkit")]
        public MemWritesConfig MemWrites { get; private set; } = new();

        /// <summary>
        /// ESP Configuration.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("esp")]
        public ESPConfig ESP { get; private set; } = new();

        /// <summary>
        /// Widgets Configuration.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("widgets")]
        public WidgetsConfig Widgets { get; private set; } = new();

        /// <summary>
        /// Contains cache data between program sessions.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("Z4mTpX7")]
        public PersistentCache Cache { get; private set; } = new();

        #region Config Interface

        /// <summary>
        /// Filename of this Config File (not full path).
        /// </summary>
        [JsonIgnore] internal const string Filename = "Config-EFTArena.json";

        [JsonIgnore] private static readonly Lock _syncRoot = new();

        [JsonIgnore]
        private static readonly FileInfo _configFile = new(Path.Combine(Program.ConfigPath.FullName, Filename));

        [JsonIgnore]
        private static readonly FileInfo _tempFile = new(Path.Combine(Program.ConfigPath.FullName, Filename + ".tmp"));

        /// <summary>
        /// Load Config Instance.
        /// Only call once! This must be a singleton.
        /// </summary>
        /// <returns>Config Instance.</returns>
        public static Config Load()
        {
            lock (_syncRoot)
            {
                try
                {
                    Config config;
                    if (_configFile.Exists)
                    {
                        try
                        {
                            try
                            {
                                var json = File.ReadAllText(_configFile.FullName);
                                config = JsonSerializer.Deserialize<Config>(json);
                            }
                            catch (JsonException)
                            {
                                if (_tempFile.Exists)
                                {
                                    var json = File.ReadAllText(_tempFile.FullName);
                                    config = JsonSerializer.Deserialize<Config>(json);
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            MessageBox.Show(
                                $"Config File Corruption Detected! If you backed up your config, you may attempt to restore it to \\%AppData%\\Lones-Client (Close the Program *before* attempting restoration).\n\n" +
                                $"Error: {ex.Message}",
                                Program.Name,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                            config = new Config();
                        }
                    }
                    else
                    {
                        config = new Config();
                        SaveInternal(config);
                    }

                    return config;
                }
                catch (Exception ex)
                {
                    throw new IOException($"ERROR Loading Config: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Save this Config Instance to Disk.
        /// </summary>
        public void Save()
        {
            lock (_syncRoot)
            {
                try
                {
                    SaveInternal(this);
                }
                catch (Exception ex)
                {
                    throw new IOException($"ERROR Saving Config: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Save this Config Instance to Disk asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync() => await Task.Run(Save);

        private static void SaveInternal(Config config)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(config, options);
            File.WriteAllText(_tempFile.FullName, json);
            _tempFile.CopyTo(_configFile.FullName, true);
            _tempFile.Delete();
        }

        #endregion
    }

    public sealed class MemWritesConfig
    {
        /// <summary>
        /// Enables DMA Memory Writing
        /// </summary>
        [JsonPropertyName("enableMemWritesRisky")]
        public bool MemWritesEnabled { get; set; } = false;

        /// <summary>
        /// Enables Advanced Mem Writes Features (NativeHook).
        /// </summary>
        [JsonPropertyName("advancedMemWritesRisky")]
        public bool AdvancedMemWrites { get; set; } = false;

        /// <summary>
        /// Enables the AntiPage Feature.
        /// </summary>
        [JsonPropertyName("antiPageEnabled")]
        public bool AntiPage { get; set; } = false;

        /// <summary>
        /// Enable No Recoil/Sway Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoRecoil")]
        public bool NoRecoil { get; set; } = false;

        /// <summary>
        /// Amount of 'No Recoil'. 0 = None, 100 = Full
        /// </summary>
        [JsonPropertyName("noRecoilAmt")]
        public int NoRecoilAmount { get; set; } = 50;

        /// <summary>
        /// Amount of 'No Sway'. 0 = None, 100 = Full
        /// </summary>
        [JsonPropertyName("noSwayAmt")]
        public int NoSwayAmount { get; set; } = 30;

        /// <summary>
        /// Enable No Visor Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoVisor")]
        public bool NoVisor { get; set; } = false;

        /// <summary>
        /// Enable No Visor Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoWepMalf")]
        public bool NoWeaponMalfunctions { get; set; } = false;

        /// <summary>
        /// Enables the Chams Feature.
        /// </summary>
        [JsonPropertyName("chams")]
        public ChamsConfig Chams { get; set; } = new();

        /// <summary>
        /// Aimbot Configuration.
        /// </summary>
        [JsonPropertyName("aimbot")]
        public AimbotConfig Aimbot { get; set; } = new();
    }

    public sealed class AimbotConfig
    {
        /// <summary>
        /// Enable Aimbot Feature on Startup.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Last Aimbot Targeting Mode that the player set.
        /// </summary>
        [JsonPropertyName("targetingMode")]
        public Aimbot.AimbotTargetingMode TargetingMode { get; set; } = Aimbot.AimbotTargetingMode.FOV;

        /// <summary>
        /// Aimbot FOV via ESP Circle.
        /// </summary>
        [JsonPropertyName("fov2")]
        public float FOV { get; set; } = 30f;

        /// <summary>
        /// Bone for the Default Aimbot Target.
        /// </summary>
        [JsonPropertyName("bone")]
        public Bones Bone { get; set; } = Bones.HumanSpine3;

        /// <summary>
        /// Silent Aim Config
        /// </summary>
        [JsonPropertyName("silentAimCfg")]
        public SilentAimConfig SilentAim { get; set; } = new();

        /// <summary>
        /// Random Bone Config
        /// </summary>
        [JsonPropertyName("randomBone")]
        public AimbotRandomBoneConfig RandomBone { get; set; } = new();
    }

    public sealed class AimbotRandomBoneConfig
    {
        [JsonIgnore]
        private static readonly Random _rng = new();

        /// <summary>
        /// Enables Random Bone Selection.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;
        /// <summary>
        /// Head shot percentage.
        /// </summary>
        [JsonPropertyName("headPercent")]
        public int HeadPercent { get; set; } = 10;
        /// <summary>
        /// Torso shot percentage.
        /// </summary>
        [JsonPropertyName("torsoPercent")]
        public int TorsoPercent { get; set; } = 30;
        /// <summary>
        /// Arms shot percentage.
        /// </summary>
        [JsonPropertyName("armsPercent")]
        public int ArmsPercent { get; set; } = 30;
        /// <summary>
        /// Legs shot percentage.
        /// </summary>
        [JsonPropertyName("legsPercent")]
        public int LegsPercent { get; set; } = 30;

        /// <summary>
        /// True if all values add up to 100% exactly, otherwise False.
        /// </summary>
        [JsonIgnore]
        public bool Is100Percent => (HeadPercent >= 0 && TorsoPercent >= 0 && ArmsPercent >= 0 && LegsPercent >= 0) &&
            (HeadPercent + TorsoPercent + ArmsPercent + LegsPercent == 100);

        /// <summary>
        /// Reset all values to defaults.
        /// </summary>
        public void ResetDefaults()
        {
            HeadPercent = 10;
            TorsoPercent = 30;
            ArmsPercent = 30;
            LegsPercent = 30;
        }
        
        /// <summary>
        /// Returns a random bone via the selected percentages.
        /// </summary>
        /// <returns>Skeleton Bone.</returns>
        public Bones GetRandomBone()
        {
            if (!Is100Percent)
                ResetDefaults();
            int roll = _rng.Next(0, 100) + 1;
            if (roll <= HeadPercent)
                return Bones.HumanHead;
            else if (roll <= HeadPercent + TorsoPercent)
                return Random.Shared.GetItems(Skeleton.AllTorsoBones.Span, 1)[0];
            else if (roll <= HeadPercent + TorsoPercent + ArmsPercent)
                return Random.Shared.GetItems(Skeleton.AllArmsBones.Span, 1)[0];
            else // Legs
                return Random.Shared.GetItems(Skeleton.AllLegsBones.Span, 1)[0];
        }
    }

    public sealed class SilentAimConfig
    {
        /// <summary>
        /// Automatically select best target bone.
        /// </summary>
        [JsonPropertyName("autoBone")]
        public bool AutoBone { get; set; } = false;

        /// <summary>
        /// Automatically 'unlock' if target bone leaves FOV.
        /// </summary>
        [JsonPropertyName("safeLock")]
        public bool SafeLock { get; set; } = true;
    }

    public sealed class ESPConfig
    {
        /// <summary>
        /// Show FPS Counter in ESP Window.
        /// </summary>
        [JsonPropertyName("showFPS")]
        public bool ShowFPS { get; set; } = false;

        /// <summary>
        /// Display grenades in ESP.
        /// </summary>
        [JsonPropertyName("showGrenades")]
        public bool ShowGrenades { get; set; } = true;

        /// <summary>
        /// Display Aimline out of the barrel fireport.
        /// </summary>
        [JsonPropertyName("showFireportAim")]
        public bool ShowFireportAim { get; set; } = true;

        [JsonPropertyName("playerRendering")]
        /// <summary>
        /// Player rendering options in ESP.
        /// </summary>
        public ESPPlayerRenderOptions PlayerRendering { get; set; } = new()
        {
            RenderingMode = ESPPlayerRenderMode.Bones,
            ShowLabels = true,
            ShowWeapons = true,
            ShowDist = false
        };

        /// <summary>
        /// Show Magazine / Ammo count in ESP.
        /// </summary>
        [JsonPropertyName("showMagazine")]
        public bool ShowMagazine { get; set; } = true;

        /// <summary>
        /// Max Distance to draw grenades.
        /// </summary>
        [JsonPropertyName("grenadeDrawDistance")]
        public float GrenadeDrawDistance { get; set; } = 50f;

        /// <summary>
        /// ESP Font Size/Scale.
        /// </summary>
        [JsonPropertyName("fontScale")]
        public float FontScale { get; set; } = 1.0f;

        /// <summary>
        /// ESP Line Thickness/Scale.
        /// </summary>
        [JsonPropertyName("lineScale")]
        public float LineScale { get; set; } = 1.0f;

        /// <summary>
        /// FPS Cap for ESP Rendering.
        /// 0 = Infinite.
        /// </summary>
        [JsonPropertyName("fpsCap")]
        public int FPSCap { get; set; } = 60;

        /// <summary>
        /// Enable 'Auto Full Screen' on Startup/Start.
        /// </summary>
        [JsonPropertyName("autoFS")]
        public bool AutoFullscreen { get; set; } = false;

        /// <summary>
        /// Selected screen for Auto Startup.
        /// </summary>
        [JsonPropertyName("selectedScreen")]
        public int SelectedScreen { get; set; } = 0;

        /// <summary>
        /// Enable 'High Alert' ESP feature.
        /// </summary>
        [JsonPropertyName("highAlert")]
        public bool HighAlert { get; set; } = false;

        /// <summary>
        /// Display Aim Lock of target locked onto via aimbot in ESP.
        /// </summary>
        [JsonPropertyName("showAimLock")]
        public bool ShowAimLock { get; set; } = false;

        /// <summary>
        /// Display Aim FOV in ESP.
        /// </summary>
        [JsonPropertyName("showAimFov")]
        public bool ShowAimFOV { get; set; } = true;

        /// <summary>
        /// Display Status (aimbot enabled, bone, etc.) in top center of ESP Screen.
        /// </summary>
        [JsonPropertyName("showStatusText")]
        public bool ShowStatusText { get; set; } = true;

        /// <summary>
        /// All defined ESP Colors.
        /// </summary>
        [JsonPropertyName("espColors")]
        public Dictionary<EspColorOption, string> Colors { get; set; } = EspColorOptions.GetDefaultColors();
    }

    public sealed class ESPPlayerRenderOptions
    {
        /// <summary>
        /// Mode to draw in ESP.
        /// </summary>
        [JsonPropertyName("renderingMode")]
        public ESPPlayerRenderMode RenderingMode { get; set; }

        /// <summary>
        /// Show text labels on this player.
        /// </summary>
        [JsonPropertyName("showLabels")]
        public bool ShowLabels { get; set; }

        /// <summary>
        /// Show weapon name on this player.
        /// </summary>
        [JsonPropertyName("showWeapons")]
        public bool ShowWeapons { get; set; }

        /// <summary>
        /// Show distance to this player.
        /// </summary>
        [JsonPropertyName("showDist")]
        public bool ShowDist { get; set; }
    }

    public sealed class WidgetsConfig
    {
        #region ESP Widget

        [JsonInclude]
        [JsonPropertyName("espWidgetLocation")]
        public RectFSer _espWidgetLoc { private get; set; }

        [JsonPropertyName("espWidgetMinimized")]
        public bool ESPWidgetMinimized { get; set; } = false;

        /// <summary>
        /// Aimview Location
        /// </summary>
        [JsonIgnore]
        public SKRect ESPWidgetLocation
        {
            get => new(_espWidgetLoc.Left, _espWidgetLoc.Top, _espWidgetLoc.Right, _espWidgetLoc.Bottom);
            set => _espWidgetLoc = new RectFSer(value.Left, value.Top, value.Right, value.Bottom);
        }

        #endregion
    }

    /// <summary>
    /// Caches runtime data between sessions.
    /// </summary>
    public sealed class PersistentCache
    {
        [JsonInclude]
        [JsonPropertyName("L8rVqZ3")]
        public LowLevelCache LowLevel { get; private set; } = new();
    }
}
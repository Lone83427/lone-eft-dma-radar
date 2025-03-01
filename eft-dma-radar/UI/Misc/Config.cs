using eft_dma_radar.Tarkov.EFTPlayer.Plugins;
using eft_dma_radar.Tarkov.Features.MemoryWrites;
using eft_dma_radar.UI.ColorPicker.ESP;
using eft_dma_radar.UI.ColorPicker.Radar;
using eft_dma_radar.UI.ESP;
using eft_dma_radar.UI.LootFilters;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.ESP;
using eft_dma_shared.Common.Misc.Config;
using eft_dma_shared.Common.Players;
using eft_dma_shared.Common.Unity;
using eft_dma_shared.Common.Unity.LowLevel;
using static eft_dma_radar.Tarkov.API.EFTProfileService;

namespace eft_dma_radar.UI.Misc
{
    /// <summary>
    /// Global Program Configuration (Config.json)
    /// </summary>
    public sealed class Config : IConfig
    {
        #region ISharedConfig

        [JsonIgnore]
        public bool MemWritesEnabled => this.MemWrites.MemWritesEnabled;
        [JsonIgnore]
        public LowLevelCache LowLevelCache => this.Cache.LowLevel;
        [JsonIgnore]
        public ChamsConfig ChamsConfig => this.MemWrites.Chams;
        [JsonIgnore]
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
        /// UI Scale Value (0.5-2.0 , default: 1.0)
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
        /// Last used 'Zoom' level.
        /// </summary>
        [JsonPropertyName("lastZoom")]
        public int Zoom { get; set; } = 100;

        /// <summary>
        /// Maximum distance to draw static containers.
        /// </summary>
        [JsonPropertyName("containerDrawDistance")]
        public float ContainerDrawDistance { get; set; } = 100f;

        /// <summary>
        /// Shows loot on map.
        /// </summary>
        [JsonPropertyName("showLoot")]
        public bool ShowLoot { get; set; } = true;

        /// <summary>
        /// Quest Helper Cfg
        /// </summary>
        [JsonPropertyName("questHelperCfg")]
        [JsonInclude]
        public QuestHelperConfig QuestHelper { get; private set; } = new();

        /// <summary>
        /// Shows Info Tab/Pane in the top right corner of radar.
        /// </summary>
        [JsonPropertyName("showInfoTab")]
        public bool ShowInfoTab { get; set; } = true;

        /// <summary>
        /// Shows bodies/corpses on map.
        /// </summary>
        [JsonPropertyName("hideCorpses")]
        public bool HideCorpses { get; set; }

        /// <summary>
        /// Show Mines/Claymores in the Radar UI.
        /// </summary>
        [JsonPropertyName("showMines")]
        public bool ShowMines { get; set; } = true;

        /// <summary>
        /// Enables ESP Widget window in Main Window.
        /// </summary>
        [JsonPropertyName("aimviewEnabled")]
        public bool ESPWidgetEnabled { get; set; } = true;

        /// <summary>
        /// Hides player names & extended player info in Radar GUI.
        /// </summary>
        [JsonPropertyName("hideNames")]
        public bool HideNames { get; set; }

        /// <summary>
        /// Connects grouped players together via a semi-transparent line.
        /// </summary>
        [JsonPropertyName("connectGroups")]
        public bool ConnectGroups { get; set; } = true;

        /// <summary>
        /// Max game distance to render targets in Aimview,
        /// and to display dynamic aimlines between two players.
        /// </summary>
        [JsonPropertyName("maxDistance")]
        public float MaxDistance { get; set; } = 350;
        /// <summary>
        /// True if teammate aimlines should be the same length as LocalPlayer.
        /// </summary>
        [JsonPropertyName("teammateAimlines")]
        public bool TeammateAimlines { get; set; } = false;

        /// <summary>
        /// True if AI Aimlines should dynamically extend.
        /// </summary>
        [JsonPropertyName("aiAimlines")]
        public bool AIAimlines { get; set; } = true;

        /// <summary>
        /// Minimum loot value (rubles) to display 'normal loot' on map.
        /// </summary>
        [JsonPropertyName("minLootValue")]
        public int MinLootValue { get; set; } = 50000;

        /// <summary>
        /// Show Loot by "Price per Slot".
        /// </summary>
        [JsonPropertyName("lootPPS")]
        public bool LootPPS { get; set; }
        
        /// <summary>
        /// Loot Price Mode.
        /// </summary>
        [JsonPropertyName("lootPriceMode")]
        public LootPriceMode LootPriceMode { get; set; } = LootPriceMode.FleaMarket;

        /// <summary>
        /// Show loot on the player's wishlist (manual only).
        /// </summary>
        [JsonPropertyName("lootWishList")]
        public bool LootWishlist { get; set; } = false;

        /// <summary>
        /// Minimum loot value (rubles) to display 'important loot' on map.
        /// </summary>
        [JsonPropertyName("minImportantLootValue")]
        public int MinValuableLootValue { get; set; } = 200000;

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
        [JsonPropertyName("radarColors3")]
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
        /// Web Radar Configuration.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("webRadar")]
        public WebRadarConfig WebRadar { get; private set; } = new();

        /// <summary>
        /// Containers configuration.
        /// </summary>
        [JsonPropertyName("containers")]
        public ContainersConfig Containers { get; set; } = new();

        /// <summary>
        /// Player Watchlist.
        /// *WARNING* Only reference from the PlayerWatchlist internal wrapper.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("playerWatchlist")]
        public List<PlayerWatchlistEntry> PlayerWatchlist { get; set; } = new();

        /// <summary>
        /// Loot Filters Config.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("lootFilters2")]
        public LootFilterConfig LootFilters { get; private set; } = new();

        /// <summary>
        /// Contains cache data between program sessions.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("R9tQvX5")]
        public PersistentCache Cache { get; private set; } = new();

        #region Config Interface

        /// <summary>
        /// Filename of this Config File (not full path).
        /// </summary>
        [JsonIgnore] internal const string Filename = "Config-EFT.json";

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

                    ValidateConfigIntegrity(config);
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
            var json = JsonSerializer.Serialize(config);
            File.WriteAllText(_tempFile.FullName, json);
            _tempFile.CopyTo(_configFile.FullName, true);
            _tempFile.Delete();
        }

        /// <summary>
        /// Ensure the config file is within proper bounds, and repair if needed.
        /// Address any config bugs here.
        /// </summary>
        /// <param name="config">Config instance to repair.</param>
        private static void ValidateConfigIntegrity(Config config)
        {

        }

        #endregion
    }

    public sealed class QuestHelperConfig
    {
        /// <summary>
        /// Enables Quest Helper
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Quests that are overridden/disabled.
        /// </summary>
        [JsonPropertyName("blacklistedQuests")]
        [JsonInclude]
        public HashSet<string> BlacklistedQuests { get; private set; } = new HashSet<string>();
    }

    public sealed class ContainersConfig
    {
        /// <summary>
        /// Shows static containers on map.
        /// </summary>
        [JsonPropertyName("show")]
        public bool Show { get; set; } = false;

        /// <summary>
        /// Select all containers.
        /// </summary>
        [JsonPropertyName("selectAll")]
        public bool SelectAll { get; set; } = true;

        /// <summary>
        /// Hide containers searched by LocalPlayer.
        /// </summary>
        [JsonPropertyName("hideSearched")]
        public bool HideSearched { get; set; } = false;

        /// <summary>
        /// Selected containers to display.
        /// </summary>
        [JsonPropertyName("selected")]
        public List<string> Selected { get; set; } = new List<string>();
    }

    /// <summary>
    /// Loot Filter Config.
    /// </summary>
    public sealed class LootFilterConfig
    {
        /// <summary>
        /// Currently selected filter.
        /// </summary>
        [JsonPropertyName("selected")]
        public string Selected { get; set; } = "default";
        /// <summary>
        /// Filter Entries.
        /// </summary>
        [JsonInclude]
        [JsonPropertyName("filters")]
        public Dictionary<string, UserLootFilter> Filters { get; private set; } = new()
        {
            ["default"] = new()
        };
    }

    public sealed class ESPConfig
    {
        /// <summary>
        /// Show FPS Counter in ESP Window.
        /// </summary>
        [JsonPropertyName("showFPS")]
        public bool ShowFPS { get; set; } = false;

        /// <summary>
        /// Display exfils in ESP.
        /// </summary>
        [JsonPropertyName("showExfils")]
        public bool ShowExfils { get; set; } = true;

        /// <summary>
        /// Display loot in ESP.
        /// </summary>
        [JsonPropertyName("showLoot")]
        public bool ShowLoot { get; set; } = true;

        /// <summary>
        /// Display grenades in ESP.
        /// </summary>
        [JsonPropertyName("showExplosives")]
        public bool ShowExplosives { get; set; } = true;

        /// <summary>
        /// Display Distance of targets in ESP.
        /// </summary>
        [JsonPropertyName("showDistance")]
        public bool ShowDistances { get; set; } = true;

        /// <summary>
        /// Display Aimline out of the barrel fireport.
        /// </summary>
        [JsonPropertyName("showFireportAim")]
        public bool ShowFireportAim { get; set; } = true;

        /// <summary>
        /// Display Aim Lock of target locked onto via aimbot in ESP.
        /// </summary>
        [JsonPropertyName("showAimLock")]
        public bool ShowAimLock { get; set; } = true;

        /// <summary>
        /// Display Mines/Claymores in ESP.
        /// </summary>
        [JsonPropertyName("showMines")]
        public bool ShowMines { get; set; } = false;

        /// <summary>
        /// Player rendering options in ESP.
        /// </summary>
        [JsonPropertyName("playerRendering")]
        public ESPPlayerRenderOptions PlayerRendering { get; set; } = new()
        {
            RenderingMode = ESPPlayerRenderMode.Bones,
            ShowLabels = true,
            ShowWeapons = true,
            ShowDist = false
        };

        /// <summary>
        /// AI rendering options in ESP.
        /// </summary>
        [JsonPropertyName("aiRendering")]
        public ESPPlayerRenderOptions AIRendering { get; set; } = new()
        {
            RenderingMode = ESPPlayerRenderMode.Bones,
            ShowLabels = false,
            ShowWeapons = false,
            ShowDist = false
        };

        /// <summary>
        /// Display Aim FOV in ESP.
        /// </summary>
        [JsonPropertyName("showAimFov")]
        public bool ShowAimFOV { get; set; } = true;

        /// <summary>
        /// Show Magazine / Ammo count in ESP.
        /// </summary>
        [JsonPropertyName("showMagazine")]
        public bool ShowMagazine { get; set; } = true;

        /// <summary>
        /// Display Raid Stats (Player Type/Count,etc.) in top right corner.
        /// </summary>
        [JsonPropertyName("showRaidStats")]
        public bool ShowRaidStats { get; set; } = false;

        /// <summary>
        /// Display Status (aimbot enabled, bone, wide lean, etc.) in top center of ESP Screen.
        /// </summary>
        [JsonPropertyName("showStatusText")]
        public bool ShowStatusText { get; set; } = true;

        /// <summary>
        /// Max Distance to draw loot.
        /// </summary>
        [JsonPropertyName("lootDrawDistance")]
        public float LootDrawDistance { get; set; } = 50f;

        /// <summary>
        /// Max Distance to draw important loot.
        /// </summary>
        [JsonPropertyName("impLootDrawDistance")]
        public float ImpLootDrawDistance { get; set; } = 50f;

        /// <summary>
        /// Max Distance to draw static containers (if enabled).
        /// </summary>
        [JsonPropertyName("containerDrawDistance")]
        public float ContainerDrawDistance { get; set; } = 50f;

        /// <summary>
        /// Max Distance to draw Static Quest Items/Locations.
        /// </summary>
        [JsonPropertyName("questHelperDrawDistance")]
        public float QuestHelperDrawDistance { get; set; } = 100f;

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
        [JsonPropertyName("highAlertMode")]
        public HighAlertMode HighAlertMode { get; set; } = HighAlertMode.Off;

        /// <summary>
        /// All defined ESP Colors.
        /// </summary>
        [JsonPropertyName("espColors3")]
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
        [JsonPropertyName("antiPage")]
        public bool AntiPage { get; set; } = false;

        /// <summary>
        /// Enable No Recoil Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoRecoil")]
        public bool NoRecoil { get; set; } = false;

        /// <summary>
        /// Amount of 'No Recoil'. 0 = None, 1 = Full
        /// </summary>
        [JsonPropertyName("noRecoilAmount")]
        public int NoRecoilAmount { get; set; } = 0;

        /// <summary>
        /// Amount of 'No Sway'. 0 = None, 1 = Full
        /// </summary>
        [JsonPropertyName("noSwayAmount")]
        public int NoSwayAmount { get; set; } = 0;

        /// <summary>
        /// Enable No Visor Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoVisor")]
        public bool NoVisor { get; set; } = false;

        /// <summary>
        /// Enable Inf Stamina Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableInfStamina2")]
        public bool InfStamina { get; set; } = false;

        /// <summary>
        /// Chams Feature Config
        /// </summary>
        [JsonPropertyName("chams")]
        public ChamsConfig Chams { get; set; } = new();

        /// <summary>
        /// Enable Always Day Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableAlwaysDay")]
        public bool AlwaysDaySunny { get; set; } = false;

        /// <summary>
        /// Enable No Weapon Malfs Feature on Startup.
        /// </summary>
        [JsonPropertyName("enableNoWepMalf")]
        public bool NoWeaponMalfunctions { get; set; } = false;

        /// <summary>
        /// Enable Loot Through Walls (LTW) on Startup.
        /// </summary>
        [JsonPropertyName("ltw")]
        public LTWConfig LootThroughWalls { get; set; } = new();

        /// <summary>
        /// Aimbot Configuration.
        /// </summary>
        [JsonPropertyName("aimbot")]
        public AimbotConfig Aimbot { get; set; } = new();

        /// <summary>
        /// Wide Lean Configuration.
        /// </summary>
        [JsonPropertyName("wideLean")]
        public WideLeanConfig WideLean { get; set; } = new();

        /// <summary>
        /// 1.2x Move Speed is Enabled.
        /// </summary>
        [JsonPropertyName("moveSpeed2")]
        public bool MoveSpeed { get; set; } = false;

        /// <summary>
        /// Full Bright is Enabled.
        /// </summary>
        [JsonPropertyName("fullBright")]
        public bool FullBright { get; set; } = false;

        /// <summary>
        /// Super Speed is Enabled.
        /// This has a high ban risk.
        /// </summary>
        [JsonPropertyName("superSpeedRisky")]
        public SuperSpeedConfig SuperSpeed { get; set; } = new();

        /// <summary>
        /// Makes weapon operations faster (ads, mag loading, etc.)
        /// </summary>
        [JsonPropertyName("fastWeaponOps")]
        public bool FastWeaponOps { get; set; } = false;

        /// <summary>
        /// Makes loading/unloading magazines faster.
        /// </summary>
        [JsonPropertyName("fastLoadUnload")]
        public bool FastLoadUnload { get; set; } = false;
    }

    public sealed class SuperSpeedConfig
    {
        /// <summary>
        /// Super Speed is Enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Speed multiplier.
        /// </summary>
        [JsonPropertyName("speed")]
        public int Speed { get; set; } = 80;

        /// <summary>
        /// Time (ms) that speed is active.
        /// </summary>
        [JsonPropertyName("onTime")]
        public int OnTime { get; set; } = 90;

        /// <summary>
        /// Time (ms) that speed is inactive.
        /// </summary>
        [JsonPropertyName("offTime")]
        public int OffTime { get; set; } = 220;
    }

    /// <summary>
    /// Loot Through Walls Config.
    /// </summary>
    public sealed class LTWConfig
    {
        /// <summary>
        /// True if LTW is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// LTW Zoom Amount.
        /// </summary>
        [JsonPropertyName("zoomAmount")]
        public int ZoomAmount { get; set; } = 200;
    }

    public sealed class WideLeanConfig
    {
        /// <summary>
        /// Enable Wide Lean Feature on Startup.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Wide lean mode (hold or toggle,etc.)
        /// </summary>
        [JsonPropertyName("mode")]
        public HotkeyMode Mode { get; set; } = HotkeyMode.Hold;

        /// <summary>
        /// Amount of wide lean (scaled to 0.01 - 1.00).
        /// </summary>
        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 50;
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
        public float FOV { get; set; } = 150f;

        /// <summary>
        /// Bone for the Default Aimbot Target.
        /// </summary>
        [JsonPropertyName("bone")]
        public Bones Bone { get; set; } = Bones.HumanSpine3;

        /// <summary>
        /// Always headshot AI Targets.
        /// </summary>
        [JsonPropertyName("headshotAI")]
        public bool HeadshotAI { get; set; } = true;

        /// <summary>
        /// True if Aimbot Re-Locking is disabled after a target dies/is no longer valid.
        /// </summary>
        [JsonPropertyName("disableReLock")]
        public bool DisableReLock { get; set; } = false;

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
        public int HeadPercent { get; set; } = 1;
        /// <summary>
        /// Torso shot percentage.
        /// </summary>
        [JsonPropertyName("torsoPercent")]
        public int TorsoPercent { get; set; } = 33;
        /// <summary>
        /// Arms shot percentage.
        /// </summary>
        [JsonPropertyName("armsPercent")]
        public int ArmsPercent { get; set; } = 33;
        /// <summary>
        /// Legs shot percentage.
        /// </summary>
        [JsonPropertyName("legsPercent")]
        public int LegsPercent { get; set; } = 33;

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
            HeadPercent = 1;
            TorsoPercent = 33;
            ArmsPercent = 33;
            LegsPercent = 33;
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
        public bool SafeLock { get; set; } = false;
    }

    public sealed class WidgetsConfig
    {
        #region Aimview

        [JsonInclude]
        [JsonPropertyName("aimviewLocation")]
        public RectFSer _aimviewLoc { private get; set; }

        [JsonPropertyName("aimviewMinimized")] public bool AimviewMinimized { get; set; } = false;

        /// <summary>
        /// Aimview Location
        /// </summary>
        [JsonIgnore]
        public SKRect AimviewLocation
        {
            get => new(_aimviewLoc.Left, _aimviewLoc.Top, _aimviewLoc.Right, _aimviewLoc.Bottom);
            set => _aimviewLoc = new RectFSer(value.Left, value.Top, value.Right, value.Bottom);
        }

        #endregion

        #region Player Info

        [JsonInclude]
        [JsonPropertyName("playerInfoLocation")]
        public RectFSer _pInfoLoc { private get; set; }

        [JsonPropertyName("playerInfoMinimized")]
        public bool PlayerInfoMinimized { get; set; } = false;

        /// <summary>
        /// Aimview Location
        /// </summary>
        [JsonIgnore]
        public SKRect PlayerInfoLocation
        {
            get => new(_pInfoLoc.Left, _pInfoLoc.Top, _pInfoLoc.Right, _pInfoLoc.Bottom);
            set => _pInfoLoc = new RectFSer(value.Left, value.Top, value.Right, value.Bottom);
        }

        #endregion
    }

    /// <summary>
    /// Configuration for Web Radar.
    /// </summary>
    public sealed class WebRadarConfig
    {
        /// <summary>
        /// True if UPnP should be enabled.
        /// </summary>
        [JsonPropertyName("upnp")]
        public bool UPnP { get; set; } = true;
        /// <summary>
        /// IP to bind to.
        /// </summary>
        [JsonPropertyName("host")]
        public string IP { get; set; }
        /// <summary>
        /// TCP Port to bind to.
        /// </summary>
        [JsonPropertyName("port")]
        public string Port { get; set; } = Random.Shared.Next(50000, 60000).ToString();
        /// <summary>
        /// Server Tick Rate (Hz).
        /// </summary>
        [JsonPropertyName("tickRate")]
        public string TickRate { get; set; } = "60";
    }

    /// <summary>
    /// Caches runtime data between sessions.
    /// </summary>
    public sealed class PersistentCache
    {
        [JsonPropertyName("F7xLmP2")]
        [JsonInclude]
        public LowLevelCache LowLevel { get; private set; } = new();

        [JsonPropertyName("profileApi")]
        [JsonInclude]
        public ProfileApiCache ProfileAPI { get; private set; } = new();
    }

    public sealed class ProfileApiCache
    {
        [JsonPropertyName("pid")]
        [JsonInclude]
        public uint PID { get; set; }

        [JsonPropertyName("cache")]
        [JsonInclude]
        public ConcurrentDictionary<string, ProfileData> Profiles { get; private set; } = new();
    }
}
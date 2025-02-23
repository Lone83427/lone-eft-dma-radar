global using SKSvg = Svg.Skia.SKSvg;
global using SkiaSharp;
global using SkiaSharp.Views.Desktop;
global using System.ComponentModel;
global using System.Data;
global using System.Reflection;
global using System.Diagnostics;
global using System.Text;
global using System.Text.Json;
global using System.Text.Json.Serialization;
global using System.Numerics;
global using System.Collections.Concurrent;
global using System.Net;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Runtime.Intrinsics;
global using System.Runtime.Intrinsics.X86;
global using System.Net.Security;
global using System.Security.Cryptography;
global using System.Security.Cryptography.X509Certificates;
global using System.Collections;
global using System.Net.Http.Headers;
global using System.Buffers;
global using System.Buffers.Binary;
global using SDK;
global using LoneShared;
global using LoneShared.Misc;
global using LoneShared.Common;
using System.Runtime.Versioning;
using arena_dma_radar;
using arena_dma_radar.UI.Radar;
using arena_dma_radar.UI.Misc;
using arena_dma_radar.Arena;
using arena_dma_radar.UI.ESP;
using LoneShared.Common.Maps;
using arena_dma_radar.Arena.Features;
using LoneShared.Common.Misc.Data;

[assembly: AssemblyTitle(Program.Name)]
[assembly: AssemblyProduct(Program.Name)]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: SupportedOSPlatform("Windows")]

namespace arena_dma_radar
{
    internal static class Program 
    {
        internal const string Name = "Arena DMA Radar";

        /// <summary>
        /// Global Program Configuration.
        /// </summary>
        public static Config Config { get; }

        /// <summary>
        /// Path to the Configuration Folder in %AppData%
        /// </summary>
        public static DirectoryInfo ConfigPath { get; } =
            new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "eft-dma-radar"));

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        [STAThread]
        static void Main(string[] args)
        {
            ConfigureProgram();
            Application.Run(new MainForm());
        }

        #region Private Members

        static Program()
        {
            ConfigPath.Create();
            var config = Config.Load();
            LoneShared.SharedProgram.Initialize(ConfigPath, config);
            Config = config;
        }

        /// <summary>
        /// Configure Program Startup.
        /// </summary>
        /// <param name="args">Args passed from entrypoint.</param>
        [Obfuscation(Feature = "Virtualization", Exclude = false)]
        private static void ConfigureProgram()
        {
            ApplicationConfiguration.Initialize();
            LoneDataManager.ModuleInitAsync().GetAwaiter().GetResult();
            LoneMapManager.ModuleInit();
            MemoryInterface.ModuleInit();
            FeatureManager.ModuleInit();
            ResourceJanitor.ModuleInit(new Action(CleanupWindowResources));
        }

        private static void CleanupWindowResources()
        {
            MainForm.Window?.PurgeSKResources();
            EspForm.Window?.PurgeSKResources();
        }

        #endregion
    }
}
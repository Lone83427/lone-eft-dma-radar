using eft_dma_shared.Misc;
using System.Diagnostics;
using eft_dma_shared.Common.Misc.Config;
using eft_dma_shared.Common.Misc.Commercial;

namespace eft_dma_shared
{
    /// <summary>
    /// Encapsulates a Shared State between this satelite module and the main application.
    /// </summary>
    public static class SharedProgram
    {
        private const string _mutexID = "0f908ff7-e614-6a93-60a3-cee36c9cea91";
#pragma warning disable IDE0052 // Remove unread private members
        private static Mutex _mutex;
#pragma warning restore IDE0052 // Remove unread private members

        internal static DirectoryInfo ConfigPath { get; private set; }
        internal static IConfig Config { get; private set; }

        /// <summary>
        /// Initialize the Shared State between this module and the main application.
        /// </summary>
        /// <param name="configPath">Config path directory.</param>
        /// <param name="config">Config file instance.</param>
        /// <exception cref="ApplicationException"></exception>
        public static void Initialize(DirectoryInfo configPath, IConfig config)
        {
            ArgumentNullException.ThrowIfNull(configPath, nameof(configPath));
            ArgumentNullException.ThrowIfNull(config, nameof(config));
            ConfigPath = configPath;
            Config = config;
            _mutex = new Mutex(true, _mutexID, out bool singleton);
            if (!singleton)
                throw new ApplicationException("The Application Is Already Running!");
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            SetHighPerformanceMode();
#if !DEBUG
            VerifyDependencies();
#endif
        }

        /// <summary>
        /// Sets High Performance mode in Windows Power Plans and Process Priority.
        /// </summary>
        private static void SetHighPerformanceMode()
        {
            /// Prepare Process for High Performance Mode
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Native.SetThreadExecutionState(Native.EXECUTION_STATE.ES_CONTINUOUS | Native.EXECUTION_STATE.ES_SYSTEM_REQUIRED |
                                           Native.EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            var highPerformanceGuid = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
            if (Native.PowerSetActiveScheme(IntPtr.Zero, ref highPerformanceGuid) != 0)
                LoneLogging.WriteLine("WARNING: Unable to set High Performance Power Plan");
            /// Set Working Set limits for process
            TrySetProcessWorkingSetSize();
        }

        /// <summary>
        /// Validates that all startup dependencies are present.
        /// </summary>
        private static void VerifyDependencies()
        {
            var dependencies = new List<string>
            {
                "vmm.dll",
                "leechcore.dll",
                "FTD3XX.dll",
                "symsrv.dll",
                "dbghelp.dll",
                "vcruntime140.dll",
                "tinylz4.dll",
                "libSkiaSharp.dll",
                "libHarfBuzzSharp.dll",
                "Maps.bin"
            };

            foreach (var dep in dependencies)
                if (!File.Exists(dep))
                    throw new FileNotFoundException($"Missing Dependency '{dep}'\n\n" +
                                                    $"==Troubleshooting==\n" +
                                                    $"1. Make sure that you unzipped the Client Files, and that all files are present in the same folder as the Radar Client (EXE).\n" +
                                                    $"2. If using a shortcut, make sure the Current Working Directory (CWD) is set to the " +
                                                    $"same folder that the Radar Client (EXE) is located in.");
        }


        /// <summary>
        /// Set the Working Set of the Process.
        /// </summary>
        private static void TrySetProcessWorkingSetSize()
        {
            const ulong chunk = 32ul * 1024 * 1024; // 32MB
            ulong workingSet = 4ul * 1024 * 1024 * 1024; // 4GB
            while (!Native.SetProcessWorkingSetSizeEx(-1, (workingSet / 2) & ~(0x1000ul - 1), workingSet, Native.QUOTA_LIMITS_HARDWS_MIN_ENABLE | Native.QUOTA_LIMITS_HARDWS_MAX_DISABLE) &&
                workingSet > chunk)
            {
                workingSet -= chunk;
            }
        }

        /// <summary>
        /// Called when AppDomain is shutting down.
        /// </summary>
        private static void CurrentDomain_ProcessExit(object sender, EventArgs e) =>
            Config.Save();
    }
}

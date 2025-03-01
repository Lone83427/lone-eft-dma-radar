using arena_dma_radar.UI.Misc;

namespace arena_dma_radar.UI.ESP
{
    internal static class ESP
    {
        /// <summary>
        /// ESP Configuration.
        /// </summary>
        public static ESPConfig Config { get; } = Program.Config.ESP;
    }
}

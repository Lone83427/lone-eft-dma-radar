using eft_dma_shared.Common.Unity.LowLevel;
using SkiaSharp;
using System.Text.Json.Serialization;

namespace eft_dma_shared.Common.Misc.Config
{
    public sealed class ChamsConfig
    {
        /// <summary>
        /// True if this feature is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Last Chams Mode that the player set.
        /// </summary>
        [JsonPropertyName("mode")]
        public ChamsManager.ChamsMode Mode { get; set; } = ChamsManager.ChamsMode.Basic;

        /// <summary>
        /// Visible color for vischeck chams.
        /// </summary>
        [JsonPropertyName("visColor")]
        public string VisibleColor { get; set; } = SKColor.Parse("FF32D42D").ToString();

        /// <summary>
        /// Invisible color for vischeck chams.
        /// </summary>
        [JsonPropertyName("invisColor")]
        public string InvisibleColor { get; set; } = SKColor.Parse("FFCD251E").ToString();
    }
}

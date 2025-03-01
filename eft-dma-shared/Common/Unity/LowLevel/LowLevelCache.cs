using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace eft_dma_shared.Common.Unity.LowLevel
{
    /// <summary>
    /// Contains Cache Data for Unity Low Level API.
    /// </summary>
    public sealed class LowLevelCache
    {
        [JsonPropertyName("ZK8MQLY")]
        public uint PID { get; set; }
        [JsonPropertyName("X8kP9Ln")]
        public ulong CodeCave { get; set; }
        [JsonPropertyName("tZ6Yv7m")]
        public ulong UnityPlayerDll { get; set; }
        [JsonPropertyName("K9XrF2q")]
        public ulong MonoDll { get; set; }
        [JsonPropertyName("XJ9TQWZ")]
        public ulong HookedMonoFuncAddress { get; set; }
        [JsonPropertyName("PL6VDRM")]
        public ulong HookedMonoFunc { get; set; }
        [JsonPropertyName("L3Wp7Tz")]
        public ConcurrentDictionary<int, int> ChamsMaterialIds { get; set; } = new();

        /// <summary>
        /// Persist the cache to disk.
        /// </summary>
        public async Task SaveAsync() => await SharedProgram.Config.SaveAsync();

        /// <summary>
        /// Reset the cache to defaults.
        /// </summary>
        public void Reset()
        {
            CodeCave = default;
            UnityPlayerDll = default;
            MonoDll = default;
            HookedMonoFuncAddress = default;
            HookedMonoFunc = default;
            ChamsMaterialIds.Clear();
        }
    }
}

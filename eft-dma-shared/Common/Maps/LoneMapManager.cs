using eft_dma_shared.Common.Misc;
using System.Collections.Frozen;
using System.IO.Compression;
using System.Text.Json;

namespace eft_dma_shared.Common.Maps
{
    /// <summary>
    /// Maintains Map Resources for this application.
    /// </summary>
    public static class LoneMapManager
    {
        private static readonly Lock _sync = new();
        private static ZipArchive _zip;
        private static FrozenDictionary<string, LoneMapConfig> _maps;

        /// <summary>
        /// Currently Loaded Map.
        /// </summary>
        public static ILoneMap Map { get; private set; }

        /// <summary>
        /// Initialize this Module.
        /// ONLY CALL ONCE!
        /// </summary>
        public static void ModuleInit()
        {
            const string mapsPath = "Maps.bin";
            try
            {
                /// Load Maps
                var mapsStream = FileCrypt.OpenEncryptedFile(mapsPath) ??
                               throw new Exception($"Failed to load Maps Bundle '{mapsPath}'");
                var zip = new ZipArchive(mapsStream, ZipArchiveMode.Read, false);
                var mapsBuilder = new Dictionary<string, LoneMapConfig>(StringComparer.OrdinalIgnoreCase);
                foreach (var file in zip.Entries)
                {
                    if (file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                    {
                        using var stream = file.Open();
                        var config = JsonSerializer.Deserialize<LoneMapConfig>(stream);
                        foreach (var id in config!.MapID)
                            mapsBuilder.Add(id, config);
                    }
                }
                _maps = mapsBuilder.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
                _zip = zip;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to Initialize Maps!", ex);
            }
        }

        /// <summary>
        /// Update the current map and load resources into Memory.
        /// </summary>
        /// <param name="mapId">Id of map to load.</param>
        /// <param name="map"></param>
        /// <exception cref="Exception"></exception>
        public static void LoadMap(string mapId)
        {
            lock (_sync)
            {
                try
                {
                    if (!_maps.TryGetValue(mapId, out var newMap))
                        newMap = _maps["default"];
                    Map?.Dispose();
                    Map = null;
                    Map = new LoneSvgMap(_zip, mapId, newMap);
                }
                catch (Exception ex)
                {
                    throw new Exception($"ERROR loading '{mapId}'", ex);
                }
            }
        }
    }
}

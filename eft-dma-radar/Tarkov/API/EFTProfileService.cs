using eft_dma_radar.UI.Misc;
using eft_dma_shared.Common.DMA;
using eft_dma_shared.Common.Misc.Commercial;

namespace eft_dma_radar.Tarkov.API
{
    public static class EFTProfileService
    {
        #region Fields / Constructor
        private static readonly HttpClient _client;
        private static readonly Lock _syncRoot = new();
        private static readonly ConcurrentDictionary<string, ProfileData> _profiles = new(StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> _eftApiNotFound = new(StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> _tdevNotFound = new(StringComparer.OrdinalIgnoreCase);

        private static CancellationTokenSource _cts = new();

        /// <summary>
        /// Persistent Cache Access.
        /// </summary>
        private static ProfileApiCache Cache { get; } = Program.Config.Cache.ProfileAPI;

        static EFTProfileService()
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            };
            _client = new HttpClient(handler);
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("identity"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            _client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            new Thread(Worker)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            }.Start();
            MemDMA.GameStarted += MemDMA_GameStarted;
            MemDMA.GameStopped += MemDMA_GameStopped;
        }

        private static void MemDMA_GameStopped(object sender, EventArgs e)
        {
            lock (_syncRoot)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = new();
            }
        }

        private static void MemDMA_GameStarted(object sender, EventArgs e)
        {
            uint pid = Memory.PID;
            if (Cache.PID != pid)
            {
                Cache.PID = pid;
                Cache.Profiles.Clear();
            }
        }

        #endregion

        #region Public API
        /// <summary>
        /// Profile data returned by the Tarkov API.
        /// </summary>
        public static IReadOnlyDictionary<string, ProfileData> Profiles => _profiles;

        /// <summary>
        /// Attempt to register a Profile for lookup.
        /// </summary>
        /// <param name="accountId">Profile's Account ID.</param>
        public static void RegisterProfile(string accountId) => _profiles.TryAdd(accountId, null);

        #endregion

        #region Internal API
        private static async void Worker()
        {
            while (true)
            {
                if (MemDMABase.WaitForProcess())
                {
                    try
                    {
                        CancellationToken ct;
                        lock (_syncRoot)
                        {
                            ct = _cts.Token;
                        }
                        var profiles = _profiles
                            .Where(x => !string.IsNullOrWhiteSpace(x.Key) && x.Value is null)
                            .Select(x => x.Key);
                        if (profiles.Any())
                        {
                            foreach (var accountId in profiles)
                            {
                                ct.ThrowIfCancellationRequested();
                                await GetProfileAsync(accountId);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LoneLogging.WriteLine($"[EFTProfileService] ERROR: {ex}");
                    }
                    finally { await Task.Delay(250); } // Rate-Limit
                }
            }
        }

        /// <summary>
        /// Get profile data for a particular Account ID.
        /// NOT thread safe. Always await this method and only run from one thread.
        /// </summary>
        /// <param name="accountId">Account ID of profile to lookup.</param>
        /// <returns></returns>
        private static async Task GetProfileAsync(string accountId)
        {
            if (Cache.Profiles.TryGetValue(accountId, out var cachedProfile))
            {
                _profiles[accountId] = cachedProfile;
            }
            else
            {
                try
                {
                    ProfileData result;
                    result = await LookupFromTarkovDevAsync(accountId);
                    /// eft-api.tech now requires a bearer token, in the future should implement a backend server to do the API requests for this.
                    /// Disabling it for now.
                    //result ??= await LookupFromEftApiTechAsync(accountId);
                    if (result is not null ||
                        (result is null && /*_eftApiNotFound.Contains(accountId) &&*/ _tdevNotFound.Contains(accountId)))
                    {
                        Cache.Profiles[accountId] = result;
                    }
                    _profiles[accountId] = result;
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(1.5d));
                }
            }
        }

        /// <summary>
        /// Perform a BEST-EFFORT profile lookup via Tarkov.dev
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async Task<ProfileData> LookupFromTarkovDevAsync(string accountId)
        {
            const string baseUrl = "https://players.tarkov.dev/profile/"; // [profileid].json
            try
            {
                if (_tdevNotFound.Contains(accountId))
                {
                    return null;
                }
                string url = baseUrl + accountId + ".json";
                using var response = await _client.GetAsync(url);
                if (response.StatusCode is HttpStatusCode.NotFound)
                {
                    LoneLogging.WriteLine($"[EFTProfileService] Profile '{accountId}' not found by Tarkov.Dev.");
                    _tdevNotFound.Add(accountId);
                    return null;
                }
                if (response.StatusCode is HttpStatusCode.TooManyRequests) // Force Rate-Limit
                {
                    LoneLogging.WriteLine("[EFTProfileService] Rate-Limited by Tarkov.Dev - Pausing for 1 minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    return null;
                }
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ProfileData>(stream) ??
                    throw new ArgumentNullException("result");
                LoneLogging.WriteLine($"[EFTProfileService] Got Profile '{accountId}' via Tarkov.Dev!");
                return result;
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[EFTProfileService] Unhandled ERROR looking up profile '{accountId}' via Tarkov.Dev: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Perform a profile lookup via eft-api.tech
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static async Task<ProfileData> LookupFromEftApiTechAsync(string accountId)
        {
            const string baseUrl = "https://eft-api.tech/api/profile/";
            try
            {
                if (_eftApiNotFound.Contains(accountId))
                {
                    return null;
                }
                string url = baseUrl + accountId + "?includeOnlyPmcStats=true";
                using var response = await _client.GetAsync(url);
                if (response.StatusCode is HttpStatusCode.NotFound)
                {
                    LoneLogging.WriteLine($"[EFTProfileService] Profile '{accountId}' not found by eft-api.tech.");
                    _eftApiNotFound.Add(accountId);
                    return null;
                }
                if (response.StatusCode is HttpStatusCode.TooManyRequests) // Force Rate-Limit
                {
                    LoneLogging.WriteLine("[EFTProfileService] Rate-Limited by eft-api.tech - Pausing for 1 minute.");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    return null;
                }
                response.EnsureSuccessStatusCode();
                using var stream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<ProfileResponseContainer>(stream) ??
                    throw new ArgumentNullException("result");
                LoneLogging.WriteLine($"[EFTProfileService] Got Profile '{accountId}' via eft-api.tech!");
                return result.Data;
            }
            catch (Exception ex)
            {
                LoneLogging.WriteLine($"[EFTProfileService] Unhandled ERROR looking up profile '{accountId}' via eft-api.tech: {ex}");
                return null;
            }
        }

        #region Profile Response JSON Structure

        public sealed class ProfileResponseContainer
        {
            [JsonPropertyName("data")]
            public ProfileData Data { get; set; }
        }

        public sealed class ProfileData
        {

            [JsonPropertyName("info")]
            public ProfileInfo Info { get; set; }

            [JsonPropertyName("pmcStats")]
            public StatsContainer PmcStats { get; set; }

        }
        public sealed class ProfileInfo
        {
            [JsonPropertyName("nickname")]
            public string Nickname { get; set; }

            [JsonPropertyName("experience")]
            public int Experience { get; set; }

            [JsonPropertyName("memberCategory")]
            public int MemberCategory { get; set; }

            [JsonPropertyName("registrationDate")]
            public int RegistrationDate { get; set; }
        }
        public sealed class StatsContainer
        {
            [JsonPropertyName("eft")]
            public CountersContainer Counters { get; set; }
        }

        public sealed class CountersContainer
        {
            [JsonPropertyName("totalInGameTime")]
            public int TotalInGameTime { get; set; }

            [JsonPropertyName("overAllCounters")]
            public OverallCounters OverallCounters { get; set; }
        }

        public sealed class OverallCounters
        {
            [JsonPropertyName("Items")]
            public List<OverAllCountersItem> Items { get; set; }
        }

        public sealed class OverAllCountersItem
        {
            [JsonPropertyName("Key")]
            public List<string> Key { get; set; } = new();

            [JsonPropertyName("Value")]
            public int Value { get; set; }
        }
        #endregion

        #endregion
    }
}

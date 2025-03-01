using eft_dma_radar.Tarkov.API;
using eft_dma_shared.Common.Misc.Data;

namespace eft_dma_radar.Tarkov.EFTPlayer.Plugins
{
    public sealed class PlayerProfile
    {
        private readonly ObservedPlayer _player;
        public PlayerProfile(ObservedPlayer player)
        {
            _player = player;
        }

        /// <summary>
        /// Player's Nickname (via Profile Data).
        /// </summary>
        public string Nickname => this.Profile?.Info?.Nickname;

        /// <summary>
        /// Player's current profile (if Profile Lookups are enabled).
        /// Returns NULL if profile cannot be retrieved.
        /// </summary>
        private EFTProfileService.ProfileData Profile
        {
            get
            {
                string acctID = _player.AccountID;
                if (string.IsNullOrEmpty(acctID))
                    return null;
                else if (EFTProfileService.Profiles.TryGetValue(acctID, out var profile))
                    return profile;
                else
                    EFTProfileService.RegisterProfile(acctID);
                return null;
            }
        }
        private float? _overallKD;
        /// <summary>
        /// Player's Overall KD (only human players).
        /// </summary>
        public float? Overall_KD
        {
            get
            {
                if (_overallKD is float kd)
                    return kd;
                var stats = Profile?.PmcStats;
                if (stats is not null)
                {
                    long? killsObj = stats.Counters?.OverallCounters?.Items?.FirstOrDefault(x => x.Key?.Contains("Kills") ?? false)?.Value;
                    long? deathsObj = stats.Counters?.OverallCounters?.Items?.FirstOrDefault(x => x.Key?.Contains("Deaths") ?? false)?.Value;
                    if (killsObj is long kills && deathsObj is long deaths)
                    {
                        if (deaths == 0)
                            return _overallKD = kills;
                        return _overallKD = (float)kills / (float)deaths;
                    }
                }
                return null;
            }
        }
        public int? _raidCount;
        /// <summary>
        /// Player's Overall Raid Count (only human players).
        /// </summary>
        public int? RaidCount
        {
            get
            {
                if (_raidCount is int raidCount)
                    return raidCount;
                var stats = Profile?.PmcStats;
                if (stats is not null)
                {
                    int? sessionsObj = stats.Counters?.OverallCounters?.Items?.FirstOrDefault(x => x.Key?.Contains("Sessions") ?? false)?.Value;
                    if (sessionsObj is int sessions)
                        return _raidCount = sessions;
                }
                return null;
            }
        }
        private int? _survivedCount;
        private float? _survivedRate;
        /// <summary>
        /// Player's Overall Survival Percentage (only human players).
        /// EX: 50 (50%)
        /// </summary>
        public float? SurvivedRate
        {
            get
            {
                if (_survivedRate is float survivedRate)
                    return survivedRate;
                var stats = Profile?.PmcStats;
                if (stats is not null)
                {
                    if (_survivedCount is not int survived)
                    {
                        int? survivedObj = stats.Counters?.OverallCounters?.Items?.FirstOrDefault(x => x.Key?.Contains("Survived") ?? false)?.Value;
                        if (survivedObj is int survivedResult)
                            _survivedCount = survivedResult;
                        return null;
                    }
                    if (RaidCount is int raidCount)
                    {
                        if (raidCount == 0)
                            return _survivedRate = 0f;
                        return _survivedRate = ((float)survived / (float)raidCount) * 100f;
                    }
                }
                return null;
            }
        }

        private int? _hours;
        /// <summary>
        /// Player's Total Hours Played (only human players).
        /// </summary>
        public int? Hours
        {
            get
            {
                if (_hours is int hours)
                    return hours;
                var counters = Profile?.PmcStats?.Counters;
                if (counters is not null && counters.TotalInGameTime != default)
                {
                    const float hoursFactor = 3600f; // Divide in-game time by this to get the total hours
                    return _hours = (int)Math.Round((float)counters.TotalInGameTime / hoursFactor);
                }
                return null;
            }
        }

        private int? _level;
        /// <summary>
        /// Player's In-Game Level (only human players).
        /// </summary>
        public int? Level
        {
            get
            {
                if (_level is int level)
                    return level;
                var info = Profile?.Info;
                if (info is not null && info.Experience != default)
                {
                    return _level = GameData.XPTable.Where(x => x.Key > info.Experience).FirstOrDefault().Value - 1;
                }
                return null;
            }
        }

        private Enums.EMemberCategory? _memberCategory;
        /// <summary>
        /// Player's Member Category (Standard/EOD/Dev/Sherpa, etc. -- only human players).
        /// </summary>
        public Enums.EMemberCategory? MemberCategory
        {
            get
            {
                if (_memberCategory is Enums.EMemberCategory memberCategory)
                    return memberCategory;
                var info = Profile?.Info;
                if (info is not null)
                {
                    return _memberCategory = (Enums.EMemberCategory)info.MemberCategory;
                }
                return null;
            }
        }

        /// <summary>
        /// True if this player is on an EOD Edition Account.
        /// </summary>
        private bool IsEOD
        {
            get
            {
                var mcObj = this.MemberCategory;
                if (mcObj is Enums.EMemberCategory mc &&
                    (mc & Enums.EMemberCategory.UniqueId) == Enums.EMemberCategory.UniqueId)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// True if this player is on an Unheard Edition Account.
        /// </summary>
        private bool IsUnheard
        {
            get
            {
                var mcObj = this.MemberCategory;
                if (mcObj is Enums.EMemberCategory mc &&
                    (mc & Enums.EMemberCategory.Unheard) == Enums.EMemberCategory.Unheard)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Account type (eod, uh, etc.)
        /// Requires Profile Lookup
        /// </summary>
        public string Acct
        {
            get
            {
                if (IsUnheard)
                    return "UH";
                else if (IsEOD)
                    return "EOD";
                return "--";
            }
        }
    }
}

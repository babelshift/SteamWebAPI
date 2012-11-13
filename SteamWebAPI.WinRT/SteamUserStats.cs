using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamUserStats : SteamWebRequest
    {
        public SteamUserStats(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<List<AchievementPercentage>> GetGlobalAchievementPercentagesForAppAsync(uint appId)
        {
            WebRequestParameter appIdParameter = new WebRequestParameter("gameid", appId.ToString());
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();
            requestParameters.Add(appIdParameter);

            JObject data = await PerformSteamRequestAsync("ISteamUserStats", "GetGlobalAchievementPercentagesForApp", 2, requestParameters);

            List<AchievementPercentage> achievementPercentages = new List<AchievementPercentage>();

            try
            {
                foreach (JObject achievementObject in data["achievementpercentages"]["achievements"])
                {
                    AchievementPercentage achievementPercentage = new AchievementPercentage();
                    achievementPercentage.Name = achievementObject["name"].ToString();
                    achievementPercentage.Percent = Convert.ToDouble(achievementObject["percent"].ToString());

                    achievementPercentages.Add(achievementPercentage);
                }

                return achievementPercentages;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }


        public void GetGlobalStatsForGameAsync(uint appId, uint statCount, IList<string> statNames)
        {
            GetGlobalStatsForGameAsync(appId, statCount, statNames, null, null);
        }

        /// <summary>
        /// I do not know how to form the "name[0]" parameter of this request
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="statCount"></param>
        /// <param name="statNames"></param>
        public void GetGlobalStatsForGameAsync(uint appId, uint statCount, IList<string> statNames, DateTime? startDate, DateTime? endDate)
        {
            // example request: https://api.steampowered.com/ISteamUserStats/GetGlobalStatsForGame/v0001/?appid=620&count=1&name[0]=

            // I do not know how to determine the values to pass for name[0] (which is documented vaguely as "the stat name")
        }

        public async Task<int> GetNumberOfCurrentPlayersAsync(uint appId)
        {
            WebRequestParameter appIdParameter = new WebRequestParameter("appid", appId.ToString());
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();
            requestParameters.Add(appIdParameter);

            JObject data = await PerformSteamRequestAsync("ISteamUserStats", "GetNumberOfCurrentPlayers", 1, requestParameters);

            try
            {
                int playerCount = 0;

                foreach (JProperty responseProperty in data["response"])
                {
                    if (responseProperty.Name == "result")
                    {
                        if (int.Parse(responseProperty.Value.ToString()) != 1)
                            throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);
                    }
                    else if (responseProperty.Name == "player_count")
                        playerCount = Convert.ToInt32(responseProperty.Value.ToString());
                }

                return playerCount;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<UserAchievements> GetPlayerAchievementsAsync(ulong steamId, uint appId, string language = "")
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            WebRequestParameter appIdParameter = new WebRequestParameter("appid", appId.ToString());
            requestParameters.Add(appIdParameter);

            if (!String.IsNullOrEmpty(language))
            {
                WebRequestParameter languageParameter = new WebRequestParameter("l", language);
                requestParameters.Add(languageParameter);
            }

            JObject data = await PerformSteamRequestAsync("ISteamUserStats", "GetPlayerAchievements", 1, requestParameters);

            try
            {
                UserAchievements playerStats = new UserAchievements();

                foreach (JProperty responseProperty in data["playerstats"])
                {
                    if (responseProperty.Name == "steamID")
                        playerStats.SteamID = Convert.ToInt64(responseProperty.Value.ToString());
                    else if (responseProperty.Name == "gameName")
                        playerStats.GameName = responseProperty.Value.ToString();
                    else if (responseProperty.Name == "achievements")
                    {
                        foreach (JObject achievementObject in data["playerstats"]["achievements"])
                        {
                            Achievement achievement = new Achievement();
                            achievement.Name = achievementObject["apiname"].ToString();

                            if (achievementObject["achieved"].ToString() == "1")
                                achievement.IsAchieved = true;
                            else
                                achievement.IsAchieved = false;

                            playerStats.Achievements.Add(achievement);
                        }
                    }
                }

                return playerStats;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<GameSchema> GetSchemaForGameAsync(uint appId, string language = "")
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter appIdParameter = new WebRequestParameter("appid", appId.ToString());
            requestParameters.Add(appIdParameter);

            if (!String.IsNullOrEmpty(language))
            {
                WebRequestParameter languageParameter = new WebRequestParameter("l", language);
                requestParameters.Add(languageParameter);
            }

            JObject data = await PerformSteamRequestAsync("ISteamUserStats", "GetSchemaForGame", 2, requestParameters);

            try
            {
                GameSchema gameSchema = new GameSchema();

                foreach (JProperty responseProperty in data["game"])
                {
                    if (responseProperty.Name == "gameName")
                        gameSchema.GameName = responseProperty.Value.ToString();
                    else if (responseProperty.Name == "gameVersion")
                        gameSchema.GameVersion = responseProperty.Value.ToString();
                    else if (responseProperty.Name == "availableGameStats")
                    {
                        foreach (JObject achievementObject in data["game"]["availableGameStats"]["achievements"])
                        {
                            AchievementSchema achievementSchema = new AchievementSchema();
                            achievementSchema.Name = achievementObject["name"].ToString();
                            achievementSchema.DefaultValue = Convert.ToInt32(achievementObject["defaultvalue"].ToString());
                            achievementSchema.DisplayName = achievementObject["displayName"].ToString();

                            if (achievementObject["hidden"].ToString() == "1")
                                achievementSchema.IsHidden = true;
                            else
                                achievementSchema.IsHidden = false;

                            achievementSchema.Icon = new Uri(achievementObject["icon"].ToString());
                            achievementSchema.IconGray = new Uri(achievementObject["icongray"].ToString());

                            gameSchema.Achievements.Add(achievementSchema);
                        }
                    }
                }

                return gameSchema;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<UserStats> GetUserStatsForGameAsync(ulong steamId, uint appId)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            WebRequestParameter appIdParameter = new WebRequestParameter("appid", appId.ToString());
            requestParameters.Add(appIdParameter);

            JObject data = await PerformSteamRequestAsync("ISteamUserStats", "GetUserStatsForGame", 2, requestParameters);

            try
            {
                UserStats playerStats = new UserStats();

                foreach (JProperty responseProperty in data["playerstats"])
                {
                    if (responseProperty.Name == "steamID")
                        playerStats.SteamID = Convert.ToInt64(responseProperty.Value.ToString());
                    else if (responseProperty.Name == "gameName")
                        playerStats.GameName = responseProperty.Value.ToString();
                    else if (responseProperty.Name == "achievements")
                    {
                        foreach (JObject achievementObject in data["playerstats"]["achievements"])
                        {
                            Achievement achievement = new Achievement();
                            achievement.Name = achievementObject["name"].ToString();

                            if (achievementObject["achieved"].ToString() == "1")
                                achievement.IsAchieved = true;
                            else
                                achievement.IsAchieved = false;

                            playerStats.Achievements.Add(achievement);
                        }
                    }
                    else if (responseProperty.Name == "stats")
                    {
                        foreach (JObject playerStatObject in data["playerstats"]["stats"])
                        {
                            PlayerStat playerStat = new PlayerStat();
                            playerStat.Name = playerStatObject["name"].ToString();
                            playerStat.Value = Convert.ToInt64(playerStatObject["value"].ToString());

                            playerStats.Stats.Add(playerStat);
                        }
                    }
                }

                return playerStats;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using SteamWebModel;

namespace SteamWebAPI
{
    /// <summary>
    /// Class allowing you to use the Steam Web API to log in and use Steam Friends functionality.
    /// </summary>
    public class SteamWebSession
    {
        #region Global Members

        private WebRequestParameter developerKey;

        #endregion

        public SteamWebSession(string developerKey)
        {
            this.developerKey = new WebRequestParameter("key", developerKey);
        }

        #region Web API Calls

        #region ISteamUser

        public async Task<List<Friend>> GetFriendListAsync(long steamId, string relationship = "")
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetFriendListAsync(steamId, relationship);
        }

        public async Task<List<UserBanStatus>> GetPlayerBansAsync(IList<long> steamIds)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetPlayerBansAsync(steamIds);
        }

        public async Task<List<UserSummary>> GetPlayerSummariesAsync(List<long> steamIds)
        {
            SteamUser steamuser = new SteamUser(this.developerKey);
            return await steamuser.GetPlayerSummariesAsync(steamIds);
        }

        public async Task<List<Group>> GetUserGroupListAsync(long steamId)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetUserGroupListAsync(steamId);
        }

        public async Task<long> ResolveVanityURLAsync(string vanityUrl)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.ResolveVanityURLAsync(vanityUrl);
        }

        #endregion

        #region ISteamMicroTxn

        public async Task<Cart> CreateCartAsync(long steamId)
        {
            SteamMicroTransaction steamMicroTransaction = new SteamMicroTransaction(this.developerKey);
            return await steamMicroTransaction.CreateCartAsync(steamId);
        }

        public async Task<Cart> IsValidCartAsync(long steamId, long cartId)
        {
            SteamMicroTransaction steamMicroTransaction = new SteamMicroTransaction(this.developerKey);
            return await steamMicroTransaction.IsValidCartAsync(steamId, cartId);
        }

        #endregion

        #region ISteamWebAPIUtil

        public async Task<SteamServerInfo> GetServerInfoAsync()
        {
            SteamWebAPIUtil steamWebUtil = new SteamWebAPIUtil(this.developerKey);
            return await steamWebUtil.GetServerInfoAsync();
        }

        public async Task<List<SteamInterface>> GetSupportedInterfacesAsync()
        {
            SteamWebAPIUtil steamWebUtil = new SteamWebAPIUtil(this.developerKey);
            return await steamWebUtil.GetSupportedAPIListAsync();
        }

        #endregion

        #region ISteamNews

        public async Task<AppNews> GetNewsForAppAsync(int appId)
        {
            SteamNews steamNews = new SteamNews(this.developerKey);
            return await steamNews.GetNewsForAppAsync(appId);
        }

        #endregion

        #region ISteamUserStats

        public async Task<List<AchievementPercentage>> GetGlobalAchievementPercentagesForAppAsync(uint appId)
        {
            SteamUserStats steamUserStats = new SteamUserStats(this.developerKey);
            return await steamUserStats.GetGlobalAchievementPercentagesForAppAsync(appId);
        }

        public async Task<int> GetNumberOfCurrentPlayersAsync(uint appId)
        {
            SteamUserStats steamUserStats = new SteamUserStats(this.developerKey);
            return await steamUserStats.GetNumberOfCurrentPlayersAsync(appId);
        }

        public async Task<UserAchievements> GetPlayerAchievementsAsync(ulong steamId, uint appId, string language = "")
        {
            SteamUserStats steamUserStats = new SteamUserStats(this.developerKey);
            return await steamUserStats.GetPlayerAchievementsAsync(steamId, appId, language);
        }

        public async Task<GameSchema> GetSchemaForGameAsync(uint appId, string language = "")
        {
            SteamUserStats steamUserStats = new SteamUserStats(this.developerKey);
            return await steamUserStats.GetSchemaForGameAsync(appId, language);
        }

        public async Task<UserStats> GetUserStatsForGameAsync(ulong steamId, uint appId)
        {
            SteamUserStats steamUserStats = new SteamUserStats(this.developerKey);
            return await steamUserStats.GetUserStatsForGameAsync(steamId, appId);
        }

        #endregion

        #region ISteamApps

        public async Task<List<App>> GetAppListAsync()
        {
            SteamApps steamApps = new SteamApps(this.developerKey);
            return await steamApps.GetAppListAsync();
        }

        #endregion

        #endregion
    }
}

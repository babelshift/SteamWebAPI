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

        //private String accessToken;
        //private String umqid;
        //private String steamid;
        //private int message = 0;

        #endregion

        public SteamWebSession(string developerKey)
        {
            this.developerKey = new WebRequestParameter("key", developerKey);
        }

        #region Web API Calls

        #region ISteamUser

        public async Task<List<Friend>> GetFriendList(long steamId, string relationship = "")
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetFriendList(steamId, relationship);
        }

        public async Task<List<UserBanStatus>> GetPlayerBans(IList<long> steamIds)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetPlayerBans(steamIds);
        }

        public async Task<List<UserSummary>> GetPlayerSummaries(List<long> steamIds)
        {
            SteamUser steamuser = new SteamUser(this.developerKey);
            return await steamuser.GetPlayerSummaries(steamIds);
        }

        public async Task<List<Group>> GetUserGroupList(long steamId)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.GetUserGroupList(steamId);
        }

        public async Task<long> ResolveVanityURL(string vanityUrl)
        {
            SteamUser steamUser = new SteamUser(this.developerKey);
            return await steamUser.ResolveVanityURL(vanityUrl);
        }

        #endregion

        #region ISteamMicroTxn

        public async Task<Cart> CreateCart(long steamId)
        {
            SteamMicroTransaction steamMicroTransaction = new SteamMicroTransaction(this.developerKey);
            return await steamMicroTransaction.CreateCart(steamId);
        }

        public async Task<Cart> IsValidCart(long steamId, long cartId)
        {
            SteamMicroTransaction steamMicroTransaction = new SteamMicroTransaction(this.developerKey);
            return await steamMicroTransaction.IsValidCart(steamId, cartId);
        }

        #endregion

        #region ISteamWebAPIUtil

        public async Task<SteamServerInfo> GetServerInfo()
        {
            SteamWebAPIUtil steamWebUtil = new SteamWebAPIUtil(this.developerKey);
            return await steamWebUtil.GetServerInfo();
        }

        public async Task<List<SteamInterface>> GetSupportedInterfaces()
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

        public async Task<List<App>> GetAppList()
        {
            SteamApps steamApps = new SteamApps(this.developerKey);
            return await steamApps.GetAppList();
        }

        #endregion

        #region IOther

        ///// <summary>
        ///// Authenticate with a username and password.
        ///// Sends the SteamGuard e-mail if it has been set up.
        ///// </summary>
        ///// <param name="username">Username</param>
        ///// <param name="password">Password</param>
        ///// <param name="emailauthcode">SteamGuard code sent by e-mail</param>
        ///// <returns>Indication of the authentication status.</returns>
        //public async Task<LoginStatus> AuthenticateAsync(String username, String password, String emailauthcode = "")
        //{
        //    String response = await PerformSteamRequestAsync("ISteamOAuth2/GetTokenWithCredentials/v0001",
        //        "client_id=DE45CD61&grant_type=password&username=" + Uri.EscapeDataString(username) + "&password=" + Uri.EscapeDataString(password) + "&x_emailauthcode=" + emailauthcode + "&scope=read_profile%20write_profile%20read_client%20write_client");

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["access_token"] != null)
        //        {
        //            accessToken = (String)data["access_token"];

        //            return await LoginAsync() ? LoginStatus.LoginSuccessful : LoginStatus.LoginFailed;
        //        }
        //        else if (((string)data["x_errorcode"]).Equals("steamguard_code_required"))
        //            return LoginStatus.SteamGuard;
        //        else
        //            return LoginStatus.LoginFailed;
        //    }
        //    else
        //    {
        //        return LoginStatus.LoginFailed;
        //    }
        //}

        ///// <summary>
        ///// Authenticate with an access token previously retrieved with a username
        ///// and password (and SteamGuard code).
        ///// </summary>
        ///// <param name="accessToken">Access token retrieved with credentials</param>
        ///// <returns>Indication of the authentication status.</returns>
        //public async Task<LoginStatus> AuthenticateAsync(String accessToken)
        //{
        //    this.accessToken = accessToken;
        //    return await LoginAsync() ? LoginStatus.LoginSuccessful : LoginStatus.LoginFailed;
        //}

        ///// <summary>
        ///// Fetch all friends of a given user.
        ///// </summary>
        ///// <remarks>This function does not provide detailed information.</remarks>
        ///// <param name="steamid">steamid of target user or self</param>
        ///// <returns>List of friends or null on failure.</returns>
        //public async Task<List<Friend>> GetFriendsAsync(String steamid = null)
        //{
        //    if (umqid == null) return null;
        //    if (steamid == null) steamid = this.steamid;

        //    String response = await PerformSteamRequestAsync("ISteamUserOAuth/GetFriendList/v0001?access_token=" + accessToken + "&steamid=" + steamid);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["friends"] != null)
        //        {
        //            List<Friend> friends = new List<Friend>();

        //            foreach (JObject friend in data["friends"])
        //            {
        //                Friend f = new Friend();
        //                f.steamid = (String)friend["steamid"];
        //                f.blocked = ((String)friend["relationship"]).Equals("ignored");
        //                f.friendSince = UnixToDateTime((long)friend["friend_since"]);
        //                friends.Add(f);
        //            }

        //            return friends;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Retrieve information about the specified users.
        ///// </summary>
        ///// <remarks>This function doesn't have the 100 users limit the original API has.</remarks>
        ///// <param name="steamids">64-bit SteamIDs of users</param>
        ///// <returns>Information about the specified users</returns>
        //public async Task<List<User>> GetUserInfoAsync(List<String> steamids)
        //{
        //    if (umqid == null) return null;

        //    String response = await PerformSteamRequestAsync("ISteamUserOAuth/GetUserSummaries/v0001?access_token=" + accessToken + "&steamids=" + String.Join(",", steamids.GetRange(0, Math.Min(steamids.Count, 100)).ToArray()));

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["players"] != null)
        //        {
        //            List<User> users = new List<User>();

        //            foreach (JObject info in data["players"])
        //            {
        //                User user = new User();

        //                user.steamid = (String)info["steamid"];
        //                user.profileVisibility = (ProfileVisibility)(int)info["communityvisibilitystate"];
        //                user.profileState = (int)info["profilestate"];
        //                user.nickname = (String)info["personaname"];
        //                user.lastLogoff = UnixToDateTime((long)info["lastlogoff"]);
        //                user.profileUrl = (String)info["profileurl"];
        //                user.status = (UserStatus)(int)info["personastate"];

        //                user.avatarUrl = info["avatar"] != null ? (String)info["avatar"] : "";
        //                if (user.avatarUrl != null) user.avatarUrl = user.avatarUrl.Substring(0, user.avatarUrl.Length - 4);

        //                user.joinDate = UnixToDateTime(info["timecreated"] != null ? (long)info["timecreated"] : 0);
        //                user.primaryGroupId = info["primaryclanid"] != null ? (String)info["primaryclanid"] : "";
        //                user.realName = info["realname"] != null ? (String)info["realname"] : "";
        //                user.locationCountryCode = info["loccountrycode"] != null ? (String)info["loccountrycode"] : "";
        //                user.locationStateCode = info["locstatecode"] != null ? (String)info["locstatecode"] : "";
        //                user.locationCityId = info["loccityid"] != null ? (int)info["loccityid"] : -1;

        //                users.Add(user);
        //            }

        //            // Requests are limited to 100 steamids, so issue multiple requests
        //            if (steamids.Count > 100)
        //                users.AddRange(await GetUserInfoAsync(steamids.GetRange(100, Math.Min(steamids.Count - 100, 100))));

        //            return users;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public async Task<List<User>> GetUserInfoAsync(List<Friend> friends)
        //{
        //    List<String> steamids = new List<String>(friends.Count);
        //    foreach (Friend f in friends) steamids.Add(f.steamid);
        //    return await GetUserInfoAsync(steamids);
        //}

        //public async Task<User> GetUserInfoAsync(String steamid = null)
        //{
        //    if (steamid == null) steamid = this.steamid;
        //    return await GetUserInfoAsync(steamid);
        //}

        ///// <summary>
        ///// Retrieve the avatar of the specified user in the specified format.
        ///// </summary>
        ///// <param name="user">User</param>
        ///// <param name="size">Requested avatar size</param>
        ///// <returns>The avatar as bitmap on success or null on failure.</returns>
        //public async Task<BitmapImage> GetUserAvatarAsync(User user, AvatarSize size = AvatarSize.Small)
        //{
        //    if (user.avatarUrl.Length == 0) return null;

        //    string avatarUrl = String.Empty;

        //    try
        //    {
        //        if (size == AvatarSize.Small)
        //            avatarUrl = user.avatarUrl + ".jpg";
        //        else if (size == AvatarSize.Medium)
        //            avatarUrl = user.avatarUrl + "_medium.jpg";
        //        else
        //            avatarUrl = user.avatarUrl + "_full.jpg";

        //        BitmapImage avatar = new BitmapImage();
        //        HttpClient client = new HttpClient();

        //        using (Stream stream = await client.GetStreamAsync(avatarUrl))
        //        {
        //            InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
        //            var outputStream = randomAccessStream.GetOutputStreamAt(0);
        //            await RandomAccessStream.CopyAndCloseAsync(stream.AsInputStream(), outputStream);
        //            await avatar.SetSourceAsync(randomAccessStream);
        //        }

        //        return avatar;
        //    }
        //    catch
        //    {
        //        throw new Exception("Could not retrieve user avatar at URL: " + avatarUrl);
        //    }
        //}

        ///// <summary>
        ///// Retrieve the avatar of the specified group in the specified format.
        ///// </summary>
        ///// <param name="group">Group</param>
        ///// <param name="size">Requested avatar size</param>
        ///// <returns>The avatar as bitmap on success or null on failure.</returns>
        //public async Task<BitmapImage> GetGroupAvatar(GroupInfo group, AvatarSize size = AvatarSize.Small)
        //{
        //    User user = new User();
        //    user.avatarUrl = group.avatarUrl;
        //    return await GetUserAvatarAsync(user, size);
        //}

        ///// <summary>
        ///// Fetch all groups of a given user.
        ///// </summary>
        ///// <param name="steamid">SteamID</param>
        ///// <returns>List of groups.</returns>
        //public async Task<List<Group>> GetGroupsAsync(String steamid = null)
        //{
        //    if (umqid == null) return null;
        //    if (steamid == null) steamid = this.steamid;

        //    String response = await PerformSteamRequestAsync("ISteamUserOAuth/GetGroupList/v0001?access_token=" + accessToken + "&steamid=" + steamid);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["groups"] != null)
        //        {
        //            List<Group> groups = new List<Group>();

        //            foreach (JObject info in data["groups"])
        //            {
        //                Group group = new Group();

        //                group.steamid = (String)info["steamid"];
        //                group.inviteonly = ((String)info["permission"]).Equals("2");

        //                if (((String)info["relationship"]).Equals("Member"))
        //                    groups.Add(group);
        //            }

        //            return groups;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Retrieve information about the specified groups.
        ///// </summary>
        ///// <param name="steamids">64-bit SteamIDs of groups</param>
        ///// <returns>Information about the specified groups</returns>
        //public async Task<List<GroupInfo>> GetGroupInfoAsync(List<String> steamids)
        //{
        //    if (umqid == null) return null;

        //    String response = await PerformSteamRequestAsync("ISteamUserOAuth/GetGroupSummaries/v0001?access_token=" + accessToken + "&steamids=" + String.Join(",", steamids.GetRange(0, Math.Min(steamids.Count, 100)).ToArray()));

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["groups"] != null)
        //        {
        //            List<GroupInfo> groups = new List<GroupInfo>();

        //            foreach (JObject info in data["groups"])
        //            {
        //                GroupInfo group = new GroupInfo();

        //                group.steamid = (String)info["steamid"];
        //                group.creationDate = UnixToDateTime((long)info["timecreated"]);
        //                group.name = (String)info["name"];
        //                group.profileUrl = "http://steamcommunity.com/groups/" + (String)info["profileurl"];
        //                group.usersOnline = (int)info["usersonline"];
        //                group.usersInChat = (int)info["usersinclanchat"];
        //                group.usersInGame = (int)info["usersingame"];
        //                group.owner = (String)info["ownerid"];
        //                group.members = (int)info["users"];

        //                group.avatarUrl = (String)info["avatar"];
        //                if (group.avatarUrl != null) group.avatarUrl = group.avatarUrl.Substring(0, group.avatarUrl.Length - 4);

        //                group.headline = info["headline"] != null ? (String)info["headline"] : "";
        //                group.summary = info["summary"] != null ? (String)info["summary"] : "";
        //                group.abbreviation = info["abbreviation"] != null ? (String)info["abbreviation"] : "";
        //                group.locationCountryCode = info["loccountrycode"] != null ? (String)info["loccountrycode"] : "";
        //                group.locationStateCode = info["locstatecode"] != null ? (String)info["locstatecode"] : "";
        //                group.locationCityId = info["loccityid"] != null ? (int)info["loccityid"] : -1;
        //                group.favoriteAppId = info["favoriteappid"] != null ? (int)info["favoriteappid"] : -1;

        //                groups.Add(group);
        //            }

        //            // Requests are limited to 100 steamids, so issue multiple requests
        //            if (steamids.Count > 100)
        //                groups.AddRange(await GetGroupInfoAsync(steamids.GetRange(100, Math.Min(steamids.Count - 100, 100))));

        //            return groups;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        //public async Task<List<GroupInfo>> GetGroupInfoAsync(List<Group> groups)
        //{
        //    List<String> steamids = new List<String>(groups.Count);
        //    foreach (Group g in groups) steamids.Add(g.steamid);
        //    return await GetGroupInfoAsync(steamids);
        //}

        //public async Task<GroupInfo> GetGroupInfoAsync(String steamid)
        //{
        //    return await GetGroupInfoAsync(steamid);
        //}

        ///// <summary>
        ///// Let a user know you're typing a message. Should be called periodically.
        ///// </summary>
        ///// <param name="steamid">Recipient of notification</param>
        ///// <returns>Returns a boolean indicating success of the request.</returns>
        //public async Task<bool> SendTypingNotificationAsync(User user)
        //{
        //    if (umqid == null) return false;

        //    String response = await PerformSteamRequestAsync("ISteamWebUserPresenceOAuth/Message/v0001", "?access_token=" + accessToken + "&umqid=" + umqid + "&type=typing&steamid_dst=" + user.steamid);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        return data["error"] != null && ((String)data["error"]).Equals("OK");
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Send a text message to the specified user.
        ///// </summary>
        ///// <param name="steamid">Recipient of message</param>
        ///// <param name="message">Message contents</param>
        ///// <returns>Returns a boolean indicating success of the request.</returns>
        //public async Task<bool> SendMessageAsync(User user, String message)
        //{
        //    if (umqid == null) return false;

        //    String response = await PerformSteamRequestAsync("ISteamWebUserPresenceOAuth/Message/v0001", "?access_token=" + accessToken + "&umqid=" + umqid + "&type=saytext&text=" + Uri.EscapeDataString(message) + "&steamid_dst=" + user.steamid);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        return data["error"] != null && ((String)data["error"]).Equals("OK");
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //public async Task<bool> SendMessageAsync(String steamid, String message)
        //{
        //    User user = new User();
        //    user.steamid = steamid;
        //    return await SendMessageAsync(user, message);
        //}

        ///// <summary>
        ///// Check for updates and new messages.
        ///// </summary>
        ///// <returns>A list of updates.</returns>
        //public async Task<List<Update>> PollAsync()
        //{
        //    if (umqid == null) return null;

        //    String response = await PerformSteamRequestAsync("ISteamWebUserPresenceOAuth/Poll/v0001", "?access_token=" + accessToken + "&umqid=" + umqid + "&message=" + message);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (((String)data["error"]).Equals("OK"))
        //        {
        //            message = (int)data["messagelast"];

        //            List<Update> updates = new List<Update>();

        //            foreach (JObject info in data["messages"])
        //            {
        //                Update update = new Update();

        //                update.timestamp = UnixToDateTime((long)info["timestamp"]);
        //                update.origin = (String)info["steamid_from"];

        //                String type = (String)info["type"];
        //                if (type.Equals("saytext") || type.Equals("my_saytext") || type.Equals("emote"))
        //                {
        //                    update.type = type.Equals("emote") ? UpdateType.Emote : UpdateType.Message;
        //                    update.message = (String)info["text"];
        //                    update.localMessage = type.Equals("my_saytext");
        //                }
        //                else if (type.Equals("typing"))
        //                {
        //                    update.type = UpdateType.TypingNotification;
        //                    update.message = (String)info["text"]; // Not sure if this is useful
        //                }
        //                else if (type.Equals("personastate"))
        //                {
        //                    update.type = UpdateType.UserUpdate;
        //                    update.status = (UserStatus)(int)info["persona_state"];
        //                    update.nick = (String)info["persona_name"];
        //                }
        //                else
        //                {
        //                    continue;
        //                }

        //                updates.Add(update);
        //            }

        //            return updates;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Retrieves information about the server.
        ///// </summary>
        ///// <returns>Returns a structure with the information.</returns>
        //public async Task<ServerInfo> GetServerInfoAsync()
        //{
        //    String response = await PerformSteamRequestAsync("ISteamWebAPIUtil/GetServerInfo/v0001");

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["servertime"] != null)
        //        {
        //            ServerInfo info = new ServerInfo();
        //            info.serverTime = UnixToDateTime((long)data["servertime"]);
        //            info.serverTimeString = (String)data["servertimestring"];
        //            return info;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// Helper function to complete the login procedure and check the
        ///// credentials.
        ///// </summary>
        ///// <returns>Whether the login was successful or not.</returns>
        //private async Task<bool> LoginAsync()
        //{
        //    String response = await PerformSteamRequestAsync("ISteamWebUserPresenceOAuth/Logon/v0001",
        //        "?access_token=" + accessToken);

        //    if (response != null)
        //    {
        //        JObject data = JObject.Parse(response);

        //        if (data["umqid"] != null)
        //        {
        //            steamid = (String)data["steamid"];
        //            umqid = (String)data["umqid"];
        //            message = (int)data["message"];
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        #endregion

        #endregion

        #region Helper Methods

        ///// <summary>
        ///// Helper function to perform Steam API requests.
        ///// </summary>
        ///// <param name="getCommand">Path URI</param>
        ///// <param name="post">Post data</param>
        ///// <returns>Web response info</returns>
        //private async Task<String> PerformSteamRequestAsync(String getCommand, String post = null)
        //{
        //    HttpClient httpClient = new HttpClient();
        //    try
        //    {
        //        HttpResponseMessage responseMessage = new HttpResponseMessage();
        //        if (post != null)
        //        {
        //            responseMessage = await httpClient.PostAsync("http://api.steampowered.com/" + getCommand, new StringContent(post, Encoding.UTF8, "application/x-www-form-urlencoded"));
        //        }
        //        else
        //        {
        //            responseMessage = await httpClient.GetAsync("http://api.steampowered.com/" + getCommand);
        //        }

        //        if (!responseMessage.IsSuccessStatusCode)
        //            return null;

        //        string response = await responseMessage.Content.ReadAsStringAsync();
        //        return response;
        //    }
        //    catch
        //    {
        //        throw new Exception("An error occurred while handling the http request. Check the URL and http command.");
        //    }
        //}

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebAPI.Utility;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamUser : SteamWebRequest
    {
        public SteamUser(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<List<Friend>> GetFriendListAsync(long steamId, string relationship = "")
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            // create parameters for the request
            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            if (!String.IsNullOrEmpty(relationship))
            {
                WebRequestParameter relationshipParameter = new WebRequestParameter("relationship", relationship.ToString());
                requestParameters.Add(relationshipParameter);
            }

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamUser", "GetFriendList", 1, requestParameters);

            try
            {
                List<Friend> friends = new List<Friend>();

                foreach (JObject friendObject in data["friendslist"]["friends"])
                {
                    Friend friend = new Friend()
                    {
                        SteamID = TypeHelper.CreateLong(friendObject["steamid"]),
                        Relationship = friendObject["relationship"].ToString(),
                        FriendSinceDate = TypeHelper.CreateDateTime(friendObject["friend_since"])
                    };

                    friends.Add(friend);
                }

                return friends;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<List<UserBanStatus>> GetPlayerBansAsync(IList<long> steamIds)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            StringBuilder steamIdList = new StringBuilder();
            foreach (long steamId in steamIds)
            {
                if (steamIds.IndexOf(steamId) == 0)
                    steamIdList.Append(steamId);
                else
                    steamIdList.Append("," + steamId);
            }

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamids", steamIdList.ToString());
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamUser", "GetPlayerBans", 1, requestParameters);

            try
            {
                List<UserBanStatus> playerBans = new List<UserBanStatus>();

                foreach (JObject playerBanObject in data["players"])
                {
                    UserBanStatus playerBan = new UserBanStatus()
                    {
                        SteamID = Convert.ToInt64(playerBanObject["SteamId"].ToString()),
                        IsCommunityBanned = Convert.ToBoolean(playerBanObject["CommunityBanned"].ToString()),
                        IsVACBanned = Convert.ToBoolean(playerBanObject["VACBanned"].ToString()),
                        EconomyBanType = playerBanObject["EconomyBan"].ToString()
                    };

                    playerBans.Add(playerBan);
                }

                return playerBans;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<List<UserSummary>> GetPlayerSummariesAsync(List<long> steamIds)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            StringBuilder steamIdList = new StringBuilder();
            foreach (long steamId in steamIds)
            {
                if (steamIds.IndexOf(steamId) == 0)
                    steamIdList.Append(steamId);
                else
                    steamIdList.Append("," + steamId);
            }

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamids", steamIdList.ToString());
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamUser", "GetPlayerSummaries", 2, requestParameters);

            try
            {
                List<UserSummary> playerSummaries = new List<UserSummary>();

                foreach (JObject playerSummaryObject in data["response"]["players"])
                {
                    UserSummary playerSummary = new UserSummary();
                    playerSummary.SteamID = TypeHelper.CreateLong(playerSummaryObject["steamid"]);

                    string communityVisibilityState = TypeHelper.CreateString(playerSummaryObject["communityvisibilitystate"]);
                    if (communityVisibilityState == "1")
                        playerSummary.ProfileVisibility = ProfileVisibility.Private;
                    else if (communityVisibilityState == "3")
                        playerSummary.ProfileVisibility = ProfileVisibility.Public;
                    else if (communityVisibilityState == "8")
                        playerSummary.ProfileVisibility = ProfileVisibility.FriendsOnly;
                    else
                        playerSummary.ProfileVisibility = ProfileVisibility.Unknown;

                    playerSummary.ProfileState = TypeHelper.CreateInt(playerSummaryObject["profilestate"]);
                    playerSummary.Nickname = TypeHelper.CreateString(playerSummaryObject["personaname"]);
                    playerSummary.LastLoggedOffDate = TypeHelper.CreateDateTime(playerSummaryObject["lastlogoff"]);

                    // comment permission can be null if the user sets his permissions to "Friends Only" (I have no idea why)
                    if (playerSummaryObject["commentpermission"] != null)
                    {
                        string commentPermission = TypeHelper.CreateString(playerSummaryObject["commentpermission"]);
                        if (commentPermission == "2")
                            playerSummary.CommentPermission = CommentPermission.Private;
                        else if (commentPermission == "3")
                            playerSummary.CommentPermission = CommentPermission.Public;
                        else
                            playerSummary.CommentPermission = CommentPermission.Unknown;
                    }
                    else
                        playerSummary.CommentPermission = CommentPermission.FriendsOnly;

                    playerSummary.Profile = TypeHelper.CreateUri(playerSummaryObject["profileurl"]);
                    playerSummary.Avatar = TypeHelper.CreateUri(playerSummaryObject["avatar"]);
                    playerSummary.AvatarMedium = TypeHelper.CreateUri(playerSummaryObject["avatarmedium"]);
                    playerSummary.AvatarFull = TypeHelper.CreateUri(playerSummaryObject["avatarfull"]);

                    string personaState = TypeHelper.CreateString(playerSummaryObject["personastate"]);
                    if (personaState == "0")
                        playerSummary.UserStatus = UserStatus.Offline;
                    else if (personaState == "1")
                        playerSummary.UserStatus = UserStatus.Online;
                    else if (personaState == "2")
                        playerSummary.UserStatus = UserStatus.Busy;
                    else if (personaState == "3")
                        playerSummary.UserStatus = UserStatus.Away;
                    else if (personaState == "4")
                        playerSummary.UserStatus = UserStatus.Snooze;
                    else
                        playerSummary.UserStatus = UserStatus.Unknown;

                    playerSummary.RealName = TypeHelper.CreateString(playerSummaryObject["realname"]);

                    playerSummary.PrimaryGroupID = TypeHelper.CreateLong(playerSummaryObject["primaryclanid"]);
                    playerSummary.AccountCreatedDate = TypeHelper.CreateDateTime(playerSummaryObject["timecreated"]);
                    playerSummary.CountryCode = TypeHelper.CreateString(playerSummaryObject["loccountrycode"]);
                    playerSummary.StateCode = TypeHelper.CreateString(playerSummaryObject["locstatecode"]);
                    playerSummary.CityCode = TypeHelper.CreateInt(playerSummaryObject["loccityid"]);

                    playerSummaries.Add(playerSummary);
                }

                return playerSummaries;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<List<Group>> GetUserGroupListAsync(long steamId)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamUser", "GetUserGroupList", 1, requestParameters);

            bool success = TypeHelper.CreateBoolean(data["response"]["success"]);
            if (!success)
                throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);

            try
            {
                List<Group> groups = new List<Group>();

                foreach (JObject groupObject in data["response"]["groups"])
                {
                    Group group = new Group()
                    {
                        ID = TypeHelper.CreateInt(groupObject["gid"])
                    };

                    groups.Add(group);
                }

                return groups;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<long> ResolveVanityURLAsync(string vanityUrl)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("vanityurl", vanityUrl);
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamUser", "ResolveVanityURL", 1, requestParameters);

            int success = TypeHelper.CreateInt(data["response"]["success"]);
            if (success != 1)
                throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);

            try
            {
                return TypeHelper.CreateLong(data["response"]["steamid"]);
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

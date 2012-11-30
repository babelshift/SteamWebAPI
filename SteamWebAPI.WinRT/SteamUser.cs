using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebAPI.Utility;
using SteamWebModel;
using Windows.Data.Xml.Dom;

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
                        Relationship = TypeHelper.CreateString(friendObject["relationship"]),
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
                        SteamID = TypeHelper.CreateLong(playerBanObject["SteamId"]),
                        IsCommunityBanned = TypeHelper.CreateBoolean(playerBanObject["CommunityBanned"]),
                        IsVACBanned = TypeHelper.CreateBoolean(playerBanObject["VACBanned"]),
                        EconomyBanType = TypeHelper.CreateString(playerBanObject["EconomyBan"])
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

        public async Task<Profile> GetUserProfile(long steamId)
        {
            XmlDocument profileXml = await PerformXmlSteamRequestAsync("http://steamcommunity.com/profiles/" + steamId.ToString() + "?xml=1");
            
            Profile profile = new Profile();
            if (profileXml.HasChildNodes())
            {
                foreach (var rootChild in profileXml.ChildNodes)
                {
                    if (rootChild.NodeName == "profile")
                    {
                        foreach (var profileChild in rootChild.ChildNodes)
                        {
                            if (profileChild.NodeName == "steamID64")
                                profile.SteamID = TypeHelper.CreateLong(profileChild.InnerText);
                            else if (profileChild.NodeName == "steamID")
                                profile.Nickname = profileChild.InnerText;
                            else if (profileChild.NodeName == "onlineState")
                                profile.State = profileChild.InnerText;
                            else if (profileChild.NodeName == "stateMessage")
                                profile.StateMessage = profileChild.InnerText;
                            else if (profileChild.NodeName == "visibilityState")
                                profile.VisibilityState = TypeHelper.CreateInt(profileChild.InnerText);
                            else if (profileChild.NodeName == "avatarIcon")
                                profile.Avatar = new Uri(profileChild.InnerText);
                            else if (profileChild.NodeName == "avatarMedium")
                                profile.AvatarMedium = new Uri(profileChild.InnerText);
                            else if (profileChild.NodeName == "avatarFull")
                                profile.AvatarFull = new Uri(profileChild.InnerText);
                            else if (profileChild.NodeName == "vacBanned")
                            {
                                if (profileChild.InnerText == "0")
                                    profile.IsVacBanned = false;
                                else
                                    profile.IsVacBanned = true;
                            }
                            else if (profileChild.NodeName == "tradeBanState")
                                profile.TradeBanState = profileChild.InnerText;
                            else if (profileChild.NodeName == "isLimitedAccount")
                            {
                                if (profileChild.InnerText == "0")
                                    profile.IsLimitedAccount = false;
                                else
                                    profile.IsLimitedAccount = true;
                            }
                            else if (profileChild.NodeName == "customURL")
                                profile.CustomURL = profileChild.InnerText;
                            else if (profileChild.NodeName == "memberSince")
                                profile.MemberSince = profileChild.InnerText;
                            else if (profileChild.NodeName == "steamRating")
                                profile.SteamRating = TypeHelper.CreateDouble(profileChild.InnerText);
                            else if (profileChild.NodeName == "hoursPlayed2Wk")
                                profile.HoursPlayedLastTwoWeeks = TypeHelper.CreateDouble(profileChild.InnerText);
                            else if (profileChild.NodeName == "headline")
                                profile.Headline = profileChild.InnerText;
                            else if (profileChild.NodeName == "location")
                                profile.Location = profileChild.InnerText;
                            else if (profileChild.NodeName == "realname")
                                profile.RealName = profileChild.InnerText;
                            else if (profileChild.NodeName == "summary")
                                profile.Summary = profileChild.InnerText;
                            else if (profileChild.NodeName == "mostPlayedGames")
                            {
                                if (profileChild.HasChildNodes())
                                {
                                    var mostPlayedGames = new List<Profile.MostPlayedGame>();
                                    // <mostPlayedGames> children
                                    foreach (var mostPlayedGameNode in profileChild.ChildNodes)
                                    {
                                        if (mostPlayedGameNode.NodeName == "mostPlayedGame")
                                        {
                                            if (mostPlayedGameNode.HasChildNodes())
                                            {
                                                var mostPlayedGame = new SteamWebModel.Profile.MostPlayedGame();
                                                // <mostPlayedGame> children
                                                foreach (var mostPlayedGameChild in mostPlayedGameNode.ChildNodes)
                                                {
                                                    if (mostPlayedGameChild.NodeName == "gameName")
                                                        mostPlayedGame.Name = mostPlayedGameChild.InnerText;
                                                    if (mostPlayedGameChild.NodeName == "gameLink")
                                                        mostPlayedGame.Link = new Uri(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "gameIcon")
                                                        mostPlayedGame.Icon = new Uri(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "gameLogo")
                                                        mostPlayedGame.Logo = new Uri(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "gameLogoSmall")
                                                        mostPlayedGame.LogoSmall = new Uri(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "hoursPlayed")
                                                        mostPlayedGame.HoursPlayed = TypeHelper.CreateDouble(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "hoursOnRecord")
                                                        mostPlayedGame.HoursOnRecord = TypeHelper.CreateDouble(mostPlayedGameChild.InnerText);
                                                    if (mostPlayedGameChild.NodeName == "statsName")
                                                        mostPlayedGame.StatsName = mostPlayedGameChild.InnerText;
                                                }

                                                mostPlayedGames.Add(mostPlayedGame);
                                            }
                                        }
                                    }

                                    profile.MostPlayedGames = mostPlayedGames;
                                }
                            }
                            else if (profileChild.NodeName == "weblinks")
                            {
                                if (profileChild.HasChildNodes())
                                {
                                    var webLinks = new List<Profile.WebLink>();
                                    // <mostPlayedGames> children
                                    foreach (var webLinkNode in profileChild.ChildNodes)
                                    {
                                        if (webLinkNode.NodeName == "weblink")
                                        {
                                            if (webLinkNode.HasChildNodes())
                                            {
                                                var webLink = new SteamWebModel.Profile.WebLink();
                                                // <mostPlayedGame> children
                                                foreach (var webLinkChild in webLinkNode.ChildNodes)
                                                {
                                                    if (webLinkChild.NodeName == "link")
                                                        webLink.Link = new Uri(webLinkChild.InnerText);
                                                    else if (webLinkChild.NodeName == "title")
                                                        webLink.Title = webLinkChild.InnerText;
                                                }

                                                webLinks.Add(webLink);
                                            }
                                        }
                                    }

                                    profile.WebLinks = webLinks;
                                }
                            }
                        }
                    }
                }
            }

            return profile;
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
                    playerSummary.PlayingGameID = TypeHelper.CreateLong(playerSummaryObject["gameid"]);
                    playerSummary.PlayingGameName = TypeHelper.CreateString(playerSummaryObject["gameextrainfo"]);

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
                    {
                        if (!String.IsNullOrEmpty(playerSummary.PlayingGameName))
                            playerSummary.UserStatus = UserStatus.InGame;
                        else
                            playerSummary.UserStatus = UserStatus.Online;
                    }
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

        public async Task<List<Group>> GetUserGroupsAsync(long steamId)
        {
            XmlDocument profileXml = await PerformXmlSteamRequestAsync("http://steamcommunity.com/profiles/" + steamId.ToString() + "?xml=1");
            XmlNodeList groupElements = profileXml.GetElementsByTagName("groups");
            IXmlNode groupNodes = groupElements.Item(0);

            List<Group> steamGroups = new List<Group>();
            if (groupNodes != null)
            {
                if (groupNodes.HasChildNodes())
                {
                    foreach (var group in groupNodes.ChildNodes)
                    {
                        // if this is a "group" node, there will be children, so skip anything without children
                        if (group.HasChildNodes())
                        {
                            Group steamGroup = new Group();
                            foreach (var groupChild in group.ChildNodes)
                            {
                                if (groupChild.NodeName == "groupID64")
                                    steamGroup.ID = TypeHelper.CreateLong(groupChild.InnerText);
                                else if (groupChild.NodeName == "groupName")
                                    steamGroup.Name = groupChild.InnerText;
                                else if (groupChild.NodeName == "groupURL")
                                    steamGroup.URL = groupChild.InnerText;
                                else if (groupChild.NodeName == "headline")
                                    steamGroup.Headline = groupChild.InnerText;
                                else if (groupChild.NodeName == "summary")
                                    steamGroup.Summary = groupChild.InnerText;
                                else if (groupChild.NodeName == "avatarIcon")
                                    steamGroup.Avatar = new Uri(groupChild.InnerText);
                                else if (groupChild.NodeName == "avatarMedium")
                                    steamGroup.AvatarMedium = new Uri(groupChild.InnerText);
                                else if (groupChild.NodeName == "avatarFull")
                                    steamGroup.AvatarFull = new Uri(groupChild.InnerText);
                                else if (groupChild.NodeName == "memberCount")
                                    steamGroup.MemberCount = TypeHelper.CreateInt(groupChild.InnerText);
                                else if (groupChild.NodeName == "membersInChat")
                                    steamGroup.MemberChatCount = TypeHelper.CreateInt(groupChild.InnerText);
                                else if (groupChild.NodeName == "membersInGame")
                                    steamGroup.MemberGameCount = TypeHelper.CreateInt(groupChild.InnerText);
                                else if (groupChild.NodeName == "membersOnline")
                                    steamGroup.MemberOnlineCount = TypeHelper.CreateInt(groupChild.InnerText);
                            }

                            steamGroups.Add(steamGroup);
                        }
                    }
                }
            }

            return steamGroups;
        }

        //public async Task<List<Group>> GetUserGroupListAsync(long steamId)
        //{
        //    List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

        //    WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
        //    requestParameters.Add(steamIdParameter);

        //    // send the request and wait for the response
        //    JObject data = await PerformSteamRequestAsync("ISteamUser", "GetUserGroupList", 1, requestParameters);

        //    bool success = TypeHelper.CreateBoolean(data["response"]["success"]);
        //    if (!success)
        //        throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);

        //    try
        //    {
        //        List<Group> groups = new List<Group>();

        //        foreach (JObject groupObject in data["response"]["groups"])
        //        {
        //            Group group = new Group()
        //            {
        //                ID = TypeHelper.CreateInt(groupObject["gid"])
        //            };

        //            groups.Add(group);
        //        }

        //        return groups;
        //    }
        //    catch
        //    {
        //        throw new Exception(E_JSON_DESERIALIZATION_FAILED);
        //    }
        //}

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

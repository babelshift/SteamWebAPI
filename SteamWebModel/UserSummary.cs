using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public enum UserStatus
    {
        Offline = 0,
        Online = 1,
        Busy = 2,
        Away = 3,
        Snooze = 4,
        Unknown = 5
    }

    public enum ProfileVisibility
    {
        Unknown = 0,
        Private = 1,
        Public = 3,
        FriendsOnly = 8,
    }

    public enum CommentPermission
    {
        Unknown = 0,
        FriendsOnly = 1,
        Private = 2,
        Public = 3
    }

    public class UserSummary
    {
        public long SteamID { get; set; }
        public ProfileVisibility ProfileVisibility { get; set; }
        public int ProfileState { get; set; }
        public string Nickname { get; set;}
        public DateTime LastLoggedOffDate { get; set; }
        public CommentPermission CommentPermission { get; set; }
        public Uri Profile { get; set; }
        public Uri Avatar { get; set; }
        public Uri AvatarMedium { get; set; }
        public Uri AvatarFull { get; set; }
        public UserStatus UserStatus { get; set; }
        public string RealName { get; set; }
        public long PrimaryGroupID { get; set; }
        public DateTime AccountCreatedDate { get; set; }
        public string CountryCode { get; set; }
        public string StateCode { get; set; }
        public int CityCode { get; set; }
        public string PlayingGameName { get; set; }
        public long PlayingGameID { get; set; }
    }
}

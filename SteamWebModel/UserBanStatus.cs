﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class UserBanStatus
    {
        public long SteamID { get; set; }
        public bool IsCommunityBanned { get; set; }
        public bool IsVACBanned { get; set; }
        public string EconomyBanType { get; set; }
    }
}

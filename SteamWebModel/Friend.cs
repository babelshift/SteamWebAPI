﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class Friend
    {
        public long SteamID { get; set; }
        public string Relationship { get; set; }
        public DateTime FriendSinceDate { get; set; }
    }
}

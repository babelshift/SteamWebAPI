﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class UserStats
    {
        public long SteamID { get; set; }
        public string GameName { get; set; }
        public IList<Achievement> Achievements { get; set; }
        public IList<PlayerStat> Stats { get; set; }

        public UserStats()
        {
            Achievements = new List<Achievement>();
            Stats = new List<PlayerStat>();
        }
    }

    public class PlayerStat
    {
        public string Name { get; set; }
        public long Value { get; set; }
    }
}

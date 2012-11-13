﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebModel
{
    public class UserAchievements
    {
        public long SteamID { get; set; }
        public string GameName { get; set; }
        public IList<Achievement> Achievements { get; set; }

        public UserAchievements()
        {
            Achievements = new List<Achievement>();
        }
    }
}

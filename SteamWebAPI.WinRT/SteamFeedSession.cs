using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamWebModel;
using Windows.Web.Syndication;

namespace SteamWebAPI
{
    public class SteamFeedSession
    {
        private SteamFeedRequest steamFeedRequest;

        public enum FeedType
        {
            Dota2,
            SteamWorkshop,
            Left4Dead,
            TeamFortress2,
            Portal2,
            CounterStrikeGo,
            SourceFilmmaker,
            RamblingsInValveTime,
            ValveEconomics,
            ValveLinuxTeamBlog,
            TeachWithPortals,
            SteamStore,
        }

        public SteamFeedSession()
        {
            steamFeedRequest = new SteamFeedRequest();
        }

        public async Task<FeedData> GetFeedAsync(FeedType feed)
        {
            string feedUri = String.Empty;

            if (feed == FeedType.Dota2)
                feedUri = "http://blog.dota2.com/feed/";
            else if (feed == FeedType.SteamWorkshop)
                feedUri = "http://steamcommunity.com/games/SteamWorkshop/rss";
            else if (feed == FeedType.Left4Dead)
                feedUri = "http://www.l4d.com/blog/rss.xml";
            else if (feed == FeedType.TeamFortress2)
                feedUri = "http://www.teamfortress.com/rss.xml";
            else if (feed == FeedType.Portal2)
                feedUri = "http://www.thinkwithportals.com/rss.xml";
            else if (feed == FeedType.SourceFilmmaker)
                feedUri = "http://www.sourcefilmmaker.com/rss.xml";
            else if (feed == FeedType.RamblingsInValveTime)
                feedUri = "http://blogs.valvesoftware.com/feed/?cat=3";
            else if (feed == FeedType.ValveEconomics)
                feedUri = "http://blogs.valvesoftware.com/feed/?cat=6";
            else if (feed == FeedType.ValveLinuxTeamBlog)
                feedUri = "http://blogs.valvesoftware.com/feed/?cat=7";
            else if (feed == FeedType.TeachWithPortals)
                feedUri = "http://www.teachwithportals.com/index.php/feed/";
            else if (feed == FeedType.CounterStrikeGo)
                feedUri = "http://blog.counter-strike.net/index.php/feed/";
            else if (feed == FeedType.SteamStore)
                feedUri = "http://store.steampowered.com/feeds/news.xml";

            return await steamFeedRequest.GetFeedAsync(feedUri);
        }
    }
}

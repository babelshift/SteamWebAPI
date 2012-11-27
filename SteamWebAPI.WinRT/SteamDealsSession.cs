using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamWebModel;
using Windows.Web.Syndication;

namespace SteamWebAPI
{
    public class SteamDealsSession
    {
        private SteamFeedRequest steamFeedRequest;

        public enum FeedType
        {
            //SteamDailyDeals,
            SteamGameSalesCom
        }

        public SteamDealsSession()
        {
            steamFeedRequest = new SteamFeedRequest();
        }

        public async Task<FeedData> GetFeedAsync(FeedType feed)
        {
            string feedUri = String.Empty;

            if (feed == FeedType.SteamGameSalesCom)
                feedUri = "http://www.steamgamesales.com/rss/?region=us&stores=steam/";
            //else if (feed == FeedType.SteamDailyDeals)
            //    feedUri = "http://store.steampowered.com/feeds/daily_deals.xml";

            return await steamFeedRequest.GetFeedAsync(feedUri);
        }
    }
}

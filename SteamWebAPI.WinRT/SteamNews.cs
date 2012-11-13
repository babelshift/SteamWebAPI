using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebAPI.Utility;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamNews : SteamWebRequest
    {
        public SteamNews(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<AppNews> GetNewsForAppAsync(int appId)
        {
            return await GetNewsForAppAsync(appId, 0, null, 20);
        }

        public async Task<AppNews> GetNewsForAppAsync(int appId, int maxContentLength, DateTime? endDate, int postCountToReturn)
        {
            // create parameters for the request
            WebRequestParameter appIdParameter = new WebRequestParameter("appid", appId.ToString());
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();
            requestParameters.Add(appIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamNews", "GetNewsForApp", 2, requestParameters);

            try
            {
                AppNews appNews = new AppNews();

                foreach (JProperty appNewsProperty in data["appnews"])
                {
                    if (appNewsProperty.Name == "appid")
                        appNews.AppID = int.Parse(appNewsProperty.Value.ToString());
                    else if (appNewsProperty.Name == "newsitems")
                    {
                        foreach (JObject newsItemObject in data["appnews"]["newsitems"])
                        {
                            NewsItem newsItem = new NewsItem();
                            newsItem.GID = newsItemObject["gid"].ToString();
                            newsItem.Title = newsItemObject["title"].ToString();
                            newsItem.Url = newsItemObject["url"].ToString();
                            newsItem.IsExternalUrl = Convert.ToBoolean(newsItemObject["is_external_url"].ToString());
                            newsItem.Author = newsItemObject["author"].ToString();
                            newsItem.Contents = newsItemObject["contents"].ToString();
                            newsItem.FeedLabel = newsItemObject["feedlabel"].ToString();
                            newsItem.Date = TypeHelper.CreateDateTime(newsItemObject["date"]);
                            newsItem.FeedName = newsItemObject["feedname"].ToString();

                            appNews.NewsItems.Add(newsItem);
                        }
                    }
                }

                return appNews;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

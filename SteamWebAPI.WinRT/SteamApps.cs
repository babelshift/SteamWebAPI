using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamApps : SteamWebRequest
    {
        public SteamApps(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<List<App>> GetAppListAsync()
        {
            JObject data = await PerformSteamRequestAsync("ISteamApps", "GetAppList", 2);

            try
            {
                List<App> apps = new List<App>();

                foreach (JObject responseObject in data["applist"]["apps"])
                {
                    App app = new App();
                    app.AppID = Convert.ToInt32(responseObject["appid"].ToString());
                    app.Name = responseObject["name"].ToString();

                    apps.Add(app);
                }

                return apps;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

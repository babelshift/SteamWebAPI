using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebAPI.Utility;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamWebAPIUtil : SteamWebRequest
    {
        public SteamWebAPIUtil(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<List<SteamInterface>> GetSupportedAPIListAsync()
        {
            JObject data = await PerformSteamRequestAsync("ISteamWebAPIUtil", "GetSupportedAPIList", 1);

            try
            {
                List<SteamInterface> interfaces = new List<SteamInterface>();

                foreach (JObject interfaceObject in data["apilist"]["interfaces"])
                {
                    SteamInterface steamInterface = new SteamInterface();

                    steamInterface.Name = interfaceObject["name"].ToString();

                    foreach (JObject method in interfaceObject["methods"])
                    {
                        SteamMethod steamMethod = new SteamMethod();
                        steamMethod.Name = method["name"].ToString();
                        steamMethod.Version = Convert.ToInt32(method["version"].ToString());
                        steamMethod.HttpMethod = method["httpmethod"].ToString();

                        foreach (JObject parameter in method["parameters"])
                        {
                            SteamParameter steamParameter = new SteamParameter();
                            steamParameter.Name = parameter["name"].ToString();
                            steamParameter.Type = parameter["type"].ToString();
                            steamParameter.IsOptional = Convert.ToBoolean(parameter["optional"].ToString());
                            steamParameter.Description = parameter["description"].ToString();

                            steamMethod.Parameters.Add(steamParameter);
                        }

                        steamInterface.Methods.Add(steamMethod);
                    }

                    interfaces.Add(steamInterface);
                }

                return interfaces;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<SteamServerInfo> GetServerInfo()
        {
            JObject data = await PerformSteamRequestAsync("ISteamWebAPIUtil", "GetServerInfo", 1);

            try
            {
                SteamServerInfo steamServerInfo = new SteamServerInfo()
                {
                    ServerTime = TypeHelper.CreateDateTime(data["servertime"]),
                    ServerTimeString = TypeHelper.CreateString(data["servertimestring"])
                };

                return steamServerInfo;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

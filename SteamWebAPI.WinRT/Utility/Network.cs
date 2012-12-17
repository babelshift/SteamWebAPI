using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebAPI.Utility
{
    public static class Network
    {
        /// <summary>
        /// Based on the active network adapters, determines if any exist and if they provide for Internet access
        /// </summary>
        /// <returns></returns>
        public static bool IsInternetAvailable()
        {
            var connection = Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile();
            if (connection != null)
                if (connection.GetNetworkConnectivityLevel() != Windows.Networking.Connectivity.NetworkConnectivityLevel.LocalAccess
                    && connection.GetNetworkConnectivityLevel() != Windows.Networking.Connectivity.NetworkConnectivityLevel.None)
                    return true;
                else
                    return false;
            else
                return false;
        }
    }
}

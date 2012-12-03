using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamWebAPI.Utility
{
    internal static class Network
    {
        internal static bool IsInternetAvailable()
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

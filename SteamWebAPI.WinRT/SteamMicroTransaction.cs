using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebAPI.Utility;
using SteamWebModel;

namespace SteamWebAPI
{
    internal class SteamMicroTransaction : SteamWebRequest
    {
        public SteamMicroTransaction(WebRequestParameter developerKey)
            : base(developerKey)
        {
        }

        public async Task<Cart> CreateCart(long steamId)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamMicroTxn", "CreateCart", 1, requestParameters);

            bool success = TypeHelper.CreateBoolean(data["result"]["success"]);
            if (!success)
                throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);

            try
            {
                Cart cart = new Cart()
                {
                    ID = TypeHelper.CreateLong(data["result"]["cartgid"]),
                    IsValid = true,
                    OwnerSteamID = steamId
                };

                return cart;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        /// <summary>
        /// This should return a list of "LineItem" objects for every item in the cart, but I don't yet know how to add items
        /// to a cart after creating it, so this method currently will never return success.
        /// </summary>
        /// <param name="steamId"></param>
        /// <returns></returns>
        public async Task<List<LineItem>> GetCartContents(long steamId)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamMicroTxn", "GetCartContents", 1, requestParameters);

            bool success = TypeHelper.CreateBoolean(data["result"]["success"]);
            if (!success)
                throw new Exception(E_HTTP_RESPONSE_RESULT_FAILED);

            try
            {
                List<LineItem> lineItems = new List<LineItem>();

                foreach (JObject groupObject in data["result"]["lineitems"])
                {
                    LineItem lineItem = new LineItem();
                    lineItems.Add(lineItem);
                }

                return lineItems;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }

        public async Task<Cart> IsValidCart(long steamId, long cartId)
        {
            List<WebRequestParameter> requestParameters = new List<WebRequestParameter>();

            WebRequestParameter steamIdParameter = new WebRequestParameter("steamid", steamId.ToString());
            requestParameters.Add(steamIdParameter);

            WebRequestParameter cartIdParameter = new WebRequestParameter("gid", cartId.ToString());
            requestParameters.Add(cartIdParameter);

            // send the request and wait for the response
            JObject data = await PerformSteamRequestAsync("ISteamMicroTxn", "IsValidCart", 1, requestParameters);

            try
            {
                Cart cart = new Cart();

                bool isFound = TypeHelper.CreateBoolean(data["result"]["found"]);
                
                cart.ID = cartId;
                cart.IsValid = isFound;

                if (isFound)
                    cart.OwnerSteamID = TypeHelper.CreateLong(data["result"]["steamidowner"]);

                return cart;
            }
            catch
            {
                throw new Exception(E_JSON_DESERIALIZATION_FAILED);
            }
        }
    }
}

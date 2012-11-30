using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SteamWebModel;
using Windows.Data.Xml.Dom;

namespace SteamWebAPI
{
    internal abstract class SteamWebRequest
    {
        protected const string E_HTTP_REQUEST_FAILED = "An error occurred while handling the request. Check the command for syntax errors.";
        protected const string E_HTTP_RESPONSE_EMPTY = "The request was successful but the response was empty.";
        protected const string E_HTTP_RESPONSE_PARSE_FAILED = "The request was successful but parsing the response failed.";
        protected const string E_HTTP_RESPONSE_RESULT_FAILED = "The request was successful but the response result indicated an error.";
        protected const string E_JSON_DESERIALIZATION_FAILED = "There was an error while deserializing the JSON data.";
        protected const string E_HTTP_XML_REQUEST_TIMED_OUT = "The XML request failed after 3 tries.";
        private const string STEAM_WEB_API_BASE_URL = "https://api.steampowered.com:443";
        private const int TIMES_TO_TRY_XML_REQUEST = 3;

        private WebRequestParameter developerKey;

        public SteamWebRequest(WebRequestParameter developerKey)
        {
            this.developerKey = developerKey;
        }

        protected async Task<XmlDocument> PerformXmlSteamRequestAsync(string url)
        {
            HttpClient httpClient = new HttpClient();
            XmlDocument responseXml = new XmlDocument();

            // try to get the xml response 3 times before giving up
            for (int triesRemaining = TIMES_TO_TRY_XML_REQUEST; triesRemaining > 0; triesRemaining--)
            {
                try
                {
                    responseXml = await XmlDocument.LoadFromUriAsync(new Uri(url));
                }
                catch(Exception e)
                {
                    // only throw the exception if the file was not found (which means the server is return an http error
                    // if it's a file not found exception, continue and try the loop again
                    if (!(e is FileNotFoundException))
                        throw new Exception(E_HTTP_REQUEST_FAILED);
                    else
                        continue;
                }

                // the request was successful but the response is empty
                if (responseXml == null && triesRemaining > 0)
                    throw new Exception(E_HTTP_RESPONSE_EMPTY);
                // there are no more request attempts left and the response is still empty, there were no successful requests
                else if (responseXml == null && triesRemaining == 0)
                    throw new Exception(E_HTTP_XML_REQUEST_TIMED_OUT);

                // got here without exceptions, the request was successful, break the loop
                break;
            }

            return responseXml;
        }

        protected async Task<JObject> PerformSteamRequestAsync(string interfaceName, string methodName, int methodVersion)
        {
            return await PerformSteamRequestAsync(interfaceName, methodName, methodVersion, null);
        }

        protected async Task<JObject> PerformSteamRequestAsync(string interfaceName, string methodName, int methodVersion, List<WebRequestParameter> parameters)
        {
            if (parameters == null)
                parameters = new List<WebRequestParameter>();

            // make developer API key the first parameter (not required, but just to standardize the requests)
            parameters.Insert(0, this.developerKey);

            HttpClient httpClient = new HttpClient();
            string response = String.Empty;

            try
            {
                string requestCommand = BuildRequestCommand(interfaceName, methodName, methodVersion, parameters);

                // http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh781240.aspx
                // above link claims GetStringAsync() is equivalent to calling GetAsync(), checking the HttpResponseMessage for success, and then reading
                // the Content property of the HttpResponseMessage as a string asynchronously
                response = await httpClient.GetStringAsync(requestCommand);
            }
            catch
            {
                throw new Exception(E_HTTP_REQUEST_FAILED);
            }

            if (String.IsNullOrEmpty(response))
                throw new Exception(E_HTTP_RESPONSE_EMPTY);

            try
            {
                return JObject.Parse(response);
            }
            catch
            {
                throw new Exception(E_HTTP_RESPONSE_PARSE_FAILED);
            }
        }

        //protected async Task<T> PerformSteamRequestAsync<T>(string interfaceName, string methodName, int methodVersion, List<WebRequestParameter> parameters)
        //{
        //    if (parameters == null)
        //        parameters = new List<WebRequestParameter>();

        //    // make developer API key the first parameter (not required, but just to standardize the requests)
        //    parameters.Insert(0, this.developerKey);

        //    HttpClient httpClient = new HttpClient();
        //    string response = String.Empty;
        //    T result;

        //    try
        //    {
        //        string requestCommand = BuildRequestCommand(interfaceName, methodName, methodVersion, parameters);

        //        // http://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh781240.aspx
        //        // above link claims GetStringAsync() is equivalent to calling GetAsync(), checking the HttpResponseMessage for success, and then reading
        //        // the Content property of the HttpResponseMessage as a string asynchronously
        //        response = await httpClient.GetStringAsync(requestCommand);
        //        MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(response));
        //        DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
        //        result = (T)jsonSerializer.ReadObject(ms);
        //    }
        //    catch
        //    {
        //        throw new Exception(E_HTTP_REQUEST_FAILED);
        //    }

        //    if (String.IsNullOrEmpty(response))
        //        throw new Exception(E_HTTP_RESPONSE_EMPTY);

        //    try
        //    {
        //        return result;
        //    }
        //    catch
        //    {
        //        throw new Exception(E_HTTP_RESPONSE_PARSE_FAILED);
        //    }
        //}

        /// <summary>
        /// Takes values and returns a command string that can be sent to the Steam Web API remote address.
        /// Example of a built command: https://api.steampowered.com:443/ISteamWebAPIUtil/GetSupportedAPIList/v0001/?key=8A05823474AB641D684EBD95AB5F2E47
        /// </summary>
        /// <param name="interfaceName">Example: ISteamWebAPIUtil</param>
        /// <param name="methodName">Example: GetSupportedAPIList</param>
        /// <param name="methodVersion">Example: 1</param>
        /// <param name="parameters">Example: { key: 8A05823474AB641D684EBD95AB5F2E47 } </param>
        /// <returns></returns>
        private string BuildRequestCommand(string interfaceName, string methodName, int methodVersion, List<WebRequestParameter> parameters)
        {
            StringBuilder command = new StringBuilder();

            command.Append(STEAM_WEB_API_BASE_URL);
            command.Append("/");
            command.Append(interfaceName);
            command.Append("/");
            command.Append(methodName);
            command.Append("/v");
            command.Append(methodVersion.ToString("0000"));
            command.Append("/");

            foreach (var parameter in parameters)
            {
                string delimiter = "&";
                if (parameters.IndexOf(parameter) == 0)
                    delimiter = "?";

                command.Append(delimiter);
                command.Append(parameter.Name);
                command.Append("=");
                command.Append(parameter.Value);
            }

            return command.ToString();
        }
    }
}

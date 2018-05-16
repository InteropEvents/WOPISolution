using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using WOPIHost.Models;
using System.Configuration;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;

namespace WOPIHost.Utils
{
    public class DiscoveryUtil
    {
        /// <summary>
        /// Get WOPI actions
        /// </summary>
        /// <returns>List of actions</returns>
        public static List<WopiAction> GetDiscoveryInfo()
        {
            List<WopiAction> actions = new List<WopiAction>();

            MemoryCache memoryCache = MemoryCache.Default;
            if (memoryCache.Contains("DiscoveryData"))
            {
                actions = (List<WopiAction>)memoryCache["DiscoveryData"];
            }
            else
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(ConfigurationManager.AppSettings["WopiDiscovery"]);
                WebResponse response = request.GetResponse();
                string xmlString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Parse the xml string into Xml
                var discoXml = XDocument.Parse(xmlString);

                // Convert the discovery xml into list of WopiApp
                var xapps = discoXml.Descendants("app");
                foreach (var xapp in xapps)
                {
                    // Parse the actions for the app
                    var xactions = xapp.Descendants("action");
                    foreach (var xaction in xactions)
                    {
                        actions.Add(new WopiAction()
                        {
                            app = xapp.Attribute("name").Value,
                            favIconUrl = xapp.Attribute("favIconUrl").Value,
                            checkLicense = Convert.ToBoolean(xapp.Attribute("checkLicense").Value),
                            name = xaction.Attribute("name").Value,
                            ext = (xaction.Attribute("ext") != null) ? xaction.Attribute("ext").Value : String.Empty,
                            progid = (xaction.Attribute("progid") != null) ? xaction.Attribute("progid").Value : String.Empty,
                            isDefault = (xaction.Attribute("default") != null) ? true : false,
                            urlsrc = xaction.Attribute("urlsrc").Value,
                            requires = (xaction.Attribute("requires") != null) ? xaction.Attribute("requires").Value : String.Empty
                        });
                    }

                    // Cache the discovey data for an hour
                    memoryCache.Add("DiscoveryData", actions, DateTimeOffset.Now.AddHours(1));
                }
            }


            return actions;
        }


        /// <summary>
        /// Get action URL of a file
        /// </summary>
        /// <param name="action">The action</param>
        /// <param name="fileName">File name</param>
        /// <returns>The URL</returns>
        public static string GetActionUrl(WopiAction action, string fileName)
        {
            string urlSrc = action.urlsrc;

            string access_token = AccessTokenUtil.WriteToken(AccessTokenUtil.GenerateToken(Environment.UserName, fileName.ToLower()));

            urlSrc = string.Format("{0}WOPISrc={1}",
                urlSrc.Substring(0, urlSrc.IndexOf('<')),
                HttpUtility.UrlEncode(string.Format("http://{0}/wopi/files/{1}", ConfigurationManager.AppSettings["WOPIServerName"], HttpUtility.UrlEncode(fileName))));

            urlSrc = urlSrc.ToLower().Replace(ConfigurationManager.AppSettings["OfficeServerName"].ToLower().Trim(), ConfigurationManager.AppSettings["OfficeServerIP"]);

            urlSrc = string.Format("{0}&access_token={1}", urlSrc, HttpUtility.UrlEncode(access_token));
            return urlSrc;
        }
    }

    /// <summary>
    /// Contains all valid URL placeholders for different WOPI actions
    /// </summary>
    public class WopiUrlPlaceholders
    {
        public static List<string> Placeholders = new List<string>() { BUSINESS_USER,
            DC_LLCC, DISABLE_ASYNC, DISABLE_CHAT, DISABLE_BROADCAST,
            EMBDDED, FULLSCREEN, PERFSTATS, RECORDING, THEME_ID, UI_LLCC, VALIDATOR_TEST_CATEGORY
        };
        public const string BUSINESS_USER = "<IsLicensedUser=BUSINESS_USER&>";
        public const string DC_LLCC = "<rs=DC_LLCC&>";
        public const string DISABLE_ASYNC = "<na=DISABLE_ASYNC&>";
        public const string DISABLE_CHAT = "<dchat=DISABLE_CHAT&>";
        public const string DISABLE_BROADCAST = "<vp=DISABLE_BROADCAST&>";
        public const string EMBDDED = "<e=EMBEDDED&>";
        public const string FULLSCREEN = "<fs=FULLSCREEN&>";
        public const string PERFSTATS = "<showpagestats=PERFSTATS&>";
        public const string RECORDING = "<rec=RECORDING&>";
        public const string THEME_ID = "<thm=THEME_ID&>";
        public const string UI_LLCC = "<ui=UI_LLCC&>";
        public const string VALIDATOR_TEST_CATEGORY = "<testcategory=VALIDATOR_TEST_CATEGORY>";

        /// <summary>
        /// Sets a specific WOPI URL placeholder with the correct value
        /// Most of these are hard-coded in this WOPI implementation
        /// </summary>
        public static string GetPlaceholderValue(string placeholder)
        {
            var ph = placeholder.Substring(1, placeholder.IndexOf("="));
            string result = "";
            switch (placeholder)
            {
                case BUSINESS_USER:
                    result = ph + "1";
                    break;
                case DC_LLCC:
                case UI_LLCC:
                    result = ph + "1033";
                    break;
                case DISABLE_ASYNC:
                case DISABLE_BROADCAST:
                case EMBDDED:
                case FULLSCREEN:
                case RECORDING:
                case THEME_ID:
                    // These are all broadcast related actions
                    result = ph + "true";
                    break;
                case DISABLE_CHAT:
                    result = ph + "false";
                    break;
                case PERFSTATS:
                    result = ""; // No documentation
                    break;
                case VALIDATOR_TEST_CATEGORY:
                    result = ph + "OfficeOnline"; //This value can be set to All, OfficeOnline or OfficeNativeClient to activate tests specific to Office Online and Office for iOS. If omitted, the default value is All.
                    break;
                default:
                    result = "";
                    break;

            }

            return result;
        }
    }
}
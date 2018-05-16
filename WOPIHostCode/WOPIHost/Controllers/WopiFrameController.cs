using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WOPIHost.Models;
using WOPIHost.Utils;

namespace WOPIHost.Controllers
{
    public class WopiFrameController : Controller
    {
        /// <summary>
        /// GET: WopiFrame
        /// </summary>
        /// <param name="id">File name</param>
        /// <returns>A ViewResult object</returns>
        public ActionResult Index(string id)
        {
            string sourceDoc = id.ToLower();
            string uid = ConfigurationManager.AppSettings["BrowserUserName"].ToLower();

            string access_token = AccessTokenUtil.WriteToken(AccessTokenUtil.GenerateToken(uid, sourceDoc));

            List<WopiAction> actions = DiscoveryUtil.GetDiscoveryInfo();
            string extention = sourceDoc.Split('.')[sourceDoc.Split('.').Length - 1];
            WopiAction action = actions.FirstOrDefault(i => i.ext == extention && i.name == "view");
            string urlSrc = action.urlsrc;

            urlSrc = string.Format("{0}WOPISrc={1}",
                urlSrc.Substring(0, urlSrc.IndexOf('<')),
                HttpUtility.UrlEncode(string.Format("http://{0}/wopi/files/{1}",
                ConfigurationManager.AppSettings["WOPIServerName"],
                HttpUtility.UrlEncode(sourceDoc))));

            urlSrc = urlSrc.ToLower().Replace(ConfigurationManager.AppSettings["OfficeServerName"].ToLower().Trim(), ConfigurationManager.AppSettings["OfficeServerIP"]);

            urlSrc = string.Format("{0}&access_token={1}", urlSrc, HttpUtility.UrlEncode(access_token));

            ViewData["URL"] = urlSrc;

            return View();
        }
    }
}
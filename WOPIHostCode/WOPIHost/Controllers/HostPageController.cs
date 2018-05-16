using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WOPIHost.Models;

namespace WOPIHost.Controllers
{
    public class HostPageController : Controller
    {
        //
        // GET: /HostPage/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get the file names with link on it
        /// </summary>
        /// <returns>A JsonResult object</returns>
        public ActionResult GetFileList()
        {
            List<FileLink> files = GetFiles();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Json(serializer.Serialize(files), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get the file names with link on it
        /// </summary>
        /// <returns>The file names with link</returns>
        public List<FileLink> GetFiles()
        {
            List<FileLink> files = new List<FileLink>();

            IFileStorage storage = new FTPFileStorage();
            List<string> fileNames = storage.GetFileNames();

            foreach (string fileName in fileNames)
            {
                FileLink fileLink = new FileLink();
                fileLink.Name = fileName;
                fileLink.Url = string.Format("http://{0}/wopiframe/Index/{1}",
                    ConfigurationManager.AppSettings["WOPIServerName"],
                    fileName);
                
                files.Add(fileLink);
            }

            return files;
        }
    }
}
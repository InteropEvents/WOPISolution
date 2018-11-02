using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WOPIHost.Models;
using WOPIHost.Utils;

namespace WOPIHost.Controllers
{
    public class WopiValidatorController : Controller
    {
        // GET: WopiValidator
        public ActionResult Index(string id)
        {
            WopiValidator wvModel = new WopiValidator();
            wvModel.FileName = id;
            string sourceDoc = id.ToLower();
            string uid = "TestUser".ToLower();

            wvModel.AccessToken = AccessTokenUtil.WriteToken(AccessTokenUtil.GenerateToken(uid, sourceDoc));
            return View(wvModel);
        }
    }
}
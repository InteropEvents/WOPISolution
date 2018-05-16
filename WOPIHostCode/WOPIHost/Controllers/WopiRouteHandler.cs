using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WOPIHost.Controllers
{
    /// <summary>
    /// A route handler
    /// </summary>
    public class WopiRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new WopiHandler();
        }
    }
}
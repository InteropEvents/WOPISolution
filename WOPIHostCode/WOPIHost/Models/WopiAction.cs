using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WOPIHost.Models
{
    public class WopiAction
    {
        /// <summary>
        /// Name of the app
        /// </summary>
        public string app { get; set; }

        /// <summary>
        /// Specifies the URL of an image resource that a WOPI server uses as the Favorites Icon for a page showing
        /// the output of a WOPI client
        /// </summary>
        public string favIconUrl { get; set; }

        /// <summary>
        /// Specifies that a WOPI server SHOULD enforce license restrictions for file types within the ct_app-name block
        /// </summary>
        public bool checkLicense { get; set; }

        /// <summary>
        /// Specifies the name of the action
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Specifies the file extension supported by this action
        /// </summary>
        public string ext { get; set; }

        /// <summary>
        /// Specifies the progid (a string that identifies a folder as being associated with a specific application)
        /// supported by this action
        /// </summary>
        public string progid { get; set; }

        /// <summary>
        /// Specifies the required capabilities of the WOPI server to be able to use this action
        /// </summary>
        public string requires { get; set; }

        /// <summary>
        /// Specifies whether the WOPI server is to use this action as the default action for this file type
        /// </summary>
        public bool? isDefault { get; set; }

        /// <summary>
        /// Specifies the URI that the WOPI server can use to call the action
        /// </summary>
        public string urlsrc { get; set; }
    }
}
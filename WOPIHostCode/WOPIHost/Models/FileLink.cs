using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WOPIHost.Models
{
    /// <summary>
    /// A link contains file name and URL
    /// </summary>
    public class FileLink
    {
        /// <summary>
        /// File name
        /// </summary>
        public string Name;

        /// <summary>
        /// URL related to the file
        /// </summary>
        public string Url;
    }
}
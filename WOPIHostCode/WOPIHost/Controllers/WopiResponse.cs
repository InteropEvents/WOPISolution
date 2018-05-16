
namespace WOPIHost.Controllers
{
    class CheckFileInfoResponse
    {
        // This is a subset of all CheckFileInfo properties.
        // Use optional properties in accordance with the [MS-WOPI] Web Application Open Platform Interface Protocol specification.

        public string BaseFileName { get; set; }
        public string OwnerId { get; set; }
        public int Size { get; set; }
        public string Version { get; set; }

        public string BreadcrumbBrandName { get; set; }
        public string BreadcrumbBrandUrl { get; set; }
        public string BreadcrumbFolderName { get; set; }
        public string BreadcrumbFolderUrl { get; set; }
        public string BreadcrumbDocName { get; set; }
        public string BreadcrumbDocUrl { get; set; }

        public bool UserCanWrite { get; set; }
        public bool ReadOnly { get; set; }
        public bool SupportsLocks { get; set; }
        public bool SupportsUpdate { get; set; }
        public bool UserCanNotWriteRelative { get; set; }

        public string UserFriendlyName { get; set; }

        public bool WebEditingDisabled { get; set; }
        public bool RestrictedWebViewOnly { get; set; }
        public bool SupportsCoauth { get; set; }
        public bool SupportsCobalt { get; set; }

        public string HostEditUrl { get; set; }
        public bool EditModePostMessage { get; set; }
        public bool DisableBrowserCachingOfUserContent { get; set; }

        public string SHA256 { get; set; }
    }

    class PutRelativeFileResponse
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string HostViewUrl { get; set; }
        public string HostEditUrl { get; set; }
    }
}
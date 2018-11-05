﻿using System.Configuration;
using System.IO;

namespace WOPIHost.Controllers
{
    enum RequestType
    {
        None,

        CheckFileInfo,
        PutRelativeFile,

        Lock,
        Unlock,
        RefreshLock,
        UnlockAndRelock,

        ExecuteCobaltRequest,

        DeleteFile,
        ReadSecureStore,
        GetRestrictedLink,
        RevokeRestrictedLink,

        CheckFolderInfo,

        GetFile,
        PutFile,

        EnumerateChildren,
    }

    static class WopiHeaders
    {
        public const string RequestType = "X-WOPI-Override";
        public const string ItemVersion = "X-WOPI-ItemVersion";

        public const string Lock = "X-WOPI-Lock";
        public const string OldLock = "X-WOPI-OldLock";
        public const string LockFailureReason = "X-WOPI-LockFailureReason";
        public const string LockedByOtherInterface = "X-WOPI-LockedByOtherInterface";

        public const string SuggestedTarget = "X-WOPI-SuggestedTarget";
        public const string RelativeTarget = "X-WOPI-RelativeTarget";
        public const string OverwriteRelativeTarget = "X-WOPI-OverwriteRelativeTarget";
    }

    class WopiRequest
    {
        public RequestType Type { get; set; }

        public string AccessToken { get; set; }

        public string Id { get; set; }

        public string FullPath
        {
            get
            {
                string storageType = ConfigurationManager.AppSettings["FileStorageType"];
                switch (storageType)
                {
                    case "FTP":
                        return Path.Combine(ConfigurationManager.AppSettings["FileServiceUrl"], Id);

                    case "Local":
                        return Path.Combine(ConfigurationManager.AppSettings["FileLocalPath"], Id);

                    default:
                        return Path.Combine(ConfigurationManager.AppSettings["FileServiceUrl"], Id);
                }
            }
        }
    }
}
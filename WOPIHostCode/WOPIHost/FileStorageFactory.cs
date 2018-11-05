
using System.Configuration;

namespace WOPIHost
{
    /// <summary>
    /// The factory for creating the file storage instance
    /// </summary>
    public class FileStorageFactory
    {
        /// <summary>
        /// The file storage instance
        /// </summary>
        /// <returns></returns>
        public static IFileStorage CreateFileStorage()
        {
            string storageType = ConfigurationManager.AppSettings["FileStorageType"];
            switch (storageType)
            {
                case "FTP":
                    return new FTPFileStorage();

                case "Local":
                    return new FileStorage();

                default:
                    return new FTPFileStorage();
            }

        }
    }
}
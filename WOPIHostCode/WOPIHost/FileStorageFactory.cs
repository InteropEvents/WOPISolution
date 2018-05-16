
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
            return new FTPFileStorage();
        }
    }
}
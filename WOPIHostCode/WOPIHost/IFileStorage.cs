using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WOPIHost
{
    public interface IFileStorage
    {
        /// <summary>
        /// Get file size.
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>File size</returns>
        long GetFileSize(string name);

        /// <summary>
        /// Get file last modified time.
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>File last modified time in UTC DateTime</returns>
        DateTime? GetLastModifiedTime(string name);

        /// <summary>
        /// Get file stream.
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>File stream</returns>
        Stream GetFile(string name);

        /// <summary>
        /// Upload file.
        /// </summary>
        /// <param name="name">File name</param>
        /// <param name="stream">File stream</param>
        /// <returns>Return 0 if success; Otherwise, return -1</returns>
        int UploadFile(string name, Stream stream);

        /// <summary>
        /// Get file name list.
        /// </summary>
        /// <returns>File name list</returns>
        List<string> GetFileNames();

        /// <summary>
        /// Get the file version.
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>Return a string representing the file version.</returns>
        string GetFileVersion(string name);

        /// <summary>
        /// Get File ReadOnly status
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>Return true if readonly, false otherwise.</returns>
        bool GetReadOnlyStatus(string name);
    }
}

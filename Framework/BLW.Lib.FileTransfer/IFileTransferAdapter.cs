using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.Setting;

namespace BLW.Lib.TransferManager
{
    public interface IFileTransferAdapter
    {
        /// <summary>
        /// Download file from server
        /// </summary>
        /// <param name="file">Source path of the file</param>
        /// <returns></returns>
        bool DownloadFile(string file);

        /// <summary>
        /// Delete file from server
        /// </summary>
        /// <param name="file">Source file</param>
        void DeleteFile(string file);

        /// <summary>
        /// Upload file to server
        /// </summary>
        /// <param name="file">Source file</param>
        /// <returns></returns>
        bool UploadFile(string file,string fileName);

        /// <summary>
        /// Delete a directory along with files
        /// </summary>
        /// <param name="directory">source directory</param>
        void DeleteDirectory(string directory);

        /// <summary>
        /// Return existing file list in directory
        /// </summary>
        /// <param name="path">source directory</param>
        /// <returns>File list</returns>
        List<string> ShowFileList(string path);

        bool IsAccessible(string file);
    }
}

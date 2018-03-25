using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileTransfer
{
    public interface IFileTransferAdapter
    {
        IAdaperSettings Settings { get; }
        /// <summary>
        /// Get file list
        /// </summary>
        /// <param name="validation">mask of file, if null no validation</param>
        /// <returns></returns>
        List<string> GetFileList(string validation);
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
        bool UploadFile(string file);
        /// <summary>
        /// Delete a directory along with files
        /// </summary>
        /// <param name="directory">source directory</param>
        void DeleteDirectory(string directory);        
    }
}

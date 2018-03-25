using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BLW.Lib.FileTransfer;
using System.Text.RegularExpressions;

namespace BLW.Lib.FileTransfer
{
    public class SFTPFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSettings _adaperSettings;
        SftpManager objSftpManager = null;
        public IAdaperSettings adaperSettings
        {
            get
            {
                return _adaperSettings;
            }
        }

        public SFTPFileTransferAdapter(IAdaperSettings AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
            objSftpManager = new SftpManager(_adaperSettings);
        }
        public IAdaperSettings Settings
        {
            get { return _adaperSettings; }
        }

        public List<string> GetFileList(string validation)
        {
            try
            {
                List<String> fileList = new List<string>();
                var allFilesDetails = objSftpManager.SftpFilesList(_adaperSettings.SourceLocation);
                if (allFilesDetails == null || allFilesDetails.Count == 0)
                {
                    return fileList;
                }

                fileList = allFilesDetails.Select(T => T.FullName).ToList();
                if (validation != null)
                {
                    var validFiles = _adaperSettings.GetFilesByMask(fileList, validation);
                    if (validFiles.Count > 0)
                        fileList = validFiles;

                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while get file list from SFTP.", ex);
            }
            
        }
        /// <summary>
        /// Download give file in Destination Location
        /// </summary>
        /// <param name="file">File to be downloaded</param>
        /// <returns>Returns true if file downloaded else false</returns>
        public bool DownloadFile(string file)
        {
            try
            {
                if (!Directory.Exists(_adaperSettings.DestinationLocation))
                    Directory.CreateDirectory(_adaperSettings.DestinationLocation);

                return objSftpManager.SftpFileDownload(_adaperSettings.DestinationLocation, file);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while Download file from SFTP.", ex);
            }
        }
        public void DeleteFile(string file)
        {            
            try { 
                objSftpManager.SftpDeleteFile(file);
               }
            catch (Exception ex) { 
                throw new Exception("Error while deleting file from SFTP.", ex);
            }
        }

        public bool UploadFile(string file)
        {

            var isConnect = objSftpManager.SftpConnect();
            if (!isConnect)
                throw new SftpConnectionExp("Sftp not connected.");

            try { objSftpManager.SftpFileUpload(file, _adaperSettings.DestinationLocation); return true; }
            catch (Exception ex) { throw new Exception("Error while uploading file.", ex); }
        }

        public void DeleteDirectory(string directory)
        {
            var isConnect = objSftpManager.SftpConnect();
            if (!isConnect)
                throw new SftpConnectionExp("Sftp not connected.");
            try { objSftpManager.SftpRemoveDirectory(directory); }
            catch (Exception ex) { throw new Exception("Error while deleting directory.", ex); }
        
        }
    }
}

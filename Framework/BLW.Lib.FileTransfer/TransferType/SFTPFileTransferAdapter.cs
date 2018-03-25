using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BLW.Lib.TransferManager.Setting;
using BLW.Lib.TransferManager.Connectors;

namespace BLW.Lib.TransferManager.TransferType
{
    class SFTPFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSetting _adaperSettings;
        SftpManager objSftpManager = null;      

        public SFTPFileTransferAdapter(IAdaperSetting AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
            objSftpManager = new SftpManager(_adaperSettings);
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
            try
            {
                objSftpManager.SftpDeleteFile(file);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting file from SFTP.", ex);
            }
        }

        public bool UploadFile(string file, string fileName)
        {

            var isConnect = objSftpManager.SftpConnect();
            if (!isConnect)
                throw new Exception("Sftp not connected.");

            try { objSftpManager.SftpFileUpload(file, _adaperSettings.DestinationLocation); return true; }
            catch (Exception ex) { throw new Exception("Error while uploading file.", ex); }
        }

        public void DeleteDirectory(string directory)
        {
            var isConnect = objSftpManager.SftpConnect();
            if (!isConnect)
                throw new Exception("Sftp not connected.");
            try { objSftpManager.SftpRemoveDirectory(directory); }
            catch (Exception ex) { throw new Exception("Error while deleting directory.", ex); }

        }


        public List<string> ShowFileList(string path)
        {
            throw new NotImplementedException();
        }


        public bool IsAccessible(string file)
        {
            throw new NotImplementedException();
        }
    }
}

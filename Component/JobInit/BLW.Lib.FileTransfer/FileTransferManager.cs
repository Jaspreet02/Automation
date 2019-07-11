using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.FileTransfer
{
    public class FileTransferManager
    {
        private IFileTransferAdapter FileTransferAdapter;
        public IAdaperSettings Settings { get { return FileTransferAdapter.Settings; } }

        public FileTransferManager(IFileTransferAdapter fileTransferAdapter)
        {
            this.FileTransferAdapter = fileTransferAdapter;
        }

        public bool DownloadFile(string file)
        {
            try
            {
                var isDownlodeSucesfully = FileTransferAdapter.DownloadFile(file);
                return isDownlodeSucesfully;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UploadFile(String file)
        {
            try
            {
                if (String.IsNullOrEmpty(file.Trim()))
                    throw new FileTransferExpt("Error :  Argument null or empty 'uploadToPath output' path is null empty in Uploding file");

                bool result = FileTransferAdapter.UploadFile(file);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<String> GetFileList(string validation)
        {
            try
            {
                List<string> fileList = new List<string>();
                if (string.IsNullOrEmpty(validation))
                    fileList = FileTransferAdapter.GetFileList(null);
                else
                    fileList = FileTransferAdapter.GetFileList(validation);
                return fileList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFile(String file)
        {
            try
            {
                FileTransferAdapter.DeleteFile(file);
            }
             catch (Exception ex)
            {
                throw ex ;
            }
        }

        public void RemoveDirectory(String directory)
        {
            try
            {
                if (String.IsNullOrEmpty(directory))
                    throw new RemoveDirectoryExp("Error : directory which want to delete is null or empty");

                FileTransferAdapter.DeleteDirectory(directory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


}

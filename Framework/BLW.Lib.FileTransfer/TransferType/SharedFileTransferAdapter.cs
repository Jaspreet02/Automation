using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.Setting;
using System.IO;

namespace BLW.Lib.TransferManager.TransferType
{
    class SharedFileTransferAdapter : IFileTransferAdapter
    {

        private IAdaperSetting _adaperSettings;

        public SharedFileTransferAdapter(IAdaperSetting AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
        }

        /// <summary>
        /// copy give file into destination Location
        /// </summary>
        /// <param name="file">File to be downloaded</param>
        /// <returns>Returns true if file downloaded else false</returns>
        public bool DownloadFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found " + file);
            if (!Directory.Exists(_adaperSettings.DestinationLocation))
                Directory.CreateDirectory(_adaperSettings.DestinationLocation);

            File.Copy(file, _adaperSettings.DestinationLocation + "\\" + Path.GetFileName(file), false);
            return true;
        }

        public void DeleteFile(string file)
        {
            if (!File.Exists(file))
                throw new Exception("Error file not exists Shared location to delete" + file);
            File.Delete(file);
        }

        public bool UploadFile(string file, string fileName)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found " + file);
            if (!Directory.Exists(_adaperSettings.DestinationLocation))
                Directory.CreateDirectory(_adaperSettings.DestinationLocation);

            File.Copy(file, _adaperSettings.DestinationLocation + "\\" + fileName, false);
            return true;
        }

        public void DeleteDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                throw new Exception("Error Directory not exists locally to delete" + directory);
            Directory.Delete(directory);
        }


        public List<string> ShowFileList(string path)
        {
            throw new NotImplementedException();
        }


        public bool IsAccessible(string file)
        {
            return true;
        }
    }
}

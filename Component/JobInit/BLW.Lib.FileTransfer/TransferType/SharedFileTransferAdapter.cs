using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BLW.Lib.FileTransfer
{
    [Obsolete("We are not using this, as Similar functionality is there in LocalFileTransferAdapter class, use LocalFileTransferAdapter.", true)]
    class SharedFileTransferAdapter : IFileTransferAdapter
    {
        
        private IAdaperSettings _adaperSettings;

        public SharedFileTransferAdapter(IAdaperSettings AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
        }
        public List<string> GetFileList(string validation)
        {
            List<string> files = new List<string>();
            var allFiles = Directory.GetFiles(_adaperSettings.SourceLocation);
            if (validation == null)
                files = allFiles.ToList();
            else
                files = _adaperSettings.GetFilesByMask(allFiles.ToList(), validation);          
            return files;
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
                throw new DeleteFileExp("Error file not exists Shared location to delete" + file);
            File.Delete(file);
        }
        public bool UploadFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found " + file);
            if (!Directory.Exists(_adaperSettings.DestinationLocation))
                Directory.CreateDirectory(_adaperSettings.DestinationLocation);

            File.Copy(file, _adaperSettings.DestinationLocation + "\\" + Path.GetFileName(file), false);
            return true;
        }
        public void DeleteDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                throw new RemoveDirectoryExp("Error Directory not exists locally to delete" + directory);
            Directory.Delete(directory);
        }
        /// <summary>
        /// 
        /// </summary>
        public IAdaperSettings Settings
        {
            get { return _adaperSettings; }
        }
    }
}

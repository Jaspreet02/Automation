using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.Setting;
using System.IO;
using System.Threading;

namespace BLW.Lib.TransferManager.TransferType
{
    class LocalFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSetting _adaperSettings;

        public LocalFileTransferAdapter(IAdaperSetting AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
        }

        /// <summary>
        /// Copy files from input filelocation into HotFolder
        /// </summary>
        /// <param name="file">File location</param>
        /// <returns>True if file is has copied into destination location,otherwise false</returns>
        public bool DownloadFile(string file)
        {
            //Is file or folder
            bool isFile = File.Exists(file);
            //Then may be it is directory
            if (isFile)
            {
                if (!File.Exists(file))
                    throw new FileNotFoundException("File not found " + file);

                FileInfo objFileInfo = null;
                long fileSizeFirst;
                objFileInfo = new FileInfo(file);
                fileSizeFirst = objFileInfo.Length;
                Thread.Sleep(5000);
                objFileInfo = new FileInfo(file);
                if (!objFileInfo.Length.Equals(fileSizeFirst))
                {
                    throw new IOException("File size is increasing at source location. Possibly still being transmitted.");
                }

                if (!Directory.Exists(_adaperSettings.DestinationLocation))
                    Directory.CreateDirectory(_adaperSettings.DestinationLocation);

                File.Copy(file, Path.Combine(_adaperSettings.DestinationLocation, Path.GetFileName(file)), false);
            }
            else
            {
                //It may be the folder, Then check for
                if (!Directory.Exists(file))
                    throw new FileNotFoundException("Folder not found " + file);
                //Copy all the files & Replaces any files with the same name
                foreach (string newPath in Directory.GetFiles(file, "*.*", SearchOption.AllDirectories))
                {
                    string destFile = newPath.Replace(file, Path.Combine(_adaperSettings.DestinationLocation, new DirectoryInfo(file).Name));
                    string directory = Path.GetDirectoryName(destFile);
                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    File.Copy(newPath, destFile, false);
                }
            }
            return true;
        }

        public void DeleteFile(string file)
        {
            if (!File.Exists(file))
                throw new Exception("File not exists : " + file);

            File.Delete(file);

        }

        public bool UploadFile(string file, string fileName)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found : " + file);
            if (!Directory.Exists(_adaperSettings.DestinationLocation))
                Directory.CreateDirectory(_adaperSettings.DestinationLocation);

            File.Copy(file, Path.Combine(_adaperSettings.DestinationLocation, fileName), false);
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
            List<string> list = new List<string>();
            try
            {
                list = Directory.GetFiles(path).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public bool IsAccessible(string file)
        {
            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Write, FileShare.None))
                return true;
        }
    }
}

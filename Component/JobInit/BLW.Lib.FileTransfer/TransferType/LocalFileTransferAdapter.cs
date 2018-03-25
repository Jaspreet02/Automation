using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using BLW.Lib.CoreUtility;

namespace BLW.Lib.FileTransfer
{
    public class LocalFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSettings _adaperSettings;

        public LocalFileTransferAdapter(IAdaperSettings AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
        }

        /// <summary>
        /// <para> get files from source location according to mask(validation) suplied,</para>
        /// <para>if mask(validation) is null then get all files present in source location</para>
        /// </summary>
        /// <param name="validation">mask that are Required</param>
        /// <returns>list of all validated files</returns>
        public List<string> GetFileList(string validation)
        {
            List<string> files = new List<string>();
            if (validation == null)
            {
                var allFiles = Directory.GetFiles(_adaperSettings.SourceLocation);
                files = allFiles.ToList();
            }
            else
            {
                if (validation.ToUpper().Contains("{{DIRECTORY}}"))
                {
                    validation = validation.Replace("{{DIRECTORY}}", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                    files = Directory.GetDirectories(_adaperSettings.SourceLocation, validation, SearchOption.TopDirectoryOnly).ToList();
                }
                else
                {
                    var allFiles = Directory.GetFiles(_adaperSettings.SourceLocation);
                    files = _adaperSettings.GetFilesByMask(allFiles.ToList(), validation);
                }
            }

            //File in used exception
            foreach (var file in files)
            {
                if (FileOperation.IsFileLocked(file))
                    throw new IOException(file + " file in use while checking for validation.");
            }
            return files;
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

                if (FileOperation.IsFileLocked(file))
                    throw new IOException(file + " file in use while checking for download.");

                File.Copy(file, Path.Combine(_adaperSettings.DestinationLocation, Path.GetFileName(file)), false);

                //verify file
                if (!FileOperation.FileCompare(file, Path.Combine(_adaperSettings.DestinationLocation, Path.GetFileName(file))))
                {
                    Directory.Delete(_adaperSettings.DestinationLocation);
                    throw new InvalidOperationException("Full file is not downloaded, difference in file sizes");
                }
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

                    if (FileOperation.IsFileLocked(newPath))
                        throw new IOException("File in used or not found.");

                    File.Copy(newPath, destFile, false);
                    //verify file
                    if (!FileOperation.FileCompare(newPath, destFile))
                    {
                        Directory.Delete(directory);
                        throw new InvalidOperationException("Full file is not downloaded, difference in file sizes");
                    }
                }
            }
            return true;
        }

        public void DeleteFile(string file)
        {
            try
            {
                if (!File.Exists(file))
                    throw new Exception(file + " file does not exist which we are go to delete.");

                if (FileOperation.IsFileLocked(file))
                    throw new IOException(file + " file in use while checking for delete.");

                File.Delete(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UploadFile(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found : " + file);

            if (FileOperation.IsFileLocked(file))
                throw new IOException("File in used or not found.");

            File.Copy(file, Path.Combine(_adaperSettings.DestinationLocation, Path.GetFileName(file)), false);
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

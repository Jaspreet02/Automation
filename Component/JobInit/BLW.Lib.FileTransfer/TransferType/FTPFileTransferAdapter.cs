using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace BLW.Lib.FileTransfer
{
    class FTPFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSettings _adaperSettings;

        public FTPFileTransferAdapter(IAdaperSettings AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
        }

        public IAdaperSettings Settings
        {
            get { return _adaperSettings; }
        }
        /// <summary>
        /// <para> Get files from source location according to mask(validation) suplied,</para>
        /// <para>if mask(validation) is null then get all files present in source location</para>
        /// </summary>
        /// <param name="validation">Mask that are Required</param>
        /// <returns>List of all valid files to be downloaded</returns>
        public List<string> GetFileList(string validation)
        {
            try
            {
                List<String> fileList = new List<string>();
                var allFilesDetails = ShowFileList(_adaperSettings.SourceLocation);
                if (allFilesDetails == null || allFilesDetails.Count == 0)
                {
                    return fileList;
                }
                if (validation != null)
                {
                    var validFiles = _adaperSettings.GetFilesByMask(allFilesDetails, validation);
                    if (validFiles.Count > 0)
                        fileList = validFiles;

                }
                return fileList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while get file list from FTP.", ex);
            }
        }

        public List<string> ShowFileList(string file)
        {
            try
            {
                string ftpfullpath = "ftp://" + _adaperSettings.Host + "/" + file;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                request.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                List<string> lines = new List<string>();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var fileLocation = "";
                    if (line.Split('/').Count() > 1)
                    {
                        //fileLocation = line.Substring(line.IndexOf("/"));
                        fileLocation = file + (string.IsNullOrEmpty(file) ? "" : "/") +Path.GetFileName(line);
                        lines.Add(fileLocation);
                    }
                    else
                        lines.Add(line);
                }
                streamReader.Close();
                return lines;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Download give file into destination location
        /// </summary>
        /// <param name="file">File to be downloaded</param>
        /// <returns>Returns true if file downloaded else false</returns>
        public bool DownloadFile(string file)
        {
            string filename = Path.GetFileName(file);
            if (!File.Exists(_adaperSettings.DestinationLocation + "/" + filename))
            {
                try
                {
                    if (!Directory.Exists(_adaperSettings.DestinationLocation))
                        Directory.CreateDirectory(_adaperSettings.DestinationLocation);
                    string ftpfullpath = "ftp://" + _adaperSettings.Host + "//" + file;
                    FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                    requestFileDownload.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);
                    requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                    FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                    Stream responseStream = responseFileDownload.GetResponseStream();
                    FileStream writeStream = new FileStream(_adaperSettings.DestinationLocation + "/" + filename, FileMode.Create);
                    int Length = 2048;
                    Byte[] buffer = new Byte[Length];
                    int bytesRead = responseStream.Read(buffer, 0, Length);
                    while (bytesRead > 0)
                    {
                        writeStream.Write(buffer, 0, bytesRead);
                        bytesRead = responseStream.Read(buffer, 0, Length);
                    }
                    responseStream.Close();
                    writeStream.Close();
                    requestFileDownload = null;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return true;
        }

        public void DeleteFile(string file)
        {
            try
            {
                string ftpfullpath = "ftp://" + _adaperSettings.Host + "/" + file;
                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                ftpRequest.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse responseFileDelete = (FtpWebResponse)ftpRequest.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UploadFile(string file)
        {
            return true;
        }

        public void DeleteDirectory(string directory)
        {
        }
    }


}

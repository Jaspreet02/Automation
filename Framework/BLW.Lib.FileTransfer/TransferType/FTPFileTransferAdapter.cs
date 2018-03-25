using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.Setting;
using System.IO;
using System.Net;
using BLW.Lib.Log;

namespace BLW.Lib.TransferManager.TransferType
{
    class FTPFileTransferAdapter : IFileTransferAdapter
    {
        private IAdaperSetting _adaperSettings;

        public FTPFileTransferAdapter(IAdaperSetting AdaperSettings)
        {
            _adaperSettings = AdaperSettings;
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
                    string ftpfullpath = "ftp://" + _adaperSettings.Host + "/" + file;
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

        public bool UploadFile(string file, string fileName)
        {
            try
            {
                SingletonLogger.Instance.Debug("UploadFile : "  + file + "  Started.");
                CreateFTPDirectory();
                SingletonLogger.Instance.Debug("UploadFile : " + file + "  Started.");
                string ftpfullpath = "ftp://" + _adaperSettings.Host + "/" + _adaperSettings.DestinationLocation + "/" + fileName;
                SingletonLogger.Instance.Debug("ftpfullpath : " + ftpfullpath );
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
               
                ftp.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);

                SingletonLogger.Instance.Debug("_adaperSettings.UserName : " + _adaperSettings.UserName);
                SingletonLogger.Instance.Debug("_adaperSettings.Password : " + _adaperSettings.Password);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                FileStream fs = File.OpenRead(file);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while uploading file.", ex);
            }
            return true;
        }

        public void DeleteDirectory(string directory)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CreateFTPDirectory()
        {

            try
            {
                //create the directory
                FtpWebRequest requestDir = (FtpWebRequest)FtpWebRequest.Create("ftp://" + _adaperSettings.Host + "/" + _adaperSettings.DestinationLocation);
                requestDir.Method = WebRequestMethods.Ftp.MakeDirectory;
                requestDir.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);
                requestDir.UsePassive = true;
                requestDir.UseBinary = true;
                requestDir.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)requestDir.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();

                return true;
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    response.Close();
                    return true;
                }
                else
                {
                    response.Close();
                    return false;
                }
            }
        }

        public List<string> ShowFileList(string path)
        {
            try
            {
                string ftpfullpath = "ftp://" + _adaperSettings.Host + "/" + path;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpfullpath);
                request.Credentials = new NetworkCredential(_adaperSettings.UserName, _adaperSettings.Password);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                StreamReader streamReader = new StreamReader(request.GetResponse().GetResponseStream());
                List<string> lines = new List<string>();
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
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
        
        public bool IsAccessible(string file)
        {
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WeOnlyDo.Client;
using BLW.Lib.TransferManager.FtpDetail;
using BLW.Lib.TransferManager.Setting;
using System.IO;

namespace BLW.Lib.TransferManager.Connectors
{
    public partial class SftpManager
    {
        /// <summary>
        /// SFTP Object
        /// </summary>
        private SFTP _objSftpClient = new SFTP();

        /// <summary>
        /// Type of file
        /// </summary>
        String FileType = String.Empty;

        /// <summary>
        /// Transaction Type
        /// </summary>
        String TransactionType = String.Empty;

        /// <summary>
        /// Genric List of IFtpFileInfo type
        /// </summary>
        public List<IFtpFileInfo> CurrentSubDirectoryFiles { get; set; }

        /// <summary>
        /// RemotePathLocation
        /// </summary>
        String RemotePathLocation = String.Empty;
        public IAdaperSetting objAdpterSettings { get; set; }


        public SftpManager(IAdaperSetting _adpterSetting)
        {
            objAdpterSettings = _adpterSetting;
        }

        /// <summary>
        /// Function Name : SftpConnect()
        /// Description   : This method is used to create SFTP Connection.   
        /// </summary>
        /// <returns>It returns true if connected and false if not connected.</returns>
        public Boolean SftpConnect()
        {
            Boolean isConnected = false;

            _objSftpClient.ConnectedEvent += new SFTP.ConnectedDelegate(ConnectedEvent);
            _objSftpClient.StateChangedEvent += new SFTP.StateChangedDelegate(StateChangedEvent);
            _objSftpClient.DisconnectedEvent += new SFTP.DisconnectedDelegate(DisconnectedEvent);
            _objSftpClient.AttributesDataEvent += new SFTP.AttributesDataDelegate(AttributesDataEvent);
            _objSftpClient.ProgressEvent += new SFTP.ProgressDelegate(ProgressEvent);
            _objSftpClient.LoopItemEvent += new SFTP.LoopDelegate(LoopItemEvent);
            _objSftpClient.Hostname = objAdpterSettings.Host;
            _objSftpClient.Login = objAdpterSettings.UserName;
            _objSftpClient.Password = objAdpterSettings.Password;
            _objSftpClient.Port = objAdpterSettings.Port;
            _objSftpClient.Blocking = true;
            _objSftpClient.LicenseKey = "NYWQ-KGCS-36UG-5SMZ";

            try
            {
                if (_objSftpClient.State == SFTP.States.Disconnected)
                {
                    _objSftpClient.Connect();
                    while (_objSftpClient.State == SFTP.States.Connecting)
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (_objSftpClient.State == SFTP.States.Idle)
                    isConnected = true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while process try to connect with SFTP .Error: " + ex.Message);
                throw ex;
            }

            return isConnected;
        }

        /// <summary>
        /// Function Name : SftpDisconnect()
        /// Description   : This method is used to close the sftp connection that have been used for data transfer.
        /// </summary>
        public void SftpDisconnect()
        {
            try
            {
                Thread.Sleep(2000);
                _objSftpClient.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while process try to disconnect with SFTP.Error:  " + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Function Name : SftpMakeDirectory()
        /// Description   : Creates a directory on SFTP Server.  
        /// </summary>
        /// <param name="sftpDirectory">Sftp Directory Name</param>
        public void SftpMakeDirectory(String sftpDirectory)
        {
            try
            {
                _objSftpClient.MakeDir(sftpDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error when creating new directory.", ex);
            }
        }

        /// <summary>
        /// Function Name : SftpRemoveDirectory()
        /// Description   : This method is used to remove a directory from a SFTP location.
        /// </summary>
        /// <param name="sftpDirectory">Sftp Directory Name</param>
        public void SftpRemoveDirectory(String sftpDirectory)
        {
            try
            {
                Thread.Sleep(5000);
                _objSftpClient.RemoveDir(sftpDirectory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting directory from SFTP", ex);
            }
        }

        /// <summary>
        /// Function Name : SftpDeleteFile()
        /// Description   : Deletes a given file from a SFTP location.  
        /// </summary>
        /// <param name="sftpFile">Name of a file to delete from SFTP</param>
        public void SftpDeleteFile(String sftpFile)
        {
            try
            {
                if (!this.SftpConnect())
                {
                    this.SftpConnect();

                    if (!this.SftpConnect())
                        throw new Exception("Sftp not connected.");
                }
                _objSftpClient.DeleteFile("/" + sftpFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while deleting file from SFTP. Error: " + ex);
                throw new Exception("Error while deleting file from SFTP. Error: " + ex); ;
            }

        }

        /// <summary>
        /// Function Name : SftpDirectoryList()
        /// Description   : This method is used to fetch list of sub directories in a given directory on the SFTP Server.   
        /// </summary>
        /// <param name="sftpDirectory">Name of Sftp Directory to get list of files</param>
        public void SftpDirectoryList(String sftpDirectory)
        {
            Thread.Sleep(2000);
            _objSftpClient.ListAttributes(sftpDirectory);
        }

        /// <summary>
        /// Function Name : SftpFilesList()
        /// Description   : This method is used to fetch list of files in a given directory on the SFTP Server.   
        /// </summary>
        public List<IFtpFileInfo> SftpFilesList(string source)
        {
            List<IFtpFileInfo> tempList = new List<IFtpFileInfo>();
            try
            {
                if (!this.SftpConnect())
                {
                    this.SftpConnect();

                    if (!this.SftpConnect())
                        throw new Exception("Sftp not connected.");
                }
                Thread.Sleep(100);
                RemotePathLocation = source;
                _objSftpClient.ListAttributes(source);
                tempList = CurrentSubDirectoryFiles;
                CurrentSubDirectoryFiles = new List<IFtpFileInfo>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while getting files information from SFTP." + ex.Message);
                throw ex;
            }
            return tempList;
        }

        /// <summary>
        /// Function Name : SftpFileUpload()
        /// Description   : This method is used to upload a file from local machine to the SFTP Server.
        /// </summary>
        /// <param name="localLocation">local file path to upload</param>
        public void SftpFileUpload(String localFile, string remortLocation)
        {
            try
            {
                if (!this.SftpConnect())
                {
                    this.SftpConnect();

                    if (!this.SftpConnect())
                        throw new Exception("Sftp not connected.");
                }
                Thread.Sleep(5000);
                _objSftpClient.PutFile(localFile, remortLocation);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while uploading file" + localFile + " on SFTP." + ex.Message);
            }
        }


        /// <summary>
        /// Function Name : SftpFileDownload()
        /// Description   : This method is used to download a file from SFTP Server to local machine.
        /// </summary>
        /// <param name="localPath">local file path </param>
        /// <param name="remoteFile">SFTP file name that need to be downloaded.</param>
        public bool SftpFileDownload(String localPath, String remoteFile)
        {
            var fullFilepath = string.Empty;
            bool IsComplete = true;
            Thread.Sleep(2000);
            try
            {
                if (!this.SftpConnect())
                {
                    this.SftpConnect();

                    if (!this.SftpConnect())
                        throw new Exception("Sftp not connected.");
                }


                //fullFilepath = objAdpterSettings.SourceLocation.TrimEnd('\\', '/') + "/" + remoteFile;

                if (File.Exists(localPath + "\\" + Path.GetFileName(remoteFile)))
                {
                    Random random = new Random();
                    File.Move(localPath + "\\" + Path.GetFileName(remoteFile), localPath + "\\" + random.Next() + "_" + Path.GetFileName(remoteFile));
                }
                _objSftpClient.GetFile(localPath, remoteFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while downloading files from SFTP. Error" + ex.Message);
                IsComplete = false;
                throw ex;

            }
            return IsComplete;
        }

        /// <summary>
        /// Function Name : SftpDownloadAllFilesWithExtension()
        /// Description   : This method is used to download files with given extension  
        /// </summary>
        /// <param name="sftpDestinationDirectory">Sftp Destination Directory</param>
        /// <param name="sftpSourceDirectory">Sftp Source Directory</param>
        /// <param name="fileExtension">File Extension</param>
        public void SftpDownloadAllFilesWithExtension(String sftpDestinationDirectory, String sftpSourceDirectory, String fileExtension)
        {
            if (!this.SftpConnect())
            {
                this.SftpConnect();

                if (!this.SftpConnect())
                    throw new Exception("Sftp not connected.");
            }
            Thread.Sleep(2000);
            FileType = fileExtension;
            TransactionType = "Download";
            _objSftpClient.GetFiles(sftpDestinationDirectory, sftpSourceDirectory, 0);
        }

        /// <summary>
        /// SftpDownloadAllFiles function used to download all files in a folder
        /// </summary>
        /// <param name="localDirectoryLocation">SFTP Destination Directory</param>
        /// <param name="remortSftpDirectory">SFTP Source Directory </param>
        public void SftpDownloadAllFiles(String localDirectoryLocation, String remortSftpDirectory)
        {
            try
            {
                if (!this.SftpConnect())
                {
                    this.SftpConnect();

                    if (!this.SftpConnect())
                        throw new Exception("Sftp not connected.");
                }
                Thread.Sleep(2000);
                _objSftpClient.GetFiles(localDirectoryLocation, remortSftpDirectory, 0);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while downloading files from host " + objAdpterSettings.Host, ex);
            }

        }

        /// <summary>
        /// Function Name : SftpUploadAllFilesWithExtension()
        /// Description   : SftpUploadAllFilesWithExtension function used to upload files with given extension
        /// </summary>
        /// <param name="SftpSourceDirectory">Sftp Source Directory</param>
        /// <param name="SftpDestinationDirectory">Sftp Destination Directory</param>
        /// <param name="FileExtension">File Extension</param>
        public void SftpUploadAllFilesWithExtension(String SftpSourceDirectory, String SftpDestinationDirectory, String FileExtension)
        {
            if (!this.SftpConnect())
            {
                this.SftpConnect();

                if (!this.SftpConnect())
                    throw new Exception("Sftp not connected.");
            }
            Thread.Sleep(5000);
            FileType = FileExtension;
            TransactionType = "Upload";
            _objSftpClient.PutFiles(SftpSourceDirectory, SftpDestinationDirectory, 0);
        }

        /// <summary>
        /// Function Name : SftpUploadAllFiles()
        /// Description   : SftpUploadAllFiles function Used for uploading all files in the given directory
        /// </summary>
        /// <param name="SftpSourceDirectory">Sftp Source Directory</param>
        /// <param name="SftpDestinationDirectory">Sftp Destination Directory</param>
        public void SftpUploadAllFiles(String SftpSourceDirectory, String SftpDestinationDirectory)
        {
            if (!this.SftpConnect())
            {
                this.SftpConnect();

                if (!this.SftpConnect())
                    throw new Exception("Sftp not connected.");
            }
            Thread.Sleep(5000);
            _objSftpClient.PutFiles(SftpSourceDirectory, SftpDestinationDirectory, 0);
        }

        /// <summary>
        /// Function Name : SftpRename()
        /// Description   : SftpRename function used for renaming a file on server
        /// </summary>
        /// <param name="NewName">New name of the file</param>
        /// <param name="OldName">Old file name</param>
        public void SftpRename(String NewName, String OldName)
        {
            Thread.Sleep(5000);
            _objSftpClient.Rename(NewName, OldName);
        }
    }
}

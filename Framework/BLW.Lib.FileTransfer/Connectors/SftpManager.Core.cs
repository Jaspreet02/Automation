using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.FtpDetail;
using WeOnlyDo.Client;

namespace BLW.Lib.TransferManager.Connectors
{
    public partial class SftpManager
    {
        /****************************************************************************************************
           Delegate Events Call
        * **************************************************************************************************
        * CmcsSftpObj_ConnectedEvent => Called on Every Connection
        * CmcsSftpObj_DoneEvent => Called When event is Complete
        * CmcsSftpObj_StateChangedEvent => Called When the state of connection is Changed
        * CmcsSftpObj_DisconnectedEvent => Called When the state of connection is closed
        * CmcsSftpObj_AttributesDataEvent => Called when Attributes from a Directory are loaded
        * CmcsSftpObj_ProgressEvent => Called when file operations (upload/download) occurs on connection
        * CmcsSftpObj_LoopItemEvent => Called when multiple uploads or downloads occurs on connection
       ****************************************************************************************************/
        private void ConnectedEvent(object Sender, SFTP.ConnectedArgs Args)
        {
            if (Args.Error != null)
            {
                Console.WriteLine(Args.Error.Message.ToString());
            }

        }

        private void DoneEvent(object Sender, SFTP.DoneArgs Args)
        {
            Console.WriteLine(Args.ToString());
        }


        private void StateChangedEvent(object Sender, SFTP.StateChangedArgs Args)
        {
            Console.WriteLine(Args.NewState.ToString());
        }


        private void DisconnectedEvent(object Sender, SFTP.ConnectedArgs Args)
        {
            if (Args.Error != null)
            {
                Console.WriteLine(Args.ToString());
            }
        }

        private void AttributesDataEvent(object Sender, SFTP.AttributesArgs[] Args)
        {
            int i = 0;
            int folderCount = 0;
            int fileCount = 0;
            int lowerB = Args.GetLowerBound(i);
            int upperB = Args.GetUpperBound(i);



            List<IFtpFileInfo> fileInfoList = new List<IFtpFileInfo>();
            for (i = lowerB; i <= upperB; i++)
            {
                if ((Args[i].Name != ".") && (Args[i].Name != ".."))
                {
                    Console.WriteLine("Name: " + Args[i].Name);

                    if (Args[i].Permissions <= long.Parse("16895"))
                    {
                        Console.WriteLine("Type: Folder");
                        folderCount++;
                    }
                    else
                    {
                        Console.WriteLine("Type: File");
                        Console.WriteLine("Size: " + Args[i].Size);
                        var objFtpFile = new FtpFileInfo();

                        objFtpFile.Name = Args[i].Name;
                        objFtpFile.FullName = RemotePathLocation.TrimEnd('\\', '/') + "/" + Args[i].Name;
                        if (String.IsNullOrEmpty(objFtpFile.FullName))
                            throw new Exception("RemotePathLocation does not exists in datbase ");
                        objFtpFile.AccessTime = Convert.ToDateTime(Args[i].AccessTime);
                        objFtpFile.Permissions = Args[i].Permissions;
                        objFtpFile.ModificationTime = Convert.ToDateTime(Args[i].ModificationTime);
                        objFtpFile.GroupId = Args[i].Gid;
                        objFtpFile.UserId = Args[i].Uid;
                        objFtpFile.Size = Args[i].Size;

                        fileInfoList.Add(objFtpFile);
                        fileCount++;
                    }
                    Console.WriteLine(" ");
                }
            }
            CurrentSubDirectoryFiles = fileInfoList;
        }

        private void ProgressEvent(object Sender, SFTP.ProgressArgs Args)
        {
            double result;
            result = ((double)Args.Position / Args.Length) * 100;
            Console.WriteLine("Progress :: " + Math.Round(result, 2) + " %");
        }

        private void LoopItemEvent(object Sender, SFTP.LoopArgs Args)
        {
            if (Args.ItemType == WeOnlyDo.Client.SFTP.DirItemTypes.Directory)
            {
                Args.Skip = false;
            }
            else
            {
                if (TransactionType == "Download")
                {
                    if (Args.RemoteFile.EndsWith(FileType))
                    {
                        Args.Skip = false;
                    }
                    else
                    {
                        Args.Skip = true;
                    }
                }
                if (TransactionType == "Upload")
                {
                    if (Args.LocalFile.EndsWith(FileType))
                    {
                        Args.Skip = false;
                    }
                    else
                    {
                        Args.Skip = true;
                    }
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.TransferManager.Setting;
using BLW.Lib.TransferManager.TransferType;

namespace BLW.Lib.TransferManager
{
    public class FileTransferAdapter
    {
        static IAdaperSetting _settings;

        public static IFileTransferAdapter GetFileTransferAdapter(IAdaperSetting settings)
        {
            IFileTransferAdapter _adaptor;
            _settings = settings;

            QueueTypes type = (QueueTypes)Enum.Parse(typeof(QueueTypes), _settings.Type.Trim(), true);
            switch (type)
            {
                case QueueTypes.SFTP:
                    _adaptor = new SFTPFileTransferAdapter(settings);
                    break;
                case QueueTypes.FTP:
                    _adaptor = new FTPFileTransferAdapter(settings);
                    break;
                case QueueTypes.SharedPath:
                    _adaptor = new LocalFileTransferAdapter(settings);
                    break;
                case QueueTypes.LocalFileSystem:
                    _adaptor = new LocalFileTransferAdapter(settings);
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} Transfer type not supported.", settings.Type));
            }
            return _adaptor;
        }
    }
}

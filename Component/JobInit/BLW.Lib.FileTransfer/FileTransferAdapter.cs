using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileTransfer
{
    public class FileTransferAdapter
    {
        static IAdaperSettings _settings;
        public IAdaperSettings Settings { get { return _settings; } }
        public static IFileTransferAdapter GetFileTransferAdapter(IAdaperSettings settings)
        {
            IFileTransferAdapter _adaptor;
            _settings = settings;

            QuequeType type = (QuequeType)Enum.Parse(typeof(QuequeType), _settings.Type.Trim(), true);
            switch (type)
            {
                case QuequeType.SFTP:
                    _adaptor = new SFTPFileTransferAdapter(settings);
                    break;
                case QuequeType.FTP:
                    _adaptor = new FTPFileTransferAdapter(settings);
                    break;
                case QuequeType.SharedPath:
                    _adaptor = new LocalFileTransferAdapter(settings); 
                    break;
                case QuequeType.LocalFileSystem:
                    _adaptor = new LocalFileTransferAdapter(settings);
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} Transfer type not supported.", settings.Type));
            }
            return _adaptor;
        }
    }
    /// <summary>
    /// Define queque types
    /// </summary>
    public enum QuequeType
    {
        SFTP =1,
        FTP=2,
        SharedPath=3,
        LocalFileSystem=4
    }
}

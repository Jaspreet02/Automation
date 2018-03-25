using BLW.Lib.Log;
using BLW.Lib.TransferManager;
using BLW.Lib.TransferManager.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uploader
{
    class TransferManager
    {

        #region Variables

        private IFileTransferAdapter TransferAdapter;

        #endregion

        #region Constructor

        public TransferManager(IAdaperSetting setting)
        {
            this.TransferAdapter = FileTransferAdapter.GetFileTransferAdapter(setting);
        }

        #endregion

        #region Methods

        public bool Processing(List<string> InputFile)
        {
            try
            {
                foreach (var item in InputFile)
                {
                    SingletonLogger.Instance.Debug("Process start to upload file = " + item);
                    TransferAdapter.UploadFile(item, Path.GetFileName(item).Trim());
                    SingletonLogger.Instance.Debug(item + " file has been uploaded.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.FileTransfer;

namespace BLW.Lib.FileValidation
{
    /// <summary>
    /// Default validation type is based on file name
    /// <para>file name can be based on a regular expression or an extension</para>
    /// </summary>
    public class DefaultValidation : IValidationPlugin
    {
        private FileTransferManager transferManager;
        List<string> _allFiles;
        List<string> _validFiles;
        List<string> _invalidFiles;
        private List<string> validations;
        bool _readyToDownload = false;
        public DefaultValidation(FileTransferManager transferManager, List<string> validations)
        {
            this.transferManager = transferManager;
            this.validations = validations;
        }

        /// <summary>
        /// Validation files, then preserve a list of all, valid and invalid files
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            try
            {
                List<string> tempFiles = new List<string>();
                if (this.transferManager == null)
                    throw new Exception("Transfer manager found null.");                              

                //Get files from provided masks
                _validFiles = new List<string>();
                if (validations.Count > 0)
                {
                    foreach (var mask in validations)
                    {
                        _validFiles.AddRange(transferManager.GetFileList(mask));
                    }
                }
                else
                {
                    _allFiles = transferManager.GetFileList(null);
                    _validFiles.AddRange(_allFiles);
                }

                //Just for safe side if system have picked same file with multiple masks e.g (test-pdf.txt, test.pdf)- Support we have to pick files with pdf keyword in name
                if (_validFiles.Count > 0)
                    _validFiles = _validFiles.Distinct().ToList();
                 _readyToDownload = true;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in validate() in defaultValidation : " + ex);
            }
        }

        protected List<string> NormalizeFiles(List<string> files)
        {
            //If files are not initialize yet change to zero
            if (files == null)
                files = new List<string>();
            return files;
        }

        public List<string> AllFiles
        {
            get { return NormalizeFiles(_allFiles); }
        }

        public List<string> ValidFiles
        {
            get { return NormalizeFiles(_validFiles); }
        }

        public List<string> InvalidFiles
        {
            get { return NormalizeFiles(_invalidFiles); }
        }

        public bool DownloadAllFiles()
        {
            try
            {
                foreach (string file in AllFiles)
                {
                    transferManager.DownloadFile(file);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error to download all files in default validation Adptor"+ex);
            }
        }

        public bool DownloadValidFiles()
        {
            try
            {
                foreach (string file in ValidFiles)
                {
                    transferManager.DownloadFile(file);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error to download valid files in default validation Adptor" + ex);
            }
        }

        public bool Ready
        {
            get { return _readyToDownload; }
        }
    }
}

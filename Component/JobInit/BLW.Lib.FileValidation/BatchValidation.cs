using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLW.Lib.FileTransfer;
using System.IO;
using BLW.Lib.Log;

namespace BLW.Lib.FileValidation
{
    /// <summary>
    /// Default validation type is based on file name
    /// <para>file name can be based on a regular expression or an extension</para>
    /// </summary>
    public class BatchValidation : IValidationPlugin
    {
        private FileTransferManager transferManager;
        List<string> _allFiles;
        List<string> _validFiles;
        List<string> _invalidFiles;
        bool _readyToDownload;
        private List<ValidationFileInfo> validations;

        public BatchValidation(FileTransferManager transferManager, List<ValidationFileInfo> validations)
        {
            this.transferManager = transferManager;
            this.validations = validations;
            this._readyToDownload = false;
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
                if(this.transferManager == null)
                    throw new Exception("Transfer manager found null.");                              

                //Get files from provided masks
                _validFiles = new List<string>();
                if(validations.Count > 0)
                {
                    var IsRequiredMasks = validations.Where(x => x.Required == true).Select(x => x.Mask).ToList();
                    foreach(var validation in IsRequiredMasks)
                    {
                        SingletonLogger.Instance.Debug("Required file mask - " + validation);
                        var files = transferManager.GetFileList(validation);
                        if (files.Count > 0)
                        {
                            SingletonLogger.Instance.Debug("");
                            _validFiles.AddRange(files);
                        }
                        else
                        {
                            Console.WriteLine("No files found for mask " + validation + " But it is required ");
                            return false;
                        }
                    }

                    var masksHavingIsNotRequired = validations.Where(x => x.Required == false).Select(x => x.Mask).ToList();
                    foreach(var validation in masksHavingIsNotRequired)
                    {
                        var files = transferManager.GetFileList(validation);
                        if(files.Count > 0)
                            _validFiles.AddRange(files);
                        else
                            Console.WriteLine("No files found for mask " + validation);
                    }


                    //var outputCount = masksHavingIsRequired.Where(e => validFilesName.Any(d => d.Contains(e))).Count();
                    if(_validFiles.Count > 0)
                        _readyToDownload = true;
                }
                else
                {
                    _allFiles = transferManager.GetFileList(null);
                    _validFiles.AddRange(_allFiles);
                    _readyToDownload = true;
                }

                //Just for safe side if system have picked same file with multiple masks e.g (test-pdf.txt, test.pdf)- Support we have to pick files with pdf keyword in name
                if (_validFiles.Count > 0)
                    _validFiles = _validFiles.Distinct().ToList();
                _readyToDownload = true;
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error in validate() in BatchValidation " + ex);
            }
        }
        protected List<string> NormalizeFiles(List<string> files)
        {
            //If files are not initailize yet change to zero
            if(files == null)
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
                foreach(string file in AllFiles)
                {
                    transferManager.DownloadFile(file);
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error to download all files in Batch validation Adptor" + ex);
            }
        }

        public bool DownloadValidFiles()
        {
            try
            {
                foreach(string file in ValidFiles)
                {
                    transferManager.DownloadFile(file);
                }
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error to download valid files in Batch validation Adptor" + ex);
            }
        }

        public bool Ready
        {
            get { return _readyToDownload; }
        }
    }
}

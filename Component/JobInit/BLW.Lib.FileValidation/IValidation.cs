using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileValidation
{
    public interface IValidationPlugin
    {
        /// <summary>
        /// Validate files
        /// </summary>
        /// <returns></returns>
        bool Validate();
        /// <summary>
        /// Download all files
        /// </summary>
        /// <returns></returns>
        bool DownloadAllFiles();
        /// <summary>
        /// Download valid files
        /// </summary>
        /// <returns></returns>
        bool DownloadValidFiles();
        /// <summary>
        /// Return list of all files found at source location
        /// </summary>
        List<string> AllFiles { get; }
        /// <summary>
        /// Return list of valid files found at source location
        /// </summary>
        List<string> ValidFiles { get; }
        /// <summary>
        /// Get list of invalid files found at source location
        /// </summary>
        List<string> InvalidFiles { get; }
        /// <summary>
        /// Is ready to download
        /// </summary>
        bool Ready { get; }
    }
}

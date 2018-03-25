using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.TransferManager.FtpDetail
{
    public interface IFtpFileInfo
    {
        /// <summary>
        /// FIle Name
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Full File Name
        /// </summary>
        String FullName { get; set; }

        /// <summary>
        /// Access Time
        /// </summary>
        DateTime AccessTime { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        long GroupId { get; set; }

        /// <summary>
        /// Modification Time
        /// </summary>
        DateTime ModificationTime { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        long Permissions { get; set; }

        /// <summary>
        /// File Size
        /// </summary>
        long Size { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        long UserId { get; set; }
    }
}

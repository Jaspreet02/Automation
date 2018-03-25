using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileTransfer
{
    class FtpFileInfo : IFtpFileInfo
    {
        /// <summary>
        /// File Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public String FullName { get; set; }

        /// <summary>
        /// Access Time
        /// </summary>
        public DateTime AccessTime { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupId { get; set; }

        /// <summary>
        /// Modification Time
        /// </summary>
        public DateTime ModificationTime { get; set; }

        /// <summary>
        /// Permissions
        /// </summary>
        public long Permissions { get; set; }

        /// <summary>
        /// File Size
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public long UserId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace BLW.Lib.File.PinnacleFiles.X937
{
   /// <summary>
    /// Record types
    /// </summary>
    public enum RecordTypes : int
    {
        FileHeader = 01,
        CashLetterHeader = 10,
        BundleHeader = 20,
        CheckDetail = 25,
        CheckDetailAddendumA = 26,
        CheckDetailAddendumC = 28,
        ImageViewDetail = 50,
        ImageViewData = 52,
        BundleControl = 70,
        CashLetterControl = 90,
        FileControl = 99
    }
}

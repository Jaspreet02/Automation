using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLW.Lib.FileTransfer
{
   public class FileTransferExpt : Exception
    {
        public FileTransferExpt(String message)
        {
            //code here for 
        }

        public FileTransferExpt(String message, Exception InnerExp)
        {
            //code here for 
        }
    }

   public class GetFileListExpt : Exception
   {
       public GetFileListExpt(String message)
       {
           //code here for 
       }

       public GetFileListExpt(String message, Exception InnerExp)
       {
           //code here for 
       }
   }

   public class DeleteFileExp : Exception
   {
       public DeleteFileExp(String message)
       {
           //code here for 
       }

       public DeleteFileExp(String message, Exception InnerExp)
       {
           //code here for 
       }
   }

   public class RemoveDirectoryExp : Exception
   {
       public RemoveDirectoryExp(String message)
       {
           //code here for 
       }

       public RemoveDirectoryExp(String message, Exception InnerExp)
       {
           //code here for 
       }
   }

   public class SftpConnectionExp : Exception
   {
       public SftpConnectionExp(String message)
       {
           //code here for 
       }

       public SftpConnectionExp(String message, Exception InnerExp)
       {
           //code here for 
       }
   }

   public class FileUploadExp : Exception
   {
       public FileUploadExp(String message)
       {
           //code here for 
       }

       public FileUploadExp(String message, Exception InnerExp)
       {
           //code here for 
       }
   }

}

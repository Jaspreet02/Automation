using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace BLW.Lib.CoreUtility
{
    public class FileOperation
    {
        static bool isLocked = false;

        /// <summary>
        /// Method:MoveFile
        /// Description:Move or copy Input file to output location ,if file already exists then rename the existing file.
        /// </summary>
        /// <param name="inputfile"></param>
        /// <param name="outputPath"></param>
        /// <param name="isCopy"></param>
        public void MoveFile(string inputfile, string outputPath, bool isCopy = false)
        {
            try
            {
                if(!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                //rename already exists file.
                if(File.Exists(outputPath + "\\" + Path.GetFileName(inputfile)))
                {


                    var todayDate = String.Format("{0}_{1}_{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    var renameFile = outputPath + "\\" + todayDate + "_" + Path.GetFileName(inputfile);
                    if(File.Exists(renameFile))
                    {
                        Random randNumber = new Random();
                        var random = randNumber.Next(9999);
                        renameFile = outputPath + "\\" + todayDate + "_" + random + "_" + Path.GetFileName(inputfile);
                    }
                    File.Move(outputPath + "\\" + Path.GetFileName(inputfile), renameFile);
                }

                if(isCopy)
                    File.Copy(inputfile, outputPath + "\\" + Path.GetFileName(inputfile));
                else
                    File.Move(inputfile, outputPath + "\\" + Path.GetFileName(inputfile));

            }

            catch(Exception)
            {

                throw;
            }

        }

        public void RenameDirectory(string diretoryPath, string RenameDirName = "")
        {
            try
            {
                string rootDir = Path.GetDirectoryName(diretoryPath);
                if(RenameDirName == "")
                {
                    Random randNumber = new Random();
                    var random = randNumber.Next(9999);
                    RenameDirName = random.ToString() + "_" + Path.GetFileName(diretoryPath);
                }

                var RenameDirpath = rootDir + "\\" + RenameDirName;
                if(!Directory.Exists(RenameDirpath))
                    Directory.CreateDirectory(RenameDirpath);

                var dirFiles = Directory.GetFiles(diretoryPath);
                foreach(var file in dirFiles)
                {
                    MoveFile(file, RenameDirpath);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }
                

        /// <summary>
        /// Check if file is locked or not
        /// 1. Check file exists or not        
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileLocked(string fileName)
        {
            //File not exists
            if(!File.Exists(fileName))
                throw new FileNotFoundException(fileName);
            //if exists then 
            try
            {
                FileInfo info = new FileInfo(fileName);
                return IsFileLocked(info);
            }
            catch(Exception)
            {
                return true;
            }
        }

        public static bool IsFileLocked(FileInfo file)
        {
            isLocked = false;
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch(IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                isLocked =  true;
            }
            finally
            {
                if(stream != null)
                    stream.Close();
            }                       
            return isLocked;
        }  
        
        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        public static bool FileCompare(string file1, string file2)
        {
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if(file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if(fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Close the files.
            fs1.Close();
            fs2.Close();
            return true;
        }
    }
}

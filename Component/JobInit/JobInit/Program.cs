using System;
using System.Linq;
using BLW.Lib.Log;
using DbHander;

/***********************************************************************
 * Date : 29/09/2014
  * Description : It will Initializ the Job.
 * And Download the fill from the given FTP location.
 * Updated :  09/12/2014 : Ravi
 * Description : It will Initialize the Job by start download the file from the given FTP/SFTP/Local/Shared location 
************************************************************************/
namespace JobInit
{
    class Program
    {
        /// <summary>
        /// Entry point of application
        /// </summary>
        /// <param name="args">Command line arguments({Session Key} and {AppId})</param>
        static void Main(string[] args)
        {
            //Declare global variables
            String sessionKey = String.Empty;
            int appId = -1;
            //start application 
            try
            {
                Console.WriteLine("======Job initialization Process started======");

                #region Validate Command Arguments
                //Validating arguments
                if (args.Count() == 0)
                    throw new ArgumentOutOfRangeException("No argument supplied for run Job initialization");
                if (String.IsNullOrEmpty(args[0]))
                    throw new ArgumentOutOfRangeException("Session key not supplied");
                else
                    sessionKey = Convert.ToString(args[0]);

                if (String.IsNullOrEmpty(args[1]))
                    throw new ArgumentOutOfRangeException("Application ID not supplied");
                else
                    appId = Convert.ToInt32(args[1]);
                #endregion

                LogInitializer.InitializeLogger("JobInitialization_"+appId);
                SingletonLogger.Instance.Info("Assembly Version Infomration : " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                SingletonLogger.Instance.Debug("Connecting to WCF.");
                JobInitHelper helper = new JobInitHelper();
                SingletonLogger.Instance.Debug("Connected.");

                var runNos = helper.Processing(sessionKey, appId);
                if (runNos.Count() != 0)
                {
                    foreach (var runNo in runNos)
                    {
                        SingletonLogger.Instance.Debug("Start preprocessing for run number " + runNo.RunNumber + " and outputPath" + runNo.Output);
                        Main processRunNo = new Main();
                        int status = processRunNo.Run(runNo.RunNumber, runNo.Output);
                        if (status == 0)
                            SingletonLogger.Instance.Debug("Completed preprocessing for run number " + runNo.RunNumber + " and outputPath" + runNo.Output);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error " + e.ToString());               
                //If errors, then do nothing (just exit)           
                try
                {
                    if (!string.IsNullOrEmpty(sessionKey))
                    {
                        using (IProcSessionsRepository repository = new ProcSessionsRepository())
                        {
                            repository.UpdateBySessionKey(sessionKey, Convert.ToByte(JobStatusType.Error));
                        }
                    }
                }
                catch (Exception ex)
                {
                    /*Error in update session key*/
                    SingletonLogger.Instance.Error("Error in update session key" + ex.ToString());
                }
                SingletonLogger.Instance.Error("Error in Job Initializer" + e.ToString());
              //  SingletonLogger.Instance.Error(e.ToString());                
            }
        }
    }
}

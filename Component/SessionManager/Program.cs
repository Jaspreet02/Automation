using System;
using System.Diagnostics;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;
using DbHander;

/*******************************************************************
 * Date : 29/09/2014
 * ---Removed
 * Date : 12/12/2014
 * Deccription : It will update running jobs table according to the
 * status of the instance launcher.
********************************************************************/
namespace ProcSessionManager
{
    /// <summary>
    /// Main class of the application
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point of the application
        /// </summary>
        /// <param name="args">Command Line arguments</param>
        static void Main(string[] args)
        {
            var systemGuid = string.Empty;
            try
            {
                #region Validate Command Arguments

                if (args.Length == 0)
                    throw new ArgumentOutOfRangeException("No argument supplied for run ProcSession manager");

                if (String.IsNullOrEmpty(args[0]))
                    throw new ArgumentOutOfRangeException("System Guid not supplied");
                else
                    systemGuid = args[0];


                #endregion

                try
                {
                    LogInitializer.InitializeLogger("ProcSessionManager");
                    SingletonLogger.Instance.Debug(" Application ProcSession Manager Component starts.");
                    SingletonLogger.Instance.Debug(" System generated GUID = " + systemGuid);

                    IProcSessionsRepository sessionRepository = new ProcSessionsRepository();

                    SingletonLogger.Instance.Debug("Getting the list of the Unused process");
                    // get list of Unused process from ProcSessions table whose killrequired= true or status=3 or status=2 etc.
                    var unUsedProcessList = sessionRepository.GetAllUnusedProcess();

                    if (unUsedProcessList != null)
                    {
                        
                        foreach (var unusedProcSession in unUsedProcessList)
                        {
                            // validating unusedProcSession 
                            if (unusedProcSession != null)
                            {
                                // kill the Process if killrequired = true or expected datetime is < current datetime
                                if (unusedProcSession.KillRequired)
                                {
                                    //Check that no component is running for it

                                    int processId = 0;
                                    // parsing the processId
                                    if (!int.TryParse(unusedProcSession.ProcessID, out processId))
                                        throw new Exception("ProcessId is not valid" + processId);

                                    Process process = Process.GetProcessById(processId);
                                    if (process == null)
                                        SingletonLogger.Instance.Error("No process is running with id{0}" + processId);
                                    else
                                        try
                                        {
                                            SingletonLogger.Instance.Debug("Start killing the ProcessId = " + processId);
                                            //Killing the process for particular ProcessId
                                            process.Kill();
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("Error while killing the process for processId" + processId, ex);
                                        }
                                }
                            }
                            //update the status of the RunningJob table
                            byte jobstatus = 0;
                            if (unusedProcSession.KillRequired)
                                jobstatus = (byte)JobStatusType.Status_Killed;
                            if (unusedProcSession.ExpectedDateTime < DateTime.Now)
                                jobstatus = (byte)JobStatusType.Status_TimeExp;
                            if (unusedProcSession.ProcStatus == (Int32)SessionStatusType.Status_Error)
                                jobstatus = (byte)JobStatusType.Error;
                            if (unusedProcSession.ProcStatus == (Int32)SessionStatusType.Status_Complete)
                                jobstatus = (byte)JobStatusType.Complete;
                            SingletonLogger.Instance.Debug("Deleting the entry from procsession table for ProcsessionId =" + unusedProcSession.ProcSessionId);
                            //Delete ProcSession entry from ProcSessions table
                            sessionRepository.Delete(unusedProcSession.ProcSessionId);
                            SingletonLogger.Instance.Debug("Proc session has been deleted due to status = " + jobstatus);
                        }
                        SingletonLogger.Instance.Debug("ProcSessionManager Component has been Completed");
                    }
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error("Error occur in ProcSession component Error detail : " + ex.Message);
                }
                finally
                {
                    #region Deleting Entry from Transaction Table
                    try
                    {
                        // SQLITE Database Initialization
                        Transaction trans = Transaction.Get(systemGuid);
                        if (Transaction.Delete(systemGuid))
                        {
                            if (trans != null)
                                Licence.UpdateLastRunEndTime(trans.ExeName, DateTime.Now);
                            Console.WriteLine("Successfully removed column. GUID = " + systemGuid);
                        }
                        else
                        {
                            Console.WriteLine("Error while removing column. GUID = " + systemGuid);
                        }
                    }
                    catch (Exception ex2)
                    {
                        SingletonLogger.Instance.Debug("Error while removing column. GUID = " + ex2.ToString());
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                //If errors, then do nothing (just exit)          
                SingletonLogger.Instance.Fatal(ex.ToString());
            }

        }
    }
}

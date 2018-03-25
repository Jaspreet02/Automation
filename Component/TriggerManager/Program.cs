using System;
using System.Collections.Generic;
using System.Linq;
using BLW.Lib.Log;
using BLW.Lib.SqliteHelper.Tables;
using BLW.Lib.CoreUtility.Exceptions;
using DbHander;

namespace TriggerManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var systemGuid = string.Empty;
            try
            {

                LogInitializer.InitializeLogger("Trigger Manager");

                SingletonLogger.Instance.Debug("Trigger Manager Component starts.");

                #region Validate Command Arguments
                if (args.Length == 0)
                    throw new ArgumentOutOfRangeException("No argument supplied for run Trigger manager");

                if (String.IsNullOrEmpty(args[0]))
                    throw new ArgumentOutOfRangeException("System Guid not supplied");
                else
                    systemGuid = Convert.ToString(args[0]);
                #endregion

                try
                {
                    SingletonLogger.Instance.Debug("System generated GUID = " + systemGuid);

                    IRunDetailsRepository _runRepository = new RunDetailsRepository();

                    // Getting Active RunNumber From RunDetails Table. Get only those RunNumber those have "RunStatus=1"
                    var runNumberDetailList = _runRepository.GetRunDetailByStatus((int)RunNumberStatusType.Ready);

                    if(runNumberDetailList.Count() == 0)
                    {
                        SingletonLogger.Instance.Debug("No run number found for trigger generation.");
                        return;
                    }

                    SingletonLogger.Instance.Debug("Total " + runNumberDetailList.Count() + " run number found for trigger generation.");

                    TriggerFileGenerator objTriggerFileGenerator = new TriggerFileGenerator();

                    foreach(var runNumber in runNumberDetailList)
                    {

                        // If any component for this run number is already running or error out 
                        // then no need to created the trigger file and continue for other run number trigger file creation.
                        bool status = objTriggerFileGenerator.CheckForTriggerFileCreation(runNumber);

                        if(!status)
                            continue;

                        // Get total trigger file to be generated for run number
                        SingletonLogger.Instance.Debug("Process has been started for creating trigger file for RunNumber = " + runNumber.RunNumber);
                        List<TriggerFileGenerator> TriggerFileGeneratorList = objTriggerFileGenerator.GetTriggerFileGeneratorList(runNumber);

                        if (TriggerFileGeneratorList.Count() > 0)
                        {
                            try
                            {
                                objTriggerFileGenerator.CreateTriggerFile(TriggerFileGeneratorList);
                            }
                            catch (IOBLWException iEx)
                            {
                                SingletonLogger.Instance.Warning(iEx.ToString());
                                continue;
                            }
                            catch (Exception)
                            {
                                throw;
                            }                            
                        }
                        else
                            SingletonLogger.Instance.Debug("No Component Found For Trigger file Generation for RunNumber = " + runNumber.RunNumber);

                        SingletonLogger.Instance.Debug("All trigger file for RunNumber = " + runNumber.RunNumber + " has been created.");

                        //Update RunStatus of RunNumber to 2   
                        using (IRunDetailsRepository repository = new RunDetailsRepository())
                        {
                            repository.UpdateRunStatusByRunNumberId(runNumber.RunDetailId, (int)RunNumberStatusType.Running);
                        }                   
                        SingletonLogger.Instance.Debug("RunStatus of RunNumber in table RunDetails has been Updated to 2.");
                    }
                }
                catch(Exception ex)
                {
                    SingletonLogger.Instance.Error("Error occur while generating trigger for RunNumber. Error detail : " + ex.GetBaseException().StackTrace);
                }
                finally
                {
                    #region Deleting Entry from Transaction Table
                    try
                    {

                        // SQLITE Database Initialization
                        Transaction trans = Transaction.Get(systemGuid);
                        if(Transaction.Delete(systemGuid))
                        {
                            if(trans != null)
                                Licence.UpdateLastRunEndTime(trans.ExeName, DateTime.Now);
                            SingletonLogger.Instance.Debug("Successfully removed column. GUID = " + systemGuid);
                        }
                        else
                        {
                            SingletonLogger.Instance.Debug("Error while removing column. GUID = " + systemGuid);
                        }
                    }
                    catch(Exception ex2)
                    {
                        SingletonLogger.Instance.Debug("Error while removing column. GUID = " + ex2.ToString());
                    }
                    #endregion
                }
                SingletonLogger.Instance.Debug("Trigger Generation process has been Completed.");
            }
            catch(Exception ex)
            {
                //If errors, then do nothing (just exit)          
                SingletonLogger.Instance.Fatal(ex.ToString());
            }
        }
    }
}

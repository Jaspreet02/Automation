using System;
using BLW.Lib.SqliteHelper;
using ConsoleApplication1;
using BLW.Lib.Log;
using System.Timers;
using System.Configuration;

namespace WSExeInvoker
{
    class Program
    {
        static void Main(string[] args)
        {
            LogInitializer.InitializeLogger("BLW.WindowServiceInvokation");
            try
            {                          
                // Invoke class instance
                Factory getInstance = new Factory();
                SingletonLogger.Instance.Error(ConfigurationManager.AppSettings["DbFile"]);
                try
                {                   
                    // Class Trigger based
                    var tiggerBased = getInstance.GetInstance("TRIGGERBASE");
                    tiggerBased.Processing();
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error(ex.GetBaseException().ToString());
                }

                // Call Time based
                try
                {
                    var timeBased = getInstance.GetInstance("TIMEBASE");
                    timeBased.Processing();
                }
                catch (Exception ex)
                {
                    SingletonLogger.Instance.Error(ex.GetBaseException().ToString());
                }
            }
            catch (Exception ex)
            {
                SingletonLogger.Instance.Error(ex.Message, ex.GetBaseException());
            }
        }

    }
}

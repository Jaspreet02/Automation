using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using System.Configuration;

namespace MyNewService
{
    public class Service1 : System.ServiceProcess.ServiceBase
    {
        Timer timer = new Timer();
        private System.Diagnostics.EventLog eventLog1;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public Service1()
        {
            // This call is required by the Windows.Forms Component Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitComponent call
            if(!System.Diagnostics.EventLog.SourceExists("DoDyLogSourse"))
            {
                System.Diagnostics.EventLog.CreateEventSource("DoDyLogSourse", "DoDyLog");
            }

            eventLog1.Source = "DoDyLogSourse";
            eventLog1.Log = "DoDyLog";

        }

        // The main entry point for the process
        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new System.ServiceProcess.ServiceBase[] { new MyNewService.Service1() };

            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            // 
            // eventLog1
            // 
            this.eventLog1.Log = "DoDyLog";
            this.eventLog1.Source = "DoDyLogSource";
            // 
            // Service1
            // 
            this.ServiceName = "MyNewService";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            eventLog1.WriteEntry("my service started");

            //handle Elapsed event
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            //This statement is used to set interval to 1 minute (= 60,000 milliseconds)
            int interval = 0;
            try
            {
                int.TryParse(System.Configuration.ConfigurationManager.AppSettings["Interval"], out interval);
            }
            catch(Exception) { interval = 60; }
            interval = interval * 1000;
            timer.Interval = interval;

            //enabling the timer
            timer.Enabled = true;
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            eventLog1.WriteEntry("my service stoped");
            timer.Enabled = false;
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("my service is continuing in working");
            TraceService("Another entry at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            TraceService("Another entry at " + DateTime.Now);
        }
        private void TraceService(string content)
        {
            try
            {
                //set up a filestream
                FileStream fs = new FileStream(string.Format("ScheduledService{0}.log", DateTime.Now.ToString("ddMMyyyy")), FileMode.OpenOrCreate, FileAccess.Write);

                //set up a streamwriter for adding text
                StreamWriter sw = new StreamWriter(fs);

                //find the end of the underlying filestream
                sw.BaseStream.Seek(0, SeekOrigin.End);

                //add the text
                sw.WriteLine(content);
                //add the text to the underlying filestream


                try
                {
                    var serviceExe = ConfigurationManager.AppSettings["ServiceExe"];
                    if(!string.IsNullOrEmpty(serviceExe))
                    {
                        System.Diagnostics.Process proces = new System.Diagnostics.Process();
                        proces.StartInfo.FileName = serviceExe;
                        proces.Start();
                        sw.WriteLine("Process Id : " + proces.Id);
                    }
                }
                catch(Exception ex) { sw.WriteLine(ex.ToString()); }


                sw.Flush();
                //close the writer
                sw.Close();
            }
            catch(Exception) { }
        }
    }
}

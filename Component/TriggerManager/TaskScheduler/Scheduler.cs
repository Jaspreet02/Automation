using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.TaskScheduler;

namespace TriggerManager.TaskScheduler
{
    class Scheduler
    {
        public void CreateTaskInTaskScheduler(string taskName, string action, int span, string arguments = "", string taskDescription = "")
        {
            try
            {
                // Get the service on the local machine
                using (TaskService taskServiceObj = new TaskService())        //"", @"Client-15-pc", "CLIENT-15", ""
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = taskServiceObj.NewTask();

                    td.RegistrationInfo.Description = taskDescription;

                    // Create a trigger that will fire the task at this time every other day
                    td.Triggers.Add(new DailyTrigger { StartBoundary = DateTime.Now.AddMinutes(span) }); //  DaysInterval = 1, // Add time interval StartBoundary = minutes

                    // Create an action that will launch Notepad whenever the trigger fires
                    td.Actions.Add(new ExecAction(action, arguments, null));

                    // Register the task in the root folder
                    taskServiceObj.RootFolder.RegisterTaskDefinition(taskName, td);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        
    }
}

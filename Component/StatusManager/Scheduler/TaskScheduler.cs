using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32.TaskScheduler;

namespace ApplicationStatusManager.Scheduler
{
    class TaskScheduler
    {
        /// <summary>
        /// Validate Trigger in Window Task scheduler
        /// </summary>
        /// <param name="taskName">Task Name</param>
        /// <returns></returns>
        private Task GetTaskSchedulerByName(string taskName)
        {
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                return ts.FindTask(taskName);
            }
        }

        /// <summary>
        /// Delete Task from Window
        /// </summary>
        /// <param name="taskName">Task Name</param>
        public void RemoveTaskFromTaskScheduler(string taskName)
        {
            var taskDetail = GetTaskSchedulerByName(taskName);
            if (taskDetail == null)
                throw new Exception("Error occur while process deleting Task from window scheduler. Task names = " + taskName);

            using (TaskService ts = new TaskService())
            {
                try
                {
                    // Create a new task definition and assign properties
                    TaskDefinition td = ts.NewTask();
                    // Remove the task we just created
                    ts.RootFolder.DeleteTask(taskName);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error occur while process deleting Task from window scheduler", ex);
                }
                
            }
        }
    }
}

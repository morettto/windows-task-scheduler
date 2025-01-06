using Microsoft.Win32.TaskScheduler;
using System.Diagnostics;
using System.Management;

namespace WindowsTaskScheduler
{
    #region public methods
    public class TaskSchedulerService
    {

        /// <param name="exePath">path to execute</param>
        /// <param name="rpaWorkingDirectory">working directory</param>
        /// <param name="taskName">task name</param>
        /// <param name="startBoundary">time to execute</param>
        /// <param name="arguments">arguments to run in cli</param>
        public void CreateTaskSchedulerAsCurrentWindowsUser(string exePath, string rpaWorkingDirectory, string taskName, DateTime startBoundary, string arguments = null)
        {
            var userId = GetWindowsLoggedUserNameWithLocalMachine();

            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();

                td.RegistrationInfo.Description = $"Task to execute. {taskName}";
                td.Actions.Add(new ExecAction(exePath, arguments, rpaWorkingDirectory));
                td.Triggers.Add(new TimeTrigger() { StartBoundary = startBoundary, EndBoundary = DateTime.Now.AddSeconds(20) });
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Settings.AllowHardTerminate = false;
                td.Settings.DeleteExpiredTaskAfter = TimeSpan.FromSeconds(1);
                td.Principal.RunLevel = TaskRunLevel.Highest;
                ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.Create, userId, null, TaskLogonType.InteractiveToken);
            }
        }

        /// <param name="taskToRun">name of a registered task</param>
        public void TaskRun(string taskToRun)
        {
            using (TaskService ts = new TaskService())
            {
                Microsoft.Win32.TaskScheduler.Task registeredTask = ts.GetTask(taskToRun);

                if (registeredTask != null)
                    registeredTask.Run();
            }
        }

        #endregion

        private string GetWindowsLoggedUserNameWithLocalMachine()
        {
            string userId = string.Empty;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem"))
            using (ManagementObjectCollection results = searcher.Get())
            {
                foreach (ManagementObject result in results)
                {
                    userId = result["UserName"]?.ToString();
                }
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = GetWindowsLoggedUserNameWithVirtualMachine();
            }

            return userId;
        }

        static string GetWindowsLoggedUserNameWithVirtualMachine()
        {
            string userId = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_Process WHERE Name = 'explorer.Exe'");
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject process in processList)
            {
                string[] ownerInfo = new string[2];
                process.InvokeMethod("GetOwner", (object[])ownerInfo);

                if (Convert.ToInt32(process["SessionId"]) == Process.GetCurrentProcess().SessionId)
                {
                    userId = ownerInfo[0];
                    break;
                }
            }
            return userId;
        }
    }
}

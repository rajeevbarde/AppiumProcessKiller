using System;
using System.Diagnostics;
using System.Management;

namespace AppiumProcessKiller
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isFound = false;

            foreach (Process PPath in Process.GetProcessesByName("node"))
            {                
                int processId = PPath.Id;
                string processArgs = "";

                var q = string.Format("select CommandLine from Win32_Process where ProcessId='{0}'", processId);
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(q);
                ManagementObjectCollection result = searcher.Get();

                foreach (ManagementObject obj in result)
                {                    
                    processArgs = obj["CommandLine"].ToString();
                    if (processArgs.ToLower().Contains("appium"))
                    {
                        isFound = true;
                        PPath.Kill();
                        Console.WriteLine("Killed process id {0} : {1}", processId, processArgs);
                    }     
                }                
            }

            if (!isFound)
                Console.WriteLine("No process found for appium");            
        }
    }
}

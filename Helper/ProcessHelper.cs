using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSCodeBuildBox.Helper
{
    public class ProcessHelper
    {
        public static string RunProcess(string filename, params string[] parameters)
        {
            var p = new System.Diagnostics.Process();
            p.StartInfo = new System.Diagnostics.ProcessStartInfo(filename, string.Join(" ", parameters));
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            string logMessage = "";
            while (!p.HasExited)
            {
                string line = p.StandardOutput.ReadToEnd();
                Console.WriteLine(line);
                logMessage += line;
            }
            return logMessage;
        }
    }
}

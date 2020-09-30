using Microsoft.AspNetCore.Mvc;
using RSCodeBuildBox.Service;
using System;
using System.IO;
using System.Linq;

namespace RSCodeBuildBox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HookController : ControllerBase
    {
        public readonly IRSGitService _gitService;
        public HookController(IRSGitService rSGitService)
        {
            _gitService = rSGitService;
        }

        [HttpGet]
        public string Hook()
        {
            Console.WriteLine("Cloning Repo...");
            var success = _gitService.GitCloneRepo((path) =>
            {
                Console.WriteLine("Complated Cloning Repo !");
            });
            if (success)
            {
                Console.WriteLine("Building Project...");
                return BuildProject();
            }
            else
                return "Have a some error...";
        }

        private string BuildProject()
        {

            var clone_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RSCodeBuildBox", "temp");
            string strCmdText = $"build {clone_path}";
            return RunProcess("dotnet", strCmdText); ;
        }

        public string RunProcess(string filename,params string[] parameters)
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
            p.Kill();
            return logMessage;
        }
    }
}

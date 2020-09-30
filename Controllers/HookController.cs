using Microsoft.AspNetCore.Mvc;
using RSCodeBuildBox.Service;
using System;

namespace RSCodeBuildBox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HookController : ControllerBase
    {
        public readonly IRSGitService _gitService;
        public readonly IRSDotnetService _dotnetService;
        public HookController(IRSGitService rSGitService, IRSDotnetService dotnetService)
        {
            _gitService = rSGitService;
            _dotnetService = dotnetService;
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
                string build_output = _dotnetService.BuildProject();
                if (build_output.Contains("success") || build_output.Contains("başarılı"))
                {
                    //Creating publish...
                    success = _dotnetService.PublishProject();
                    if(success == false)
                        return "-- Have a some error on publishing !";
                }
            }
            else
                return "-- Have a some error on cloning repo !";

            return "*** Publishing Success ***";
        }
    }
}

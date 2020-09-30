using RSCodeBuildBox.Helper;
using System;
using System.IO;
using System.IO.Compression;

namespace RSCodeBuildBox.Service
{
    public interface IRSDotnetService
    {
        bool PublishProject();
        string BuildProject();
    }

    public class RSDotnetService : IRSDotnetService
    {
        public bool PublishProject()
        {
            try
            {
                ProcessHelper.RunProcess("dotnet", new[] { "publish", Utils.Utilities.Temp_Repo_Path });
                IOHelper.CleanOrInitDirectory(Utils.Utilities.Copy_Path);
                var publish_path = Path.Combine(Utils.Utilities.Temp_Repo_Path, "bin", "Debug", "netcoreapp3.1", "publish");
                var target_zip_path = Path.Combine(Utils.Utilities.Copy_Path, "temp.zip");
                ZipFile.CreateFromDirectory(publish_path, target_zip_path);
                ZipFile.ExtractToDirectory(target_zip_path, Utils.Utilities.Copy_Path);
                File.Delete(target_zip_path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return true;
        }

        public string BuildProject()
        {
            var clone_path = Utils.Utilities.Temp_Repo_Path;
            string strCmdText = $"build {clone_path}";
            return ProcessHelper.RunProcess("dotnet", strCmdText);
        }
    }
}

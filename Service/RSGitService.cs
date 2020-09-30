using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using RSCodeBuildBox.Models;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;

namespace RSCodeBuildBox.Service
{
    public interface IRSGitService
    {
        bool GitCloneRepo(Action<string> clone_complated);
    }

    public class RSGitService : IRSGitService
    {
        public readonly RSHookConfig _config;
        public RSGitService(IConfiguration conf)
        {
            var section = conf.GetSection(nameof(RSHookConfig));
            if (section != null)
                _config = section.Get<RSHookConfig>();
        }
        public string RunProcess(string filename, params string[] parameters)
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

        public bool GitCloneRepo(Action<string> clone_complated)
        {
            if (_config == null)
                return false;
            var git_repo = _config.Github;
            var clone_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "RSCodeBuildBox", "temp");
            if (Directory.Exists(clone_path) == false)
            {
                Directory.CreateDirectory(clone_path);
            }
            else
            {

            }
            {
                try
                {
                    string[] projectDirectories = Directory.GetFileSystemEntries(clone_path, "", SearchOption.AllDirectories);
                    foreach (var projectDirectory in projectDirectories)
                    {
                        File.SetAttributes(projectDirectory, FileAttributes.Normal);
                    }
                    Directory.Delete(clone_path, true);
                    Directory.CreateDirectory(clone_path);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex);
                    //return false;
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }

            var co = new CloneOptions();
            co.RepositoryOperationCompleted = new LibGit2Sharp.Handlers.RepositoryOperationCompleted((r) =>
            {
                Console.WriteLine(r.RemoteUrl);
                clone_complated?.Invoke(r.RemoteUrl);
            });
            co.CredentialsProvider = (_url, _user, _cred) =>
            {
                return new UsernamePasswordCredentials
                {
                    Username = git_repo.Access_Token,
                    Password = string.Empty
                };
            };
            co.Checkout = true;
            var path = LibGit2Sharp.Repository.Clone(git_repo.Target_Repo, clone_path, co);
            var repo = new LibGit2Sharp.Repository(path);
            repo.Reset(ResetMode.Hard);
            repo.Dispose();
            return true;
        }
    }
}

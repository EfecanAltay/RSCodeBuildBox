using LibGit2Sharp;
using RSCodeBuildBox.Helper;
using RSCodeBuildBox.Models;
using System;

namespace RSCodeBuildBox.Service
{
    public interface IRSGitService
    {
        bool GitCloneRepo(Action<string> clone_complated);
    }

    public class RSGitService : IRSGitService
    {
        public readonly GithubConfig _config;
        public RSGitService(GithubConfig config)
        {
            _config = config;
        }

        public bool GitCloneRepo(Action<string> clone_complated)
        {
            if (_config == null)
                return false;
            var clone_path = Utils.Utilities.Temp_Repo_Path;
            
            var success = IOHelper.CleanOrInitDirectory(clone_path);
            if (success == false)
                return false;

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
                    Username = _config.Access_Token,
                    Password = string.Empty
                };
            };
            Repository.Clone(_config.Target_Repo, clone_path, co);
            return true;
        }
    }
}
namespace RSCodeBuildBox.Models
{
    public class RSHookConfig
    {
        public string Path { get; set; }
        public GithubConfig GithubConfig { get; set; }
        public string Copy_Path { get; set; }
    }

    public class GithubConfig
    {
        public string Access_Token { get; set; }
        public string Clone_Path { get; set; }
        public string Target_Repo { get; set; }
    }
}

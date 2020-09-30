namespace RSCodeBuildBox.Models
{
    public class RSHookConfig
    {
        public string Path { get; set; }
        public Github Github { get; set; }
    }

    public class Github
    {
        public string Access_Token { get; set; }
        public string Clone_Path { get; set; }
        public string Target_Repo { get; set; }
    }
}

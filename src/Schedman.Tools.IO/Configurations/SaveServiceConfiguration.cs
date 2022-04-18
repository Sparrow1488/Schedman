namespace Schedman.Tools.IO.Configurations
{
    public class SaveServiceConfiguration
    {
        public SaveServiceConfiguration(
            string directoryName, 
            string rootPath = "./")
        {
            DirectoryName = directoryName;
            Root = rootPath;
        }

        public string DirectoryName { get; }
        public string Root { get; }
    }
}
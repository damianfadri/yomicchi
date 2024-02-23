namespace Yomicchi.Core
{
    public class Source
    {
        public string Title { get; }
        public string Revision { get; }
        public string Filepath { get; }

        public Source(string filepath, string title = "", string revision = "")
        {
            Title = title;
            Revision = revision;
            Filepath = filepath;
        }
    }
}

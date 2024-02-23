namespace Yomicchi.Core
{
    public static class FileHelpers
    {
        public static string? GetMatchingFile(string directory, string pattern)
        {
            return GetMatchingFiles(directory, pattern).FirstOrDefault();
        }
        public static IEnumerable<string> GetMatchingFiles(string directory, string pattern)
        {
            return Directory.EnumerateFiles(directory, pattern, SearchOption.TopDirectoryOnly);
        }

    }
}

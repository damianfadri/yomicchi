namespace Yomicchi.Core
{
    public partial class Tag
    {
        private static readonly Dictionary<string, string> Colors =
            new()
            {
                { "default", "#8a8a91" },
                { "name", "#b6327a" },
                { "expression", "#f0ad4e" },
                { "popular", "#0275d8" },
                { "frequent", "#5bc0de" },
                { "archaism", "#d9534f" },
                { "dictionary", "#aa66cc" },
                { "frequency", "#5cb85c" },
                { "partOfSpeech", "#8a8a91" },
                { "search", "#8a8a91" },
                { "pronunciationDictionary", "#6640be" },
            };
    }
}

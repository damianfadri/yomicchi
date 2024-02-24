namespace Yomicchi.Core
{
    public class Definition
    {
        public Tag Source { get; }
        public IEnumerable<Tag> PartsOfSpeech { get; }
        public IEnumerable<string> Definitions { get; }

        public Definition(Tag source, IEnumerable<Tag> partsOfSpeech, IEnumerable<string> definitions)
        {
            Source = source;
            PartsOfSpeech = partsOfSpeech.OrderBy(tag => tag.Order);
            Definitions = definitions;
        }
    }
}

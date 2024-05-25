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

        public override bool Equals(object? obj)
        {
            if (obj is not Definition that)
            {
                return false;
            }

            return Definitions.Count() == that.Definitions.Count()
                && Definitions.Intersect(that.Definitions).Count() == that.Definitions.Count();
        }

        public override int GetHashCode()
        {
            return Definitions.Count();
        }
    }
}

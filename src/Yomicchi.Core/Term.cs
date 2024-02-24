using System.Text;
using System.Text.RegularExpressions;

namespace Yomicchi.Core
{
    public class Term
    {
        private static readonly int[] HiraganaConversionRange = [0x3041, 0x3096];
        private static readonly int[] KatakanaConversionRange = [0x30a1, 0x30f6];

        public string Text { get; }
        public string Reading { get; }
        public int PopularityScore { get; }
        public IEnumerable<Segment> Expression { get; }
        public List<Definition> Definitions { get; private set; }
        public IEnumerable<Tag>? Tags { get; private set; }
        public IEnumerable<string>? Inflections { get; private set; }

        public Term(string text, string reading, int popularityScore = 0)
        {
            Text = text;
            Reading = reading;
            Expression = GetSegments(text, reading);

            PopularityScore = popularityScore;
            Definitions = new List<Definition>();
        }
        public void AddDefinition(Definition definition)
        {
            Definitions.Add(definition);
        }

        public void SetTags(IEnumerable<Tag> tags)
        {
            Tags = tags.OrderBy(tag => tag.Order);
        }
        public void SetInflections(IEnumerable<string> inflections)
        {
            Inflections = inflections;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not Term other)
            {
                return false;
            }

            return Text.Equals(other.Text) && Reading.Equals(other.Reading);
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode() * Reading.GetHashCode();
        }

        private List<Segment> GetSegments(string expression, string reading)
        {
            if (IsKana(expression))
            {
                return [new Segment(expression)];
            }

            var pattern = new StringBuilder();
            var expressionSegment = Segment(expression).ToList();

            foreach (string segment in expressionSegment)
            {
                pattern.Append("(");
                if (IsKana(segment))
                {
                    pattern.Append(segment);
                }
                else
                {
                    pattern.Append(".+");
                }

                pattern.Append(")");
            }

            var matches = Regex.Match(reading, pattern.ToString());
            if (!matches.Success)
            {
                return [];
            }

            if (expressionSegment.Count != matches.Groups.Count - 1)
            {
                return [];
            }

            var segments = new List<Segment>();
            for (int i = 1; i < matches.Groups.Count; i++)
            {
                var group = matches.Groups[i];

                string kanjiSegment = expressionSegment[i - 1];
                string readingSegment = group.Value;

                if (kanjiSegment.Equals(readingSegment))
                {
                    segments.Add(new Segment(kanjiSegment));
                }
                else
                {
                    segments.Add(new Segment(kanjiSegment, readingSegment));
                }
            }

            return segments;
        }

        public static bool IsKana(string expression)
        {
            foreach (var letter in expression)
                if (!IsKana(letter))
                    return false;

            return true;
        }

        public static bool IsKana(char character)
        {
            var codePoint = character;
            return IsCodePointInRange(codePoint, HiraganaConversionRange[0], HiraganaConversionRange[1])
                    || IsCodePointInRange(codePoint, KatakanaConversionRange[0], KatakanaConversionRange[1]);
        }

        private static bool IsCodePointInRange(int codePoint, int min, int max)
        {
            return codePoint >= min && codePoint <= max;
        }

        public static IEnumerable<string> Segment(string expression)
        {
            bool? isPrevKana = null;
            var currentSegment = new StringBuilder();

            foreach (var exprChar in expression)
            {
                var isKana = IsKana(exprChar);
                if (isPrevKana != null && !isKana.Equals(isPrevKana))
                {
                    yield return currentSegment.ToString();
                    currentSegment = new StringBuilder();
                }

                currentSegment.Append(exprChar);
                isPrevKana = isKana;
            }

            yield return currentSegment.ToString();
        }
    }
}

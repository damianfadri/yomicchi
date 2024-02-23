using System.Text;

namespace Yomicchi.Core
{
    public class Normalizer
    {
        private static readonly int[] HiraganaConversionRange = [0x3041, 0x3096];
        private static readonly int[] KatakanaConversionRange = [0x30a1, 0x30f6];

        public string OriginalText { get; }
        public string NormalizedText { get; }

        public Normalizer(string original)
        {
            OriginalText = original;
            NormalizedText = ConvertKatakanaToHiragana(original);
        }

        private static bool IsCodePointInRange(int codePoint, int min, int max)
        {
            return codePoint >= min && codePoint <= max;
        }

        public static string ConvertKatakanaToHiragana(string text)
        {
            var result = new StringBuilder();

            var offset = HiraganaConversionRange[0] - KatakanaConversionRange[0];
            foreach (var letter in text)
            {
                var codePoint = letter;
                if (IsCodePointInRange(codePoint, KatakanaConversionRange[0], KatakanaConversionRange[1]))
                {
                    result.Append((char)(codePoint + offset));
                }
                else
                {
                    result.Append(letter);
                }
            }

            return result.ToString();
        }
    }
}

namespace Yomicchi.Core
{
    public class RuleVariant
    {
        public string KanaIn { get; set; }
        public string KanaOut { get; set; }
        public RuleType RulesIn { get; set; }
        public RuleType RulesOut { get; set; }

        public RuleVariant(string kanaIn, string kanaOut, RuleType rulesIn, RuleType rulesOut)
        {
            KanaIn = kanaIn;
            KanaOut = kanaOut;
            RulesIn = rulesIn;
            RulesOut = rulesOut;
        }
    }
}

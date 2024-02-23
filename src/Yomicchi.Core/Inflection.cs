namespace Yomicchi.Core
{
    public class Inflection
    {
        public string Text { get; set; }
        public RuleType Rules { get; set; }
        public string[] Inflections { get; set; }

        public Inflection(string term)
        {
            Text = term;
            Rules = RuleType.None;
            Inflections = Array.Empty<string>();
        }

        public Inflection(string term, RuleType ruleType, string[] inflections)
        {
            Text = term;
            Rules = ruleType;
            Inflections = inflections;
        }
    }
}

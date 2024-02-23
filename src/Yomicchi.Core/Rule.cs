namespace Yomicchi.Core
{
    public class Rule
    {
        public string Name { get; }
        public List<RuleVariant> Variants { get; }

        public Rule(string name, List<RuleVariant> variants)
        {
            Name = name;
            Variants = variants;
        }
    }
}

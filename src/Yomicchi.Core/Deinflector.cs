namespace Yomicchi.Core
{
    public partial class Deinflector
    {
        private string _text;

        public Deinflector(string text)
        {
            _text = text;
        }

        public IEnumerable<Inflection> Deinflect()
        {
            var inflections = new List<Inflection>
            {
                new Inflection(_text),
            };

            for (int i = 0; i < inflections.Count; i++)
            {
                var current = inflections[i];
                foreach (var rule in Rules)
                {
                    foreach (var variant in rule.Variants)
                    {
                        if ((current.Rules != RuleType.None 
                                && ((current.Rules & variant.RulesIn) == RuleType.None))
                                || (current.Text.Length - variant.KanaIn.Length + variant.KanaOut.Length) <= 0
                                || !current.Text.EndsWith(variant.KanaIn))
                        {
                            continue;
                        }

                        var updatedTerm = current.Text.Substring(
                                0, current.Text.Length - variant.KanaIn.Length) + variant.KanaOut;

                        string[] updatedReasons = current.Inflections != null
                                ? [rule.Name, ..current.Inflections]
                                : [rule.Name];

                        inflections.Add(new Inflection(updatedTerm, variant.RulesOut, updatedReasons));
                    }
                }
            }

            return inflections;
        }
    }
}

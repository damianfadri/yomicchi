using System.Collections.Concurrent;
using System.Diagnostics;

namespace Yomicchi.Core.Services
{
    public class Glossary
    {
        private readonly ConcurrentDictionary<string, List<Term>> _glossary;

        public Glossary()
        {
            _glossary = new ConcurrentDictionary<string, List<Term>>();
        }

        public void Load(IEnumerable<Term> terms)
        {
            Parallel.ForEach(terms, term =>
            {
                var kanji = new Normalizer(term.Text);
                var reading = new Normalizer(term.Reading);

                InsertOrAddDefinitions(kanji.NormalizedText, term);
                InsertOrAddDefinitions(reading.NormalizedText, term);
            });
        }

        public IEnumerable<Term> Lookup(string keyword)
        {
            Trace.WriteLine($"Glossary::Lookup({keyword})");
            var search = new Normalizer(keyword);

            return _glossary.GetValueOrDefault(search.NormalizedText) ?? [];
        }

        private void InsertOrAddDefinitions(string key, Term term)
        {
            var existingTerms = _glossary.GetValueOrDefault(key) ?? [];
            if (existingTerms.Count == 0)
            {
                _glossary.TryAdd(key, [term]);
                return;
            }

            var foundTerm = existingTerms.FirstOrDefault(existingTerm => existingTerm.Equals(term));
            if (foundTerm == null)
            {
                _glossary.TryAdd(key, [term]);
                return;
            }

            if (foundTerm == term)
            {
                return;
            }

            foreach (var definition in term.Definitions)
            {
                foundTerm.AddDefinition(definition);
            }
        }
    }
}

using System.Collections.Concurrent;
using System.Diagnostics;

namespace Yomicchi.Core.Services
{
    public class Glossary
    {
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Term, Term>> _glossary;

        public Glossary()
        {
            _glossary = new ConcurrentDictionary<string, ConcurrentDictionary<Term, Term>>();
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

            return _glossary.GetValueOrDefault(search.NormalizedText)?.Values ?? [];
        }

        private void InsertOrAddDefinitions(string key, Term term)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var existingTerms = _glossary.GetValueOrDefault(key) ?? [];
            if (existingTerms.Count == 0)
            {
                var content = new ConcurrentDictionary<Term, Term>();
                content.TryAdd(term, term);

                _glossary.TryAdd(key, content);
                return;
            }

            if (!existingTerms.TryGetValue(term, out var foundTerm))
            {
                existingTerms.TryAdd(term, term);
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

using System.Collections.Concurrent;
using System.Diagnostics;
using Yomicchi.Core.Interfaces;

namespace Yomicchi.Core.Services
{
    public class Glossary
    {
        private readonly Trie<Term> _glossary;

        public Glossary()
        {
            _glossary = new Trie<Term>();
        }

        //public async Task LoadAsync(string directory)
        //{
        //    var indexFile = FileHelpers.GetMatchingFile(directory, "index.json");
        //    if (indexFile == null)
        //    {
        //        throw new InvalidOperationException("index.json cannot be found.");
        //    }

        //    var termBank = await _indexLoader.LoadAsync(indexFile);
        //    var dictionaryTag = new Tag(termBank.Title, "dictionary");

        //    var terms = FileHelpers.GetMatchingFiles(directory, @"term_bank_*.json")
        //        .Select(termFile => _termLoader.LoadAsync(termFile));

        //    var result = await Task.WhenAll(terms);

        //    var dictionaryCache = new ConcurrentDictionary<Term, Term>();

        //    Parallel.ForEach(result.SelectMany(terms => terms), term =>
        //    {
        //        term.AddDictionaryTag(dictionaryTag);

        //        dictionaryCache.AddOrUpdate(term, term, (key, oldTerm) =>
        //        {
        //            oldTerm.Merge(term);
        //            return oldTerm;
        //        });
        //    });

        //    foreach (var term in dictionaryCache.Values)
        //    {
        //        var kanji = new Normalizer(term.Text);
        //        var reading = new Normalizer(term.Reading);

        //        _glossary.Insert(kanji.NormalizedText, term);
        //        _glossary.Insert(reading.NormalizedText, term);
        //    }

        //    Trace.WriteLine("Loading done.");
        //}

        public void Load(IEnumerable<Term> terms)
        {
            var dictionaryCache = new ConcurrentDictionary<Term, Term>();
            Parallel.ForEach(terms, term =>
            {
                dictionaryCache.AddOrUpdate(term, term, (key, oldTerm) =>
                {
                    foreach (var definition in term.Definitions)
                    {
                        oldTerm.Definitions.Add(definition);
                    }

                    return oldTerm;
                });
            });

            foreach (var term in dictionaryCache.Values)
            {
                var kanji = new Normalizer(term.Text);
                var reading = new Normalizer(term.Reading);

                _glossary.Insert(kanji.NormalizedText, term);
                _glossary.Insert(reading.NormalizedText, term);
            }
        }
        public IEnumerable<Term> Lookup(string keyword)
        {
            Trace.WriteLine($"Glossary::Lookup({keyword})");
            var search = new Normalizer(keyword);

            return _glossary.GetAll(search.NormalizedText);
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Yomicchi.Core.Events;
using Yomicchi.Core.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Yomicchi.Core.ViewModels
{
    public class SearchResultsViewModel : ObservableObject, IRecipient<TextDetectedEvent>
    {
        private readonly Glossary _glossary;
        private ObservableCollection<Term> _terms;

        public ObservableCollection<Term> Terms
        {
            get => _terms;
            set => SetProperty(ref _terms, value);
        }

        public SearchResultsViewModel(Glossary glossary)
        {
            _glossary = glossary;
            _terms = new ObservableCollection<Term>();

            WeakReferenceMessenger.Default.Register(this);
        }

        public void Receive(TextDetectedEvent message)
        {
            Trace.WriteLine($"SearchResultsViewModel::Receive({{ {message.Text}, {message.X}, {message.Y}, {message.Width}, {message.Height} }})");

            var terms = SearchTerms(message.Text)
                .OrderByDescending(term => term.Inflections?.Count() ?? 0)
                .ThenByDescending(term => term.Text.Length)
                .ThenByDescending(term => term.PopularityScore)
                .Distinct();

            Terms.Clear();

            foreach (var term in terms)
            {
                Terms.Add(term);
            }

            if (Terms.Count == 0)
            {
                return;
            }
        }

        private IEnumerable<Term> SearchTerms(string text)
        {
            var normalizer = new Normalizer(text);
            var original = normalizer.NormalizedText;

            for (int i = 1; i <= original.Length; i++)
            {
                var current = original.Substring(0, i);
                var inflections = new Deinflector(current).Deinflect();
                foreach (var validInflection in GetValidInflections(inflections))
                {
                    yield return validInflection;
                }
            }
        }

        private IEnumerable<Term> GetValidInflections(IEnumerable<Inflection> inflections)
        {
            foreach (var inflection in inflections)
            {
                var validTerms = _glossary.Lookup(inflection.Text);
                foreach (var validTerm in validTerms)
                {
                    Trace.WriteLine($"ValidTerm: {validTerm.Text} {validTerm.Reading} {string.Join(">", inflection.Inflections)}");
                    validTerm.SetInflections(inflection.Inflections);
                    yield return validTerm;
                }
            }
        }
    }
}

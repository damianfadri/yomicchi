using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Yomicchi.Core.Events;
using Yomicchi.Core.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Yomicchi.Core.ViewModels
{
    public class MainViewModel : ObservableObject, IRecipient<TextDetectedEvent>
    {
        private readonly Glossary _glossary;

        private ObservableCollection<Source> _sources;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ObservableCollection<Source> Sources
        {
            get => _sources;
            set => SetProperty(ref _sources, value);
        }

        public MainViewModel(Glossary glossary)
        {
            _glossary = glossary;
            _sources = new ObservableCollection<Source>();
            _isLoading = false;

            WeakReferenceMessenger.Default.Register(this);
        }

        public void LoadTerms(IEnumerable<Term> terms)
        {
            Trace.WriteLine($"MainViewModel::LoadTerms({terms.Count()} terms)");
            _glossary.Load(terms);
        }

        public void AddSource(Source source)
        {
            Sources.Add(source);
        }

        public void SaveConfiguration()
        {
            throw new NotImplementedException();
        }

        public void Receive(TextDetectedEvent message)
        {
            
        }
    }
}

using CommunityToolkit.Mvvm.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using Yomicchi.Core;
using Yomicchi.Core.ViewModels;
using Yomicchi.Desktop.Services;

namespace Yomicchi.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<MainViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Left = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - Width;
            Top = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - Height;

            LoadState();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveState();
            Application.Current.Shutdown();
        }

        private void OnSourceDrop(object sender, DragEventArgs e)
        {
            var filepaths = ((DataObject)e.Data)
                .GetFileDropList()
                .Cast<string>()
                .Select(filepath => new Source(filepath));

            LoadSources(filepaths);
        }

        private void LoadSources(IEnumerable<Source> sources)
        {
            Task.Run(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    _viewModel.IsLoading = true;
                });

                var termLoader = new TermV3Loader();
                var indexLoader = new IndexLoader();

                var terms = new List<IEnumerable<Term>>();
                var termSources = new List<Source>();

                foreach (var source in sources)
                {
                    if (source == null)
                    {
                        continue;
                    }

                    var currSource = indexLoader.Load(source.Filepath);
                    if (currSource == null)
                    {
                        continue;
                    }

                    var currTerms = termLoader.Load(source.Filepath);

                    terms.Add(currTerms);
                    termSources.Add(currSource);
                }

                _viewModel.LoadTerms(terms.SelectMany(term => term));

                App.Current.Dispatcher.Invoke(() =>
                {
                    _viewModel.IsLoading = false;
                    foreach (var source in termSources)
                    {
                        _viewModel.AddSource(source);
                    }
                });
            });
        }

        private void SaveState()
        {
            var state = new State
            {
                Sources = _viewModel.Sources.ToList(),
            };

            state.Save();
        }

        private void LoadState()
        {
            var state = new State();

            state.Load();

            LoadSources(state.Sources ?? []);
        }
    }
}
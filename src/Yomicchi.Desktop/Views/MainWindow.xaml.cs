using System.Collections.Concurrent;
using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
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

            var corner = SystemParameters.WorkArea.BottomRight;

            Left = corner.X - Width;
            Top = corner.Y - Height;

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

                var termSources = new ConcurrentBag<Source>();

                Parallel.ForEach(sources, source =>
                {
                    if (source == null)
                    {
                        return;
                    }

                    var currSource = indexLoader.Load(source.Filepath);
                    if (currSource == null)
                    {
                        return;
                    }

                    _viewModel.LoadTerms(
                        termLoader.Load(source.Filepath));

                    termSources.Add(currSource);
                });

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
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Yomicchi.Core.Events;
using Yomicchi.Core.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Yomicchi.Desktop.Views
{
    /// <summary>
    /// Interaction logic for SearchResultsWindow.xaml
    /// </summary>
    public partial class SearchResultsWindow : Window, IRecipient<TextDetectedEvent>
    {
        private readonly SearchResultsViewModel _viewModel;

        public SearchResultsWindow()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<SearchResultsViewModel>();
            DataContext = _viewModel;

            WeakReferenceMessenger.Default.Register(this);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;

            base.OnClosing(e);
        }

        public void Receive(TextDetectedEvent message)
        {
            var right = SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth;
            var bottom = SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight;

            var leftThreshold = right - Width;
            var topThreshold = bottom - Height;

            Left = Math.Min(leftThreshold, message.X + message.Width);
            Top = Math.Min(topThreshold, message.Y);

            Visibility = Visibility.Visible;
        }
    }
}

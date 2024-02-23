using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using Yomicchi.Core.ViewModels;
using Yomicchi.Desktop.Models;

namespace Yomicchi.Desktop.Views
{
    /// <summary>
    /// Interaction logic for SnippingWindow.xaml
    /// </summary>
    public partial class SnippingWindow : Window
    {
        private readonly SnippingViewModel _viewModel;

        public SnippingWindow()
        {
            InitializeComponent();

            _viewModel = Ioc.Default.GetRequiredService<SnippingViewModel>();
            DataContext = _viewModel;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Left = SystemParameters.VirtualScreenLeft;
            Top = SystemParameters.VirtualScreenTop;
            Width = SystemParameters.VirtualScreenWidth;
            Height = SystemParameters.VirtualScreenHeight;
        }

        private void OnSnipped(object sender, SnippedEventArgs e)
        {
            _viewModel.SearchRegionForText(e.X, e.Y, e.Width, e.Height);
        }
        private void OnHoverModeChanged(object sender, ModeChangedEventArgs e)
        {
            if (e.Enabled)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Hidden;
            }
        }
    }
}

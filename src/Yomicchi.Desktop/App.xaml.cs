using CommunityToolkit.Mvvm.DependencyInjection;
using Yomicchi.Core;
using Yomicchi.Core.Interfaces;
using Yomicchi.Core.Services;
using Yomicchi.Core.ViewModels;
using Yomicchi.Desktop.Services;
using Yomicchi.Desktop.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Reflection;

namespace Yomicchi.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly MainWindow _main;
        private readonly SnippingWindow _snipping;
        private readonly SearchResultsWindow _search;

        public App()
        {
            InitializeComponent();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<MainViewModel>()
                    .AddSingleton<SnippingViewModel>()
                    .AddSingleton<SearchResultsViewModel>()
                    .AddSingleton<Glossary>()
                    .AddTransient<IScreenshot, Win32Screenshot>()
                    .AddTransient<ITextReader, AzureTextReader>()
                    .Configure<AzureOptions>(
                        configuration.GetSection(nameof(AzureOptions)))
                    .BuildServiceProvider());

            _main = new MainWindow();
            _snipping = new SnippingWindow();
            _search = new SearchResultsWindow();
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _main.Show();
            _snipping.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            _main?.Close();
            _snipping?.Close();
            _search?.Close();
        }
    }

}

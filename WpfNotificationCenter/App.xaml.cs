using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Wpf.NotificationCenter.Extensions;

namespace WpfNotificationCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        ///     Configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; set; }

        /// <summary>
        /// The underlying <see cref="IServiceProvider"/> used by PowerStigUI.
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// The underlying <see cref="Dispatcher"/> used by PowerStigUI.
        /// </summary>
        public new static Dispatcher Dispatcher => Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

        public App()
        {

            Dispatcher.UnhandledException += OnDispatcherUnhandledException;

            //Load configurations
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());

            Configuration = builder?.Build() ?? throw new InvalidDataException("Unable to build configuration.");

            //Configure services and dependency injection, then build service provider.
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            LaunchMainWindow();
        }

        private static void ConfigureServices(IServiceCollection services) =>
            services.UseWpfNotificationCenter()
                .AddSingleton<MainWindowViewModel>();

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //Log.Error(errorMessage, e.Exception);

            e.Handled = true;
        }

        private void LaunchMainWindow()
        {
            var mainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
            };

            mainWindow.Show();
        }
    }}

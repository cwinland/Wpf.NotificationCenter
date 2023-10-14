using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Input;
using Wpf.NotificationCenter;
using Wpf.NotificationCenter.Services;

namespace WpfNotificationCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        #region Overrides of FrameworkElement

        public static ICommand CreateNotificationCommand => new RelayCommand<NotificationType>((t) =>
            {
                var notificationService = (Application.Current as App).ServiceProvider.GetRequiredService<IWpfNotificationService>();
                notificationService.Create($"{t} {DateTime.Now.ToLongTimeString()}", $"{t}\n{DateTime.Now}", t);
            }
        );

        public static ICommand CreateNotification2Command => new RelayCommand<NotificationType>((t) =>
            {
                var notificationService = (Application.Current as App).ServiceProvider.GetRequiredService<IWpfNotificationService>();
                notificationService.Create($"{t} {DateTime.Now.ToLongTimeString()}", $"{t}\n{DateTime.Now}", t, "NotificationCenter2");
            }
        );
        #endregion
    }
}

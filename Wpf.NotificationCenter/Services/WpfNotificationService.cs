using System.Collections.Generic;
using System.Windows;
using Wpf.NotificationCenter.Extensions;

namespace Wpf.NotificationCenter.Services
{
    public class WpfNotificationService : IWpfNotificationService
    {
        private NotificationCenter notificationCenter { get; }

        public WpfNotificationService()
        {
            notificationCenter = Application.Current.MainWindow.FindChild<NotificationCenter>(null, "");
        }

        public Notification Create(string title, string text, NotificationType type = NotificationType.Information, string? notificationCenterName = null)
        {
            var notification = new Notification {Title = title, Text = text, NotificationType = type};

            var center = notificationCenterName == null ? notificationCenter : Application.Current.MainWindow.FindChild<NotificationCenter>(childName: notificationCenterName) ?? throw new KeyNotFoundException($"{notificationCenterName} not found.");
            center.CreateNotification(notification);

            return notification;
        }
    }

    public interface IWpfNotificationService
    {
        Notification Create(string title, string text, NotificationType type = NotificationType.Information, string? notificationCenterName = null);
    }
}

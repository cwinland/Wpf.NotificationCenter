using System.Windows;
using Wpf.NotificationCenter.Enums;
using Wpf.NotificationCenter.Extensions;

namespace Wpf.NotificationCenter.Services
{
    /// <summary>
    ///     Class WpfNotificationService.
    ///     Implements the <see cref="Wpf.NotificationCenter.Services.IWpfNotificationService" />
    /// </summary>
    /// <inheritdoc />
    /// <seealso cref="Wpf.NotificationCenter.Services.IWpfNotificationService" />
    public class WpfNotificationService : IWpfNotificationService
    {
        private static NotificationCenter GetNotificationCenter(string? notificationCenterName = null) =>
            Application.Current?.MainWindow?.FindChild<NotificationCenter>(notificationCenterName) ??
            throw NotFound(notificationCenterName);

        private static KeyNotFoundException NotFound(string? name = null) => new($"{name ?? "Notification Center"} not found.");

        #region IWpfNotificationService

        /// <inheritdoc />
        public void Close(Notification.Notification notification, string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).RemoveNotification(notification);


        /// <inheritdoc />
        public Notification.Notification Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All)
        {
            var notification = new Notification.Notification {Title = title, Text = text, NotificationType = notificationType};
            var center = GetNotificationCenter(notificationCenterName);

            if (alertType != AlertType.NotificationPopup)
            {
                center.CreateNotificationAlert(notification);
            }

            if (alertType != AlertType.NotificationCenter)
            {
                center.CreateNotificationPopup(notification);
            }

            return notification;
        }

        /// <inheritdoc />
        public Notification.Notification
            Error(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All) =>
            Create(title, text, NotificationType.Error, notificationCenterName, alertType);

        /// <inheritdoc />
        public Notification.Notification Information(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Information, notificationCenterName, alertType);

        /// <inheritdoc />
        public Notification.Notification Success(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Success, notificationCenterName, alertType);

        /// <inheritdoc />
        public Notification.Notification Warning(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Warning, notificationCenterName, alertType);

        #endregion
    }
}

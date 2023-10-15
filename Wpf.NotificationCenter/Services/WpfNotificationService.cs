using System.Collections.Generic;
using System.Windows;
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
        /// <summary>
        ///     Initializes a new instance of the <see cref="WpfNotificationService" /> class.
        /// </summary>
        public WpfNotificationService() { }

        /// <summary>
        ///     Closes the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        public void Close(Notification.Notification notification, string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).RemoveNotification(notification);

        private static NotificationCenter GetNotificationCenter(string? notificationCenterName = null) =>
            Application.Current?.MainWindow?.FindChild<NotificationCenter>(notificationCenterName) ??
            throw NotFound(notificationCenterName);

        private static KeyNotFoundException NotFound(string? name = null) => new($"{name ?? "Notification Center"} not found.");

        #region IWpfNotificationService

        /// <inheritdoc />
        /// <summary>
        ///     Creates the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType"></param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType"></param>
        /// <returns>Creates.</returns>
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

        #endregion
    }
}

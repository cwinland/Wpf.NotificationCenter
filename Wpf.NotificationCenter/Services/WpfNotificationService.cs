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
        #region Fields

        private readonly NotificationCenter notificationCenter;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="WpfNotificationService" /> class.
        /// </summary>
        public WpfNotificationService() => notificationCenter = GetNotificationCenter();

        /// <summary>
        ///     Closes the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        public void Close(Notification notification, string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).RemoveNotification(notification);

        private NotificationCenter GetNotificationCenter(string? notificationCenterName = null) => notificationCenterName == null
            ? notificationCenter
            : Application.Current.MainWindow.FindChild<NotificationCenter>(childName: notificationCenterName) ??
              throw NotFound(notificationCenterName);

        private static KeyNotFoundException NotFound(string name = "Notification Center") => new($"{name} not found.");

        #region IWpfNotificationService

        /// <summary>
        ///     Creates the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="type">The type.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Creates.</returns>
        public Notification Create(string title, string text, NotificationType type = NotificationType.Information,
            string? notificationCenterName = null)
        {
            var notification = new Notification {Title = title, Text = text, NotificationType = type};

            var center = GetNotificationCenter(notificationCenterName);

            center.CreateNotification(notification);

            return notification;
        }

        #endregion
    }
}

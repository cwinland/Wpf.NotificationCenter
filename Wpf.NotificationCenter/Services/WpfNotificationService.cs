using System.Windows;
using System.Windows.Threading;
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
        #region Fields

        private readonly Dispatcher dispatcher;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="WpfNotificationService" /> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        public WpfNotificationService(Dispatcher? dispatcher = null)
        {
            dispatcher ??= Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        private static NotificationCenter GetNotificationCenter(string? notificationCenterName = null) => Application.Current?.MainWindow?.FindChild<NotificationCenter>(notificationCenterName) ??
                   throw NotFound(notificationCenterName);

        private static KeyNotFoundException NotFound(string? name = null) => new($"{name ?? "Notification Center"} not found.");

        #region IWpfNotificationService

        /// <inheritdoc />
        public void Close(Notification.Notification notification, string? notificationCenterName = null) => GetNotificationCenter(notificationCenterName).RemoveNotification(notification);

        /// <inheritdoc />
        public Notification.Notification Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All)
        {
            if (!dispatcher.CheckAccess())
            {
                return dispatcher.Invoke(() => Create(title, text, notificationType, notificationCenterName, alertType));
            }

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
        public Notification.Notification Error(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All) =>
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

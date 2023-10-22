using System.Windows;
using System.Windows.Threading;
using Wpf.NotificationCenter.Enums;
using Wpf.NotificationCenter.Extensions;
using Note = Wpf.NotificationCenter.Notification.Notification;

namespace Wpf.NotificationCenter.Services
{
    /// <summary>
    ///     Class WpfNotificationService.
    ///     Implements the <see cref="IWpfNotificationService" />
    /// </summary>
    /// <inheritdoc />
    /// <seealso cref="IWpfNotificationService" />
    public class WpfNotificationService : IWpfNotificationService
    {
        #region Fields

        private readonly Dispatcher dispatcher;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="WpfNotificationService" /> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <exception cref="ArgumentNullException">dispatcher</exception>
        public WpfNotificationService(Dispatcher? dispatcher = null)
        {
            dispatcher ??= Application.Current?.Dispatcher ?? Dispatcher.CurrentDispatcher;

            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        private static NotificationCenter GetNotificationCenter(string? notificationCenterName = null) =>
            Application.Current?.MainWindow?.FindChild<NotificationCenter>(notificationCenterName) ??
            throw NotFound(notificationCenterName);

        private static KeyNotFoundException NotFound(string? name = null) => new($"{name ?? "Notification Center"} not found.");
        private static InvalidProgramException ErrorCreating() => new ("Error Creating Notification");

        #region IWpfNotificationService

        /// <inheritdoc />
        public void Close(Note notification, string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).RemoveNotification(notification);

        /// <inheritdoc />
        public Note Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All) =>
            !dispatcher.CheckAccess()
                ? dispatcher.Invoke(() => Create(title, text, notificationType, notificationCenterName, alertType)) ?? throw ErrorCreating()
                : alertType switch
                {
                    AlertType.NotificationPopup => CreateToastNotification(title, text, notificationType, notificationCenterName),
                    AlertType.NotificationCenter => CreateAlertNotification(title, text, notificationType, notificationCenterName),
                    _ => CreateBoth(title, text, notificationType, notificationCenterName),
                };

        private Note CreateBoth(string title, string text, NotificationType notificationType, string? notificationCenterName)
        {
            _ = CreateToastNotification(title, text, notificationType, notificationCenterName);
            return CreateAlertNotification(title, text, notificationType, notificationCenterName);
        }

        /// <inheritdoc />
        public Note CreateToastNotification(string title, string text,
            NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null)
        {
            if (!dispatcher.CheckAccess())
            {
                return dispatcher.Invoke(() => CreateToastNotification(title, text, notificationType, notificationCenterName)) ??
                       throw ErrorCreating();
            }

            var notification = new Note { Title = title, Text = text, NotificationType = notificationType};
            var center = GetNotificationCenter(notificationCenterName);

            return center.CreateNotificationPopup(notification);
        }

        /// <inheritdoc />
        public Note CreateAlertNotification(string title, string text,
            NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null)
        {
            if (!dispatcher.CheckAccess())
            {
                return dispatcher.Invoke(() => CreateAlertNotification(title, text, notificationType, notificationCenterName)) ??
                       throw ErrorCreating();
            }

            var notification = new Note { Title = title, Text = text, NotificationType = notificationType};
            var center = GetNotificationCenter(notificationCenterName);

            return center.CreateNotificationAlert(notification);
        }

        /// <inheritdoc />
        public Note Error(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) =>
            Create(title, text, NotificationType.Error, notificationCenterName, alertType);

        /// <inheritdoc />
        public Note GetLastAlertCenterNotification(string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).DisplayNotes.Last();

        /// <inheritdoc />
        public Note GetLastToastNotification(string? notificationCenterName = null) =>
            GetNotificationCenter(notificationCenterName).Notifications.Last();

        /// <inheritdoc />
        public Note Information(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Information, notificationCenterName, alertType);

        /// <inheritdoc />
        public Note Success(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Success, notificationCenterName, alertType);

        /// <inheritdoc />
        public Note Warning(string title, string text, string? notificationCenterName = null,
            AlertType alertType = AlertType.All) => Create(title, text, NotificationType.Warning, notificationCenterName, alertType);

        #endregion
    }
}

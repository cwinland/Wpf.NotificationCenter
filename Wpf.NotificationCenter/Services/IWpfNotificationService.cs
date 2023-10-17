using Wpf.NotificationCenter.Enums;

namespace Wpf.NotificationCenter.Services
{
    /// <summary>
    ///     Interface IWpfNotificationService
    /// </summary>
    public interface IWpfNotificationService
    {
        void Close(Notification.Notification notification, string? notificationCenterName = null);
        /// <summary>
        ///     Creates the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType"></param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType"></param>
        /// <returns>Creates.</returns>
        Notification.Notification Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All);

        Notification.Notification Error(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);
        Notification.Notification Warning(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);
        Notification.Notification Information(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);
        Notification.Notification Success(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);

    }
}

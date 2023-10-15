namespace Wpf.NotificationCenter.Services
{
    /// <summary>
    ///     Interface IWpfNotificationService
    /// </summary>
    public interface IWpfNotificationService
    {
        /// <summary>
        ///     Creates the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType"></param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType"></param>
        /// <returns>Creates.</returns>
        Notification Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All);
    }
}

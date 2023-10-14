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
        /// <param name="type">The type.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Creates.</returns>
        Notification Create(string title, string text, NotificationType type = NotificationType.Information, string? notificationCenterName = null);
    }
}

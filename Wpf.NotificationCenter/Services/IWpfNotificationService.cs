using Wpf.NotificationCenter.Enums;
using Note = Wpf.NotificationCenter.Notification.Notification;

namespace Wpf.NotificationCenter.Services
{
    /// <summary>
    ///     Interface IWpfNotificationService
    /// </summary>
    public interface IWpfNotificationService
    {
        /// <summary>
        ///     Closes the specified notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        void Close(Note notification, string? notificationCenterName = null);

        /// <summary>
        ///     Creates the Notification(s).
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Alert Center Notification.</returns>
        Note Create(string title, string text, NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Creates the alert notification.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Creates the alert notification.</returns>
        Note CreateAlertNotification(string title, string text,
            NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null);

        /// <summary>
        ///     Creates the toast notification.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationType">Type of the notification.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Creates the toast notification.</returns>
        Note CreateToastNotification(string title, string text,
            NotificationType notificationType = NotificationType.Information,
            string? notificationCenterName = null);

        /// <summary>
        ///     Creates the error notification(s).
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Alert Center Notification.</returns>
        Note Error(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Errors the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Errors.</returns>
        Note Error(string text, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Gets the last alert center notification.
        /// </summary>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Notification ofNotification of the last alert center notification.</returns>
        Note GetLastAlertCenterNotification(string? notificationCenterName = null);

        /// <summary>
        ///     Gets the last toast notification.
        /// </summary>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <returns>Notification ofNotification of the last toast notification.</returns>
        Note GetLastToastNotification(string? notificationCenterName = null);

        /// <summary>
        ///     Creates the information notification(s).
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Alert Center Notification.</returns>
        Note Information(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Informations the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Informations.</returns>
        Note Information(string text, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Creates the success notification(s).
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Alert Center Notification.</returns>
        Note Success(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Successes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Successes.</returns>
        Note Success(string text, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Creates the warning notification(s).
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="text">The text.</param>
        /// <param name="notificationCenterName">Name of the notification center.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Alert Center Notification.</returns>
        Note Warning(string title, string text, string? notificationCenterName = null, AlertType alertType = AlertType.All);

        /// <summary>
        ///     Warnings the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="alertType">Type of the alert.</param>
        /// <returns>Warnings.</returns>
        Note Warning(string text, AlertType alertType = AlertType.All);
    }
}

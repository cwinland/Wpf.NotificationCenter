using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Wpf.NotificationCenter
{
    /// <summary>
    ///     Interaction logic for NotificationPopup.xaml
    /// </summary>
    /// <inheritdoc cref="System.Windows.Controls.Primitives.Popup" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    public partial class NotificationPopup : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationPopup" /> class.
        /// </summary>
        public NotificationPopup() => InitializeComponent();

        /// <summary>
        ///     Adds the notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public void AddNotification(Notification notification)
        {
            MyGrid.Children.Add(notification);
            MyGrid.InvalidateVisual();
            OnPropertyChanged(nameof(MyGrid));

            var timer = new DispatcherTimer(TimeSpan.FromSeconds(5),
                DispatcherPriority.Render,
                (sender, args) => { MyGrid.Children.Remove(notification); },
                Dispatcher.CurrentDispatcher
            );
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

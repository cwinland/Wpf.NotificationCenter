using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace Wpf.NotificationCenter
{
    /// <inheritdoc cref="System.Windows.Controls.Primitives.Popup" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    /// Interaction logic for NotificationPopup.xaml
    /// </summary>
    public partial class NotificationPopup : INotifyPropertyChanged
    {
        public NotificationPopup() => InitializeComponent();

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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

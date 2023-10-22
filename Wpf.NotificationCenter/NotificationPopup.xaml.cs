using System.ComponentModel;
using System.Runtime.CompilerServices;

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

        /// <inheritdoc />
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
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

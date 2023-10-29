using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf.NotificationCenter
{
    /// <inheritdoc cref="ContentControl" />
    /// <summary>
    ///     Interaction logic for NotificationCenterButton.xaml
    /// </summary>
    public partial class NotificationCenterButton
    {
        #region Fields

        /// <summary>
        ///     The toggle command property
        /// </summary>
        public static readonly DependencyProperty ToggleCommandProperty = DependencyProperty.Register(
            nameof(ToggleCommand),
            typeof(ICommand),
            typeof(NotificationCenterButton),
            new PropertyMetadata(default(ICommand))
        );

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the toggle command.
        /// </summary>
        /// <value>The toggle command.</value>
        public ICommand ToggleCommand
        {
            get => (ICommand) GetValue(ToggleCommandProperty);
            set => SetValue(ToggleCommandProperty, value);
        }

        #endregion

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Wpf.NotificationCenter.NotificationCenterButton" /> class.
        /// </summary>
        public NotificationCenterButton() => InitializeComponent();
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf.NotificationCenter
{
    /// <summary>
    ///     Interaction logic for Notification.xaml
    /// </summary>
    /// <inheritdoc cref="System.Windows.Controls.Expander" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    public partial class Notification : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        ///     Occurs when [on clicked].
        /// </summary>
        public event EventHandler? OnClicked;

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Fields

        /// <summary>
        ///     The notification type property
        /// </summary>
        public static readonly DependencyProperty NotificationTypeProperty = DependencyProperty.Register(
            nameof(NotificationType),
            typeof(NotificationType),
            typeof(Notification),
            new PropertyMetadata(default(NotificationType))
        );

        /// <summary>
        ///     The title property
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(string.Empty)
        );

        /// <summary>
        ///     The text property
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(string.Empty)
        );

        /// <summary>
        ///     The unread property
        /// </summary>
        public static readonly DependencyProperty UnreadProperty = DependencyProperty.Register(
            nameof(Unread),
            typeof(bool),
            typeof(Notification),
            new PropertyMetadata(true)
        );

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the display time.
        /// </summary>
        /// <value>The display time.</value>
        public TimeSpan DisplayTime { get; set; } = TimeSpan.FromSeconds(5);

        /// <summary>
        ///     Gets the icon brush.
        /// </summary>
        /// <value>The icon brush.</value>
        public Brush IconBrush => ConvertTypeToBrush(NotificationType);

        /// <summary>
        ///     Gets or sets the type of the notification.
        /// </summary>
        /// <value>The type of the notification.</value>
        public NotificationType NotificationType
        {
            get => (NotificationType) GetValue(NotificationTypeProperty);
            set => SetValue(NotificationTypeProperty, value);
        }

        /// <summary>
        ///     Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string? Text
        {
            get => GetValue(TextProperty)?.ToString();
            set => SetValue(TextProperty, value ?? string.Empty);
        }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string? Title
        {
            get => GetValue(TitleProperty)?.ToString();
            set => SetValue(TitleProperty, value ?? string.Empty);
        }

        /// <summary>
        ///     Gets the title weight.
        /// </summary>
        /// <value>The title weight.</value>
        public FontWeight TitleWeight => Unread ? FontWeights.Bold : FontWeights.Medium;

        /// <summary>
        ///     Gets or sets a value indicating whether this <see cref="Notification" /> is unread.
        /// </summary>
        /// <value><c>true</c> if unread; otherwise, <c>false</c>.</value>
        public bool Unread
        {
            get => (bool) GetValue(UnreadProperty);
            set
            {
                {
                    SetValue(UnreadProperty, value);
                    OnPropertyChanged(nameof(TitleWeight));
                }
            }
        }

        #endregion

        static Notification() => DefaultStyleKeyProperty?.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));

        /// <summary>
        ///     Initializes a new instance of the <see cref="Notification" /> class.
        /// </summary>
        public Notification()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Notification" /> class.
        /// </summary>
        /// <param name="notification">The notification.</param>
        public Notification(Notification notification) : this()
        {
            Title = notification.Title;
            Text = notification.Text;
            NotificationType = notification.NotificationType;
            Background = Brushes.Wheat;
            IsExpanded = true;
        }

        /// <summary>
        ///     Converts the type to brush.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Converts the type to brush.</returns>
        public virtual SolidColorBrush ConvertTypeToBrush(NotificationType type) => type switch
        {
            NotificationType.Information => Brushes.Blue,
            NotificationType.Error => Brushes.Red,
            NotificationType.Warning => Brushes.DarkGoldenrod,
            NotificationType.Success => Brushes.MediumSeaGreen,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        /// <inheritdoc />
        protected override void OnExpanded()
        {
            Unread = false;
            base.OnExpanded();
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void Notification_OnMouseDown(object sender, MouseButtonEventArgs e) => OnClicked?.Invoke(sender, e);
    }
}

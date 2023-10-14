using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Wpf.NotificationCenter
{
    /// <summary>
    ///     Interaction logic for NotificationCenter.xaml
    /// </summary>
    /// <inheritdoc cref="System.Windows.Controls.Primitives.Selector" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public partial class NotificationCenter : INotifyPropertyChanged
    {
        #region Events

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Fields

        /// <summary>
        ///     The notifications visibility property
        /// </summary>
        public static readonly DependencyProperty NotificationsVisibilityProperty = DependencyProperty.Register(
            nameof(NotificationsVisibility),
            typeof(Visibility),
            typeof(NotificationCenter),
            new PropertyMetadata(Visibility.Collapsed, Refresh)
        );

        /// <summary>
        ///     Creates new alertproperty.
        /// </summary>
        public static readonly DependencyProperty NewAlertProperty = DependencyProperty.Register(
            nameof(NewAlert),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(default(bool), Refresh)
        );

        /// <summary>
        ///     Creates new alertcolorproperty.
        /// </summary>
        public static readonly DependencyProperty NewAlertColorProperty = DependencyProperty.Register(
            nameof(NewAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Goldenrod, Refresh)
        );

        /// <summary>
        ///     The no alert color property
        /// </summary>
        public static readonly DependencyProperty NoAlertColorProperty = DependencyProperty.Register(
            nameof(NoAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Black, Refresh)
        );

        /// <summary>
        ///     The notifications visible property
        /// </summary>
        public static readonly DependencyProperty NotificationsVisibleProperty = DependencyProperty.Register(
            nameof(NotificationsVisible),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(default(bool), Refresh)
        );

        /// <summary>
        ///     The no alert icon property
        /// </summary>
        public static readonly DependencyProperty NoAlertIconProperty = DependencyProperty.Register(
            nameof(NoAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.Notifications, Refresh)
        );

        /// <summary>
        ///     Creates new alerticonproperty.
        /// </summary>
        public static readonly DependencyProperty NewAlertIconProperty = DependencyProperty.Register(
            nameof(NewAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.BellAlert, Refresh)
        );

        /// <summary>
        ///     The alert maximum width property
        /// </summary>
        public static readonly DependencyProperty AlertMaxWidthProperty = DependencyProperty.Register(
            nameof(AlertMaxWidth),
            typeof(double),
            typeof(NotificationCenter),
            new PropertyMetadata(double.NaN, Refresh)
        );

        /// <summary>
        ///     The maximum notifications property
        /// </summary>
        public static readonly DependencyProperty MaxNotificationsProperty = DependencyProperty.Register(
            nameof(MaxNotifications),
            typeof(byte),
            typeof(NotificationCenter),
            new PropertyMetadata(default(byte), Refresh)
        );

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the color of the alert.
        /// </summary>
        /// <value>The color of the alert.</value>
        public SolidColorBrush AlertColor => NewAlert ? NewAlertColor : NoAlertColor;

        /// <summary>
        ///     Gets the alert icon.
        /// </summary>
        /// <value>The alert icon.</value>
        public PackIconKind AlertIcon => NewAlert ? NewAlertIcon : NoAlertIcon;

        /// <summary>
        ///     Gets or sets the maximum width of the alert.
        /// </summary>
        /// <value>The maximum width of the alert.</value>
        public double AlertMaxWidth
        {
            get => (double) GetValue(AlertMaxWidthProperty);
            set => SetValue(AlertMaxWidthProperty, value);
        }

        /// <summary>
        ///     Gets the data visibility.
        /// </summary>
        /// <value>The data visibility.</value>
        public Visibility DataVisibility => Notifications.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Gets the delete all notifications command.
        /// </summary>
        /// <value>The delete all notifications command.</value>
        public ICommand DeleteAllNotificationsCommand => new RelayCommand(() => Notifications.Clear());

        /// <summary>
        ///     Gets or sets the display notes.
        /// </summary>
        /// <value>The display notes.</value>
        public ObservableCollection<Notification> DisplayNotes { get; set; } = new();

        /// <summary>
        ///     Gets or sets the maximum notifications.
        /// </summary>
        /// <value>The maximum notifications.</value>
        public byte MaxNotifications
        {
            get => (byte) GetValue(MaxNotificationsProperty);
            set => SetValue(MaxNotificationsProperty, value);
        }

        /// <summary>
        ///     Creates new alert.
        /// </summary>
        /// <value><c>true</c> if [new alert]; otherwise, <c>false</c>.</value>
        public bool NewAlert => NewNotificationCount > 0;

        /// <summary>
        ///     Creates new alertcolor.
        /// </summary>
        /// <value>The new color of the alert.</value>
        public SolidColorBrush NewAlertColor
        {
            get => (SolidColorBrush) GetValue(NewAlertColorProperty);
            set
            {
                SetValue(NewAlertColorProperty, value);
                SetValue(NewAlertColorProperty, NewAlertColor);
            }
        }

        /// <summary>
        ///     Creates new alerticon.
        /// </summary>
        /// <value>The new alert icon.</value>
        public PackIconKind NewAlertIcon
        {
            get => (PackIconKind) GetValue(NewAlertIconProperty);
            set => SetValue(NewAlertIconProperty, value);
        }

        /// <summary>
        ///     Creates new alertvisibility.
        /// </summary>
        /// <value>The new alert visibility.</value>
        public Visibility NewAlertVisibility => NewAlert ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Creates new notificationcount.
        /// </summary>
        /// <value>The new notification count.</value>
        public int NewNotificationCount => Notifications.Count(x => x.Unread);

        /// <summary>
        ///     Gets or sets the color of the no alert.
        /// </summary>
        /// <value>The color of the no alert.</value>
        public SolidColorBrush NoAlertColor
        {
            get => (SolidColorBrush) GetValue(NoAlertColorProperty);
            set
            {
                SetValue(NoAlertColorProperty, value);
                SetValue(NoAlertColorProperty, NoAlertColor);
            }
        }

        /// <summary>
        ///     Gets or sets the no alert icon.
        /// </summary>
        /// <value>The no alert icon.</value>
        public PackIconKind NoAlertIcon
        {
            get => (PackIconKind) GetValue(NoAlertIconProperty);
            set => SetValue(NoAlertIconProperty, value);
        }

        /// <summary>
        ///     Gets the no data visibility.
        /// </summary>
        /// <value>The no data visibility.</value>
        public Visibility NoDataVisibility => Notifications.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Gets the notifications.
        /// </summary>
        /// <value>The notifications.</value>
        public ObservableCollection<Notification> Notifications { get; } = new();

        /// <summary>
        ///     Gets or sets the notifications visibility.
        /// </summary>
        /// <value>The notifications visibility.</value>
        public Visibility NotificationsVisibility
        {
            get => (Visibility) GetValue(NotificationsVisibilityProperty);
            set => SetValue(NotificationsVisibilityProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [notifications visible].
        /// </summary>
        /// <value><c>true</c> if [notifications visible]; otherwise, <c>false</c>.</value>
        public bool NotificationsVisible
        {
            get => (bool) GetValue(NotificationsVisibleProperty);
            set
            {
                SetValue(NotificationsVisibleProperty, value);
                SetValue(NotificationsVisibilityProperty, value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        /// <summary>
        ///     Gets the toggle command.
        /// </summary>
        /// <value>The toggle command.</value>
        public ICommand ToggleCommand => new RelayCommand(() => NotificationsVisible = !NotificationsVisible);

        #endregion

        static NotificationCenter() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationCenter), new FrameworkPropertyMetadata(typeof(NotificationCenter)));

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotificationCenter" /> class.
        /// </summary>
        public NotificationCenter()
        {
            InitializeComponent();
            DataContext = this;
            Notifications.CollectionChanged += (sender, args) => Refresh();
            DisplayNotes.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(DisplayNotes));
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void CreateNotification(Notification notification)
        {
            notification.Expanded += (sender, args) => Refresh();
            Notifications.Add(notification);

            var newNote = new Notification
            {
                Text = notification.Text,
                NotificationType = notification.NotificationType,
                Background = Brushes.Wheat,
                IsExpanded = true,
                MinWidth = ActualWidth / 4
            };

            DisplayNotes.Add(newNote);

            var timer = new DispatcherTimer(TimeSpan.FromSeconds(5),
                DispatcherPriority.Render,
                (sender, args) =>
                {
                    if (sender is DispatcherTimer timer)
                    {
                        timer.Stop();
                    }

                    DisplayNotes.Remove(newNote);
                    OnPropertyChanged(nameof(DisplayNotes));
                },
                Dispatcher.CurrentDispatcher
            );

            timer.Start();

            if (MaxNotifications > 0 && Notifications.Count > MaxNotifications)
            {
                Notifications.RemoveAt(0);
            }

            Refresh();
        }

        internal void Refresh()
        {
            OnPropertyChanged(nameof(NotificationPopup));

            OnPropertyChanged(nameof(Notifications));
            OnPropertyChanged(nameof(DisplayNotes));

            OnPropertyChanged(nameof(NoDataVisibility));
            OnPropertyChanged(nameof(DataVisibility));

            OnPropertyChanged(nameof(NewAlert));
            OnPropertyChanged(nameof(NewAlertVisibility));
            OnPropertyChanged(nameof(AlertIcon));
            OnPropertyChanged(nameof(AlertColor));
            OnPropertyChanged(nameof(NewNotificationCount));
        }

        internal void RemoveNotification(Notification notification)
        {
            DisplayNotes.Remove(notification);
            Notifications.Remove(notification);
            Refresh();
        }

        private static void Refresh(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NotificationCenter nc)
            {
                nc.Refresh();
            }
        }
    }
}

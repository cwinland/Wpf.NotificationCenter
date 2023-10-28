using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf.NotificationCenter.Enums;
using Note = Wpf.NotificationCenter.Notification.Notification;

namespace Wpf.NotificationCenter
{
    /// <summary>
    ///     Interaction logic for NotificationCenter.xaml
    /// </summary>
    /// <inheritdoc cref="HeaderedContentControl" />
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
        ///     The button z index property
        /// </summary>
        public static readonly DependencyProperty ButtonZIndexProperty = DependencyProperty.Register(
            nameof(ButtonZIndex),
            typeof(int),
            typeof(NotificationCenter),
            new UIPropertyMetadata(999)
        );

        /// <summary>
        ///     Color of the icon when there is a new alert.
        /// </summary>
        public static readonly DependencyProperty NewAlertColorProperty = DependencyProperty.Register(
            nameof(NewAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Goldenrod, Refresh)
        );

        /// <summary>
        ///     Color of the icon when there are no unread alerts.
        /// </summary>
        public static readonly DependencyProperty NoAlertColorProperty = DependencyProperty.Register(
            nameof(NoAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Black, Refresh)
        );

        /// <summary>
        ///     The icon when there are no unread alerts.
        /// </summary>
        public static readonly DependencyProperty NoAlertIconProperty = DependencyProperty.Register(
            nameof(NoAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.Notifications, Refresh)
        );

        /// <summary>
        ///     The icon when there is a new alert.
        /// </summary>
        public static readonly DependencyProperty NewAlertIconProperty = DependencyProperty.Register(
            nameof(NewAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.BellAlert, Refresh)
        );

        /// <summary>
        ///     The alert text content maximum height in the alert center.
        /// </summary>
        public static readonly DependencyProperty AlertMaxHeightProperty = DependencyProperty.Register(
            nameof(AlertMaxHeight),
            typeof(double),
            typeof(NotificationCenter),
            new UIPropertyMetadata(200d)
        );

        /// <summary>
        ///     The alert maximum width property of the notification center popup.
        /// </summary>
        public static readonly DependencyProperty AlertMaxWidthProperty = DependencyProperty.Register(
            nameof(AlertMaxWidth),
            typeof(double),
            typeof(NotificationCenter),
            new UIPropertyMetadata(double.NaN, Refresh)
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

        /// <summary>
        ///     The is items ascending property
        /// </summary>
        public static readonly DependencyProperty IsItemsAscendingProperty = DependencyProperty.Register(
            nameof(IsItemsAscending),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(false)
        );

        /// <summary>
        ///     The button alignment property
        /// </summary>
        public static readonly DependencyProperty ButtonHorizontalAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonHorizontalAlignment),
            typeof(HorizontalAlignment),
            typeof(NotificationCenter),
            new UIPropertyMetadata(HorizontalAlignment.Right)
        );

        /// <summary>
        ///     The show button in header property
        /// </summary>
        public static readonly DependencyProperty ShowButtonInHeaderProperty = DependencyProperty.Register(
            nameof(ShowButtonInHeader),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(true)
        );

        /// <summary>
        ///     The show button in content property
        /// </summary>
        public static readonly DependencyProperty ShowButtonInContentProperty = DependencyProperty.Register(
            nameof(ShowButtonInContent),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(false)
        );

        /// <summary>
        ///     The button vertical alignment property
        /// </summary>
        public static readonly DependencyProperty ButtonVerticalAlignmentProperty = DependencyProperty.Register(
            nameof(ButtonVerticalAlignment),
            typeof(VerticalAlignment),
            typeof(NotificationCenter),
            new UIPropertyMetadata(default(VerticalAlignment))
        );

        /// <summary>
        ///     The popup placement property
        /// </summary>
        public static readonly DependencyProperty PopupPlacementProperty = DependencyProperty.Register(
            nameof(PopupPlacement),
            typeof(PlacementMode),
            typeof(NotificationCenter),
            new FrameworkPropertyMetadata(PlacementMode.Bottom)
        );

        private readonly SolidColorBrush defaultColor = Brushes.Black;
        private bool notificationsVisible;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the maximum height of the alert.
        /// </summary>
        /// <value>The maximum height of the alert.</value>
        public double AlertMaxHeight
        {
            get => (double) GetValue(AlertMaxHeightProperty);
            set => SetValue(AlertMaxHeightProperty, value);
        }

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
        ///     Gets or sets the button alignment.
        /// </summary>
        /// <value>The button alignment.</value>
        public HorizontalAlignment ButtonHorizontalAlignment
        {
            get => (HorizontalAlignment) GetValue(ButtonHorizontalAlignmentProperty);
            set => SetValue(ButtonHorizontalAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the button vertical alignment.
        /// </summary>
        /// <value>The button vertical alignment.</value>
        public VerticalAlignment ButtonVerticalAlignment
        {
            get => (VerticalAlignment) GetValue(ButtonVerticalAlignmentProperty);
            set => SetValue(ButtonVerticalAlignmentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the index of the button z.
        /// </summary>
        /// <value>The index of the button z.</value>
        public int ButtonZIndex
        {
            get => (int) GetValue(ButtonZIndexProperty);
            set => SetValue(ButtonZIndexProperty, value);
        }

        /// <summary>
        ///     Gets the data visibility.
        /// </summary>
        /// <value>The data visibility.</value>
        /// <exclude />
        public Visibility DataVisibility => Notifications.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Gets the delete all notifications command.
        /// </summary>
        /// <value>The delete all notifications command.</value>
        /// <exclude />
        public ICommand DeleteAllNotificationsCommand => new RelayCommand(() => Notifications.Clear());

        /// <summary>
        ///     Gets the toasts.
        /// </summary>
        /// <value>The toast message notifications.</value>
        public ObservableCollection<Note> DisplayNotes { get; } = new();

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is items ascending.
        /// </summary>
        /// <value><c>true</c> if this instance is items ascending; otherwise, <c>false</c>.</value>
        public bool IsItemsAscending
        {
            get => (bool) GetValue(IsItemsAscendingProperty);
            set => SetValue(IsItemsAscendingProperty, value);
        }

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
        /// <exclude />
        public bool NewAlert => NewNotificationCount > 0;

        /// <summary>
        ///     The color used when a new alert is detected.
        /// </summary>
        /// <value>The new color of the alert.</value>
        public SolidColorBrush NewAlertColor
        {
            get => (SolidColorBrush) (GetValue(NewAlertColorProperty) ?? defaultColor);
            set
            {
                SetValue(NewAlertColorProperty, value);
                SetValue(NewAlertColorProperty, NewAlertColor);
            }
        }

        /// <summary>
        ///     The icon used when a new alert is detected.
        /// </summary>
        /// <value>The new alert icon.</value>
        public PackIconKind NewAlertIcon
        {
            get => (PackIconKind) GetValue(NewAlertIconProperty);
            set => SetValue(NewAlertIconProperty, value);
        }

        /// <summary>
        ///     Gets new notification count.
        /// </summary>
        /// <value>The new notification count.</value>
        public int NewNotificationCount => Notifications.Count(x => x.Unread);

        /// <summary>
        ///     Gets or sets the color of the no alert.
        /// </summary>
        /// <value>The color of the no alert.</value>
        public SolidColorBrush NoAlertColor
        {
            get => (SolidColorBrush) (GetValue(NoAlertColorProperty) ?? defaultColor);
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
        /// <exclude />
        public Visibility NoDataVisibility => Notifications.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        /// <summary>
        ///     Gets the alert center notifications.
        /// </summary>
        /// <value>The notifications.</value>
        public ObservableCollection<Note> Notifications { get; } = new();

        /// <summary>
        ///     Gets or sets a value indicating whether [notifications visible].
        /// </summary>
        /// <value><c>true</c> if [notifications visible]; otherwise, <c>false</c>.</value>
        public bool NotificationsVisible
        {
            get => notificationsVisible;
            set
            {
                notificationsVisible = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the popup placement.
        /// </summary>
        /// <value>The popup placement.</value>
        public PlacementMode PopupPlacement
        {
            get => (PlacementMode) GetValue(PopupPlacementProperty);
            set => SetValue(PopupPlacementProperty, value);
        }

        /// <summary>
        ///     The popup stays open property
        /// </summary>
        public static readonly DependencyProperty PopupStaysOpenProperty = DependencyProperty.Register(
            nameof(PopupStaysOpen),
            typeof(bool),
            typeof(NotificationCenter),
            new FrameworkPropertyMetadata(false)
        );

        /// <summary>
        ///     Gets or sets a value indicating whether [popup stays open].
        /// </summary>
        /// <value><c>true</c> if [popup stays open]; otherwise, <c>false</c>.</value>
        public bool PopupStaysOpen
        {
            get => (bool) GetValue(PopupStaysOpenProperty);
            set => SetValue(PopupStaysOpenProperty, value);
        }
        /// <summary>
        ///     The popup horizontal placement property
        /// </summary>
        public static readonly DependencyProperty PopupHorizontalPlacementProperty = DependencyProperty.Register(
            nameof(PopupHorizontalPlacement),
            typeof(double),
            typeof(NotificationCenter),
            new FrameworkPropertyMetadata(default(double))
        );

        /// <summary>
        ///     Gets or sets the popup horizontal placement.
        /// </summary>
        /// <value>The popup horizontal placement.</value>
        public double PopupHorizontalPlacement
        {
            get => (double) GetValue(PopupHorizontalPlacementProperty);
            set => SetValue(PopupHorizontalPlacementProperty, value);
        }

        /// <summary>
        ///     The popup vertical placement property
        /// </summary>
        public static readonly DependencyProperty PopupVerticalPlacementProperty = DependencyProperty.Register(
            nameof(PopupVerticalPlacement),
            typeof(double),
            typeof(NotificationCenter),
            new FrameworkPropertyMetadata(default(double))
        );

        /// <summary>
        ///     Gets or sets the popup vertical placement.
        /// </summary>
        /// <value>The popup vertical placement.</value>
        public double PopupVerticalPlacement
        {
            get => (double) GetValue(PopupVerticalPlacementProperty);
            set => SetValue(PopupVerticalPlacementProperty, value);
        }
        /// <summary>
        ///     Gets or sets a value indicating whether [show button in content].
        /// </summary>
        /// <value><c>true</c> if [show button in content]; otherwise, <c>false</c>.</value>
        public bool ShowButtonInContent
        {
            get => (bool) GetValue(ShowButtonInContentProperty);
            set => SetValue(ShowButtonInContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [show button in header].
        /// </summary>
        /// <value><c>true</c> if [show button in header]; otherwise, <c>false</c>.</value>
        public bool ShowButtonInHeader
        {
            get => (bool) GetValue(ShowButtonInHeaderProperty);
            set => SetValue(ShowButtonInHeaderProperty, value);
        }

        /// <summary>
        ///     Gets the toggle command.
        /// </summary>
        /// <value>The toggle command.</value>
        public ICommand ToggleCommand => new RelayCommand(() =>
            {
                foreach (var notification in Notifications)
                {
                    notification.IsExpanded = false;
                }

                NotificationsVisible = !NotificationsVisible;
            }
        );

        #endregion

        /// <summary>
        ///     Initializes static members of the <see cref="NotificationCenter" /> class.
        /// </summary>
        /// <remarks>Sets the Style key to NotificationCenter.</remarks>
        static NotificationCenter() =>
            DefaultStyleKeyProperty?.OverrideMetadata(typeof(NotificationCenter), new FrameworkPropertyMetadata(typeof(NotificationCenter)));

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Wpf.NotificationCenter.NotificationCenter" /> class.
        /// </summary>
        public NotificationCenter()
        {
            InitializeComponent();
            DataContext = this;
            Notifications.CollectionChanged += (_, _) => Refresh();
            DisplayNotes.CollectionChanged += (_, _) => OnPropertyChanged(nameof(DisplayNotes));
        }

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal Note CreateNotificationAlert(Note notification)
        {
            notification.AlertType = AlertType.NotificationCenter;
            notification.IsExpanded = false;
            notification.ShowExpander = true;
            notification.Expanded += Refresh;
            notification.AlertMaxHeight = AlertMaxHeight;
            notification.RemoveNotificationCommand = new RelayCommand<Note>(RemoveNotification);
            Notifications.Add(notification);

            if (MaxNotifications > 0 && Notifications.Count > MaxNotifications && Notifications.Any(x => !x.Unread))
            {
                var removeNote = Notifications.First(x => !x.Unread);
                Notifications.Remove(removeNote);
            }

            Refresh();

            return notification;
        }

        /// <summary>
        ///     The notification seconds property
        /// </summary>
        public static readonly DependencyProperty NotificationSecondsProperty = DependencyProperty.Register(
            nameof(NotificationSeconds),
            typeof(int),
            typeof(NotificationCenter),
            new PropertyMetadata(5)
        );

        /// <summary>
        ///     Gets or sets the notification seconds.
        /// </summary>
        /// <value>The notification seconds.</value>
        public int NotificationSeconds
        {
            get => (int) GetValue(NotificationSecondsProperty);
            set => SetValue(NotificationSecondsProperty, value);
        }
        internal Note CreateNotificationPopup(Note notification)
        {
            var newNote = new Note(notification)
            {
                ShowExpander = false,
                IsExpanded = true,
                AlertType = AlertType.NotificationPopup,
                MaxHeight = AlertMaxHeight,
                CreatedOnVisibility = Visibility.Collapsed,
            };

            try
            {
                DisplayNotes.Add(newNote);

                var timer = new DispatcherTimer(TimeSpan.FromSeconds(NotificationSeconds),
                    DispatcherPriority.Render,
                    TimerCallback,
                    Dispatcher.CurrentDispatcher
                );

                timer.Start();

                void TimerCallback(object? sender, EventArgs? args)
                {
                    // Don't close notification if the mouse is over it.
                    if (newNote.IsMouseOver)
                    {
                        return;
                    }

                    if (sender is DispatcherTimer t)
                    {
                        t.Stop();
                    }

                    DisplayNotes.Remove(newNote);
                    OnPropertyChanged(nameof(DisplayNotes));
                }
            }
            finally
            {
                Refresh();
            }

            return newNote;
        }

        internal void Refresh(object? sender = null, EventArgs? args = null)
        {
            OnPropertyChanged(nameof(Notifications));
            OnPropertyChanged(nameof(DisplayNotes));

            OnPropertyChanged(nameof(NoDataVisibility));
            OnPropertyChanged(nameof(DataVisibility));

            OnPropertyChanged(nameof(NewNotificationCount));
            OnPropertyChanged(nameof(NewAlert));
        }

        internal void RemoveNotification(Note? notification)
        {
            if (notification == null)
            {
                return;
            }

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

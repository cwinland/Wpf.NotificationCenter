using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Wpf.NotificationCenter.Extensions;

namespace Wpf.NotificationCenter
{
    /// <inheritdoc cref="System.Windows.Controls.Primitives.Selector" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Interaction logic for NotificationCenter.xaml
    /// </summary>
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public partial class NotificationCenter : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Fields

        public static readonly DependencyProperty NotificationsVisibilityProperty = DependencyProperty.Register(
            nameof(NotificationsVisibility),
            typeof(Visibility),
            typeof(NotificationCenter),
            new PropertyMetadata(Visibility.Collapsed, Refresh)
        );

        public static readonly DependencyProperty NewAlertProperty = DependencyProperty.Register(
            nameof(NewAlert),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(default(bool), Refresh)
        );

        public static readonly DependencyProperty NewAlertColorProperty = DependencyProperty.Register(
            nameof(NewAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Goldenrod, Refresh)
        );

        public static readonly DependencyProperty NoAlertColorProperty = DependencyProperty.Register(
            nameof(NoAlertColor),
            typeof(SolidColorBrush),
            typeof(NotificationCenter),
            new PropertyMetadata(Brushes.Black, Refresh)
        );

        public static readonly DependencyProperty NotificationsVisibleProperty = DependencyProperty.Register(
            nameof(NotificationsVisible),
            typeof(bool),
            typeof(NotificationCenter),
            new PropertyMetadata(default(bool), Refresh)
        );

        public static readonly DependencyProperty NoAlertIconProperty = DependencyProperty.Register(
            nameof(NoAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.Notifications, Refresh)
        );

        public static readonly DependencyProperty NewAlertIconProperty = DependencyProperty.Register(
            nameof(NewAlertIcon),
            typeof(PackIconKind),
            typeof(NotificationCenter),
            new PropertyMetadata(PackIconKind.BellAlert, Refresh)
        );

        public static readonly DependencyProperty AlertMaxWidthProperty = DependencyProperty.Register(
            nameof(AlertMaxWidth),
            typeof(double),
            typeof(NotificationCenter),
            new PropertyMetadata(double.NaN, Refresh)
        );

        public static readonly DependencyProperty MaxNotificationsProperty = DependencyProperty.Register(
            nameof(MaxNotifications),
            typeof(byte),
            typeof(NotificationCenter),
            new PropertyMetadata(default(byte), Refresh)
        );

        #endregion

        #region Properties

        public SolidColorBrush AlertColor => NewAlert ? NewAlertColor : NoAlertColor;

        public PackIconKind AlertIcon => NewAlert ? NewAlertIcon : NoAlertIcon;

        public double AlertMaxWidth
        {
            get => (double)GetValue(AlertMaxWidthProperty);
            set => SetValue(AlertMaxWidthProperty, value);
        }

        public Visibility DataVisibility => Notifications.Count > 0 ? Visibility.Visible : Visibility.Collapsed;

        public ICommand DeleteAllNotificationsCommand => new RelayCommand(() => Notifications.Clear());

        public byte MaxNotifications
        {
            get => (byte)GetValue(MaxNotificationsProperty);
            set => SetValue(MaxNotificationsProperty, value);
        }

        public bool NewAlert => NewNotificationCount > 0;

        public SolidColorBrush NewAlertColor
        {
            get => (SolidColorBrush)GetValue(NewAlertColorProperty);
            set
            {
                SetValue(NewAlertColorProperty, value);
                SetValue(NewAlertColorProperty, NewAlertColor);
            }
        }

        public PackIconKind NewAlertIcon
        {
            get => (PackIconKind)GetValue(NewAlertIconProperty);
            set => SetValue(NewAlertIconProperty, value);
        }

        public Visibility NewAlertVisibility => NewAlert ? Visibility.Visible : Visibility.Collapsed;

        public int NewNotificationCount => Notifications.Count(x => x.Unread);

        public SolidColorBrush NoAlertColor
        {
            get => (SolidColorBrush)GetValue(NoAlertColorProperty);
            set
            {
                SetValue(NoAlertColorProperty, value);
                SetValue(NoAlertColorProperty, NoAlertColor);
            }
        }

        public PackIconKind NoAlertIcon
        {
            get => (PackIconKind)GetValue(NoAlertIconProperty);
            set => SetValue(NoAlertIconProperty, value);
        }

        public Visibility NoDataVisibility => Notifications.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

        public ObservableCollection<Notification> Notifications { get; } = new();
        public ObservableCollection<Notification> DisplayNotes { get; set; }= new();

        public Visibility NotificationsVisibility
        {
            get => (Visibility)GetValue(NotificationsVisibilityProperty);
            set => SetValue(NotificationsVisibilityProperty, value);
        }

        public bool NotificationsVisible
        {
            get => (bool)GetValue(NotificationsVisibleProperty);
            set
            {
                SetValue(NotificationsVisibleProperty, value);
                SetValue(NotificationsVisibilityProperty, value ? Visibility.Visible : Visibility.Collapsed);
            }
        }

        public ICommand ToggleCommand => new RelayCommand(() => NotificationsVisible = !NotificationsVisible);

        #endregion

        static NotificationCenter() =>
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationCenter), new FrameworkPropertyMetadata(typeof(NotificationCenter)));

        public NotificationCenter()
        {
            InitializeComponent();
            DataContext = this;
            Notifications.CollectionChanged += (sender, args) => Refresh();
            DisplayNotes.CollectionChanged += (sender, args) => OnPropertyChanged(nameof(DisplayNotes));
        }

        public CustomPopupPlacement[] PlacePopup(Size popupSize,
            Size targetSize,
            Point offset)
        {
            var placement1 =
                new CustomPopupPlacement(new Point(ContentElement.ActualWidth, 0), PopupPrimaryAxis.Vertical);

            var placement2 =
                new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] ttplaces =
                {placement1, placement2};

            return ttplaces;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void CreateNotification(Notification notification)
        {
            notification.Expanded += (sender, args) => Refresh();
            Notifications.Add(notification);

            var newNote = new Notification()
            {
                Text = notification.Text,
                NotificationType = notification.NotificationType,
                Background = Brushes.Wheat,
                IsExpanded = true,
                MinWidth = ActualWidth / 4,
            };

            DisplayNotes.Add(newNote);
            var timer = new DispatcherTimer(TimeSpan.FromSeconds(5), DispatcherPriority.Render, (sender, args) =>
            {
                if (sender is DispatcherTimer timer)
                {
                    timer.Stop();
                }
                DisplayNotes.Remove(newNote);
                OnPropertyChanged(nameof(DisplayNotes));
            }, Dispatcher.CurrentDispatcher);
            OnPropertyChanged(nameof(DisplayNotes));
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
            OnPropertyChanged(nameof(NoDataVisibility));
            OnPropertyChanged(nameof(DataVisibility));

            OnPropertyChanged(nameof(NewAlert));
            OnPropertyChanged(nameof(NewAlertVisibility));
            OnPropertyChanged(nameof(AlertIcon));
            OnPropertyChanged(nameof(AlertColor));
            OnPropertyChanged(nameof(NewNotificationCount));
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

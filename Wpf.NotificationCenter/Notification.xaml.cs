using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace Wpf.NotificationCenter
{
    /// <summary>
    ///     Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Fields

        public static readonly DependencyProperty NotificationTypeProperty = DependencyProperty.Register(
            nameof(NotificationType),
            typeof(NotificationType),
            typeof(Notification),
            new PropertyMetadata(default(NotificationType))
        );

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(string.Empty)
        );

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(string.Empty)
        );

        public static readonly DependencyProperty UnreadProperty = DependencyProperty.Register(
            nameof(Unread),
            typeof(bool),
            typeof(Notification),
            new PropertyMetadata(true)
        );

        #endregion

        #region Properties

        public Brush IconBrush => ConvertTypeToBrush(NotificationType);

        public virtual SolidColorBrush ConvertTypeToBrush(NotificationType type) => type switch
        {
            NotificationType.Information => Brushes.Blue,
            NotificationType.Error => Brushes.Red,
            NotificationType.Warning => Brushes.DarkGoldenrod,
            NotificationType.Success => Brushes.MediumSeaGreen,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };

        public NotificationType NotificationType
        {
            get => (NotificationType) GetValue(NotificationTypeProperty);
            set => SetValue(NotificationTypeProperty, value);
        }

        public string? Text
        {
            get => GetValue(TextProperty)?.ToString();
            set => SetValue(TextProperty, value);
        }

        public string? Title
        {
            get => GetValue(TitleProperty)?.ToString();
            set => SetValue(TitleProperty, value);
        }

        public FontWeight TitleWeight => Unread ? FontWeights.Bold : FontWeights.Medium;

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

        static Notification() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));

        public Notification()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <inheritdoc />
        protected override void OnExpanded()
        {
            Unread = false;
            base.OnExpanded();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}

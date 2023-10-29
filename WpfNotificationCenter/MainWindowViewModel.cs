using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Wpf.NotificationCenter.Enums;
using Wpf.NotificationCenter.Services;

namespace WpfNotificationCenter
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Fields

        private readonly IWpfNotificationService notificationService;
        private double alertMaxHeight = 200d;
        private double alertMaxWidth = 250d;
        private string alertText = "Alert Text";
        private AlertType selectedAlertType = AlertType.All;
        private HorizontalAlignment selectedHorizontalAlignment = HorizontalAlignment.Right;
        private NotificationType selectedNotificationType = NotificationType.Information;
        private VerticalAlignment selectedVerticalAlignment = VerticalAlignment.Bottom;
        private bool showInContent;
        private bool showInHeader = true;
        private ComboBoxItem themeLightDark = new() {Content = "Light", IsSelected = true};
        private ComboBoxItem themePrimaryColor = new() {Content = "DeepPurple", IsSelected = true};
        private ComboBoxItem themeSecondaryColor = new() {Content = "Amber", IsSelected = true};
        private ComboBoxItem notificationBackground = new() { Content = "Wheat", IsSelected = true};

        #endregion

        #region Properties

        public double AlertMaxHeight
        {
            get => alertMaxHeight;
            set => SetField(ref alertMaxHeight, value);
        }

        public double AlertMaxWidth
        {
            get => alertMaxWidth;
            set => SetField(ref alertMaxWidth, value);
        }

        public string AlertText
        {
            get => alertText;
            set
            {
                if (SetField(ref alertText, value))
                {
                    OnPropertyChanged(nameof(CreateNotificationCommand));
                }
            }
        }

        public static List<AlertType> AlertTypes => Enum.GetValues<AlertType>().ToList();

        public IEnumerable<ComboBoxItem> BrushList => typeof(Colors)
            .GetProperties(BindingFlags.Static | BindingFlags.Public)
            .Select(x=>new ComboBoxItem()
            {
                Content = x.Name,
            });

        public static IEnumerable<ComboBoxItem> ColorItemsAccent => new List<ComboBoxItem>
        {
            new() {Content = "Amber"},
            new() {Content = "Blue"},
            new() {Content = "Cyan"},
            new() {Content = "DeepOrange"},
            new() {Content = "DeepPurple"},
            new() {Content = "Green"},
            new() {Content = "Indigo"},
            new() {Content = "LightBlue"},
            new() {Content = "LightGreen"},
            new() {Content = "Lime"},
            new() {Content = "Orange"},
            new() {Content = "Pink"},
            new() {Content = "Purple"},
            new() {Content = "Red"},
            new() {Content = "Teal"},
            new() {Content = "Yellow"}
        };

        public IEnumerable<ComboBoxItem> ColorItemsPrimary => new List<ComboBoxItem>
        {
            new() {Content = "Amber"},
            new() {Content = "Blue"},
            new() {Content = "BlueGrey"},
            new() {Content = "Brown"},
            new() {Content = "Cyan"},
            new() {Content = "DeepOrange"},
            new() {Content = "DeepPurple"},
            new() {Content = "Green"},
            new() {Content = "Grey"},
            new() {Content = "Indigo"},
            new() {Content = "LightBlue"},
            new() {Content = "LightGreen"},
            new() {Content = "Lime"},
            new() {Content = "Orange"},
            new() {Content = "Pink"},
            new() {Content = "Purple"},
            new() {Content = "Red"},
            new() {Content = "Teal"},
            new() {Content = "Yellow"}
        };

        public ICommand CreateNotificationCommand => new RelayCommand(() =>
            {
                var alertType = SelectedAlertType;
                var notificationType = SelectedNotificationType;

                notificationService.Create($"{notificationType} {DateTime.Now.ToLongTimeString()}",
                    AlertText,
                    notificationType,
                    alertType: alertType
                );
            }
        );

        public static List<HorizontalAlignment> HorizontalAlignments => Enum.GetValues<HorizontalAlignment>().ToList();

        public ComboBoxItem NotificationBackground
        {
            get => notificationBackground;
            set => SetField(ref notificationBackground, value);
        }

        public static List<NotificationType> NotificationTypes => Enum.GetValues<NotificationType>().ToList();

        public AlertType SelectedAlertType
        {
            get => selectedAlertType;
            set
            {
                if (SetField(ref selectedAlertType, value))
                {
                    OnPropertyChanged(nameof(CreateNotificationCommand));
                }
            }
        }

        public HorizontalAlignment SelectedHorizontalAlignment
        {
            get => selectedHorizontalAlignment;
            set => SetField(ref selectedHorizontalAlignment, value);
        }

        public NotificationType SelectedNotificationType
        {
            get => selectedNotificationType;
            set
            {
                if (SetField(ref selectedNotificationType, value))
                {
                    OnPropertyChanged(nameof(CreateNotificationCommand));
                }
            }
        }

        public VerticalAlignment SelectedVerticalAlignment
        {
            get => selectedVerticalAlignment;
            set => SetField(ref selectedVerticalAlignment, value);
        }

        public bool ShowInContent
        {
            get => showInContent;
            set => SetField(ref showInContent, value);
        }

        public bool ShowInHeader
        {
            get => showInHeader;
            set => SetField(ref showInHeader, value);
        }

        public ComboBoxItem ThemeLightDark
        {
            get => themeLightDark;
            set => SetField(ref themeLightDark, value);
        }

        public ComboBoxItem ThemePrimaryColor
        {
            get => themePrimaryColor;
            set => SetField(ref themePrimaryColor, value);
        }

        public ComboBoxItem ThemeSecondaryColor
        {
            get => themeSecondaryColor;
            set => SetField(ref themeSecondaryColor, value);
        }

        public static List<VerticalAlignment> VerticalAlignments => Enum.GetValues<VerticalAlignment>().ToList();

        #endregion

        public MainWindowViewModel(IWpfNotificationService notificationService)
        {
            this.notificationService = notificationService;
            PropertyChanged += (sender, args) => ApplyResources();
            ApplyResources();
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

        private static void AddDict(Uri primarySrc, MainWindow? a)
        {
            var dict = new ResourceDictionary
            {
                Source = primarySrc
            };

            foreach (var mergeDict in dict.MergedDictionaries)
            {
                a.Resources.MergedDictionaries.Add(mergeDict);
            }

            foreach (var key in dict.Keys)
            {
                a.Resources[key] = dict[key];
            }
        }

        private void ApplyResources()
        {
            var themeSrc = new Uri(
                $"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{ThemeLightDark.Content}.xaml"
            );

            var primarySrc =
                new Uri(
                    $"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{ThemePrimaryColor.Content}.xaml"
                );

            var secondarySrc =
                new Uri(
                    $"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.{ThemeSecondaryColor.Content}.xaml"
                );

            var a = Application.Current.MainWindow as MainWindow;

            AddDict(themeSrc, a);
            AddDict(primarySrc, a);
            AddDict(secondarySrc, a);
        }
    }
}

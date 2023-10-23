using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
        private VerticalAlignment selectedVerticalAlignment = VerticalAlignment.Bottom;
        private HorizontalAlignment selectedHorizontalAlignment = HorizontalAlignment.Right;
        private bool showInHeader = true;
        private bool showInContent = false;
        private PlacementMode placementMode = PlacementMode.Bottom;
        private AlertType selectedAlertType = AlertType.All;
        private double alertMaxHeight = 200d;
        private string alertText = "Alert Text";
        private double alertMaxWidth = 250d;
        private NotificationType selectedNotificationType = NotificationType.Information;

        #endregion

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

        public List<PlacementMode> PlacementModes => Enum.GetValues<PlacementMode>().ToList();
        public List<AlertType> AlertTypes => Enum.GetValues<AlertType>().ToList();
        public List<NotificationType> NotificationTypes => Enum.GetValues<NotificationType>().ToList();
        public List<HorizontalAlignment> HorizontalAlignments => Enum.GetValues<HorizontalAlignment>().ToList();
        public List<VerticalAlignment> VerticalAlignments => Enum.GetValues<VerticalAlignment>().ToList();

        public double AlertMaxWidth
        {
            get => alertMaxWidth;
            set => SetField(ref alertMaxWidth, value);
        }

        public double AlertMaxHeight
        {
            get => alertMaxHeight;
            set => SetField(ref alertMaxHeight, value);
        }

        #region Properties

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

        public HorizontalAlignment SelectedHorizontalAlignment
        {
            get => selectedHorizontalAlignment;
            set => SetField(ref selectedHorizontalAlignment, value);
        }

        public PlacementMode PlacementMode
        {
            get => placementMode;
            set => SetField(ref placementMode, value);
        }

        public VerticalAlignment SelectedVerticalAlignment
        {
            get => selectedVerticalAlignment;
            set => SetField(ref selectedVerticalAlignment, value);
        }

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

        public ICommand CreateNotificationCommand => new RelayCommand(() =>
            {
                var alertType = SelectedAlertType;
                var notificationType = SelectedNotificationType;

                notificationService.Create($"{notificationType} {DateTime.Now.ToLongTimeString()}",
                    AlertText,
                    notificationType, alertType: alertType
                );
            }
        );

        public bool ShowInHeader
        {
            get => showInHeader;
            set => SetField(ref showInHeader, value);
        }

        public bool ShowInContent
        {
            get => showInContent;
            set => SetField(ref showInContent, value);
        }

        #endregion

        public MainWindowViewModel(IWpfNotificationService notificationService) => this.notificationService = notificationService;

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

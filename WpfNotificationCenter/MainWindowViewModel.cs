using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        #endregion

        public string AlertText { get; set; } = "Alert Text";
        public List<AlertType> AlertTypes => Enum.GetValues<AlertType>().ToList();
        public List<NotificationType> NotificationTypes => Enum.GetValues<NotificationType>().ToList();

        #region Properties
        public NotificationType SelectedNotificationType { get; set; }
        public AlertType SelectedAlertType { get; set; }

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

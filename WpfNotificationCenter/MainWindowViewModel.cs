using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #region Properties

        public ICommand CreateNotification2Command => new RelayCommand<NotificationType>(t =>
            {
                notificationService.Create($"{t} {DateTime.Now.ToLongTimeString()}",
                    $"{t}\n{DateTime.Now}",
                    t,
                    "NotificationCenter2",
                    AlertType.NotificationCenter
                );
            }
        );

        public ICommand CreateNotificationCommand => new RelayCommand<NotificationType>(t =>
            {
                notificationService.Create($"{t} {DateTime.Now.ToLongTimeString()}",
                    $"kls lasdfng oksdnf lgk;sndfkljg nserdoig jseriog jseoirpgj seopirg jseporgjsedprog jnserpog jserpoig sjeroigp sjeroipg sjeoirg jseroipgjseroig j\n dfhng iousdhf kjlasdbfnkjl asnrdfiklja slern fgljksednrfg kljsenrg oisenrg kjnsergoi nseroikg nseior gosiper gjseoirg nsekljrng soielrgn soeipr jgseriogj seorigj seroipg jseroigj seroipg jseroipg jseroip gjseroipg jesaoirgpj\nsndfkljawn fkjlasdnf kjlawenf iulajwenf iuawen fjklawenf iuawlen faiowuef nwaeiukjln{t}\n{DateTime.Now}",
                    t
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

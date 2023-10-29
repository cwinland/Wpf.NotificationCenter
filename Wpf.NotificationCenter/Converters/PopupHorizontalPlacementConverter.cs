using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.NotificationCenter.Converters
{
    /// <summary>
    ///     Class PopupHorizontalPlacementConverter.
    ///     Implements the <see cref="IMultiValueConverter" />
    /// </summary>
    /// <inheritdoc />
    /// <seealso cref="IMultiValueConverter" />
    public class PopupHorizontalPlacementConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        /// <inheritdoc />
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null)
            {
                return 0d;
            }

            var popup = values.OfType<NotificationPopup>().First();
            var alignment = values.OfType<HorizontalAlignment>().First();
            var popupWidth = popup.ActualWidth > 0 ? popup.ActualWidth : popup.Width;

            return alignment switch
            {
                HorizontalAlignment.Left when popupWidth > 0 => popupWidth,
                HorizontalAlignment.Right => 0d,
                HorizontalAlignment.Center => popupWidth / 2,
                _ => popup.HorizontalOffset,
            };
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion
    }
}

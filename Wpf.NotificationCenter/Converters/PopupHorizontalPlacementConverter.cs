using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.NotificationCenter.Converters
{
    /// <summary>
    ///     Class PopupHorizontalPlacementConverter.
    ///     Implements the <see cref="IMultiValueConverter" />
    /// </summary>
    /// <inheritdoc />
    /// <seealso cref="IMultiValueConverter" />
    internal class PopupHorizontalPlacementConverter : IMultiValueConverter
    {
        #region Implementation of IMultiValueConverter

        public double LeftOffset { get; set; }
        public double CenterOffset { get; set; }
        public double RightOffset { get; set; }

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
                HorizontalAlignment.Left when popupWidth > 0 => popupWidth - LeftOffset,
                HorizontalAlignment.Center => popupWidth / 2 - CenterOffset,
                HorizontalAlignment.Right => 0d - RightOffset,
                HorizontalAlignment.Stretch => 0d,
                _ => popup.HorizontalOffset, // This is to make sure that a default 0 is not applied
            };
        }

        /// <inheritdoc />
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion
    }
}

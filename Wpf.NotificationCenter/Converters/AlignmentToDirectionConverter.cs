using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Wpf.NotificationCenter.Converters
{
    /// <summary>
    ///     Class AlignmentToDirectionConverter.
    ///     Implements the <see cref="IValueConverter" />
    /// </summary>
    /// <inheritdoc />
    /// <seealso cref="IValueConverter" />
    internal class AlignmentToDirectionConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not HorizontalAlignment alignment)
            {
                return 0d;
            }

            return alignment switch
            {
                HorizontalAlignment.Center => 270d,
                HorizontalAlignment.Left => 315d,
                HorizontalAlignment.Right => 225d,
                HorizontalAlignment.Stretch => 270d,
                _ => 0d,
            };

        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

        #endregion
    }
}

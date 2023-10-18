using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Wpf.NotificationCenter.Converters
{
    /// <summary>
    ///     Class ExpanderRotateAngleConverter.
    /// Implements the <see cref="IValueConverter" />
    /// </summary>
    /// <seealso cref="IValueConverter" />
    /// <inheritdoc />
    public class ExpanderRotateAngleConverter : IValueConverter
    {
        /// <inheritdoc />
        /// <summary>
        ///     Converts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.Object.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var factor = 1.0;
            if (parameter != null && !double.TryParse(parameter.ToString(), out factor))
            {
                factor = 1.0;
            }

            return value switch
            {
                ExpandDirection.Left => 90 * factor,
                ExpandDirection.Right => -90 * factor,
                _ => 0,
            };
        }

        /// <summary>
        ///     Converts the back.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>System.Object.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

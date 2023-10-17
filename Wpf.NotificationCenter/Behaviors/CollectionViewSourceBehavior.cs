using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Wpf.NotificationCenter.Behaviors
{
    /// <summary>
    ///     Class CollectionViewSourceBehavior.
    /// </summary>
    public static class CollectionViewSourceBehavior
    {
        #region Fields

        /// <summary>
        ///     Gets if the source is in the ascending sort direction.
        /// </summary>
        public static readonly DependencyProperty IsAscendingProperty =
            DependencyProperty.RegisterAttached(
                "IsAscending",
                typeof(bool),
                typeof(CollectionViewSourceBehavior),
                new UIPropertyMetadata(false, OnIsAscendingChanged)
            );

        #endregion

        /// <summary>
        ///     Gets if the source is in the ascending sort direction.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>System ofObject of the is ascending.</returns>
        public static object GetIsAscending(FrameworkElement element) => element.GetValue(IsAscendingProperty);

        /// <summary>
        ///     Handles the <see cref="E:IsAscendingChanged" /> event.
        /// </summary>
        /// <param name="dependencyObject">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        public static void OnIsAscendingChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (dependencyObject is not CollectionViewSource collectionViewSource)
            {
                return;
            }

            var isAscending = e.NewValue as bool? == true;

            var newSortDescription = new SortDescription
            {
                Direction = isAscending ? ListSortDirection.Ascending : ListSortDirection.Descending,
                PropertyName = collectionViewSource.SortDescriptions.FirstOrDefault().PropertyName
            };

            collectionViewSource.SortDescriptions.Clear();
            collectionViewSource.SortDescriptions.Add(newSortDescription);
        }

        /// <summary>
        ///     Sets if the source is in the ascending sort direction.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetIsAscending(FrameworkElement element, object value) => element.SetValue(IsAscendingProperty, value);
    }
}

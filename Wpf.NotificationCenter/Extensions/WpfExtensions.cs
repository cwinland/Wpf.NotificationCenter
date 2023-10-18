using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using Wpf.NotificationCenter.Services;

namespace Wpf.NotificationCenter.Extensions
{
    /// <summary>
    ///     Class WpfExtensions.
    /// </summary>
    public static class WpfExtensions
    {
        /// <summary>
        ///     Uses the WPF notification center.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>Uses the WPF notification center.</returns>
        [ExcludeFromCodeCoverage]
        public static IServiceCollection UseWpfNotificationCenter(this IServiceCollection services)
        {
            services.AddSingleton<IWpfNotificationService, WpfNotificationService>()
                .AddSingleton<NotificationCenter>();

            return services;
        }

        /// <summary>
        ///     Finds a Child of a given item in the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <param name="childName">x:Name or Name of child.</param>
        /// <returns>
        ///     The first parent item that matches the submitted type parameter.
        ///     If not matching item can be found,
        ///     a null parent is being returned.
        /// </returns>
        [ExcludeFromCodeCoverage]
        internal static T? FindChild<T>(this DependencyObject? parent, string? childName = "") where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            parent ??= Application.Current.MainWindow;

            T? foundChild = null;
            IEnumerable<DependencyObject> children = LogicalTreeHelper.GetChildren(parent).OfType<DependencyObject>();

            foreach (var child in children)
            {
                if (child is not T childType)
                {
                    // recursively drill down the tree
                    foundChild = child.FindChild<T>(childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (childType is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = childType;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = childType;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        ///     Removes the child.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="child">The child.</param>
        [ExcludeFromCodeCoverage]
        internal static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            switch (parent)
            {
                case Panel panel:
                    panel.Children.Remove(child);
                    return;
                case Decorator decorator:
                {
                    if (decorator.Child == child)
                    {
                        decorator.Child = null;
                    }

                    return;
                }
                case ContentPresenter contentPresenter:
                {
                    if (Equals(contentPresenter.Content, child))
                    {
                        contentPresenter.Content = null;
                    }

                    return;
                }
                case ContentControl contentControl:
                {
                    if (Equals(contentControl.Content, child))
                    {
                        contentControl.Content = null;
                    }

                    break;
                }
                default:
                    throw new NotSupportedException($"{parent.GetType().Name} not supported.");
            }
        }
    }
}

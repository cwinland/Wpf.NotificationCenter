using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.NotificationCenter.Extensions
{
    public static class WpfExtensions
    {
        public static void RemoveChild(this DependencyObject parent, UIElement child)
        {
            var panel = parent as Panel;
            if (panel != null)
            {
                panel.Children.Remove(child);
                return;
            }

            var decorator = parent as Decorator;
            if (decorator != null)
            {
                if (decorator.Child == child)
                {
                    decorator.Child = null;
                }
                return;
            }

            var contentPresenter = parent as ContentPresenter;
            if (contentPresenter != null)
            {
                if (contentPresenter.Content == child)
                {
                    contentPresenter.Content = null;
                }
                return;
            }

            var contentControl = parent as ContentControl;
            if (contentControl != null)
            {
                if (contentControl.Content == child)
                {
                    contentControl.Content = null;
                }
                return;
            }

            // maybe more
        }
        public static T? FindChild<T>(this ContentControl control, DependencyObject? parent = null, string childName = "") where T : DependencyObject => FindChild<T>(parent, childName);

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T? FindChild<T>(this DependencyObject? parent, string childName = "") where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            parent ??= Application.Current.MainWindow;
            T foundChild = null;
            var children = LogicalTreeHelper.GetChildren(parent).OfType<DependencyObject>();
            foreach (var child in children)
            {
                if (child is not T childType)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
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
    }
}

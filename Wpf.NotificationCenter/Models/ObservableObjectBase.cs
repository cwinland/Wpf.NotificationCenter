using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Wpf.NotificationCenter.Models
{
    /// <summary>
    ///     Class ObservableObjectBase.
    ///     Implements the <see cref="ObservableObject" />
    ///     Implements the <see cref="IDisposable" />
    /// </summary>
    /// <inheritDoc cref="ObservableObject" />
    /// <inheritDoc cref="IDisposable" />
    /// <seealso cref="ObservableObject" />
    /// <seealso cref="IDisposable" />
    public abstract class ObservableObjectBase : ObservableObject, IDisposable
    {
        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { }
        }

        /// <summary>
        ///     Sets the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="additionalPropertyNames">The additional property names.</param>
        /// <returns><c>true</c> if property was changed, <c>false</c> otherwise.</returns>
        /// <seealso cref="ObservableObject.SetProperty{T}(ref T,T,string?)" />
        protected bool SetProperty<T>([NotNullIfNotNull(nameof(newValue))] ref T field, T newValue, [CallerMemberName] string? propertyName = null,
            params string[] additionalPropertyNames)
        {
            var result = base.SetProperty(ref field, newValue, propertyName);

            foreach (var additionalPropertyName in additionalPropertyNames)
            {
                OnPropertyChanged(additionalPropertyName);
            }

            return result;
        }

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Wpf.NotificationCenter
{
    public abstract class ObservableObjectBase : ObservableObject, IDisposable
    {
        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
        #endregion
    }
}

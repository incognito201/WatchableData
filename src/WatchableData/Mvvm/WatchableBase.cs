using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WatchableData.Mvvm
{
    public abstract class WatchableBase : BindableBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return _errors.Values;
            }

            if (!_errors.ContainsKey(propertyName))
            {
                return Enumerable.Empty<List<string>>();
            }

            return _errors[propertyName];
        }

        public bool HasErrors
        {
            get => _errors.Count > 0;
        }

        protected void AddError(string error, [CallerMemberName] string propertyName = null)
        {
            RemoveErrors(propertyName);
            AddErrors(new List<string> { error }, propertyName);
        }

        protected void AddErrors(List<string> error, [CallerMemberName] string propertyName = null)
        {
            _errors[propertyName] = error;
            RaisePropertyErrorChanged(propertyName);
        }

        protected void RemoveErrors([CallerMemberName] string propertyName = "")
        {
            _errors.Remove(propertyName);
            RaisePropertyErrorChanged(propertyName);
        }

        protected void RaisePropertyErrorChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyErrorChanged(new DataErrorsChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyErrorChanged(DataErrorsChangedEventArgs args)
        {
            ErrorsChanged?.Invoke(this, args);
        }
    }
}

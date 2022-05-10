using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;

namespace WatchableData.Mvvm
{
    public abstract class WatchableBase : BindableBase, INotifyDataErrorInfo, ICleanup
    {
        private readonly Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected readonly CompositeDisposable Disposer = new CompositeDisposable();

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

        private bool _hasErrors;
        public bool HasErrors
        {
            get { return _hasErrors; }
            private set { SetProperty(ref _hasErrors, value); }
        }

        protected void AddError(string error, [CallerMemberName] string propertyName = null)
        {
            RemoveErrors(propertyName);
            AddErrors(new List<string> { error }, propertyName);
        }

        protected void AddErrors(List<string> error, [CallerMemberName] string propertyName = null)
        {
            _errors[propertyName] = error;
            HasErrors = _errors.Count > 0;
            RaisePropertyErrorChanged(propertyName);
        }

        protected void RemoveErrors([CallerMemberName] string propertyName = "")
        {
            _errors.Remove(propertyName);
            HasErrors = _errors.Count > 0;
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

        public virtual void Cleanup()
        {
            if (!Disposer.IsDisposed)
            {
                Disposer.Dispose();
            }
        }
    }
}

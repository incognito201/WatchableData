using WatchableData.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WatchableData.Mvvm;

namespace WatchableData.Collection
{
    public class WatchableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly bool _cleanup;

        public event EventHandler<PropertyChangedEventArgs<T>> ItemPropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs<T>> ItemPropertyErrorsChanged;
        public event EventHandler<ICollection<T>> Cleaned;

        public WatchableCollection(bool cleanup = true)
        {
            _cleanup = cleanup;
        }

        public WatchableCollection(IEnumerable<T> list, bool cleanup = true)
            : this(cleanup)
        {
            foreach(var item in list)
            {
                this.Add(item);
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new PropertyChangedEventArgs<T>(e.PropertyName, (T)sender);
            ItemPropertyChanged?.Invoke(this, args);
        }

        private void Item_ErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var args = new DataErrorsChangedEventArgs<T>(e.PropertyName, (T)sender);
            ItemPropertyErrorsChanged?.Invoke(this, args);
        }

        private void RegisterItem(T item)
        {
            WeakEventManager<T, PropertyChangedEventArgs>.AddHandler(item, "PropertyChanged", Item_PropertyChanged);
            WeakEventManager<T, DataErrorsChangedEventArgs>.AddHandler(item, "ErrorsChanged", Item_ErrorsChanged);
        }

        private void UnregisterItem(T item)
        {
            WeakEventManager<T, PropertyChangedEventArgs>.RemoveHandler(item, "PropertyChanged", Item_PropertyChanged);
            WeakEventManager<T, DataErrorsChangedEventArgs>.RemoveHandler(item, "ErrorsChanged", Item_ErrorsChanged);
            if(_cleanup && item is ICleanup cleanup)
            {
                cleanup.Cleanup();
            }
        }

        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, item);
            RegisterItem(item);
        }

        protected override void RemoveItem(int index)
        {
            var removed = this.Items[index];
            base.RemoveItem(index);
            UnregisterItem(removed);
        }

        protected override void SetItem(int index, T item)
        {
            var oldItem = this.Items[index];
            base.SetItem(index, item);
            UnregisterItem(oldItem);
            RegisterItem(item);
        }

        protected override void ClearItems()
        {
            var removeds = new List<T>(this.Items);
            base.ClearItems();
            foreach (var item in removeds)
            {
                UnregisterItem(item);
            }
            Cleaned?.Invoke(this, removeds);
        }
    }
}

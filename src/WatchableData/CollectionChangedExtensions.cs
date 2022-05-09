using System;
using System.Linq;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Collections.ObjectModel;
using WatchableData.Models;

namespace WatchableData
{
    public static class CollectionChangedExtensions
    {
        public static IObservable<NotifyCollectionChangedEventArgs> WhenCollectionChanged(this INotifyCollectionChanged source)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(source, nameof(source.CollectionChanged))
                .Select(e => e.EventArgs);
        }

        public static IObservable<T> WhenAdded<T>(this ObservableCollection<T> source)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(source, nameof(source.CollectionChanged))
                .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Add)
                .Select(e => e.EventArgs.NewItems.Cast<T>().First());
        }

        public static IObservable<T> WhenRemoved<T>(this ObservableCollection<T> source)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(source, nameof(source.CollectionChanged))
                .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Remove)
                .Select(e => e.EventArgs.OldItems.Cast<T>().First());
        }

        public static IObservable<ItemReplacedEventArgs<T>> WhenReplaced<T>(this ObservableCollection<T> source)
        {
            return Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(source, nameof(source.CollectionChanged))
                .Where(e => e.EventArgs.Action == NotifyCollectionChangedAction.Replace)
                .Select(e => new ItemReplacedEventArgs<T>(
                                e.EventArgs.NewItems.Cast<T>().First(),
                                e.EventArgs.OldItems.Cast<T>().First()));
        }
    }
}

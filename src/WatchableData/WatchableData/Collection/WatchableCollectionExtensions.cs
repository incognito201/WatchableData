using WatchableData.Models;
using WatchableData.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace WatchableData.Collection
{
    public static class WatchableCollectionExtensions
    {
        public static IObservable<PropertyChangedEventArgs<T>> WhenItemPropertyChanged<T>(this WatchableCollection<T> source, string propertyName)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            return Observable.FromEventPattern<PropertyChangedEventArgs<T>>(
                    h => source.ItemPropertyChanged += h,
                    h => source.ItemPropertyChanged -= h)
                .Select(e => e.EventArgs)
                .Where(e => e.PropertyName == propertyName);
        }

        public static IObservable<DataErrorsChangedEventArgs<T>> WhenItemPropertyErrorsChanged<T>(this WatchableCollection<T> source, string propertyName)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            return Observable.FromEventPattern<DataErrorsChangedEventArgs<T>>(
                    h => source.ItemPropertyErrorsChanged += h,
                    h => source.ItemPropertyErrorsChanged -= h)
                .Select(e => e.EventArgs)
                .Where(e => e.PropertyName == propertyName);
        }

        public static IObservable<PropertyChangedEventArgs<T>> WhenAnyItemPropertyChanged<T>(this WatchableCollection<T> source, params string[] properties)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            return Observable.FromEventPattern<PropertyChangedEventArgs<T>>(
                    h => source.ItemPropertyChanged += h,
                    h => source.ItemPropertyChanged -= h)
                .Select(e => e.EventArgs)
                .Where(e => properties.Any(p => p == e.PropertyName));
        }

        public static IObservable<DataErrorsChangedEventArgs<T>> WhenAnyItemPropertyErrosChanged<T>(this WatchableCollection<T> source, params string[] properties)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            return Observable.FromEventPattern<DataErrorsChangedEventArgs<T>>(
                    h => source.ItemPropertyErrorsChanged += h,
                    h => source.ItemPropertyErrorsChanged -= h)
                .Select(e => e.EventArgs)
                .Where(e => properties.Any(p => p == e.PropertyName));
        }

        public static IObservable<PropertyChangedEventArgs<T>> WhenItemPropertyChanged<T, TRet>(this WatchableCollection<T> source, Expression<Func<T, TRet>> propertyLambda)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
            => WhenItemPropertyChanged(source, propertyLambda.GetPropertyName());

        public static IObservable<DataErrorsChangedEventArgs<T>> WhenItemPropertyErrorsChanged<T, TRet>(this WatchableCollection<T> source, Expression<Func<T, TRet>> propertyLambda)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
            => WhenItemPropertyErrorsChanged(source, propertyLambda.GetPropertyName());

        public static IObservable<PropertyChangedEventArgs<T>> WhenAnyItemPropertyChanged<T>(this WatchableCollection<T> source, params Expression<Func<T, object>>[] properties)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
            => WhenAnyItemPropertyChanged(source, properties.Select(p => p.GetMemberInfo().Name).ToArray());

        public static IObservable<DataErrorsChangedEventArgs<T>> WhenAnyItemPropertyErrosChanged<T>(this WatchableCollection<T> source, params Expression<Func<T, object>>[] properties)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
            => WhenAnyItemPropertyErrosChanged(source, properties.Select(p => p.GetMemberInfo().Name).ToArray());

        public static IObservable<ICollection<T>> WhenCleaned<T>(this WatchableCollection<T> source)
            where T : INotifyPropertyChanged, INotifyDataErrorInfo
        {
            return Observable.FromEventPattern<ICollection<T>>(
                    h => source.Cleaned += h,
                    h => source.Cleaned -= h)
                .Select(e => e.EventArgs);
        }
    }
}

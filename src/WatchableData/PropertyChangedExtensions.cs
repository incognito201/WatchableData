using WatchableData.Utils;
using System;
using System.Linq;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace WatchableData
{
    public static class PropertyChangedExtensions
    {
        public static IObservable<PropertyChangedEventArgs> WhenPropertyChanged(this INotifyPropertyChanged source, string propertyName)
        {
            return Observable.FromEventPattern<PropertyChangedEventArgs>(source, nameof(source.PropertyChanged))
                .Select(e => e.EventArgs)
                .Where(e => e.PropertyName == propertyName);
        }

        public static IObservable<PropertyChangedEventArgs> WhenPropertyChanged<T, TRet>(this T source, Expression<Func<T, TRet>> propertyLambda)
            where T : INotifyPropertyChanged
            => WhenPropertyChanged(source, propertyLambda.GetPropertyName());

        public static IObservable<PropertyChangedEventArgs> WhenAnyPropertyChanged(this INotifyPropertyChanged source, params string[] properties)
        {
            var observable = Observable.FromEventPattern<PropertyChangedEventArgs>(source, nameof(source.PropertyChanged))
                .Select(e => e.EventArgs);

            if (properties.Any())
            {
                return observable
                    .Where(e => properties.Any(p => p == e.PropertyName));
            }
            return observable;
        }

        public static IObservable<PropertyChangedEventArgs> WhenAnyPropertyChanged<T>(this T source, params Expression<Func<T, object>>[] properties)
            where T : INotifyPropertyChanged
            => WhenAnyPropertyChanged(source, properties.Select(p => p.GetMemberInfo().Name).ToArray());
    }
}

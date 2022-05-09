using WatchableData.Utils;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace WatchableData
{
    public static class NotifyDataErrorInfoExtensions
    {
        public static IObservable<DataErrorsChangedEventArgs> WhenPropertyErrorsChanged(this INotifyDataErrorInfo source, string propertyName)
        {
            return Observable.FromEventPattern<DataErrorsChangedEventArgs>(source, nameof(source.ErrorsChanged))
                .Select(e => e.EventArgs)
                .Where(e => e.PropertyName == propertyName);
        }

        public static IObservable<DataErrorsChangedEventArgs> WhenPropertyErrorsChanged<T, TRet>(this T source, Expression<Func<T, TRet>> propertyLambda)
            where T : INotifyDataErrorInfo
            => WhenPropertyErrorsChanged(source, propertyLambda.GetPropertyName());

        public static IObservable<DataErrorsChangedEventArgs> WhenAnyPropertyErrorsChanged(this INotifyDataErrorInfo source, params string[] properties)
        {
            var observable = Observable.FromEventPattern<DataErrorsChangedEventArgs>(source, nameof(source.ErrorsChanged))
                .Select(e => e.EventArgs);

            if (properties.Any())
            {
                return observable
                    .Where(e => properties.Any(p => p == e.PropertyName));
            }
            return observable;
        }

        public static IObservable<DataErrorsChangedEventArgs> WhenAnyPropertyErrorsChanged<T>(this T source, params Expression<Func<T, object>>[] properties)
            where T : INotifyDataErrorInfo
            => WhenAnyPropertyErrorsChanged(source, properties.Select(p => p.GetMemberInfo().Name).ToArray());
    }
}

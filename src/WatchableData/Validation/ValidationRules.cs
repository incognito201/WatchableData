using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using WatchableData.Mvvm;
using WatchableData.Utils;

namespace WatchableData.Validation
{
    public static class ValidationRules
    {
        public static IDisposable ApplyRule(this IObservable<ValidationArgs> observable, string error, Action onInvalid = null, Action onValid = null)
        {
            return observable.Subscribe(e =>
            {
                string propertyName = e.PropertyName;
                var source = e.Source;
                if (e.IsInvalid)
                {
                    source.AddError(error, propertyName);
                    onInvalid?.Invoke();
                }
                else
                {
                    source.RemoveErrors(propertyName);
                    onValid?.Invoke();
                }
            });
        }

        public static IObservable<ValidationArgs> LessThan<TValue>(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source, TValue value)
            where TValue : IComparable
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = ((TValue)source.GetPropertyValue(e.PropertyName)).IsLessThan(value)
                    };
                });
        }

        public static IObservable<ValidationArgs> GreaterThan<TValue>(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source, TValue value)
            where TValue : IComparable
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = ((TValue)source.GetPropertyValue(e.PropertyName)).IsGreaterThan(value)
                    };
                });
        }

        public static IObservable<ValidationArgs> Equals<TValue>(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source, TValue value)
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = EqualityComparer<TValue>.Default.Equals((TValue)source.GetPropertyValue(e.PropertyName), value)
                    };
                });
        }

        public static IObservable<ValidationArgs> Null(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source)
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = source.GetPropertyValue(e.PropertyName) is null
                    };
                });
        }

        public static IObservable<ValidationArgs> NullOrEmpty(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source)
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = string.IsNullOrEmpty((string)source.GetPropertyValue(e.PropertyName))
                    };
                });
        }

        public static IObservable<ValidationArgs> NullOrWhiteSpace(this IObservable<PropertyChangedEventArgs> observable, WatchableBase source)
        {
            return observable
                .Select(e =>
                {
                    return new ValidationArgs
                    {
                        PropertyName = e.PropertyName,
                        Source = source,
                        IsInvalid = string.IsNullOrWhiteSpace((string)source.GetPropertyValue(e.PropertyName))
                    };
                });
        }
    }
}

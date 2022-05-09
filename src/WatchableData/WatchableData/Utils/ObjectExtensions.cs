using System.Linq;

namespace WatchableData.Utils
{
    public static class ObjectExtensions
    {
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj
                .GetType()
                .GetProperties()
                .Single(pi => pi.Name == propertyName)
                .GetValue(obj, null);
        }
    }
}

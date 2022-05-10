using WatchableData.Mvvm;

namespace WatchableData.Validation
{
    public class ValidationArgs
    {
        public WatchableBase Source { get; set; }
        public string PropertyName { get; set; }
        public bool IsInvalid { get; set; }
    }
}

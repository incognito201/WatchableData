using System.Linq;
using WatchableData.Collection;
using WatchableData.Mvvm;
using System;
using System.Reactive.Linq;

namespace WatchableData.Demo.Fody
{
    public class MainViewModel : WatchableBase
    {
        public MainViewModel()
        {
            OrderItems = new WatchableCollection<OrderItem>(OrderItemService.GetAll());

            OrderItems.WhenAnyItemPropertyChanged(x => x.Quantity, x => x.Price)
                .Where(e => !e.Source.HasErrors)
                .Subscribe(e =>
                {
                    RaisePropertyChanged(nameof(Total));
                });
        }

        public decimal Total
        {
            get => OrderItems.Sum(i => i.Quantity * i.Price);
        }

        public WatchableCollection<OrderItem> OrderItems { get; }
    }
}

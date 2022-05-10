using WatchableData.Mvvm;
using WatchableData.Validation;

namespace WatchableData.Demo.Fody
{
    public class OrderItem : WatchableBase
    {
        public OrderItem()
        {
            var quantityDisposer = this.WhenPropertyChanged(x => x.Quantity)
                .LessThan(this, 1)
                .ApplyRule("Invalid quantity!");

            var priceDisposer = this.WhenPropertyChanged(x => x.Price)
                .LessThan<decimal>(this, 0)
                .ApplyRule("Value cannot be negative!");

            Disposer.Add(quantityDisposer);
            Disposer.Add(priceDisposer);

            Quantity = 1;
        }

        public string Item { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}

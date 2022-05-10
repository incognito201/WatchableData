using WatchableData.Mvvm;
using WatchableData.Validation;

namespace WatchableData.Demo
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

        private string _item;
        public string Item
        {
            get { return _item; }
            set { SetProperty(ref _item, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value); }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }
    }
}

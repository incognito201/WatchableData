using System.Collections.Generic;

namespace WatchableData.Demo
{
    public static class OrderItemService
    {
        public static ICollection<OrderItem> GetAll()
        {
            var items = new List<OrderItem>
            {
                new OrderItem { Item = "Foo", Price = 25, Quantity = 2 },
                new OrderItem { Item = "Bar", Price = 150, Quantity = 4 }
            };
            return items;
        }
    }
}

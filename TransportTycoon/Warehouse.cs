using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
    public class Warehouse
    {
        private readonly Queue<Order> orders = new Queue<Order>();

        public bool AllOrdersSent => orders.Count == 0;

        public string Name { get; }

        public List<Order> Inventory => orders.ToList();

        public Warehouse(string name)
        {
            Name = name;
        }

        public void PlaceOrders(IEnumerable<string> destinations)
        {
            foreach(var destination in destinations)
            {
                orders.Enqueue(new Order(destination));
            }
        }

        public Order? GetNextOrder()
        {
            if (AllOrdersSent)
            {
                return null;
            }

            return orders.Dequeue();
        }

        public void DeliverOrder(Order order)
        {
            orders.Enqueue(order);
        }
    }
}

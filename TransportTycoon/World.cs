using System;
using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
    public class World
    {
        private readonly IEnumerable<string> destinations;
        private readonly Warehouse factory;
        private readonly Warehouse port;
        private readonly Warehouse warehouseA;
        private readonly Warehouse warehouseB;
        private readonly Vehicle truck1;
        private readonly Vehicle truck2;
        private readonly Vehicle ship;
        private readonly TimeTracker timeTracker;

        public World(IEnumerable<string> destinations)
        {
            this.destinations = destinations;
            factory = new Warehouse("Factory");
            factory.PlaceOrders(destinations);
            port = new Warehouse("Port");
            warehouseA = new Warehouse("A");
            warehouseB = new Warehouse("B");
            var routePlanner = new RoutePlanner(new[]
            {
                new Route(factory, warehouseB, 5),
                new Route(warehouseB, factory, 5),
                new Route(factory, warehouseA, 5, new Route(factory, port, 1)),
                new Route(port, factory, 1),
                new Route(port, warehouseA, 4),
                new Route(warehouseA, port, 4)
            });
            truck1 = new Vehicle(factory, factory, routePlanner);
            EventPublisher.AddSubscriber<TimeTick>(truck1);
            truck2 = new Vehicle(factory, factory, routePlanner);
            EventPublisher.AddSubscriber<TimeTick>(truck2);
            ship = new Vehicle(port, port, routePlanner);
            EventPublisher.AddSubscriber<TimeTick>(ship);
            timeTracker = new TimeTracker();
            EventPublisher.AddSubscriber<TimeTick>(timeTracker);
        }

        public int CurrentTime => timeTracker.CurrentTime;

        public void Deliver()
        {
            while ((warehouseA.Inventory.Count + warehouseB.Inventory.Count) < destinations.Count())
            {
                EventPublisher.Publish(new TimeTick());
            }
        }
    }


}

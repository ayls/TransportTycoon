namespace TransportTycoon
{
    public class Vehicle : ISubscriber<TimeTick>
    {
        private readonly RoutePlanner routePlanner;

        public Warehouse HomeLocation { get; }
        public Warehouse? CurrentLocation { get; private set; }
        public Order? CurrentOrder { get; private set; }
        public Route? CurrentRoute { get; private set; }
        public int? RemainingTimeOnRoute { get; private set; } 
        public bool IsAtHome => CurrentLocation == HomeLocation;

        public Vehicle(Warehouse homeLocation, Warehouse currentLocation, RoutePlanner routePlanner)
        {
            HomeLocation = homeLocation;
            CurrentLocation = currentLocation;
            this.routePlanner = routePlanner;
            CurrentOrder = null;
            CurrentRoute = null;
            RemainingTimeOnRoute = null;
        }

        public void Apply(TimeTick @event)
        {
            if (RemainingTimeOnRoute > 0)
            {
                RemainingTimeOnRoute--;
            }

            if (RemainingTimeOnRoute == 0)
            {
                Park();
            }

            if (IsAtHome)
            {
                var nextOrder = HomeLocation.GetNextOrder();
                if (nextOrder != null)
                {
                    LoadCargo(nextOrder);
                }
            }
        }

        private void Park()
        {
            if (CurrentOrder != null)
            {
                CurrentRoute!.Destination.DeliverOrder(CurrentOrder);
            }
            CurrentLocation = CurrentRoute!.Destination;
            CurrentOrder = null;
            if (IsAtHome)
            {
                CurrentRoute = null;
                RemainingTimeOnRoute = null;
            }
            else
            {
                CurrentRoute = routePlanner.PlanRoute(CurrentLocation.Name, HomeLocation.Name);
                RemainingTimeOnRoute = CurrentRoute.Duration;
            }
        }

        private void LoadCargo(Order order)
        {
            CurrentOrder = order;
            CurrentRoute = routePlanner.PlanRoute(HomeLocation.Name, order.Destination);
            RemainingTimeOnRoute = CurrentRoute.Duration;
            CurrentLocation = null;
        }
    }
}

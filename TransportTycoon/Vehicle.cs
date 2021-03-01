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
            SetRoute(null);
        }

        public void Apply(TimeTick @event)
        {
            if (RemainingTimeOnRoute > 0)
            {
                RemainingTimeOnRoute--;
            }

            if (RemainingTimeOnRoute == 0)
            {
                HandleArrival();
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

        private void HandleArrival()
        {
            if (CurrentOrder != null)
            {
                CurrentRoute!.Destination.DeliverOrder(CurrentOrder);
            }
            CurrentOrder = null;
            CurrentLocation = CurrentRoute!.Destination;
            if (IsAtHome)
            {
                SetRoute(null);
            }
            else
            {
                var routeBackHome = routePlanner.PlanRoute(CurrentLocation.Name, HomeLocation.Name);
                SetRoute(routeBackHome);
            }
        }

        private void LoadCargo(Order order)
        {
            CurrentOrder = order;
            CurrentLocation = null;
            var routeToDestination = routePlanner.PlanRoute(HomeLocation.Name, order.Destination);
            SetRoute(routeToDestination);

        }

        private void SetRoute(Route? route)
        {
            CurrentRoute = route;
            RemainingTimeOnRoute = route?.Duration;
        }
    }
}

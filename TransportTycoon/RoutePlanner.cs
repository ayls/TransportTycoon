using System.Collections.Generic;
using System.Linq;

namespace TransportTycoon
{
    public class Route
    {
        public Route(Warehouse origin, Warehouse destination, int duration)
        {
            Origin = origin;
            Destination = destination;
            Duration = duration;
        }

        public Route(Warehouse origin, Warehouse destination, int duration, Route subRoute)
        {
            Origin = origin;
            Destination = destination;
            Duration = duration;
            SubRoute = subRoute;
        }

        public Warehouse Origin { get; }
        public Warehouse Destination { get; }
        public int Duration { get; }
        public Route? SubRoute { get; }
    }

    public class RoutePlanner
    {
        private readonly List<Route> routes = new List<Route>();

        public RoutePlanner(IEnumerable<Route> routes)
        {
            this.routes.AddRange(routes);
        }

        public Route PlanRoute(string origin, string destination)
        {
            var routeMatch = routes.Where(route => route.Origin.Name == origin && route.Destination.Name == destination).Single();

            if (routeMatch.SubRoute != null)
            {
                return routeMatch.SubRoute;
            }

            return routeMatch;
        }
    }
}

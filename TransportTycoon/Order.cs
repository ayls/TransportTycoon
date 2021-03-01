namespace TransportTycoon
{
    public class Order
    {
        public Order(string destination)
        {
            Destination = destination;
        }

        public string Destination { get; }
    }
}

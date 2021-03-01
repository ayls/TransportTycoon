namespace TransportTycoon
{
    public class TimeTracker : ISubscriber<TimeTick>
    {
        public int CurrentTime { get; private set; }

        public TimeTracker()
        {
            CurrentTime = -1; // We want to start at 0 when the first event arrives
        }

        public void Apply(TimeTick @event)
        {
            CurrentTime++;
        }
    }
}

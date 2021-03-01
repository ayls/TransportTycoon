using System;
using System.Collections.Generic;

namespace TransportTycoon
{
    public interface ISubscriber<T> where T : IEvent
    {
        void Apply(T @event);
    }

    public static class EventPublisher
    {
        private static Dictionary<Type, List<dynamic>> subscribers = new Dictionary<Type, List<dynamic>>();

        public static void AddSubscriber<T>(ISubscriber<T> subscriber) where T : IEvent
        {
            var eventType = typeof(T);
            if (!subscribers.ContainsKey(eventType))
            {
                var eventSubscribers = new List<dynamic>();
                eventSubscribers.Add(subscriber);
                subscribers.Add(eventType, eventSubscribers);
            } 
            else
            {
                subscribers[eventType].Add(subscriber);
            }
        }

        public static void Publish<T>(T @event)
        {
            var eventType = typeof(T);
            if (subscribers.ContainsKey(eventType))
            {
                subscribers[eventType].ForEach(subscriber => subscriber.Apply(@event));
            }
        }
    }
}

namespace MultiPurposeProject.DesingPatterns.Observer
{
    public class Publisher : IPublisher
    {

        private Dictionary<string, List<ISubscriber>> eventSubscribers = new();

        public void Subscribe(string eventType, ISubscriber subscriber)
        {
            List<ISubscriber> subscribers;
            
            if (!eventSubscribers.TryGetValue(eventType, out subscribers))
            {
                subscribers = new List<ISubscriber>();
                eventSubscribers.Add(eventType, subscribers);
            }

            subscribers.Add(subscriber);
        }

        public void Unsubscribe(string eventType, ISubscriber subscriber)
        {
            List<ISubscriber> subscribers;

            if (eventSubscribers.TryGetValue(eventType, out subscribers))
            {
                subscribers.Remove(subscriber);
            }
        }

        public void Notify(string eventType, string data)
        {
            List<ISubscriber> subscribers;

            if (eventSubscribers.TryGetValue(eventType, out subscribers))
            {
                foreach (var subscriber in subscribers)
                {
                    subscriber.Update(data);
                }
            }
        }

    }
}

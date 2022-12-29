namespace MultiPurposeProject.DesingPatterns.Observer
{
    public interface IPublisher
    {

        void Subscribe(string eventType, ISubscriber subscriber);
        void Unsubscribe(string eventType, ISubscriber subscriber);
        void Notify(string eventType, string data);

    }
}

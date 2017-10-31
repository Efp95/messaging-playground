using System.Collections.Concurrent;
using Demo.Common;
using Microsoft.ServiceBus;

namespace Demo.ServiceBus
{
    public class TopicMessageProvider : IMessageProvider
    {
        private readonly ConcurrentDictionary<string, TopicServiceBusPublisher> _publishers;

        public TopicMessageProvider()
        {
            _publishers = new ConcurrentDictionary<string, TopicServiceBusPublisher>();
        }

        public IMessagePublisher GetPublisher(string connectionString, string publisherName)
        {
            var publisherKey = $"{connectionString}_{publisherName}";
            if (_publishers.TryGetValue(publisherKey, out TopicServiceBusPublisher existingPublisher))
                return existingPublisher;

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            var topicDescription = namespaceManager.TopicExists(publisherName)
                ? namespaceManager.GetTopic(publisherName)
                : namespaceManager.CreateTopic(publisherName);

            var publisher = new TopicServiceBusPublisher(connectionString, topicDescription.Path);
            _publishers.TryAdd(publisherKey, publisher);

            return publisher;
        }

        public IMessageSubscriber GetSubscriber(string connectionString, string publisherName, string subscriptionName)
        {
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            var subscriptionDescription = namespaceManager.SubscriptionExists(publisherName, subscriptionName)
                ? namespaceManager.GetSubscription(publisherName, subscriptionName)
                : namespaceManager.CreateSubscription(publisherName, subscriptionName);

            return new TopicServiceBusSubscriber(connectionString, publisherName, subscriptionDescription.Name);
        }

        public void Dispose()
        {
            foreach (var topicServiceBusPublisher in _publishers.Values)
            {
                topicServiceBusPublisher.Close();
            }
            _publishers.Clear();
        }
    }
}
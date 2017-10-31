using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Demo.Common;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Demo.ServiceBus
{
    public class TopicServiceBusSubscriber : IMessageSubscriber
    {
        private readonly SubscriptionClient _subscriptionClient;

        public TopicServiceBusSubscriber(string connectionString, string topicPath, string subscriptionName)
        {
            _subscriptionClient = SubscriptionClient.CreateFromConnectionString(connectionString, topicPath, subscriptionName);
        }

        public void OnMessage<T>(Action<T> handler)
        {
            _subscriptionClient.OnMessage(brokeredMessage =>
            {
                var message = GetMessage<T>(brokeredMessage);
                handler(message);
            });
        }

        public void OnMessageAsync<T>(Func<T, Task> handler)
        {
            _subscriptionClient.OnMessageAsync(brokeredMessage =>
            {
                var message = GetMessage<T>(brokeredMessage);
                return handler(message);
            });
        }

        public void Dispose()
        {
            _subscriptionClient.Close();
        }

        private static T GetMessage<T>(BrokeredMessage brokeredMessage)
        {
            var messageStream = brokeredMessage.GetBody<Stream>();
            var jsonMessage = new StreamReader(messageStream, Encoding.UTF8).ReadToEnd();
            return JsonConvert.DeserializeObject<T>(jsonMessage);
        }
    }
}
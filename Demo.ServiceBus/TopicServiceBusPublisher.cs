using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Demo.ServiceBus
{
    public class TopicServiceBusPublisher : IMessagePublisher
    {
        private readonly TopicClient _topicClient;

        public TopicServiceBusPublisher(string connectionString, string topicPath)
        {
            _topicClient = TopicClient.CreateFromConnectionString(connectionString, topicPath);
        }

        public void Send<T>(T message, IReadOnlyDictionary<string, object> properties = null)
        {
            var brokeredMessage = BuildMessage(message, properties);

            _topicClient.Send(brokeredMessage);
        }

        public async Task SendAsync<T>(T message, IReadOnlyDictionary<string, object> properties = null)
        {
            var brokeredMessage = BuildMessage(message, properties);

            await _topicClient.SendAsync(brokeredMessage);
        }

        public void Close()
        {
            _topicClient.Close();
        }

        private static BrokeredMessage BuildMessage(object message, IReadOnlyDictionary<string, object> properties = null)
        {
            var byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            var brokeredMessage = new BrokeredMessage(new MemoryStream(byteArray));
            properties?.ToList().ForEach(brokeredMessage.Properties.Add);

            return brokeredMessage;
        }
    }
}
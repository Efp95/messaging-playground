using System;
using System.Linq;
using Demo.Common;
using Demo.ServiceBus;

namespace Demo.App
{
    internal class Program
    {
        private static void Main()
        {
            const string connString = "<connectionString>";
            const string topicName = "topicName";
            const string subscriptionName = "subscriptionName";

            IMessageProvider messageProvider = new TopicMessageProvider();

            var publisher = messageProvider.GetPublisher(connString, topicName);
            var subscriber = messageProvider.GetSubscriber(connString, topicName, subscriptionName);

            subscriber.OnMessage<string>(message =>
            {
                Console.WriteLine($"This is what I received: {message}");
            });

            var dummyList = Enumerable.Range(1, 5).ToList();
            foreach (var i in dummyList)
            {
                publisher.Send($"Test {i}");
            }

            Console.ReadLine();
        }
    }
}
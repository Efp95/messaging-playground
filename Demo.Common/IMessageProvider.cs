using System;

namespace Demo.Common
{
    public interface IMessageProvider : IDisposable
    {
        IMessagePublisher GetPublisher(string connectionString, string publisherName);
        IMessageSubscriber GetSubscriber(string connectionString, string publisherName, string subscriptionName);
    }
}
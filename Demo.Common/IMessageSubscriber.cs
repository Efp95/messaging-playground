using System;
using System.Threading.Tasks;

namespace Demo.Common
{
    public interface IMessageSubscriber : IDisposable
    {
        void OnMessage<T>(Action<T> handler);
        void OnMessageAsync<T>(Func<T, Task> handler);
    }
}
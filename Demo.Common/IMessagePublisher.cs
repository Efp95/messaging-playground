using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Common
{
    public interface IMessagePublisher
    {
        void Send<T>(T message, IReadOnlyDictionary<string, object> properties = null);
        Task SendAsync<T>(T message, IReadOnlyDictionary<string, object> properties = null);
    }
}
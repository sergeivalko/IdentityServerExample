using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StormShop.Common.Bus
{
    public interface IBusProducer<in T>
    {
        Task Publish(string key, T model, Dictionary<string, string> metadata = default,
            CancellationToken cancellationToken = default);
    }
}
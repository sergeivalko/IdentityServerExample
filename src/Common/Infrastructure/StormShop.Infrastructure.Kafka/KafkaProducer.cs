using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using StormShop.Common.Bus;

namespace StormShop.Infrastructure.Kafka
{
    public class KafkaProducer<T> : IBusProducer<T>, IDisposable
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IProducer<string, string> producer, string topic)
        {
            _producer = producer;
            _topic = topic;
        }

        public Task Publish(string key, T model, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
        {
            var message = new Message<string, string>()
            {
                Key = key,
                Value = JsonSerializer.Serialize(model)
            };
            
            return _producer.ProduceAsync(_topic, message, cancellationToken);
        }

        public void Dispose()
        {
            _producer.Dispose();
        }
    }
}
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StormShop.Common.Bus;

namespace StormShop.Infrastructure.Kafka
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddProducer<T>(this IServiceCollection services, IConfiguration configuration, string topicName)
        {
            var producerConfig = configuration.GetSection("ProducerConfig").Get<ProducerConfig>();
            var producer = new ProducerBuilder<string, string>(producerConfig).Build();
            
            services.AddScoped<IBusProducer<T>>(_ => new KafkaProducer<T>(producer, topicName));
            return services;
        }
    }
}
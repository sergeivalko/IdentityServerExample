using Confluent.Kafka;

namespace Profile.Infrastructure
{
    public class KafkaOptions
    {
        public KafkaOptionsItem UserCreatedConsumer { get; set; }
        public ConsumerConfig ConsumerConfig { get; set; }
    }

    public class KafkaOptionsItem
    {
        public string TopicName { get; set; }
        public bool Enabled { get; set; }
    }
}
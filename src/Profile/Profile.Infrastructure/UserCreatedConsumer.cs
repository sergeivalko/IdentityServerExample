using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Profile.Application.Features.CreateProfile;

namespace Profile.Infrastructure
{
    public class UserCreatedConsumer : BackgroundService
    {
        private readonly ILogger<UserCreatedConsumer> _logger;
        private readonly IConsumer<string, string> _consumer;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _topicName;
        private readonly bool _enabled;
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(5);

        public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger, IOptions<KafkaOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _consumer = new ConsumerBuilder<string, string>(options.Value.ConsumerConfig).Build();
            _serviceScopeFactory = serviceScopeFactory;
            _topicName = options.Value.UserCreatedConsumer.TopicName;
            _enabled = options.Value.UserCreatedConsumer.Enabled;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            if (!_enabled)
            {
                return;
            }

            _consumer.Subscribe(_topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(_timeout);

                    if (consumeResult?.Message == null)
                    {
                        continue;
                    }

                    _logger.LogInformation("UserCreatedConsumer handle message: {Message}", consumeResult.Message);
                    var userCreated = JsonSerializer.Deserialize<UserCreated>(consumeResult.Message.Value);

                    if (userCreated == null)
                    {
                        _consumer.Commit();
                        continue;
                    }

                    await using (var scope = _serviceScopeFactory.CreateAsyncScope())
                    {
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Publish(new CreateProfileCommand(userCreated.AccountId), stoppingToken);
                    }
                    
                    _consumer.Commit();
                }
                catch (Exception exception)
                {
                    _logger.LogInformation("UserCreatedConsumer handle Error: {Message}", exception.Message);
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace StormShop.Infrastructure.Mongo
{
    public sealed class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _db;
        private readonly MongoClient _mongoClient;
        private readonly List<Func<Task>> _commands;
        private IClientSessionHandle _session;

        public MongoContext(IOptions<MongoOptions> optionsSnapshot)
        {
            if (optionsSnapshot.Value?.Connection == null || optionsSnapshot.Value.DatabaseName == null)
            {
                throw new ArgumentNullException(
                    $"invalid configuration database");
            }

            _mongoClient = new MongoClient(optionsSnapshot.Value.Connection);
            _db = _mongoClient.GetDatabase(optionsSnapshot.Value.DatabaseName);

            _commands = new List<Func<Task>>();
        }

        public async Task<int> SaveChanges()
        {
            using (_session = await _mongoClient.StartSessionAsync())
            {
                //_session.StartTransaction();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                //await _session.CommitTransactionAsync();
            }

            return _commands.Count;
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }

        public void Dispose()
        {
            _session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }
    }
}

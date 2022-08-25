using Microsoft.Extensions.Options;
using ModelBindingSample.Models;
using MongoDB.Driver;

namespace ModelBindingSample.Services
{
    public class MongoContext : IMongoContext<Issue>
    {
        private readonly IMongoDatabase _database;
        private MongoClient _client;
        protected IMongoCollection<Issue> issuesCollection;

        public IClientSessionHandle SessionHandle { get; set; }

        public MongoContext(IOptions<MongoSettings> options)
        {
            _client = new MongoClient(options.Value.Connection);
            _database = _client.GetDatabase(options.Value.Database);
            SessionHandle = _client.StartSession();
            var collections = _database.ListCollectionNamesAsync().GetAwaiter().GetResult().ToList();
            if (!collections.Contains(options.Value.Issues)) 
                _database.CreateCollectionAsync(options.Value.Issues);
            issuesCollection = _database.GetCollection<Issue>(options.Value.Issues);
        }
        public async Task<List<Issue>> FindAsync(FilterDefinition<Issue> f) => (await issuesCollection.FindAsync(f,default,default)).ToList();
        public async Task InsertOneAsync(Issue i) => await issuesCollection.InsertOneAsync(i);
        public async Task DeleteOneAsync(FilterDefinition<Issue> f) => await issuesCollection.DeleteOneAsync(f);
        public async Task<Issue> UpdateOneAsync(FilterDefinition<Issue> f, Issue t) => await issuesCollection.FindOneAndReplaceAsync(f, t, default, default);
    }
}

using MongoDB.Driver;

namespace ModelBindingSample.Services
{
    public interface IMongoContext<T>
    { 
        Task<List<T>> FindAsync(FilterDefinition<T> f);
        Task InsertOneAsync(T t);
        Task DeleteOneAsync(FilterDefinition<T> f);
        Task<T> UpdateOneAsync(FilterDefinition<T> f, T t);
    }
}

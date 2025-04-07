using Blazesoft.SlotMachine.Common.Interfaces;
using Blazesoft.SlotMachine.Common.Types;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Blazesoft.SlotMachine.Common.Data
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseMongoModel, new()
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoClient _mongoClient;

        public BaseRepository(IMongoClient mongoClient,IMongoDatabase database) {
            var collectionName = typeof(T).Name.ToLowerInvariant();
            _collection = database.GetCollection<T>(collectionName);
            _mongoClient = mongoClient;
        }
        public async Task<RepoResponse<T>> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            try
            {
                await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
                return RepoResponse<T>.Success(value: entity);
            }
            catch (Exception e)
            {
                return RepoResponse<T>.Fail(e.Message);
            }
        }

        public Task<RepoResponse<int>> AddManyAsync(T[] entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<bool>> AnyAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IBaseRepository<U> Cast<U>()
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<int>> CountAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<int>> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<int>> DeleteManyAsync(int[] id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<List<T>>> DistinctAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<K>> ExecuteCustomQueryAsync<K>(string sqlQuery, object[]? parameters, CancellationToken cancellationToken = default) where K : class
        {
            throw new NotImplementedException();
        }

        public async Task<RepoResponse<T?>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            
            try
            {
                //var filters = Builders<T>.Filter.Eq(doc => doc.Id, id);
                var objectId = ObjectId.Parse(id); // Throws if the id is not a valid ObjectId
                var filter = Builders<T>.Filter.Eq("_id", objectId);
                //var filter = Builders<T>.Filter.Empty;
                var result = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                return RepoResponse<T?>.Success(value: result);
            }
            catch (Exception e)
            {
                return RepoResponse<T?>.Fail(e.Message);
            }
        }

        public async Task<RepoResponse<T?>> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? wherePredicate = null, FieldDefinition<T>? includeFields = null, Dictionary<string, bool>? orderFields = null, CancellationToken cancellationToken = default)
        {
            try
            {
                // Default filter if no predicate is provided
                var filter = wherePredicate ?? (_ => true);  // No filter if null

                // Prepare projection if fields are specified
                var projection = includeFields == null
                    ? Builders<T>.Projection.Exclude("_id") // Or use an empty projection for all fields
                    : Builders<T>.Projection.Include(includeFields);

                // Prepare sorting if order fields are specified
                var sort = orderFields == null
                    ? Builders<T>.Sort.Ascending("_id")  // Default sort if no order fields
                    : Builders<T>.Sort.Combine(orderFields.Select(f => f.Value
                        ? Builders<T>.Sort.Ascending(f.Key)
                        : Builders<T>.Sort.Descending(f.Key)));

                var result = await _collection
                    .Find(filter)
                    //.Project<T>(projection)
                    //.Sort(sort)
                    .FirstOrDefaultAsync(cancellationToken);

                // Return result wrapped in a custom response object
                return RepoResponse<T?>.Success(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or return an error response
                return RepoResponse<T?>.Fail(ex.Message);
            }
        }

        public Task<RepoResponse<List<T>>> GetManyAsync(Expression<Func<T, bool>>? wherePredicate = null, string[]? includeFields = null, Dictionary<string, bool>? orderFields = null, int? offset = null, int? number = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<T?>> GetSingleOrDefaultAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<IQueryable<IGrouping<TKey, T>>>> GroupByAsync<TKey>(Expression<Func<T, TKey>> keySelector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RepoResponse<List<TResult>>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<RepoResponse<T?>> UpdateAsync(T entity, CancellationToken cancellationToken = default)
         {

            try
            {
                
                var filter = Builders<T>.Filter.Eq(doc => doc.Id, entity.Id);

                var result = await _collection.FindOneAndReplaceAsync(
                    filter: filter,
                    replacement: entity,
                    options: new FindOneAndReplaceOptions<T>
                    {
                        ReturnDocument = ReturnDocument.After // returns the updated document
                    },
                    cancellationToken: cancellationToken
                );


                return RepoResponse<T?>.Success(result);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error writing to MongoDB: " + e.Message);
                return RepoResponse<T?>.Fail(e.Message);
            }

        }

        public Task<RepoResponse<int>> UpdateManyAsync(T[] entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

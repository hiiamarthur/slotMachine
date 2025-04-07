using Blazesoft.SlotMachine.Common.Types;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Blazesoft.SlotMachine.Common.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<RepoResponse<T?>> GetAsync(string id, CancellationToken cancellationToken = default);
        Task<RepoResponse<List<T>>> GetManyAsync(Expression<Func<T, bool>>? wherePredicate = null, string[]? includeFields = null, Dictionary<string, bool>? orderFields = null, int? offset = null, int? number = null, CancellationToken cancellationToken = default);
        Task<RepoResponse<int>> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<RepoResponse<T>> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<RepoResponse<int>> AddManyAsync(T[] entities, CancellationToken cancellationToken = default);
        Task<RepoResponse<int>> UpdateManyAsync(T[] entities, CancellationToken cancellationToken = default);
        Task<RepoResponse<int>> DeleteManyAsync(int[] id, CancellationToken cancellationToken = default);
        Task<RepoResponse<int>> CountAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default);
        Task<RepoResponse<bool>> AnyAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default);
        Task<RepoResponse<T?>> GetSingleOrDefaultAsync(Expression<Func<T, bool>>? wherePredicate = null, CancellationToken cancellationToken = default);
        Task<RepoResponse<List<TResult>>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);
        Task<RepoResponse<List<T>>> DistinctAsync(CancellationToken cancellationToken = default);
        Task<RepoResponse<IQueryable<IGrouping<TKey, T>>>> GroupByAsync<TKey>(Expression<Func<T, TKey>> keySelector, CancellationToken cancellationToken = default);
        Task<RepoResponse<K>> ExecuteCustomQueryAsync<K>(string sqlQuery, object[]? parameters, CancellationToken cancellationToken = default) where K : class;
        IBaseRepository<U> Cast<U>();
        Task<RepoResponse<T?>> GetFirstOrDefaultAsync(Expression<Func<T, bool>>? wherePredicate = null, FieldDefinition<T>? includeFields = null, Dictionary<string, bool>? orderFields = null, CancellationToken cancellationToken = default);
        Task<RepoResponse<T?>> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    }
}

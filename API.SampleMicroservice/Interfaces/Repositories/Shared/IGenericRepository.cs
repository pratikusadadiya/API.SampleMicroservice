using System.Linq.Expressions;
using API.SampleMicroservice.DataModels.Shared;

namespace API.SampleMicroservice.Interfaces.Repositories.Shared
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetById(int id);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
        Task<TEntity?> GetByIdAsync(long id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate);
        Task<DataQueryResponse<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, PageRequest? model = null, ExpressionIncluder<TEntity>? includer = null);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, ExpressionIncluder<TEntity>? includer = null); void RemoveEntities(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TType>> SelectAsync<TType>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> select) where TType : class;
        Task<TEntity?> FindDataAsync(Expression<Func<TEntity, bool>>? predicate, ExpressionIncluder<TEntity>? includer = null);
        Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyDataAsync(Expression<Func<TEntity, bool>>? predicate);
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate);
    }
}

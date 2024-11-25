using API.SampleMicroservice.DataModels.Shared;
using System.Linq.Expressions;

namespace API.SampleMicroservice.Interfaces.Services.Shared
{
    public interface IGenericService<TEntity> where TEntity : class
    {
        TResponse ToResponseModel<TResponse>(dynamic obj) where TResponse : class;
        IEnumerable<TEntity> ToEntity(IEnumerable<dynamic> obj);
        TEntity ToEntity(dynamic obj);
        TEntity ToEntity<TResponse>(TResponse dto, TEntity entity) where TResponse : class;
        Task<TEntity> UpdateEntity<TDto>(TDto dto, int id) where TDto : class;
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task<DataQueryResponse<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, PageRequest? model = null, ExpressionIncluder<TEntity>? includer = null);
        Task<TEntity> GetByIdAsync(int id);
        Task RemoveAsync(int id);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null);
        DEntity ToDTO<DEntity>(dynamic obj) where DEntity : class;
        Task<IEnumerable<TEntity>> GetAllDataAsync(Expression<Func<TEntity, bool>>? predicate = null, ExpressionIncluder<TEntity>? includer = null);
        Task RemoveEntitiesAsync(Expression<Func<TEntity, bool>> predicate);
        TClass ToClass<TClass>(dynamic obj) where TClass : class;
        TClass ToClass<TClass>(TClass c1, dynamic c2) where TClass : class;
        Task<TEntity> FindDataAsync(Expression<Func<TEntity, bool>> predicate, ExpressionIncluder<TEntity>? includer = null);
        Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> AnyDataAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TType>> SelectAsync<TType>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> select) where TType : class;
        Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate);
    }
}

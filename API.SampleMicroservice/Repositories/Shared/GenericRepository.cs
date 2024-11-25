using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using API.SampleMicroservice.Interfaces.Repositories.Shared;
using API.SampleMicroservice.Entities;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Constants;
using API.SampleMicroservice.Extensions;

namespace API.SampleMicroservice.Repositories.Shared
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly SampleMicroserviceContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepository(SampleMicroserviceContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity?> GetById(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
            await _dbSet.AddAsync(entity, cancellationToken);

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default) =>
            await _dbSet.AddRangeAsync(entities, cancellationToken);

        public virtual async Task<DataQueryResponse<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, PageRequest? model = null, ExpressionIncluder<TEntity>? includer = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable().OrderBy("Id");

            if (includer != null && includer.Includes != null)
            {
                foreach (var include in includer.Includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (model != null && !string.IsNullOrEmpty(model.SortBy))
            {
                if (model.SortDirection == SystemConstants.DESCENDING)
                {
                    query = query.OrderByDescending($"{model.SortBy}");
                }
                else
                {
                    query = query.OrderBy($"{model.SortBy}");
                }
            }
            return new DataQueryResponse<TEntity>()
            {
                TotalRecords = await query.CountAsync(),
                Records = model != null && (model.PageNumber > 0 && model.PageSize > 0) ?
                await query.Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToListAsync() :
                await query.ToListAsync(),
            };
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, ExpressionIncluder<TEntity>? includer = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (includer != null && includer.Includes != null)
            {
                foreach (var include in includer.Includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            query = query.Where(predicate);
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByIdAsync(long id)
        {
            return await _dbContext.Set<TEntity>().FindAsync((int)id);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate)
        {
            if (predicate == null)
            {
                return await _dbSet.ToListAsync();
            }
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public void RemoveEntities(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _dbContext.Set<TEntity>().Where(predicate);
            _dbContext.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<IEnumerable<TType>> SelectAsync<TType>(
            Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await _dbContext.Set<TEntity>().Where(expression).Select(select).ToListAsync();
        }

        public async Task<TEntity?> FindDataAsync(Expression<Func<TEntity, bool>>? predicate, ExpressionIncluder<TEntity>? includer = null)
        {
            IQueryable<TEntity> query = _dbSet.AsQueryable();

            if (includer != null && includer.Includes != null)
            {
                foreach (var include in includer.Includes)
                {
                    query = query.Include(include);
                }
            }

            if (predicate == null)
            {
                return await query.FirstOrDefaultAsync();
            }

            return await query.Where(predicate).FirstOrDefaultAsync();
        }

        public async Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        public async Task<bool> AnyDataAsync(Expression<Func<TEntity, bool>>? predicate)
        {
            return predicate != null ? await _dbSet.AnyAsync(predicate) : await _dbSet.AnyAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate)
        {
            return predicate != null ? await _dbSet.CountAsync(predicate) : await _dbSet.CountAsync();
        }
    }
}

using API.SampleMicroservice.Interfaces.Repositories.Shared;
using API.SampleMicroservice.Interfaces.Services.Shared;
using AutoMapper;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Exceptions;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using API.SampleMicroservice.Resources;

namespace API.SampleMicroservice.Services.Shared
{
    public class GenericService<TEntity>(IGenericRepository<TEntity> genericRepository, IUnitOfWork unitOfWork, IMapper mapper, ClaimsPrincipal principal) : IGenericService<TEntity> where TEntity : class
	{
		public int? CurrentMemberId
		{
			get
			{
				////return principal.GetMemberId();
                return null;
			}
		}

		public TResponse ToResponseModel<TResponse>(dynamic obj) where TResponse : class
        {
            return mapper.Map<TResponse>(obj);
        }

        public TEntity ToEntity(dynamic obj)
        {
            return mapper.Map<TEntity>(obj);
        }

        public TEntity ToEntity<TResponse>(TResponse dto, TEntity entity) where TResponse : class
        {
            return mapper.Map(dto, entity);
        }

        public IEnumerable<TEntity> ToEntity(IEnumerable<dynamic> obj)
        {
            var entities = new List<TEntity>();
            foreach (var item in obj)
            {
                var entity = mapper.Map<TEntity>(item);
                entities.Add(entity);
            }
            return entities;
        }

        public TClass ToClass<TClass>(TClass c1, dynamic c2) where TClass : class
        {
            return mapper.Map(c2, c1);
        }

        public TClass ToClass<TClass>(dynamic obj) where TClass : class
        {
            return mapper.Map<TClass>(obj);
        }

        public async Task<TEntity> UpdateEntity<TDto>(TDto dto, int id) where TDto : class
        {
            TEntity entity = await GetByIdAsync(id);
            entity = ToEntity(dto, entity);
            return await UpdateAsync(entity);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            PropertyInfo? createdBy = entity.GetType().GetProperty("CreatedBy");
            createdBy?.SetValue(entity, CurrentMemberId, null);

            using (CancellationTokenSource cancellationTokenSource = new())
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                await genericRepository.AddAsync(entity, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                PropertyInfo? createdBy = entity.GetType().GetProperty("CreatedBy");
                createdBy?.SetValue(entity, CurrentMemberId, null);
            }

            using (CancellationTokenSource cancellationTokenSource = new())
            {
                CancellationToken cancellationToken = cancellationTokenSource.Token;
                await genericRepository.AddRangeAsync(entities, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync();

            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            PropertyInfo? modifiedOn = entity.GetType().GetProperty("ModifiedOn");
            modifiedOn?.SetValue(entity, DateTime.Now, null);

            PropertyInfo? modifiedBy = entity.GetType().GetProperty("ModifiedBy");
            modifiedBy?.SetValue(entity, CurrentMemberId, null);

            genericRepository.Update(entity);
            await unitOfWork.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                PropertyInfo? modifiedOn = entity.GetType().GetProperty("ModifiedOn");
                modifiedOn?.SetValue(entity, DateTime.Now, null);

                PropertyInfo? modifiedBy = entity.GetType().GetProperty("ModifiedBy");
                modifiedBy?.SetValue(entity, CurrentMemberId, null);
            }

            genericRepository.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync();
            return entities;
        }

        public async Task<DataQueryResponse<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate, PageRequest? model = null, ExpressionIncluder<TEntity>? includer = null)
        {
            return await genericRepository.GetAllAsync(predicate, model, includer);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            TEntity? entity = await genericRepository.GetByIdAsync(id);
            return entity is null ? throw new NotFoundException(Messages.NoRecordsFound) : entity;
        }

        public virtual async Task RemoveAsync(int id)
        {
            TEntity entity = await genericRepository.GetByIdAsync(id) ?? throw new NotFoundException(Messages.NoRecordsFound);
            genericRepository.Remove(entity);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>>? predicate = null)
        {
            return await genericRepository.FindAsync(predicate);
        }

        public DEntity ToDTO<DEntity>(dynamic obj) where DEntity : class
        {
            return mapper.Map<DEntity>(obj);
        }

        public async Task<IEnumerable<TEntity>> GetAllDataAsync(Expression<Func<TEntity, bool>>? predicate = null, ExpressionIncluder<TEntity>? includer = null)
        {
            return await genericRepository.GetAllAsync(predicate, includer);
        }

        public virtual async Task RemoveEntitiesAsync(Expression<Func<TEntity, bool>> predicate)
        {
            genericRepository.RemoveEntities(predicate);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<TEntity> FindDataAsync(Expression<Func<TEntity, bool>> predicate, ExpressionIncluder<TEntity>? includer = null)
        {
            TEntity? entity = await genericRepository.FindDataAsync(predicate, includer);
            return entity is null ? throw new NotFoundException(Messages.NoRecordsFound) : entity;
        }

        public async Task<bool> CheckDuplicateAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await genericRepository.CheckDuplicateAsync(predicate);
        }

        public async Task<bool> AnyDataAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await genericRepository.AnyDataAsync(predicate);
        }

        public async Task<IEnumerable<TType>> SelectAsync<TType>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, TType>> select) where TType : class
        {
            return await genericRepository.SelectAsync(expression, select);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? predicate)
        {
            return await genericRepository.GetCountAsync(predicate);
        }
    }
}

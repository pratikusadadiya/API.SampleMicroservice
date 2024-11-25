using AutoMapper;
using System.Linq.Expressions;
using System.Security.Claims;
using API.SampleMicroservice.Interfaces.Repositories;
using API.SampleMicroservice.Interfaces.Repositories.Shared;
using API.SampleMicroservice.Services.Shared;
using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Services;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.DataModels.Response;
using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.Extensions;
using API.SampleMicroservice.Exceptions;
using API.SampleMicroservice.Resources;

namespace API.SampleMicroservice.Services
{
    public class SampleEntityService(ISampleEntityRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ClaimsPrincipal principal) : GenericService<SampleMicroserviceEntity>(repository, unitOfWork, mapper, principal), ISampleEntityService
    {
        public async Task<DataQueryResponseModel<SampleEntityResponseModel>> GetSampleEntityAsync(SampleEntitySearchPageRequest request)
        {
            Expression<Func<SampleMicroserviceEntity, bool>> expression = x => true;

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                expression = expression.Add(x => x.Name.Trim().ToLower().Contains(request.Search.Trim().ToLower()));
            }
            if (request.IsActive != null)
            {
                expression = expression.Add(x => x.IsActive == request.IsActive);
            }
            if (!string.IsNullOrWhiteSpace(request.PhoneNo))
            {
                expression = expression.Add(x => x.PhoneNo.Trim().ToLower().Contains(request.PhoneNo.Trim().ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(request.AlternatePhoneNo))
            {
                expression = expression.Add(x => x.AlternatePhoneNo!.Trim().ToLower().Contains(request.AlternatePhoneNo.Trim().ToLower()));
            }

            var response = await GetAllAsync(expression, request);

            DataQueryResponseModel<SampleEntityResponseModel> model = new()
            {
                TotalRecords = response.TotalRecords,
                Records = ToResponseModel<IEnumerable<SampleEntityResponseModel>>(response.Records)
            };

            return model;
        }

        public async Task<SampleEntityResponseModel> GetSampleEntityByIdAsync(int id)
            => ToResponseModel<SampleEntityResponseModel>(await GetByIdAsync(id));

        public async Task<SampleEntityResponseModel> CreateSampleEntityAsync(SampleEntityCreateUpdateCommand dto)
        {
            dto.IsActive = true;

            if (await CheckDuplicateAsync(x => !string.IsNullOrWhiteSpace(x.Name) && x.Name.Trim().ToLower() == dto.Name.Trim().ToLower() && x.IsActive == dto.IsActive))
                throw new DuplicateRecordException(string.Format(Messages.EntityWithThisPropertyAlreadyExists, Messages.SampleEntity, Messages.Name));

            SampleMicroserviceEntity SampleEntity = ToEntity(dto);
            return ToResponseModel<SampleEntityResponseModel>(await AddAsync(SampleEntity));
        }

        public async Task<SampleEntityResponseModel> UpdateSampleEntityAsync(int id, SampleEntityCreateUpdateCommand dto)
        {
            if (dto.IsActive && await CheckDuplicateAsync(x => x.Id != id && x.IsActive && !string.IsNullOrWhiteSpace(x.Name) && x.Name.Trim().ToLower() == dto.Name.Trim().ToLower()))
                throw new DuplicateRecordException(string.Format(Messages.EntityWithThisPropertyAlreadyExists, Messages.SampleEntity, Messages.Name));

            return ToResponseModel<SampleEntityResponseModel>(await UpdateEntity(dto, id));
        }

        public async Task<SampleEntityResponseModel> UpdateStatusAsync(int id, bool status)
        {
            var dto = await GetByIdAsync(id);

            if (status && await CheckDuplicateAsync(x => x.Id != id && x.IsActive && !string.IsNullOrWhiteSpace(x.Name) && x.Name.Trim().ToLower() == dto.Name.Trim().ToLower()))
                throw new DuplicateRecordException(string.Format(Messages.AlreadyExist, Messages.SampleEntity));

            dto.IsActive = status;
            return ToResponseModel<SampleEntityResponseModel>(await UpdateEntity(dto, id));
        }

        public async Task<IList<DropdownOptionResponseModel>> GetSampleEntityDropdownOptionsAsync()
        {
            return (await GetAllDataAsync(p => p.IsActive)).OrderBy(x => x.Name)
            .Select(x => new DropdownOptionResponseModel { Value = x.Id, Text = x.Name }).ToList();
        }
    }
}

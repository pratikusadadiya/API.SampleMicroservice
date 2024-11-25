using API.SampleMicroservice.DataModels.Request;
using API.SampleMicroservice.DataModels.Response;
using API.SampleMicroservice.DataModels.Shared;
using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Services.Shared;

namespace API.SampleMicroservice.Interfaces.Services
{
    public interface ISampleEntityService : IGenericService<SampleMicroserviceEntity>
    {
        Task<DataQueryResponseModel<SampleEntityResponseModel>> GetSampleEntityAsync(SampleEntitySearchPageRequest request);
        Task<SampleEntityResponseModel> GetSampleEntityByIdAsync(int id);
        Task<SampleEntityResponseModel> CreateSampleEntityAsync(SampleEntityCreateUpdateCommand dto);
        Task<SampleEntityResponseModel> UpdateSampleEntityAsync(int id, SampleEntityCreateUpdateCommand dto);
        Task<SampleEntityResponseModel> UpdateStatusAsync(int id, bool status);
        Task<IList<DropdownOptionResponseModel>> GetSampleEntityDropdownOptionsAsync();
    }
}

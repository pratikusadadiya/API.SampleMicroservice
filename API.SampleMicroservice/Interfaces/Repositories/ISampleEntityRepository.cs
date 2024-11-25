using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Repositories.Shared;

namespace API.SampleMicroservice.Interfaces.Repositories
{
    public interface ISampleEntityRepository : IGenericRepository<SampleMicroserviceEntity>
    {
    }
}

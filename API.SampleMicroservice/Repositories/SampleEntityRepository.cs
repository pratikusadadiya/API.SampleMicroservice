using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Repositories;
using API.SampleMicroservice.Repositories.Shared;

namespace API.SampleMicroservice.Repositories
{
    public class SampleEntityRepository(SampleMicroserviceContext dbContext) : GenericRepository<SampleMicroserviceEntity>(dbContext),
        ISampleEntityRepository
    {
    }
}

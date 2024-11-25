using API.SampleMicroservice.Entities;
using API.SampleMicroservice.Interfaces.Repositories.Shared;

namespace API.SampleMicroservice.Repositories.Shared
{
    public class UnitOfWork(SampleMicroserviceContext context) : IUnitOfWork
    {
        public int Save() => context.SaveChanges();

        public void Dispose() => context.Dispose();

        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

        public async Task RollbackAsync() => await context.DisposeAsync();
    }
}

namespace API.SampleMicroservice.Interfaces.Repositories.Shared
{
    public interface IUnitOfWork : IDisposable
    {
        int Save();
        Task SaveChangesAsync();
        Task RollbackAsync();
    }
}

namespace Demo.Core.Repositories
{
    public interface IRepository
    {
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
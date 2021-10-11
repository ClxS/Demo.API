using Demo.Domain;

namespace Demo.Core.Repositories
{
    public interface IGuitarRepository : IRepository
    {
        Task CreateAsync(Guitar guitar);
        Task<Guitar> ReadAsync(int id);
        Task<List<Guitar>> ReadAllAsync();
        Task<Guitar> FindAsync(int id);
        void Update(Guitar guitar);
        void Delete(Guitar guitar);
    }
}
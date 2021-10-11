using Demo.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    public abstract class EFRepository : IRepository
    {
        protected readonly DemoContext DbContext;

        protected EFRepository(DbContext dbContext)
        {
            DbContext = (DemoContext)dbContext;
        }

        public void SaveChanges() => DbContext.SaveChanges();

        public async Task SaveChangesAsync() => await DbContext.SaveChangesAsync();
    }
}
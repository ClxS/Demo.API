using Demo.Domain;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure
{
    public class DemoContext : DbContext
    {
        public DemoContext() { }

        public DemoContext(DbContextOptions<DemoContext> options) : base(options) { }        

        public virtual DbSet<Guitar> Guitar { get; set; }

        public virtual DbSet<GuitarString> GuitarString { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // don't like this, but not sure how to get around it atm
            optionsBuilder.UseMySQL("Server=127.0.0.1;Database=demo;User Id=demouser;Password=test123;port=3306");
        }
    }
}
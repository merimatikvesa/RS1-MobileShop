using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

       public DbSet<Phone> Phones { get; set; }
    }
}

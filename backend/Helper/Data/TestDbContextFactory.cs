using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Helper.Data
{
    public class TestDbContextFactory
    {
        public static MyDbContext Create()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new MyDbContext(options);
        }
    }
}

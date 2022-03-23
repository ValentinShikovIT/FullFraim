using System;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public interface ISeeder
    {
        Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider);
    }
}

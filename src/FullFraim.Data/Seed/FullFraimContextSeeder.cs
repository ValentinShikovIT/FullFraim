using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class FullFraimContextSeeder : ISeeder
    {
        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException();
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException();
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(FullFraimContextSeeder));

            var seeders = new List<ISeeder>()
            {
                new RanksSeed(),
                new UsersRolesSeeder(),
                new ContestTypesSeed(),
                new PhasesSeed(),
                new ContestCategoriesSeed(),
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}

using FullFraim.Data.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullFraim.Data.Seed
{
    public class ContestCategoriesSeed : ISeeder
    {
        public static readonly List<ContestCategory> SeedData = new List<ContestCategory>()
        {
           new ContestCategory()
           {
              Name = Constants.ConstestCategory.Abstract
           },
           new ContestCategory()
           {
              Name = Constants.ConstestCategory.Architecture
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Conceptual
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Fashion_Beauty
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Fine_Art
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Landscapes
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Nature
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Boudoir
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Photojournalism
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Portrait
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Street
           },
           new ContestCategory()
           {
               Name = Constants.ConstestCategory.Wildlife
           }
        };

        public async Task SeedAsync(FullFraimDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.ContestCategories.Any())
                await dbContext.AddRangeAsync(SeedData);
        }
    }
}

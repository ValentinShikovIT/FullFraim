using FullFraim.Data;
using FullFraim.Models.Dto_s.ContestCategories;
using FullFraim.Services.ContestCatgeoryServices;
using FullFraim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.ContestCategoryServices
{
    [TestClass]
    public class ContestCategoryServiceTests
    {
        [TestMethod]
        public async Task Create_ShouldReturnCorrectModel_WhenInputModelIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Create_ShouldReturnCorrectModel_WhenInputModelIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);
                var contestCategoryDto = new ContestCategoryDto()
                {
                    Id = 13,
                    Name = "NewCategory",
                };

                //Act
                var result = await contestCategoryService.CreateAsync(contestCategoryDto);

                //Assert
                Assert.AreEqual(contestCategoryDto.Id, result.Id);
                Assert.AreEqual(contestCategoryDto.Name, result.Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Create_ShouldReturnCorrectModel_FromDB()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Create_ShouldReturnCorrectModel_FromDB));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);
                var contestCategoryDto = new ContestCategoryDto()
                {
                    Id = 13,
                    Name = "NewCategory",
                };

                //Act
                await contestCategoryService.CreateAsync(contestCategoryDto);
                var result = await context.ContestCategories
                    .FirstOrDefaultAsync(cc => cc.Id == 13);

                //Assert
                Assert.AreEqual(contestCategoryDto.Id, result.Id);
                Assert.AreEqual(contestCategoryDto.Name, result.Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Create_ShouldThrowException_IfModelIsNull()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Create_ShouldThrowException_IfModelIsNull));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<NullModelException>
                    (async () => await contestCategoryService.CreateAsync(null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Delete_ShouldThrowException_IfIdIsZero()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Delete_ShouldThrowException_IfIdIsZero));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestCategoryService.DeleteAsync(0));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Delete_ShouldThrowException_IfIdIsNegative()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Delete_ShouldThrowException_IfIdIsNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestCategoryService.DeleteAsync(-1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Delete_ShouldSetTheDeletedOn_IfIdIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Delete_ShouldSetTheDeletedOn_IfIdIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act&AssertBeforeDeletion
                var result = await context.ContestCategories.ToListAsync();
                Assert.AreEqual(12, result.Count());

                //Act
                await contestCategoryService.DeleteAsync(1);
                result = await context.ContestCategories.ToListAsync();

                //Assert
                Assert.AreEqual(11, result.Count());

                Assert.AreEqual(2, result[0].Id);
                Assert.AreEqual(3, result[1].Id);

                Assert.AreEqual(null, result.FirstOrDefault(i => i.Id == 1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnCollection()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnCollection));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                var result = (List<ContestCategoryDto>)await contestCategoryService.GetAllAsync();

                //Assert
                Assert.AreEqual(12, result.Count());
                Assert.AreEqual(Constants.ConstestCategory.Abstract, result[0].Name);
                Assert.AreEqual(Constants.ConstestCategory.Architecture, result[1].Name);
                Assert.AreEqual(Constants.ConstestCategory.Conceptual, result[2].Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCorrectModel_WhenIdIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetById_ShouldReturnCorrectModel_WhenIdIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                var result = await contestCategoryService.GetByIdAsync(1);
                var result2 = await contestCategoryService.GetByIdAsync(2);
                var result3 = await contestCategoryService.GetByIdAsync(3);

                //Assert
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result2.Id);
                Assert.AreEqual(3, result3.Id);

                Assert.AreEqual(Constants.ConstestCategory.Abstract, result.Name);
                Assert.AreEqual(Constants.ConstestCategory.Architecture, result2.Name);
                Assert.AreEqual(Constants.ConstestCategory.Conceptual, result3.Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetById_ShouldThrowException_WhenIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetById_ShouldThrowException_WhenIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestCategoryService.GetByIdAsync(id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(13)]
        [DataRow(20)]
        [DataRow(100)]
        public async Task GetById_ShouldThrowException_WhenModelIsNotFound(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetById_ShouldThrowException_WhenModelIsNotFound));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await contestCategoryService.GetByIdAsync(id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task Update_ShouldThrowException_WhenModelIsInvalid(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldThrowException_WhenModelIsInvalid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await contestCategoryService.UpdateAsync(id, null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-10)]
        public async Task Update_ShouldThrowException_WhenIdIsZeroOrInvalid(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldThrowException_WhenIdIsZeroOrInvalid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestCategoryService.UpdateAsync(id, new ContestCategoryDto()));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-2)]
        public async Task Update_ShouldThrowException_WhenIdAndModelAreInvalid(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldThrowException_WhenIdAndModelAreInvalid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await contestCategoryService.UpdateAsync(id, null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(15)]
        [DataRow(20)]
        [DataRow(25)]
        public async Task Update_ShouldThrowException_WhenModelIsNotFound(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldThrowException_WhenModelIsNotFound));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await contestCategoryService.UpdateAsync(id, new ContestCategoryDto()));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Update_ShouldReturnTheUpdatedModel()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldReturnTheUpdatedModel));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);
                var model = new ContestCategoryDto()
                {
                    Name = "NewName"
                };

                //Act
                var result = await contestCategoryService.UpdateAsync(1, model);

                //Assert
                Assert.AreEqual("NewName", result.Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Update_ShouldReturnTheUpdatedModel_FromTheDB()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldReturnTheUpdatedModel_FromTheDB));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestCategoryService = new ContestCategoryService(context);
                var model = new ContestCategoryDto()
                {
                    Name = "NewName"
                };

                //Act
                await contestCategoryService.UpdateAsync(1, model);
                var result = await context.ContestCategories
                    .FirstOrDefaultAsync(cc => cc.Id == 1);

                //Assert
                Assert.AreEqual("NewName", result.Name);

                context.Database.EnsureDeleted();
            }
        }
    }
}

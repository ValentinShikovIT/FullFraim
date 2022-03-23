using FullFraim.Data;
using FullFraim.Models.Dto_s.ContestTypes;
using FullFraim.Services.ContestTypeServices;
using FullFraim.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.ContestTypeServices
{
    [TestClass]
    public class ContestTypeServiceTests
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
                var contestTypeService = new ContestTypeService(context);
                var contestTypeDto = new ContestTypeDto()
                {
                    Id = 4,
                    Name = "NewType",
                };

                //Act
                var result = await contestTypeService.CreateAsync(contestTypeDto);

                //Assert
                Assert.AreEqual(contestTypeDto.Id, result.Id);
                Assert.AreEqual(contestTypeDto.Name, result.Name);

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
                var contestTypeService = new ContestTypeService(context);
                var contestTypeDto = new ContestTypeDto()
                {
                    Id = 3,
                    Name = "NewType",
                };

                //Act
                await contestTypeService.CreateAsync(contestTypeDto);
                var result = await context.ContestTypes
                    .FirstOrDefaultAsync(ct => ct.Id == 3);

                //Assert
                Assert.AreEqual(contestTypeDto.Id, result.Id);
                Assert.AreEqual(contestTypeDto.Name, result.Name);

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<NullModelException>
                    (async () => await contestTypeService.CreateAsync(null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task Delete_ShouldThrowException_IfIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Delete_ShouldThrowException_IfIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestTypeService.DeleteAsync(id));

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
                var contestTypeService = new ContestTypeService(context);

                //Act&AssertBeforeDeletion
                var result = await context.ContestTypes.ToListAsync();
                Assert.AreEqual(2, result.Count());

                //Act
                await contestTypeService.DeleteAsync(1);
                result = await context.ContestTypes.ToListAsync();

                //Assert
                Assert.AreEqual(1, result.Count());

                Assert.AreEqual(2, result[0].Id);

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                var result = (List<ContestTypeDto>)await contestTypeService.GetAllAsync();

                //Assert
                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(Constants.ContestType.Open, result[0].Name);
                Assert.AreEqual(Constants.ContestType.Invitational, result[1].Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCorrect_WhenIdIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetById_ShouldReturnCorrect_WhenIdIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var contestTypeService = new ContestTypeService(context);

                //Act
                var result = await contestTypeService.GetByIdAsync(1);
                var result2 = await contestTypeService.GetByIdAsync(2);

                //Assert
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result2.Id);

                Assert.AreEqual(Constants.ContestType.Open, result.Name);
                Assert.AreEqual(Constants.ContestType.Invitational, result2.Name);

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestTypeService.GetByIdAsync(id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(10)]
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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await contestTypeService.GetByIdAsync(id));

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await contestTypeService.UpdateAsync(id, new ContestTypeDto()));

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await contestTypeService.UpdateAsync(id, null));

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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await contestTypeService.UpdateAsync(id, null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
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
                var contestTypeService = new ContestTypeService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await contestTypeService.UpdateAsync(id, new ContestTypeDto()));

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
                var contestTypeService = new ContestTypeService(context);
                var model = new ContestTypeDto()
                {
                    Name = "NewName"
                };

                //Act
                var result = await contestTypeService.UpdateAsync(1, model);

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
                var contestTypeService = new ContestTypeService(context);
                var model = new ContestTypeDto()
                {
                    Name = "NewName"
                };

                //Act
                await contestTypeService.UpdateAsync(1, model);
                var result = await context.ContestTypes
                    .FirstOrDefaultAsync(ct => ct.Id == 1);

                //Assert
                Assert.AreEqual("NewName", result.Name);

                context.Database.EnsureDeleted();
            }
        }
    }
}

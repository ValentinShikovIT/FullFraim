using FullFraim.Data;
using FullFraim.Models.Dto_s.Phases;
using FullFraim.Services.Exceptions;
using FullFraim.Services.PhaseServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.PhaseServices
{
    [TestClass]
    public class PhaseServiceTests
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
                var phaseService = new PhaseService(context);
                var phaseDto = new PhaseDto()
                {
                    Id = 4,
                    Name = "NewPhase",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(1)
                };

                //Act
                var result = await phaseService.CreateAsync(phaseDto);

                //Assert
                Assert.AreEqual(phaseDto.Id, result.Id);
                Assert.AreEqual(phaseDto.Name, result.Name);
                Assert.AreEqual(phaseDto.StartDate, result.StartDate);
                Assert.AreEqual(phaseDto.EndDate, result.EndDate);

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
                var phaseService = new PhaseService(context);
                var phaseDto = new PhaseDto()
                {
                    Id = 4,
                    Name = "NewPhase",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(1)
                };

                //Act
                await phaseService.CreateAsync(phaseDto);
                var result = await context.Phases
                    .FirstOrDefaultAsync(ph => ph.Id == 4);

                //Assert
                Assert.AreEqual(phaseDto.Id, result.Id);
                Assert.AreEqual(phaseDto.Name, result.Name);

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<NullModelException>
                    (async () => await phaseService.CreateAsync(null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task Delete_ShouldThrowException_IfIdIsZeroOrNegative()
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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert
                    .ThrowsExceptionAsync<InvalidIdException>
                    (async () => await phaseService.DeleteAsync(0));

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
                var phaseService = new PhaseService(context);

                //Act&AssertBeforeDeletion
                var result = await context.Phases.ToListAsync();
                Assert.AreEqual(3, result.Count());

                //Act
                await phaseService.DeleteAsync(1);
                result = await context.Phases.ToListAsync();

                //Assert
                Assert.AreEqual(2, result.Count());

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
                var phaseService = new PhaseService(context);

                //Act
                var result = (List<PhaseDto>)await phaseService.GetAllAsync();

                //Assert
                Assert.AreEqual(3, result.Count());
                Assert.AreEqual(Constants.Phases.PhaseI, result[0].Name);
                Assert.AreEqual(Constants.Phases.PhaseII, result[1].Name);
                Assert.AreEqual(Constants.Phases.Finished, result[2].Name);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetById_ShouldReturnCorrect_WhenIdValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetById_ShouldReturnCorrect_WhenIdValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var phaseService = new PhaseService(context);

                //Act
                var result = await phaseService.GetByIdAsync(1);
                var result2 = await phaseService.GetByIdAsync(2);
                var result3 = await phaseService.GetByIdAsync(3);

                //Assert
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result2.Id);
                Assert.AreEqual(3, result3.Id);

                Assert.AreEqual(Constants.Phases.PhaseI, result.Name);
                Assert.AreEqual(Constants.Phases.PhaseII, result2.Name);
                Assert.AreEqual(Constants.Phases.Finished, result3.Name);

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await phaseService.GetByIdAsync(id));

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await phaseService.GetByIdAsync(id));

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await phaseService.UpdateAsync(id, new PhaseDto()));

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await phaseService.UpdateAsync(id, null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-2)]
        public async Task Update_ShouldThrowException_WhenIdAndModelIsInvalid(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(Update_ShouldThrowException_WhenIdAndModelIsInvalid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await phaseService.UpdateAsync(id, null));

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
                var phaseService = new PhaseService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await phaseService.UpdateAsync(id, new PhaseDto()));

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
                var phaseService = new PhaseService(context);
                var model = new PhaseDto()
                {
                    Name = "NewName"
                };

                //Act
                var result = await phaseService.UpdateAsync(1, model);

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
                var phaseService = new PhaseService(context);
                var model = new PhaseDto()
                {
                    Name = "NewName"
                };

                //Act
                await phaseService.UpdateAsync(1, model);
                var result = await context.Phases
                    .FirstOrDefaultAsync(ph => ph.Id == 1);

                //Assert
                Assert.AreEqual("NewName", result.Name);

                context.Database.EnsureDeleted();
            }
        }
    }
}

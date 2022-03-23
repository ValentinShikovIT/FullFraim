using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.Dto_s.PhotoJunkies;
using FullFraim.Models.Dto_s.Users;
using FullFraim.Services.Exceptions;
using FullFraim.Services.PhotoJunkieServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.PhotoJunkieServices
{
    [TestClass]
    public class PhotoJunkieServiceTests
    {
        [TestMethod]
        public async Task EnrollForContestAsync_ShouldThrowException_WhenModelIsNull()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldThrowException_WhenModelIsNull));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>
                    (async () => await photoJunkieService.EnrollForContestAsync(null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldThrowException_WhenUserIsNotFound()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldThrowException_WhenUserIsNotFound));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 10,
                    ContestId = 1,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld"
                };
                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await photoJunkieService.EnrollForContestAsync(inputModel));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldThrowException_WhenContestIsNotFound()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldThrowException_WhenContestIsNotFound));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 2,
                    ContestId = 14,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld"
                };
                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<ArgumentException>
                    (async () => await photoJunkieService.EnrollForContestAsync(inputModel));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldAddPoints_WhenContestIsOpen()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldAddPoints_WhenContestIsOpen));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 6,
                    ContestId = 2,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld"
                };

                //Act
                await photoJunkieService.EnrollForContestAsync(inputModel);
                var result = await context.Users.FirstOrDefaultAsync(u => u.Id == 6);
                //Assert
                Assert.AreEqual((uint)1, result.Points);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldAddPoints_WhenContestIsInvitational()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldAddPoints_WhenContestIsInvitational));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 6,
                    ContestId = 1,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld"
                };

                //Act
                await photoJunkieService.EnrollForContestAsync(inputModel);
                var result = await context.Users.FirstOrDefaultAsync(u => u.Id == 6);
                //Assert
                Assert.AreEqual((uint)3, result.Points);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldAddParticipantContestEntry()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldAddParticipantContestEntry));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 6,
                    ContestId = 1,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld",
                    ImageTitle = "Loop"
                };

                //Act
                await photoJunkieService.EnrollForContestAsync(inputModel);
                var result = await context.ParticipantContests
                    .FirstOrDefaultAsync(pc => pc.UserId == 2 && pc.ContestId == 1);
                //Assert
                Assert.IsTrue(result != null);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task EnrollForContestAsync_ShouldAddAPhotoEntry()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(EnrollForContestAsync_ShouldAddAPhotoEntry));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var inputModel = new InputEnrollForContestDto()
                {
                    UserId = 6,
                    ContestId = 1,
                    PhotoUrl = "some",
                    ImageDescription = "helloWorld",
                    ImageTitle = "Loop",
                };

                //Act
                await photoJunkieService.EnrollForContestAsync(inputModel);
                var result = await context.Photos
                    .FirstOrDefaultAsync(ph => ph.Id == 18);

                //Assert
                Assert.IsTrue(result != null);

                Assert.AreEqual(inputModel.ImageTitle, result.Title);
                Assert.AreEqual(inputModel.ImageDescription, result.Story);
                Assert.AreEqual(inputModel.PhotoUrl, result.Url);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithoutFilter()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithoutFilter));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel(), new PaginationFilter());

                //Assert
                Assert.AreEqual(5, result.Model.Count());

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByFirstName()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByFirstName));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderBy(x => x.FirstName);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "firstnameasc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].FirstName == resultList[0].FirstName);
                Assert.IsTrue(expectedList[1].FirstName == resultList[1].FirstName);
                Assert.IsTrue(expectedList[2].FirstName == resultList[2].FirstName);
                Assert.IsTrue(expectedList[3].FirstName == resultList[3].FirstName);
                Assert.IsTrue(expectedList[4].FirstName == resultList[4].FirstName);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByLastName()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByLastName));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderBy(x => x.LastName);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "lastnameasc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].LastName == resultList[0].LastName);
                Assert.IsTrue(expectedList[1].LastName == resultList[1].LastName);
                Assert.IsTrue(expectedList[2].LastName == resultList[2].LastName);
                Assert.IsTrue(expectedList[3].LastName == resultList[3].LastName);
                Assert.IsTrue(expectedList[4].LastName == resultList[4].LastName);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingLastName()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingLastName));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderByDescending(x => x.LastName);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "lastnamedesc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].LastName == resultList[0].LastName);
                Assert.IsTrue(expectedList[1].LastName == resultList[1].LastName);
                Assert.IsTrue(expectedList[2].LastName == resultList[2].LastName);
                Assert.IsTrue(expectedList[3].LastName == resultList[3].LastName);
                Assert.IsTrue(expectedList[4].LastName == resultList[4].LastName);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingFirstName()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingFirstName));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderByDescending(x => x.FirstName);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "firstnamedesc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].FirstName == resultList[0].FirstName);
                Assert.IsTrue(expectedList[1].FirstName == resultList[1].FirstName);
                Assert.IsTrue(expectedList[2].FirstName == resultList[2].FirstName);
                Assert.IsTrue(expectedList[3].FirstName == resultList[3].FirstName);
                Assert.IsTrue(expectedList[4].FirstName == resultList[4].FirstName);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByPoints()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByPoints));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderBy(x => x.Points);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "pointsasc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].Points == resultList[0].Points);
                Assert.IsTrue(expectedList[1].Points == resultList[1].Points);
                Assert.IsTrue(expectedList[2].Points == resultList[2].Points);
                Assert.IsTrue(expectedList[3].Points == resultList[3].Points);
                Assert.IsTrue(expectedList[4].Points == resultList[4].Points);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingPoints()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingPoints));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderByDescending(x => x.Points);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "pointsdesc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].Points == resultList[0].Points);
                Assert.IsTrue(expectedList[1].Points == resultList[1].Points);
                Assert.IsTrue(expectedList[2].Points == resultList[2].Points);
                Assert.IsTrue(expectedList[3].Points == resultList[3].Points);
                Assert.IsTrue(expectedList[4].Points == resultList[4].Points);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByRank()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByRank));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderBy(x => x.Rank.Name);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "rankasc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].Rank.Name == resultList[0].Rank);
                Assert.IsTrue(expectedList[1].Rank.Name == resultList[1].Rank);
                Assert.IsTrue(expectedList[2].Rank.Name == resultList[2].Rank);
                Assert.IsTrue(expectedList[3].Rank.Name == resultList[3].Rank);
                Assert.IsTrue(expectedList[4].Rank.Name == resultList[4].Rank);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingRank()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAll_ShouldReturnAll_WithSortingModel_OrderByDescendingRank));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);
                var expected = TestUtils.GetUsers().OrderByDescending(x => x.Rank.Name);
                var expectedList = new List<User>(expected);

                //Act
                var result = await photoJunkieService
                    .GetAllAsync(new SortingModel() { OrderBy = "rankdesc" }, new PaginationFilter());
                var resultList = new List<PhotoJunkyDto>(result.Model);

                //Assert
                Assert.IsTrue(expectedList[0].Rank.Name == resultList[0].Rank);
                Assert.IsTrue(expectedList[1].Rank.Name == resultList[1].Rank);
                Assert.IsTrue(expectedList[2].Rank.Name == resultList[2].Rank);
                Assert.IsTrue(expectedList[3].Rank.Name == resultList[3].Rank);
                Assert.IsTrue(expectedList[4].Rank.Name == resultList[4].Rank);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsUserParticipant_ShouldReturnTrue_IfUserHasAlreadyEnrolled()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserParticipant_ShouldReturnTrue_IfUserHasAlreadyEnrolled));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                var result = await photoJunkieService
                    .IsUserParticipant(3, 2);

                //Assert
                Assert.IsTrue(result);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsUserParticipant_ShouldReturnFalse_IfUserHasAlreadyEnrolled()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserParticipant_ShouldReturnFalse_IfUserHasAlreadyEnrolled));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                var result = await photoJunkieService
                    .IsUserParticipant(10, 5);

                //Assert
                Assert.IsTrue(result == false);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserParticipant_ShouldThrowException_IfContestIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserParticipant_ShouldThrowException_IfContestIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoJunkieService
                    .IsUserParticipant(id, 4));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserParticipant_ShouldThrowException_IfUserIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserParticipant_ShouldThrowException_IfUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoJunkieService
                    .IsUserParticipant(4, id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsUserJury_ShouldReturnTrue_IfUserIsJuryForTheCurrentContest()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJury_ShouldReturnTrue_IfUserIsJuryForTheCurrentContest));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                var result = await photoJunkieService
                    .IsUserJury(3, 1);

                //Assert
                Assert.IsTrue(result);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsUserJury_ShouldReturnFalse_IfUserIsJuryForTheCurrentContest()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJury_ShouldReturnFalse_IfUserIsJuryForTheCurrentContest));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                var result = await photoJunkieService
                    .IsUserJury(3, 5);

                //Assert
                Assert.IsTrue(result == false);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserJury_ShouldThrowException_IfContestIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJury_ShouldThrowException_IfContestIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoJunkieService
                    .IsUserJury(id, 4));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserJury_ShouldThrowException_IfUserIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJury_ShouldThrowException_IfUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoJunkieService
                    .IsUserJury(4, id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetPointsTillNextRank_ShouldThrowException_IfUserIdIsZeroOrNegative()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPointsTillNextRank_ShouldThrowException_IfUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                User us = null;
                userMocked.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(us);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await photoJunkieService
                    .GetPointsTillNextRankAsync(1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0, 51)]
        [DataRow(60, 91)]
        [DataRow(500, 501)]
        public async Task GetPointsTillNextRank_ShouldReturnCorrectValue(int given, int expected)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPointsTillNextRank_ShouldReturnCorrectValue));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var user = new User()
                {
                    Id = 2,
                    FirstName = "Lolo",
                    LastName = "Lolo",
                    UserName = "Lolo@lo.com",
                    NormalizedUserName = "Lolo",
                    Email = "Lolo",
                    NormalizedEmail = "Lolo",
                    EmailConfirmed = true,
                    Points = (uint)given,
                    RankId = 1
                };
                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                userMocked.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

                //Act
                var result = await photoJunkieService
                    .GetPointsTillNextRankAsync(1);

                //Assert
                Assert.AreEqual(expected, result.PointsTillNextRank);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(20, 1, Constants.Ranks.Junkie)]
        [DataRow(200, 2, Constants.Ranks.Enthusiast)]
        [DataRow(500, 3, Constants.Ranks.Master)]
        [DataRow(1000, 4, Constants.Ranks.WiseAndBenevolentPhotoDictator)]
        public async Task GetPointsTillNextRank_ShouldMapRankNameCorrectly(int points, int rankId, string rank)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPointsTillNextRank_ShouldMapRankNameCorrectly));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userMocked = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var user = new User()
                {
                    Id = 2,
                    FirstName = "Lolo",
                    LastName = "Lolo",
                    UserName = "Lolo@lo.com",
                    NormalizedUserName = "Lolo",
                    Email = "Lolo",
                    NormalizedEmail = "Lolo",
                    EmailConfirmed = true,
                    Points = (uint)points,
                    RankId = rankId
                };
                var photoJunkieService = new PhotoJunkieService(context, userMocked.Object);

                userMocked.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);

                //Act
                var result = await photoJunkieService
                    .GetPointsTillNextRankAsync(1);

                //Assert
                Assert.AreEqual(rank, result.Rank);
                Assert.AreEqual(points, result.RankPoints);

                context.Database.EnsureDeleted();
            }
        }
    }
}

using FullFraim.Data;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Services.Exceptions;
using FullFraim.Services.PhotoService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.PhotoServices
{
    [TestClass]
    public class PhotoServiceTests
    {
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
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetByIdAsync(1);
                var result2 = await photoService.GetByIdAsync(2);
                var result3 = await photoService.GetByIdAsync(3);

                //Assert
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result2.Id);
                Assert.AreEqual(3, result3.Id);

                Assert.AreEqual(Constants.Images.WildlifeImgUrl, result.Url);
                Assert.AreEqual(Constants.Images.WildlifeImg2Url, result2.Url);
                Assert.AreEqual(Constants.Images.WildlifeImg3Url, result3.Url);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetDetailedSubmissionsForPhoto_ShouldReturnCorrectCount_WhenIdIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetDetailedSubmissionsForPhoto_ShouldReturnCorrectCount_WhenIdIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetDetailedSubmissionsForPhoto(1, 1, new PaginationFilter());

                //Assert
                Assert.AreEqual(1, result.Model.Count);

                context.Database.EnsureDeleted();
            }
        }


        [TestMethod]
        public async Task GetDetailedSubmissionsForPhoto_ShouldThrowInvalidIdException_WhenIsNegative()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetDetailedSubmissionsForPhoto_ShouldThrowInvalidIdException_WhenIsNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetDetailedSubmissionsForPhoto(-1, 1, new PaginationFilter()));

                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetDetailedSubmissionsForPhoto(1, -1, new PaginationFilter()));

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
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetByIdAsync(id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(18)]
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
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await photoService.GetByIdAsync(id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task IsPhotoSubmitedByUserAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsPhotoSubmitedByUserAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.IsPhotoSubmitedByUserAsync(id, 1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task IsPhotoSubmitedByUserAsync_ShouldThrowException_WhenPhotoIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsPhotoSubmitedByUserAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.IsPhotoSubmitedByUserAsync(2, id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsPhotoSubmitedByUserAsync_ShouldReturnTrue_IfUserHasSubmittedThePhoto()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsPhotoSubmitedByUserAsync_ShouldReturnTrue_IfUserHasSubmittedThePhoto));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.IsPhotoSubmitedByUserAsync(2, 1);

                //Assert
                Assert.IsTrue(result);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsPhotoSubmitedByUserAsync_ShouldReturnFalse_IfUserHasNotSubmittedThePhoto()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsPhotoSubmitedByUserAsync_ShouldReturnTrue_IfUserHasSubmittedThePhoto));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.IsPhotoSubmitedByUserAsync(2, 2);

                //Assert
                Assert.IsTrue(!result);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetPhotosForContestAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPhotosForContestAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetPhotosForContestAsync(id, 1, new PaginationFilter()));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetPhotosForContestAsync_ShouldThrowException_WhenContestIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPhotosForContestAsync_ShouldThrowException_WhenContestIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetPhotosForContestAsync(2, id, new PaginationFilter()));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(35)]
        [DataRow(100)]
        public async Task GetPhotosForContestAsync_ShouldReturnEmptyCollection_WhenUserIdDoesntExist(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPhotosForContestAsync_ShouldReturnEmptyCollection_WhenUserIdDoesntExist));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var photos = await photoService.GetPhotosForContestAsync(id, 1, new PaginationFilter());

                //Assert
                Assert.AreEqual(0, photos.Model.Count);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(35)]
        [DataRow(100)]
        public async Task GetPhotosForContestAsync_ShouldReturnEmptyCollection_WhenContestIdDoesntExist(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPhotosForContestAsync_ShouldReturnEmptyCollection_WhenContestIdDoesntExist));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var photos = await photoService.GetPhotosForContestAsync(2, id, new PaginationFilter());

                //Assert
                Assert.AreEqual(0, photos.Model.Count);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetPhotosForContestAsync_ShouldReturnCorrect_WhenInputIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPhotosForContestAsync_ShouldReturnCorrect_WhenInputIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetPhotosForContestAsync(1, 1, new PaginationFilter());

                //Assert
                Assert.AreEqual(4, result.Model.Count);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetUserSubmissionForContestAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetUserSubmissionForContestAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetUserSubmissionForContestAsync(id, 1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetUserSubmissionForContestAsync_ShouldThrowException_WhenContestIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetUserSubmissionForContestAsync_ShouldThrowException_WhenUserIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetUserSubmissionForContestAsync(2, id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(35)]
        [DataRow(100)]
        public async Task GetUserSubmissionForContestAsync_ShouldThrowException_WhenUserIdDoesntExist(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetUserSubmissionForContestAsync_ShouldThrowException_WhenUserIdDoesntExist));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await photoService.GetUserSubmissionForContestAsync(id, 1));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(35)]
        [DataRow(100)]
        public async Task GetUserSubmissionForContestAsync_ShouldThrowException_WhenContestIdDoesntExist(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetUserSubmissionForContestAsync_ShouldThrowException_WhenContestIdDoesntExist));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<NotFoundException>
                    (async () => await photoService.GetUserSubmissionForContestAsync(2, id));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetUserSubmissionForContestAsync_ShouldReturnCorrect_WhenInputIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetUserSubmissionForContestAsync_ShouldReturnCorrect_WhenInputIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetUserSubmissionForContestAsync(2, 1);


                //Assert
                Assert.AreEqual(1, result.Id);

                Assert.AreEqual(Constants.Images.WildlifeImgUrl, result.Url);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(-100)]
        public async Task GetDetailedSubmissionsFromContest_ShouldThrowException_WhenContestIdIsZeroOrNegative(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetDetailedSubmissionsFromContest_ShouldThrowException_WhenContestIdIsZeroOrNegative));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await photoService.GetDetailedSubmissionsFromContestAsync(id, new PaginationFilter()));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(35)]
        [DataRow(100)]
        public async Task GetDetailedSubmissionsFromContest_ShouldReturnEmptyCollection_WhenContestIdDoesntExist(int id)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetDetailedSubmissionsFromContest_ShouldReturnEmptyCollection_WhenContestIdDoesntExist));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var photos = await photoService.GetDetailedSubmissionsFromContestAsync(id, new PaginationFilter());

                //Assert
                Assert.AreEqual(0, photos.Model.Count);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetDetailedSubmissionsFromContest_ShouldReturnCorrect_WhenInputIsValid()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetDetailedSubmissionsFromContest_ShouldReturnCorrect_WhenInputIsValid));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetDetailedSubmissionsFromContestAsync(1, new PaginationFilter());

                //Assert
                Assert.AreEqual(4, result.Model.Count);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GetTopRecentPhotosAsync_ShouldReturnTopRecentPhotos()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetTopRecentPhotosAsync_ShouldReturnTopRecentPhotos));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var photoService = new PhotoService(context);

                //Act
                var result = await photoService.GetTopRecentPhotosAsync();

                //Assert
                Assert.AreEqual(4, result.Count());

                context.Database.EnsureDeleted();
            }
        }
    }
}

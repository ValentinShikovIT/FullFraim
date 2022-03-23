using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Reviews;
using FullFraim.Services.Exceptions;
using FullFraim.Services.JuryServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.JuryServices
{
    [TestClass]
    public class JuryServiceTests
    {
        [TestMethod]
        public async Task HasJuryAlreadyGivenReview_ShouldReturnTrue()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(HasJuryAlreadyGivenReview_ShouldReturnTrue));

            using (var dbContext = new FullFraimDbContext(options))
            {
                var collectionPhotoReviews = new PhotoReview[]
                    {
                        new PhotoReview()
                            {
                                JuryContestId = 1,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                        new PhotoReview()
                            {
                                JuryContestId = 2,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                        new PhotoReview()
                            {
                                JuryContestId = 3,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                    };

                await dbContext.Photos.AddRangeAsync(TestUtils.GetPhotos());
                await dbContext.JuryContests.AddRangeAsync(TestUtils.GetJuryContests());
                await dbContext.PhotoReviews.AddRangeAsync(collectionPhotoReviews);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);

                //Act
                bool reviewShouldBePositive = await juryService.HasJuryAlreadyGivenReviewAsync(1, 1);

                //Assert
                Assert.AreEqual(true, reviewShouldBePositive);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task HasJuryAlreadyGivenReview_ShouldReturnFalse()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(HasJuryAlreadyGivenReview_ShouldReturnFalse));

            using (var dbContext = new FullFraimDbContext(options))
            {
                var collectionPhotoReviews = new PhotoReview[]
                    {
                        new PhotoReview()
                            {
                                JuryContestId = 1,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                        new PhotoReview()
                            {
                                JuryContestId = 2,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                        new PhotoReview()
                            {
                                JuryContestId = 3,
                                PhotoId = 1,
                                Score = 4,
                                Checkbox = false,
                                Comment = "nice",
                            },
                    };

                await dbContext.Photos.AddRangeAsync(TestUtils.GetPhotos());
                await dbContext.JuryContests.AddRangeAsync(TestUtils.GetJuryContests());
                await dbContext.PhotoReviews.AddRangeAsync(collectionPhotoReviews);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);

                //Act
                bool reviewShouldBeNegative = await juryService.HasJuryAlreadyGivenReviewAsync(2, 2);

                //Assert
                Assert.AreEqual(false, reviewShouldBeNegative);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task IsContestInPhaseTwo_ShouldReturnTrue()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsContestInPhaseTwo_ShouldReturnTrue));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);

                //Act
                bool reviewShouldBePositive = await juryService.IsContestInPhaseTwoAsync(5);

                //Assert
                Assert.AreEqual(true, reviewShouldBePositive);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public async Task IsContestInPhaseTwo_ShouldReturnFalse(int photoId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsContestInPhaseTwo_ShouldReturnFalse));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);

                //Act
                bool reviewShouldBePositive = await juryService.IsContestInPhaseTwoAsync(photoId);

                //Assert
                Assert.AreEqual(false, reviewShouldBePositive);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldReturnCorrectOutput()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldReturnCorrectOutput));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                var result = await juryService
                    .GiveReviewAsync(inputReviewModel);

                //Assert
                Assert.AreEqual(inputReviewModel.PhotoId, result.PhotoId);
                Assert.AreEqual(inputReviewModel.JuryId, result.JuryId);
                Assert.AreEqual(inputReviewModel.Checkbox, result.Checkbox);
                Assert.AreEqual(inputReviewModel.Comment, result.Comment);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldAddInTheDatabase()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldAddInTheDatabase));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                var result = await juryService
                    .GiveReviewAsync(inputReviewModel);

                var reviewFromDB = context.PhotoReviews.FirstOrDefault(phR => phR.Id == 11);

                //Assert
                Assert.AreEqual(inputReviewModel.PhotoId, reviewFromDB.PhotoId);
                Assert.AreEqual(inputReviewModel.JuryId, reviewFromDB.JuryContestId);
                Assert.AreEqual(inputReviewModel.Checkbox, reviewFromDB.Checkbox);
                Assert.AreEqual(inputReviewModel.Comment, reviewFromDB.Comment);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldThrowException_WhenNullModelIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldThrowException_WhenNullModelIsPassed));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);

                //Act

                //Assert
                await Assert.ThrowsExceptionAsync<NullModelException>(async () => await juryService
                    .GiveReviewAsync(null));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldNotThrowException_WhenCorrectModelIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldNotThrowException_WhenCorrectModelIsPassed));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                var result = await juryService
                    .GiveReviewAsync(inputReviewModel);
                //Assert
                Assert.IsTrue(result != null);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task GetReview_ShouldThrowException_WhenInvalidJuryId(int juryId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetReview_ShouldThrowException_WhenInvalidJuryId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .GetReviewAsync(juryId, 4));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task GetReview_ShouldThrowException_WhenInvalidPhotoId(int photoId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetReview_ShouldThrowException_WhenInvalidPhotoId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .GetReviewAsync(4, photoId));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsJuryGivenReviewForPhotoAsync_ShouldThrowException_WhenInvalidJuryId(int juryId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsJuryGivenReviewForPhotoAsync_ShouldThrowException_WhenInvalidJuryId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .IsJuryGivenReviewForPhotoAsync(juryId, 4));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsJuryGivenReviewForPhotoAsync_ShouldThrowException_WhenInvalidPhotoId(int photoId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsJuryGivenReviewForPhotoAsync_ShouldThrowException_WhenInvalidPhotoId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .IsJuryGivenReviewForPhotoAsync(4, photoId));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserJuryForContest_ShouldThrowException_WhenInvalidJuryId(int juryId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJuryForContest_ShouldThrowException_WhenInvalidJuryId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .IsUserJuryForContest(juryId, 4));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        [DataRow(-5)]
        public async Task IsUserJuryForContest_ShouldThrowException_WhenInvalidContestId(int contestId)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsUserJuryForContest_ShouldThrowException_WhenInvalidContestId));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                //Assert
                await Assert.ThrowsExceptionAsync<InvalidIdException>
                    (async () => await juryService
                    .IsUserJuryForContest(4, contestId));

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldReturnZeroScore_WhenCheckboxIsTrue()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldReturnZeroScore_WhenCheckboxIsTrue));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = true,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                var result = await juryService
                    .GiveReviewAsync(inputReviewModel);

                //Assert
                Assert.AreEqual((uint)0, result.Score);
                Assert.AreEqual("Wrong category", result.Comment);

                context.Database.EnsureDeleted();
            }
        }

        [TestMethod]
        public async Task GiveReview_ShouldReturnCorrectScore_WhenCheckboxIsFalse()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GiveReview_ShouldReturnCorrectScore_WhenCheckboxIsFalse));

            using (var dbContext = new FullFraimDbContext(options))
            {
                await TestUtils.DatabaseFullSeed(dbContext);

                await dbContext.SaveChangesAsync();
            }

            using (var context = new FullFraimDbContext(options))
            {
                var juryService = new JuryService(context);
                var inputReviewModel = new InputGiveReviewDto()
                {
                    PhotoId = 1,
                    JuryId = 1,
                    Checkbox = false,
                    Comment = "Biva",
                    Score = 6
                };

                //Act
                var result = await juryService
                    .GiveReviewAsync(inputReviewModel);

                //Assert
                Assert.AreEqual((uint)6, result.Score);
                Assert.AreEqual("Biva", result.Comment);

                context.Database.EnsureDeleted();
            }
        }
    }
}

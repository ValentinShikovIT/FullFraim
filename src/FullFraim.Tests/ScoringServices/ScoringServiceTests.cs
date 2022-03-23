using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Services.ScoringServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.ScoringServices
{
    [TestClass]
    public class ScoringServiceTests
    {
        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(1, 1)]
        [DataRow(2, 2)]
        [DataRow(3, 3)]
        [DataRow(10, 10)]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreOnlyTwoWinnersWithSameScore(int scoreOne, int scoreTwo)
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreOnlyTwoWinnersWithSameScore));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)scoreOne,
                    PhotoId = 1,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)scoreTwo,
                    PhotoId = 2,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                Assert.AreEqual((uint)40, firstUserPoints);
                Assert.AreEqual((uint)40, secondUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreOnlyTwoWinnersWithDifferentReviewsCount()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreOnlyTwoWinnersWithDifferentReviewsCount));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)1,
                    PhotoId = 1,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)1,
                    PhotoId = 1,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)2,
                    PhotoId = 2,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                Assert.AreEqual((uint)40, firstUserPoints);
                Assert.AreEqual((uint)40, secondUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersWithSameReviewsCountWithDifferentScoreAndSameFinalScore()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersWithSameReviewsCountWithDifferentScoreAndSameFinalScore));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)3,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)4,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)2,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview()
                {
                    Score = (uint)5,
                    PhotoId = 10,
                });

                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                Assert.AreEqual((uint)40, firstUserPoints);
                Assert.AreEqual((uint)40, secondUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreThreeWinnersWithDifferentScore()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreThreeWinnersWithDifferentScore));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)5,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)6,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)7,
                    PhotoId = 11,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;

                Assert.AreEqual((uint)20, firstUserPoints);
                Assert.AreEqual((uint)35, secondUserPoints);
                Assert.AreEqual((uint)50, thirdUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForFirstPrize()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForFirstPrize));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)5,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)7,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)7,
                    PhotoId = 11,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;

                Assert.AreEqual((uint)35, firstUserPoints);
                Assert.AreEqual((uint)40, secondUserPoints);
                Assert.AreEqual((uint)40, thirdUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenFirstWinnerScoreDoublesTheSecondWinnerScore()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenFirstWinnerScoreDoublesTheSecondWinnerScore));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)4,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)5,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)10,
                    PhotoId = 11,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;

                Assert.AreEqual((uint)20, firstUserPoints);
                Assert.AreEqual((uint)35, secondUserPoints);
                Assert.AreEqual((uint)75, thirdUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoFirstWinnersWhichScoreDoublesTheSecondWinnerScore()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoFirstWinnersWhichScoreDoublesTheSecondWinnerScore));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)4,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)5,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)10,
                    PhotoId = 11,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 5
                {
                    Score = (uint)10,
                    PhotoId = 12,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;
                var fourthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 5).Points;

                Assert.AreEqual((uint)20, firstUserPoints);
                Assert.AreEqual((uint)35, secondUserPoints);
                Assert.AreEqual((uint)75, thirdUserPoints);
                Assert.AreEqual((uint)75, fourthUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForSecondPrize()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForSecondPrize));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)4,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)6,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)6,
                    PhotoId = 11,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 5
                {
                    Score = (uint)10,
                    PhotoId = 12,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;
                var fourthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 5).Points;

                Assert.AreEqual((uint)20, firstUserPoints);
                Assert.AreEqual((uint)25, secondUserPoints);
                Assert.AreEqual((uint)25, thirdUserPoints);
                Assert.AreEqual((uint)50, fourthUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForThirdPrize()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreTwoWinnersForThirdPrize));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 2
                {
                    Score = (uint)4,
                    PhotoId = 9,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 3
                {
                    Score = (uint)4,
                    PhotoId = 10,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 4
                {
                    Score = (uint)6,
                    PhotoId = 11,
                });

                arrangeContext.PhotoReviews.Add(new PhotoReview() // userId 5
                {
                    Score = (uint)10,
                    PhotoId = 12,
                });

                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;
                var fourthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 5).Points;

                Assert.AreEqual((uint)10, firstUserPoints);
                Assert.AreEqual((uint)10, secondUserPoints);
                Assert.AreEqual((uint)35, thirdUserPoints);
                Assert.AreEqual((uint)50, fourthUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldNotUpdateWinnersPointsInDatabase_WhenThereAreNoParticipantsInTheContest()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldNotUpdateWinnersPointsInDatabase_WhenThereAreNoParticipantsInTheContest));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(5);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;
                var fourthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 5).Points;
                var fifthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 6).Points;

                Assert.AreEqual((uint)0, firstUserPoints);
                Assert.AreEqual((uint)0, secondUserPoints);
                Assert.AreEqual((uint)0, thirdUserPoints);
                Assert.AreEqual((uint)0, fourthUserPoints);
                Assert.AreEqual((uint)0, fifthUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreFourParticipantsWithoutReviews()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(AwardWinnersAsync_ShouldUpdateWinnersPointsInDatabase_WhenThereAreFourParticipantsWithoutReviews));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Photos.AddRange(TestUtils.GetPhotos());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var sut = new ScoringService(assertContext);
                // Act
                await sut.AwardWinnersAsync(3);

                // Assert
                var firstUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 2).Points;
                var secondUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 3).Points;
                var thirdUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 4).Points;
                var fourthUserPoints = assertContext.Users.FirstOrDefault(u => u.Id == 5).Points;

                Assert.AreEqual((uint)40, firstUserPoints);
                Assert.AreEqual((uint)40, secondUserPoints);
                Assert.AreEqual((uint)40, thirdUserPoints);
                Assert.AreEqual((uint)40, fourthUserPoints);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
    }
}

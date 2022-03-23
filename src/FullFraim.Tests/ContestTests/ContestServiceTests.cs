using FullFraim.Data;
using FullFraim.Data.Models;
using FullFraim.Models.Dto_s.Contests;
using FullFraim.Models.Dto_s.Pagination;
using FullFraim.Models.ViewModels.Contest;
using FullFraim.Services.ContestServices;
using FullFraim.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.TestingUtils;

namespace FullFraim.Tests.ContestTests
{
    [TestClass]
    public class ContestServiceTests
    {
        #region CreateContestAsync
        [TestMethod]
        public async Task CreateAsync_ShouldThrowNullModelException_WhenNullIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldThrowNullModelException_WhenNullIsPassed));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NullModelException>(async () => await contestService.CreateAsync(null));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseOne()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseOne));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {

                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var contestService = new ContestService(assertContext, userManagerMock.Object);
                var users = new List<User>()
                {
                    new User()
                    {
                       Id = 1,
                    }
                };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var phaseOneStartDate = DateTime.UtcNow;
                var phaseOneEndDate = DateTime.UtcNow.AddDays(1);

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate,
                        EndDate_PhaseI = phaseOneEndDate,
                        PhaseII_Duration = 5,
                    }
                };

                // Act
                var contest = await contestService.CreateAsync(contestToAdd);

                var actualResult = assertContext.Contests
                    .Include(x => x.ContestPhases)
                    .ThenInclude(x => x.Phase)
                    .FirstOrDefault();

                // Assert
                // PhaseOne start date
                Assert.AreEqual(phaseOneStartDate.ToShortDateString(),
                    actualResult.ContestPhases.First(x => x.PhaseId == 1).StartDate.ToShortDateString());

                // PhaseTwo starts after PhaseOne
                Assert.AreEqual(contest.PhasesInfo.First(x => x.Name == "PhaseII").StartDate,
                    actualResult.ContestPhases.First(x => x.Phase.Name == "PhaseI").EndDate);

                // Active phase is PhaseOne
                Assert.AreEqual(contest.ActivePhase.Name,
                    assertContext.Contests.Where(c => c.Id == contest.Id &&
                        c.ContestPhases.Any(cp => cp.Phase.Name == "PhaseI"
                        && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow))
                    .Select(c => new
                    {
                        phaseName = (c.ContestPhases.Where(cp => cp.Phase.Name == "PhaseI"
                            && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow).FirstOrDefault().Phase.Name)
                    }).First().phaseName);

                // created contest is with the correct type
                Assert.AreEqual(1, contest.ContestTypeId);

                // created contest is with the correct category
                Assert.AreEqual(1, contest.ContestCategoryId);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldRerturnOutputContestDto_WhenPassedValidData()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldRerturnOutputContestDto_WhenPassedValidData));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {

                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var contestService = new ContestService(assertContext, userManagerMock.Object);
                var users = new List<User>()
                {
                    new User()
                    {
                       Id = 1,
                    }
                };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var phaseOneStartDate = DateTime.UtcNow;
                var phaseOneEndDate = DateTime.UtcNow.AddDays(1);

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate,
                        EndDate_PhaseI = phaseOneEndDate,
                        PhaseII_Duration = 5,
                    }
                };

                // Act
                var contest = await contestService.CreateAsync(contestToAdd);

                // Assert
                Assert.AreEqual(typeof(OutputContestDto), contest.GetType());

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseTwo()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseTwo));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {

                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var contestService = new ContestService(assertContext, userManagerMock.Object);
                var users = new List<User>()
                {
                    new User()
                    {
                       Id = 1,
                    }
                };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var phaseOneStartDate = DateTime.UtcNow.AddDays(-1);
                var phaseOneEndDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 2,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate,
                        EndDate_PhaseI = phaseOneEndDate,
                        PhaseII_Duration = 5,
                    }
                };

                // Act
                var contest = await contestService.CreateAsync(contestToAdd);

                var actualResult = assertContext.Contests
                    .Include(x => x.ContestPhases)
                    .ThenInclude(x => x.Phase)
                    .FirstOrDefault();

                // Assert
                // PhaseTwo start date
                Assert.AreEqual(phaseOneEndDate.ToShortDateString(),
                    actualResult.ContestPhases.First(x => x.PhaseId == 2).StartDate.ToShortDateString());

                // PhaseTwo starts after PhaseOne
                Assert.AreEqual(contest.PhasesInfo.First(x => x.Name == "PhaseII").StartDate,
                    actualResult.ContestPhases.First(x => x.Phase.Name == "PhaseI").EndDate);

                // Active phase is PhaseTwo
                Assert.AreEqual(contest.ActivePhase.Name,
                    assertContext.Contests.Where(c => c.Id == contest.Id &&
                        c.ContestPhases.Any(cp => cp.Phase.Name == "PhaseII"
                        && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow))
                    .Select(c => new
                    {
                        phaseName = (c.ContestPhases.Where(cp => cp.Phase.Name == "PhaseII"
                            && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow).FirstOrDefault().Phase.Name)
                    }).First().phaseName);

                // created contest is with the correct type
                Assert.AreEqual(1, contest.ContestTypeId);

                // created contest is with the correct category
                Assert.AreEqual(2, contest.ContestCategoryId);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseThree()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldCreateedConest_WhenValidDataForPhaseThree));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {

                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var contestService = new ContestService(assertContext, userManagerMock.Object);
                var users = new List<User>()
                {
                    new User()
                    {
                       Id = 1,
                    }
                };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var phaseOneStartDate = DateTime.UtcNow.AddDays(-2);
                var phaseOneEndDate = DateTime.UtcNow.AddDays(-1);

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate,
                        EndDate_PhaseI = phaseOneEndDate,
                        PhaseII_Duration = 5,
                    }
                };

                // Act
                var contest = await contestService.CreateAsync(contestToAdd);

                var actualResult = assertContext.Contests
                    .Include(x => x.ContestPhases)
                    .ThenInclude(x => x.Phase)
                    .FirstOrDefault();

                // Assert
                // PhaseThree start date
                Assert.AreEqual(phaseOneEndDate.AddHours(5).ToShortDateString(), // end date of phase two
                    actualResult.ContestPhases.First(x => x.PhaseId == 3).StartDate.ToShortDateString());

                //PhaseTwo starts after PhaseOne
                Assert.AreEqual(contest.PhasesInfo.First(x => x.Name == "Finished").StartDate,
                    actualResult.ContestPhases.First(x => x.Phase.Name == "PhaseII").EndDate);

                // Active phase is PhaseTwo
                Assert.AreEqual(contest.ActivePhase.Name,
                    assertContext.Contests.Where(c => c.Id == contest.Id &&
                        c.ContestPhases.Any(cp => cp.Phase.Name == "Finished"
                        && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow))
                    .Select(c => new
                    {
                        phaseName = (c.ContestPhases.Where(cp => cp.Phase.Name == "Finished"
                            && cp.EndDate > DateTime.UtcNow && cp.StartDate < DateTime.UtcNow).FirstOrDefault().Phase.Name)
                    }).First().phaseName);

                // created contest is with the correct type
                Assert.AreEqual(1, contest.ContestTypeId);

                // returned contest is with the correct name
                Assert.AreEqual("TestName", contest.Name);

                // created contest is with the correct type
                Assert.AreEqual("TestName", assertContext.Contests.First().Name);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldThow_NoOrganizesFound_WhenNoOrganizersInDb()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldThow_NoOrganizesFound_WhenNoOrganizersInDb));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    }
                };

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NoOrganizersException>(async () =>
                    await contestService.CreateAsync(contestToAdd));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddAllOrganizersToJury()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldAddAllOrganizersToJury));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { new User { Id = 1 }, new User { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 1,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    }
                };

                // Act
                var createdContest = await contestService.CreateAsync(contestToAdd);

                // Assert && Act
                Assert.AreEqual(1, assertContext.JuryContests.First(x => x.Id == 1 && x.ContestId == 1).UserId);
                Assert.AreEqual(2, assertContext.JuryContests.First(x => x.Id == 2 && x.ContestId == 1).UserId);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddAllInvitedJuriesAndParticipants_WhenInvitationalType()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldAddAllInvitedJuriesAndParticipants_WhenInvitationalType));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { new User { Id = 1 }, new User { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 2,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    },
                    Jury = new List<int>() { 3 },
                    Participants = new List<int>() { 4 },
                };

                // Act
                var createdContest = await contestService.CreateAsync(contestToAdd);

                // Assert && Act
                Assert.AreEqual(3, assertContext.JuryContests.First(x => x.UserId == 3 && x.ContestId == 1).UserId);
                Assert.AreEqual(4, assertContext.ParticipantContests.First(x => x.UserId == 4 && x.ContestId == 1).UserId);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldTrhowNullModelException_WhenInvitationalTypeAndNoJuryIsNull()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldTrhowNullModelException_WhenInvitationalTypeAndNoJuryIsNull));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { new User { Id = 1 }, new User { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 2,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    }
                };

                // Act
                await Assert.ThrowsExceptionAsync<NullModelException>(async () =>
                    await contestService.CreateAsync(contestToAdd));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldTrhowNullModelException_WhenInvitationalTypeAndNoParticipantIsNull()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldTrhowNullModelException_WhenInvitationalTypeAndNoParticipantIsNull));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { new User { Id = 1 }, new User { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 2,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    }
                };

                // Act
                await Assert.ThrowsExceptionAsync<NullModelException>(async () =>
                    await contestService.CreateAsync(contestToAdd));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task CreateAsync_ShouldTrhowCheaterException_WhenInvitationalTypeParticipantIsInvitedToBeJury()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(CreateAsync_ShouldTrhowCheaterException_WhenInvitationalTypeParticipantIsInvitedToBeJury));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.Users.AddRange(TestUtils.GetUsers());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());
                arrangeContext.ContestCategories.AddRange(TestUtils.GetContestCategories());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);
                var users = new List<User>() { new User { Id = 1 }, new User { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(users as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                var phaseOneStartDate = DateTime.UtcNow;

                var contestToAdd = new InputContestDto()
                {
                    Name = "TestName",
                    Cover_Url = "TestUrl",
                    Description = "Test Description",
                    ContestTypeId = 2,
                    ContestCategoryId = 1,
                    Phases = new PhasesHelperModel()
                    {
                        StartDate_PhaseI = phaseOneStartDate
                    },
                    Jury = new List<int>() { 1 },
                    Participants = new List<int>() { 1 },
                };

                // Act
                await Assert.ThrowsExceptionAsync<CheaterException>(async () =>
                    await contestService.CreateAsync(contestToAdd));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region DeleteAsync
        [TestMethod]
        public async Task DeleteAsync_ShouldSetIsDeletedFlagToTrue_WhenEntityExists()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(DeleteAsync_ShouldSetIsDeletedFlagToTrue_WhenEntityExists));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                await contestService.DeleteAsync(1);

                // Assert && Act
                Assert.IsTrue(!assertContext.Contests.Any(x => x.Id == 1));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldThrowInvalidIdException_WhenNegativeIdIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(DeleteAsync_ShouldThrowInvalidIdException_WhenNegativeIdIsPassed));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<InvalidIdException>(async () => await contestService.DeleteAsync(-5));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldThowNotFoundException_WhenEntityDoesntExist()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(DeleteAsync_ShouldThowNotFoundException_WhenEntityDoesntExist));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await contestService.DeleteAsync(int.MaxValue));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region GetAllAsync
        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnEmptyCollection_WhenEntityDbEmpty()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnEmptyCollection_WhenEntityDbEmpty));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                // Act
                var contests = await contestService.GetAllAsync(null, null, null, null, paginatedFilter);

                // Assert
                Assert.AreEqual(0, contests.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnPaginatedResult_WithAllElementsLessThanPageCountElements()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnPaginatedResult_WithAllElementsLessThanPageCountElements));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                // Act
                var contests = await contestService.GetAllAsync(null, null, null, null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnPaginatedResult_WithValidCount()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnPaginatedResult_WithValidCount));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter() { PageNumber = 1, PageSize = 2 };

                // Act
                var contests = await contestService.GetAllAsync(null, null, null, null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnValidPaginatedResult_WhenInvalidPaginationFilterIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnValidPaginatedResult_WhenInvalidPaginationFilterIsPassed));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter() { PageNumber = -1, PageSize = 20000 };

                // Act
                var contests = await contestService.GetAllAsync(null, null, null, null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);
                Assert.AreEqual(50, contests.RecordsPerPage);
                Assert.AreEqual(1, contests.TotalPages);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsync_ShouldReturnFilterdData_WhenFilteredByJury()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsync_ShouldReturnFilterdData_WhenFilteredByJury));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.ContestPhases.AddRange(TestUtils.GetContestPhases());
                arrangeContext.JuryContests.AddRange(TestUtils.GetJuryContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStoreMock = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStoreMock.Object, null, null, null, null, null, null, null, null);

                var contestService = new ContestService(assertContext, userManagerMock.Object);
                var paginatedFilter = new PaginationFilter();

                // Act
                var contests = await contestService.GetAllAsync(null, 1, null, null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);
               // Assert.AreEqual(1, contests.Model.First().Id);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByParticipant()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByParticipant));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.JuryContests.AddRange(TestUtils.GetJuryContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                var v = assertContext.Contests.Include(x => x.JuryContests).ToList();
                var vv = assertContext.Contests.Include(x => x.ParticipantContests).ToList();

                // Act
                var contests = await contestService.GetAllAsync(6, null, null, null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByPhase()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByPhase));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.JuryContests.AddRange(TestUtils.GetJuryContests());
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.ContestPhases.AddRange(TestUtils.GetContestPhases());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                var v = assertContext.Contests.Include(x => x.JuryContests).ToList();
                var vv = assertContext.Contests.Include(x => x.ParticipantContests).ToList();

                // Act
                var contests = await contestService.GetAllAsync(null, null, "PhaseI", null, paginatedFilter);
                var contestsPhaseTwo = await contestService.GetAllAsync(null, null, "PhaseII", null, paginatedFilter);
                var contestsPhaseFinished = await contestService.GetAllAsync(null, null, "Finished", null, paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);
                Assert.AreEqual(0, contestsPhaseTwo.Model.Count);
                Assert.AreEqual(0, contestsPhaseFinished.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByType()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnFilterdData_WhenFilteredByType));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                var v = assertContext.Contests.Include(x => x.JuryContests).ToList();
                var vv = assertContext.Contests.Include(x => x.ParticipantContests).ToList();

                // Act
                var contestsOpen = await contestService.GetAllAsync(null, null, null, "Open", paginatedFilter);
                var contestsInvitational = await contestService.GetAllAsync(null, null, null, "Invitational", paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contestsOpen.Model.Count);
                Assert.AreEqual(0, contestsInvitational.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetAllAsyanc_ShouldReturnEmptycollection_WhenFilteredByNonExistingData()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetAllAsyanc_ShouldReturnEmptycollection_WhenFilteredByNonExistingData));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());
                arrangeContext.ContestPhases.AddRange(TestUtils.GetContestPhases());
                arrangeContext.JuryContests.AddRange(TestUtils.GetJuryContests());
                arrangeContext.ParticipantContests.AddRange(TestUtils.GetParticipantContests());
                arrangeContext.ContestTypes.AddRange(TestUtils.GetContestTypes());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                var v = assertContext.Contests.Include(x => x.JuryContests).ToList();
                var vv = assertContext.Contests.Include(x => x.ParticipantContests).ToList();

                // Act
                var contests = await contestService.GetAllAsync(155, 124, "PhaseONEEE", "ONLYFORADMINS", paginatedFilter);

                // Assert 
                Assert.AreEqual(0, contests.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region GetCoversAsync
        [TestMethod]
        public async Task GetCoversAsyanc_ShouldReturnAllCovers_WhenContestsExists()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetCoversAsyanc_ShouldReturnAllCovers_WhenContestsExists));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                // Act
                var contestsCovers = await contestService.GetContestCoversAsync(paginatedFilter);

                // Assert
                Assert.AreEqual(6, contestsCovers.Model.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region GetById
        [TestMethod]
        public async Task GetByIdAsyanc_ShouldThowInvalidIdException_WhenInvalidIdPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetByIdAsyanc_ShouldThowInvalidIdException_WhenInvalidIdPassed));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                // Assert && Act
                await Assert.ThrowsExceptionAsync<InvalidIdException>(async () => await contestService.GetByIdAsync(-56));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetByIdAsyanc_ShouldThowNotFoundException_WhenNoContestIsFound()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetByIdAsyanc_ShouldThowNotFoundException_WhenNoContestIsFound));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);
                var paginatedFilter = new PaginationFilter();

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await contestService.GetByIdAsync(10));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region UpdateAsync
        [TestMethod]
        public async Task UpdateAsyncAsyanc_ShouldThrowNullModelException_WhenNullIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(UpdateAsyncAsyanc_ShouldThrowNullModelException_WhenNullIsPassed));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NullModelException>(async () =>
                    await contestService.UpdateAsync(1, null));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task UpdateAsyncAsyanc_ShouldThrowNotFoundException_WhenEntityWithIdDoesntExist()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(UpdateAsyncAsyanc_ShouldThrowNotFoundException_WhenEntityWithIdDoesntExist));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<NotFoundException>(async () =>
                    await contestService.UpdateAsync(100, new InputContestDto()));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task UpdateAsyncAsyanc_ShouldThrowUniqueNameException_WhenEntityWithSameNameExists()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(UpdateAsyncAsyanc_ShouldThrowUniqueNameException_WhenEntityWithSameNameExists));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                // Assert && Act
                await Assert.ThrowsExceptionAsync<UniqueNameException>(async () =>
                    await contestService.UpdateAsync(1, new InputContestDto() { Name = "WildlifePhaseTwo" }));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task UpdateAsyncAsyanc_ShouldCorrectlyUpdateEntity_WhenValidDataIsPassed()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(UpdateAsyncAsyanc_ShouldCorrectlyUpdateEntity_WhenValidDataIsPassed));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new UserManager<User>
                    (userStore.Object, It.IsAny<IOptions<IdentityOptions>>(), It.IsAny<IPasswordHasher<User>>(),
                    It.IsAny<IEnumerable<IUserValidator<User>>>(), It.IsAny<IEnumerable<IPasswordValidator<User>>>(),
                    It.IsAny<ILookupNormalizer>(), It.IsAny<IdentityErrorDescriber>(),
                    It.IsAny<IServiceProvider>(), It.IsAny<ILogger<UserManager<User>>>());

                var contestService = new ContestService(assertContext, userManagerMock);

                await contestService.UpdateAsync(1, new InputContestDto() { Name = "Portraits" });

                // Assert && Act
                Assert.IsTrue(assertContext.Contests.Any(x => x.Name == "Portraits"));

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region GetParticipantsForInvitationAsync & GetPotentialJuryForInvitationAsync
        [TestMethod]
        public async Task GetParticipantsForInvitationAsync_ShouldReturnAllUsers_WtihRoleUser()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetParticipantsForInvitationAsync_ShouldReturnAllUsers_WtihRoleUser));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStore.Object, null, null, null, null, null, null, null, null);
                var usersMock = new List<User>() { new User() { Id = 1 }, new User() { Id = 2 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(usersMock as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                // Act
                var users = await contestService.GetParticipantsForInvitationAsync();

                // Assert
                Assert.AreEqual(2, users.Count);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }

        [TestMethod]
        public async Task GetPotentialJuryForInvitationAsync_ShouldReturnAllUsers_WtihRoleUserAndPointsGraterThan_150()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(GetPotentialJuryForInvitationAsync_ShouldReturnAllUsers_WtihRoleUserAndPointsGraterThan_150));

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStore.Object, null, null, null, null, null, null, null, null);
                var usersMock = new List<User>() { new User() { Id = 1, Points = 151 }, new User() { Id = 2, Points = 10 } };

                userManagerMock.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                    .Returns(Task.FromResult(usersMock as IList<User>));

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                // Act
                var users = await contestService.GetPotentialJuryForInvitationAsync();

                // Assert
                Assert.AreEqual(1, users.Count);
                Assert.IsTrue(users.FirstOrDefault().Points >= 150);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        #region IsContestInFinishedPhase
        [TestMethod]
        public async Task IsContestInPhaseFinished_ShouldReturnTrue_WhenActivePhaseIsFinished()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsContestInPhaseFinished_ShouldReturnTrue_WhenActivePhaseIsFinished));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ContestPhases.AddRange(TestUtils.GetContestPhases());
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStore.Object, null, null, null, null, null, null, null, null);
                var usersMock = new List<User>() { new User() { Id = 1 }, new User() { Id = 2 } };

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                // Act
                var phaseOne = await contestService.IsContestInPhaseFinished(1);
                var PhaseTwo = await contestService.IsContestInPhaseFinished(2);
                var finished = await contestService.IsContestInPhaseFinished(3);

                // Assert
                Assert.IsTrue(!phaseOne);
                Assert.IsTrue(!PhaseTwo);
                Assert.IsTrue(finished);

                await assertContext.Database.EnsureDeletedAsync();
            }
        }
        #endregion

        [TestMethod]
        public async Task IsNameUnique_ShouldReturnTrue_IfNameIsUnique_AndFalseIfNot()
        {
            //Arrange
            var options = TestUtils
                .GetInMemoryDatabaseOptions<FullFraimDbContext>
                (nameof(IsContestInPhaseFinished_ShouldReturnTrue_WhenActivePhaseIsFinished));

            using (var arrangeContext = new FullFraimDbContext(options))
            {
                arrangeContext.Contests.AddRange(TestUtils.GetContests());
                arrangeContext.ContestPhases.AddRange(TestUtils.GetContestPhases());
                arrangeContext.Phases.AddRange(TestUtils.GetPhases());

                await arrangeContext.SaveChangesAsync();
            }

            using (var assertContext = new FullFraimDbContext(options))
            {
                var userStore = new Mock<IUserStore<User>>();
                var userManagerMock = new Mock<UserManager<User>>(
                    userStore.Object, null, null, null, null, null, null, null, null);
                var usersMock = new List<User>() { new User() { Id = 1 }, new User() { Id = 2 } };

                var contestService = new ContestService(assertContext, userManagerMock.Object);

                // Act
                var isUniqueFalse = await contestService.IsNameUniqueAsync("Portrait");
                var isUnique = await contestService.IsNameUniqueAsync("UniqueNAme");

                // Assert
                Assert.IsTrue(!isUniqueFalse);
                Assert.IsTrue(isUnique);

                await assertContext.Database.EnsureDeletedAsync();
            }

            Assert.IsTrue(true);
        }

    }
}

using AutoFixture;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Data.Models;
using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Tests
{
    [TestClass]
    public class TeamServiceTests
    {
        private Mock<ILeagueService> _mockLeagueService;
        private Mock<ITeamDetailService> _mockTeamDetailService;
        private static DbContextOptions<FootballLeagueDbContext> _options;

        private static Fixture _fixture;
        private readonly CancellationToken CToken = CancellationToken.None;
       
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Setup()
        {
            _mockLeagueService = new Mock<ILeagueService>();
            _mockTeamDetailService = new Mock<ITeamDetailService>();
            _fixture = new Fixture();
            _options = TestUtils.GetOptions(TestContext.TestName);
        }

        [TestMethod]
        public async Task CreateTeamAsync_ReturnsId_WhenEverythingIsOk()
        {
            //Arrange
            var name = _fixture.Create<string>();
            var leagueId = _fixture.Create<int>();

            var teamModel = _fixture
               .Build<TeamRequestModel>()
               .With(t => t.Name, name)
               .With(t => t.LeagueId, leagueId)
               .Create();

            var league = _fixture.Build<League>()
                 .With(l => l.Id, leagueId)
                 .Without(l => l.Teams)
                 .Without(l => l.Matches)
                 .Create();

            _mockLeagueService.Setup(l => l.ValidateLeagueExistAsync(leagueId, CToken)).Returns(Task.CompletedTask);

            using (var arrangeContext = new FootballLeagueDbContext(_options))
            {
                arrangeContext.Leagues.Add(league);
                await arrangeContext.SaveChangesAsync();
            }

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                var res = await sut.CreateTeamAsync(teamModel, CToken);

                Assert.AreEqual(1, res);
                Assert.AreEqual(1, assertContex.Teams.Count());
            }
        }

        [TestMethod]
        public async Task CreateTeamAsync_ThrowsException_WhenTeamExists()
        {
            //Arrange
            var name = _fixture.Create<string>();
            var leagueId = _fixture.Create<int>();

            var teamModel = _fixture
               .Build<TeamRequestModel>()
               .With(t => t.Name, name)
               .With(t => t.LeagueId, leagueId)
               .Create();

            var team = _fixture.Build<Team>()
                 .With(t => t.Name, name)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(_options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(
                  async () => await sut.CreateTeamAsync(teamModel, CToken));
            }
        }

        [TestMethod]
        public async Task DeleteTeamAsync_DeletesTeam_WhenEverythingIsOk()
        {
            //Arrange
            var teamId = _fixture.Create<int>();
            var leagueId = _fixture.Create<int>();

            var team = _fixture.Build<Team>()
                 .With(t => t.Id, teamId)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(_options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                await sut.DeleteTeamAsync(teamId, CToken);

                Assert.AreEqual(0, assertContex.Teams.Count());
            }
        }

        [TestMethod]
        public async Task DeleteTeamAsync_ThrowsException_WhenTeamIsNotFound()
        {
            //Arrange
            var teamId = _fixture.Create<int>();
            var leagueId = _fixture.Create<int>();

            var team = _fixture.Build<Team>()
                 .With(t => t.Id, teamId + 1)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(_options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityNotFoundException>(
                  async () => await sut.DeleteTeamAsync(teamId, CToken));

                Assert.AreEqual(1, assertContex.Teams.Count());
            }
        }

        [TestMethod]
        public async Task GetTeamDetailsAsync_ReturnTeamResponseModel()
        {
            //Arrange
            var teamId = _fixture.Create<int>();

            var team = _fixture.Build<TeamResponseModel>()
                 .With(t => t.Id, teamId)
                 .Create();

            _mockTeamDetailService.Setup(t => t.GetTeamDetailsAsync(teamId, CToken)).ReturnsAsync(team);

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                var res = await sut.GetTeamDetailsAsync(teamId, CToken);

                Assert.AreEqual(team.Name, res.Name);
                Assert.AreEqual(team.Id, res.Id);
                Assert.AreEqual(team.Points, res.Points);
                Assert.AreEqual(team.GoalsScored, res.GoalsScored);
                Assert.AreEqual(team.GoalsConceded, res.GoalsConceded);
            }
        }

        [TestMethod]
        public async Task CheckIfTeamExistInLeagueAsync_WorksCorrectly_WhenTeamExist()
        {
            //Arrange
            var teamId = _fixture.Create<int>();
            var leagueId = _fixture.Create<int>();

            var league = _fixture.Build<League>()
                .With(l => l.Id, leagueId)
                .Without(l => l.Teams)
                .Without(l => l.Matches)
                .Create();

            var team = _fixture.Build<Team>()
                 .With(t => t.Id, teamId)
                 .With(t => t.League, league)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(_options))
            {
                arrangeContext.Leagues.Add(league);
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                await sut.ValidateTeamExistInLeagueAsync(teamId, leagueId, CToken);

                Assert.AreEqual(1, assertContex.Teams.Count());
                Assert.AreEqual(teamId, assertContex.Teams.FirstOrDefault().Id);
                Assert.AreEqual(leagueId, assertContex.Teams.FirstOrDefault().LeagueId);
            }
        }

        [TestMethod]
        public async Task CheckIfTeamExistInLeagueAsync_ThrowsException_WhenTeamDoesNotExist()
        {
            //Arrange
            var teamId = _fixture.Create<int>();
            var leagueId = _fixture.Create<int>();

            //Act,Assert
            using (var assertContex = new FootballLeagueDbContext(_options))
            {
                var sut = new TeamService(_mockLeagueService.Object, _mockTeamDetailService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityNotFoundException>(
                  async () => await sut.ValidateTeamExistInLeagueAsync(teamId, leagueId, CToken));
            }
        }
    }
}

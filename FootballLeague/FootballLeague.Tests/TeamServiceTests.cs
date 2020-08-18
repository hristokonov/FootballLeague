﻿using AutoFixture;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Data.Models;
using FootballLeague.Models.Request;
using FootballLeague.Services;
using FootballLeague.Services.Interfaces;
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
        private static readonly CancellationToken CToken = CancellationToken.None;
        private static readonly Fixture Fixture = new Fixture();

        [TestMethod]
        public async Task CreateTeamAsync_ReturnsId_WhenEverythingIsOk()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(CreateTeamAsync_ReturnsId_WhenEverythingIsOk));

            var name = Fixture.Create<string>();
            var leagueId = Fixture.Create<int>();

            var teamModel = Fixture
               .Build<TeamRequestModel>()
               .With(t => t.Name, name)
               .With(t => t.LeagueId, leagueId)
               .Create();

            var league = Fixture.Build<League>()
                 .With(l => l.Id, leagueId)
                 .Without(l => l.Teams)
                 .Without(l => l.Matches)
                 .Create();

            leagueService.Setup(l => l.CheckIfLeagueExistByIdAsync(leagueId, CToken)).Returns(Task.CompletedTask);

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Leagues.Add(league);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var res = await sut.CreateTeamAsync(teamModel, CToken);

                Assert.AreEqual(res, 1);
                Assert.AreEqual(assertContex.Teams.Count(), 1);
            }
        }

        [TestMethod]
        public async Task CreateTeamAsync_ThrowsException_WhenTeamExists()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(CreateTeamAsync_ThrowsException_WhenTeamExists));

            var name = Fixture.Create<string>();
            var leagueId = Fixture.Create<int>();

            var teamModel = Fixture
               .Build<TeamRequestModel>()
               .With(t => t.Name, name)
               .With(t => t.LeagueId, leagueId)
               .Create();

            var team = Fixture.Build<Team>()
                 .With(t => t.Name, name)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityAlreadyExistsException>(
                  async () => await sut.CreateTeamAsync(teamModel, CToken));
            }
        }

        [TestMethod]
        public async Task DeleteTeamAsync_DeletesTeam_WhenEverythingIsOk()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(DeleteTeamAsync_DeletesTeam_WhenEverythingIsOk));

            var teamId = Fixture.Create<int>();
            var leagueId = Fixture.Create<int>();

            var team = Fixture.Build<Team>()
                 .With(t => t.Id, teamId)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                await sut.DeleteTeamAsync(teamId, CToken);

                Assert.AreEqual(assertContex.Teams.Count(), 0);
            }
        }

        [TestMethod]
        public async Task DeleteTeamAsync_ThrowsException_WhenTeamIsNotFound()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(DeleteTeamAsync_ThrowsException_WhenTeamIsNotFound));

            var teamId = Fixture.Create<int>();
            var leagueId = Fixture.Create<int>();

            var team = Fixture.Build<Team>()
                 .With(t => t.Id, teamId + 1)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Without(t => t.League)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityNotFoundException>(
                  async () => await sut.DeleteTeamAsync(teamId, CToken));

                Assert.AreEqual(assertContex.Teams.Count(), 1);
            }
        }

        [TestMethod]
        public async Task GetTeamDetailsAsync_ReturnTeamResponseModel()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(GetTeamDetailsAsync_ReturnTeamResponseModel));

            var teamId = Fixture.Create<int>();
            var leagueId = Fixture.Create<int>();

            var league = Fixture.Build<League>()
                .With(l => l.Id, leagueId)
                .Without(l => l.Teams)
                .Without(l => l.Matches)
                .Create();

            var team = Fixture.Build<Team>()
                 .With(t => t.Id, teamId)
                 .With(t => t.League, league)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Leagues.Add(league);
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var res = await sut.GetTeamDetailsAsync(teamId, CToken);

                Assert.AreEqual(res.Name, team.Name);
                Assert.AreEqual(res.Id, team.Id);
                Assert.AreEqual(res.LeagueName, league.Name);
                Assert.AreEqual(res.Points, team.Points);
                Assert.AreEqual(res.GoalsScored, team.GoalsScored);
                Assert.AreEqual(res.GoalsConceded, team.GoalsConceded);
                Assert.AreEqual(res.GoalDifference, team.GoalsScored - team.GoalsConceded);
                Assert.AreEqual(res.MatchesPlayed, 0);
            }
        }

        [TestMethod]
        public async Task GetTeamDetailsAsync_ThrowsException_WhenTeamDoesNotExist()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(GetTeamDetailsAsync_ThrowsException_WhenTeamDoesNotExist));

            var teamId = Fixture.Create<int>();

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityNotFoundException>(
                  async () => await sut.GetTeamDetailsAsync(teamId, CToken));
            }
        }

        [TestMethod]
        public async Task CheckIfTeamExistInLeagueAsync_WorksCorrectly_WhenTeamExist()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(CheckIfTeamExistInLeagueAsync_WorksCorrectly_WhenTeamExist));

            var teamId = Fixture.Create<int>();
            var leagueId = Fixture.Create<int>();

            var league = Fixture.Build<League>()
                .With(l => l.Id, leagueId)
                .Without(l => l.Teams)
                .Without(l => l.Matches)
                .Create();

            var team = Fixture.Build<Team>()
                 .With(t => t.Id, teamId)
                 .With(t => t.League, league)
                 .Without(t => t.HomeMatches)
                 .Without(t => t.AwayMatches)
                 .Create();

            using (var arrangeContext = new FootballLeagueDbContext(options))
            {
                arrangeContext.Leagues.Add(league);
                arrangeContext.Teams.Add(team);
                await arrangeContext.SaveChangesAsync();
            }

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                await sut.CheckIfTeamExistInLeagueAsync(teamId, leagueId, CToken);

                Assert.AreEqual(assertContex.Teams.Count(), 1);
                Assert.AreEqual(assertContex.Teams.FirstOrDefault().Id, teamId);
                Assert.AreEqual(assertContex.Teams.FirstOrDefault().LeagueId, leagueId);
            }
        }

        [TestMethod]
        public async Task CheckIfTeamExistInLeagueAsync_ThrowsException_WhenTeamDoesNotExist()
        {
            //arrange
            var leagueService = new Mock<ILeagueService>();
            var options = TestUtils.GetOptions(nameof(CheckIfTeamExistInLeagueAsync_ThrowsException_WhenTeamDoesNotExist));

            var teamId = Fixture.Create<int>();
            var leagueId = Fixture.Create<int>();

            //act,assert
            using (var assertContex = new FootballLeagueDbContext(options))
            {
                var sut = new TeamService(leagueService.Object, assertContex);

                var ex = await Assert.ThrowsExceptionAsync<EntityNotFoundException>(
                  async () => await sut.CheckIfTeamExistInLeagueAsync(teamId, leagueId, CToken));
            }
        }
    }
}

using AutoFixture;
using FootballLeague.Controllers;
using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Tests
{
    [TestClass]
    public class LeagueControllerTests
    {
        private Mock<ILeagueService> _mockLeagueService;
        private Mock<ILogger<LeagueController>> _mockLogger;
        private LeagueController _leagueController;
        private static Fixture _fixture;
        private readonly CancellationToken CToken = CancellationToken.None;

        [TestInitialize]
        public void Setup()
        {
            _mockLeagueService = new Mock<ILeagueService>();
            _mockLogger = new Mock<ILogger<LeagueController>>();
            _fixture = new Fixture();
            _leagueController = new LeagueController(_mockLeagueService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task CreateLeague_ReturnStatusCodeCreated()
        {
            //Arrange
            var leagueId = _fixture.Create<int>();
            var leagueModel = _fixture.Create<LeagueRequestModel>();

            _mockLeagueService.Setup(l => l.CreateLeagueAsync(leagueModel, CToken))
                .ReturnsAsync(leagueId)
                .Verifiable();

            //Act
            var result = await _leagueController.CreateLeague(leagueModel, CToken);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.AreEqual(StatusCodes.Status201Created, objectResult.StatusCode);
            _mockLeagueService.Verify();
        }

        [TestMethod]
        public async Task GetLeagueTable_ReturnStatusCodeOk()
        {
            //Arrange
            var leagueId = _fixture.Create<int>();
            var leagueModel = _fixture.Create<LeagueResponseModel>();

            _mockLeagueService.Setup(l => l.GetLeagueTableAsync(leagueId, CToken))
                .ReturnsAsync(leagueModel)
                .Verifiable();

            //Act
            var result = await _leagueController.GetLeagueTable(leagueId, CToken);

            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<LeagueResponseModel>));
            _mockLeagueService.Verify();
        }
    }
}

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
        private static readonly CancellationToken CToken = CancellationToken.None;
        private static readonly Fixture _fixture = new Fixture();

        [TestMethod]
        public async Task CreateLeagueAsync_ReturnStatusCodeCreated()
        {
            //Arrange
            var _mockLeagueService = new Mock<ILeagueService>();
            var logger = new Mock<ILogger<LeagueController>>();
         
            var controller = new LeagueController(_mockLeagueService.Object, logger.Object);

            var leagueId = _fixture.Create<int>();
            var leagueModel = _fixture.Create<LeagueRequestModel>();

            _mockLeagueService.Setup(l => l.CreateLeagueAsync(leagueModel, CToken))
                .ReturnsAsync(leagueId)
                .Verifiable();

            //Act
            var result = await controller.CreateLeagueAsync(leagueModel, CToken);
            var objectResult = result as ObjectResult;

            //Assert
            Assert.AreEqual(StatusCodes.Status201Created, objectResult.StatusCode);
            _mockLeagueService.Verify();
        }

        [TestMethod]
        public async Task GetLeagueTableAsync_ReturnStatusCodeOk()
        {
            //Arrange
            var _mockLeagueService = new Mock<ILeagueService>();
            var logger = new Mock<ILogger<LeagueController>>();

            var controller = new LeagueController(_mockLeagueService.Object, logger.Object);

            var leagueId = _fixture.Create<int>();
            var leagueModel = _fixture.Create<LeagueResponseModel>();

            _mockLeagueService.Setup(l => l.GetLeagueTableAsync(leagueId, CToken))
                .ReturnsAsync(leagueModel)
                .Verifiable();

            //Act
            var result = await controller.GetLeagueTableAsync(leagueId, CToken);
          
            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<LeagueResponseModel>));
            _mockLeagueService.Verify();
        }
    }
}

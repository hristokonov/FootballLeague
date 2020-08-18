using FootballLeague.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FootballLeague.Tests
{
    public class TestUtils
    {
        public static DbContextOptions<FootballLeagueDbContext> GetOptions(string databaseName)
        {
            var provider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

            return new DbContextOptionsBuilder<FootballLeagueDbContext>()
            .UseInMemoryDatabase(databaseName)
            .UseInternalServiceProvider(provider)
            .Options;
        }
    }
}

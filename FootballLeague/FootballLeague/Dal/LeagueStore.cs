using FootballLeague.CustomExceptions;
using FootballLeague.Dal.Interfaces;
using FootballLeague.Data;
using FootballLeague.Models.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Dal
{
    public class LeagueStore : ILeagueStore
    {
        private readonly FootballLeagueDbContext _context;

        public LeagueStore(FootballLeagueDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<int> CreateLeagueAsync(LeagueRequestModel leagueModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var leagueExists = await _context.Leagues
                                         .Where(l => l.Name == leagueModel.Name)
                                         .AnyAsync(cancellationToken);

            if (leagueExists)
            {
                throw new EntityAlreadyExistsException("League with this name already exist");
            }

            var league = new League { Name = leagueModel.Name };

            _context.Leagues.Add(league);

            await _context.SaveChangesAsync(cancellationToken);

            return league.Id;
        }
    }
}

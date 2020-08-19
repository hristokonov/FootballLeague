using FootballLeague.Constants;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Data.Models;
using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class LeagueService : ILeagueService
    {
        private readonly FootballLeagueDbContext _context;

        public LeagueService(FootballLeagueDbContext context)
        {
            _context = context;
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
                throw new EntityAlreadyExistsException(string.Format(ErrorMessages.LeagueExists, leagueModel.Name));
            }

            var league = new League { Name = leagueModel.Name };

            _context.Leagues.Add(league);
            await _context.SaveChangesAsync(cancellationToken);

            return league.Id;
        }

        /// <inheritdoc />
        public async Task<LeagueResponseModel> GetLeagueTableAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var leagueModel = await _context.Leagues
                                      .Include(l => l.Teams)
                                      .ThenInclude(m => m.HomeMatches)
                                      .Include(l => l.Teams)
                                      .ThenInclude(m => m.AwayMatches)
                                      .Where(l => l.Id == id)
                                      .Select(l => new LeagueResponseModel
                                      {
                                          Name = l.Name,
                                          Teams = l.Teams.Select(t => new TeamResponseModel()
                                          {
                                              Id = t.Id,
                                              Name = t.Name,
                                              MatchesPlayed = t.HomeMatches.Count + t.AwayMatches.Count,
                                              GoalsScored = t.GoalsScored,
                                              GoalsConceded = t.GoalsConceded,
                                              GoalDifference = t.GoalsScored - t.GoalsConceded,
                                              Points = t.Points,
                                              LeagueName = l.Name
                                          }).OrderByDescending(t => t.Points)
                                           .ThenByDescending(t => t.GoalDifference)
                                           .ToList()
                                      }).FirstOrDefaultAsync(cancellationToken);

            if (leagueModel == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.LeagueNotFound, id));
            }

            AssignTablePosition(leagueModel.Teams);

            return leagueModel;
        }

        /// <inheritdoc />
        public async Task ValidateLeagueExistAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var leagueExists = await _context.Leagues.Where(l => l.Id == id)
                                                     .AnyAsync(cancellationToken);

            if (!leagueExists)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.LeagueNotFound, id));
            }
        }

        private void AssignTablePosition(ICollection<TeamResponseModel> teams)
        {
            var i = 0;
            foreach (var team in teams)
            {
                i += 1;
                team.Position = i;
            }
        }
    }
}

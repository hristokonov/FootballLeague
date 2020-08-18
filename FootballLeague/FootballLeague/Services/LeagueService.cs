using FootballLeague.Constants;
using FootballLeague.CustomExceptions;
using FootballLeague.Data;
using FootballLeague.Models.Request;
using FootballLeague.Models.Response;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
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
                throw new EntityAlreadyExistsException($"League with this name {leagueModel.Name} already exist");
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

            var league = await _context.Leagues
                                      .Include(l => l.Teams)
                                      .ThenInclude(m => m.HomeMatches)
                                      .Include(l => l.Teams)
                                      .ThenInclude(m => m.AwayMatches)
                                      .FirstOrDefaultAsync(l => l.Id == id);
            if (league == null)
            {
                throw new EntityNotFoundException($"League with this id {id} doesn't exist");
            }

            var leagueModel = new LeagueResponseModel
            {
                Name = league.Name,
                Teams = league.Teams.Select(t => new TeamResponseModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    MatchesPlayed = t.HomeMatches.Count + t.AwayMatches.Count,
                    GoalsScored = t.GoalsScored,
                    GoalsConceded = t.GoalsConceded,
                    GoalDifference = t.GoalsScored - t.GoalsConceded,
                    Points = t.Points,
                    LeagueName = league.Name
                }).OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ToList()
            };

            AssignTablePosition(leagueModel.Teams);

            return leagueModel;
        }

        /// <inheritdoc />
        public async Task CheckIfLeagueExistById(int leagueId, CancellationToken cancellationToken)
        {
            var leagueExists = await _context.Leagues.Where(l => l.Id == leagueId)
                                                   .AnyAsync(cancellationToken);

            if (!leagueExists)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.LeagueNotFound, leagueId));
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

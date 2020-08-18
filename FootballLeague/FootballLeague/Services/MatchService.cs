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
    public class MatchService : IMatchService
    {
        private readonly ITeamService _teamService;
        private readonly ILeagueService _leagueService;
        private readonly FootballLeagueDbContext _context;

        public MatchService(ITeamService teamService, ILeagueService leagueService, FootballLeagueDbContext context)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _leagueService = leagueService ?? throw new ArgumentNullException(nameof(leagueService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<int> CreateMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            await _leagueService.CheckIfLeagueExistById(matchModel.LeagueId, cancellationToken);
            await _teamService.CheckIfTeamExistInLeague(matchModel.HomeTeamId, matchModel.LeagueId, cancellationToken);
            await _teamService.CheckIfTeamExistInLeague(matchModel.AwayTeamId, matchModel.LeagueId, cancellationToken);

            var match = new Match()
            {
                HomeTeamId = matchModel.HomeTeamId,
                AwayTeamId = matchModel.AwayTeamId,
                IsMatchPlayed = false,
                LeagueId = matchModel.LeagueId
            };

            _context.Matches.Add(match);

            await _context.SaveChangesAsync(cancellationToken);

            return match.Id;
        }

        /// <inheritdoc />
        public async Task DeleteMatchAsync(int id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var match = await GetMatchAsync(id, cancellationToken);

            if (match.IsMatchPlayed)
            {
                match.HomeTeam.GoalsScored -= match.HomeTeamGoals;
                match.HomeTeam.GoalsConceded -= match.AwayTeamGoals;
                match.AwayTeam.GoalsScored -= match.AwayTeamGoals;
                match.AwayTeam.GoalsConceded -= match.HomeTeamGoals;

                if (match.HomeTeamGoals > match.AwayTeamGoals)
                {
                    match.HomeTeam.Points -= 3;
                }
                else if (match.HomeTeamGoals < match.AwayTeamGoals)
                {
                    match.AwayTeam.Points -= 3;
                }
                else
                {
                    match.HomeTeam.Points -= 1;
                    match.AwayTeam.Points -= 1;
                }
            }

            _context.Matches.Remove(match);

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MatchResponseModel>> GetAllMatchesAsync(CancellationToken cancellationToken)
        {
            return await _context.Matches
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Select(m => new MatchResponseModel()
                {
                    Id = m.Id,
                    LeagueName = m.League.Name,
                    HomeTeamName = m.HomeTeam.Name,
                    AwayTeamName = m.AwayTeam.Name,
                    HomeTeamGoals = m.HomeTeamGoals,
                    AwayTeamGoals = m.AwayTeamGoals
                })
                .OrderBy(m => m.LeagueName)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MatchResponseModel>> GetAllTeamMatchesAsync(int teamId, CancellationToken cancellationToken)
        {
            return await _context.Matches
                .Include(m => m.League)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
                .Select(m => new MatchResponseModel()
                {
                    Id = m.Id,
                    LeagueName = m.League.Name,
                    HomeTeamName = m.HomeTeam.Name,
                    AwayTeamName = m.AwayTeam.Name,
                    HomeTeamGoals = m.HomeTeamGoals,
                    AwayTeamGoals = m.AwayTeamGoals
                })
                .OrderBy(m => m.LeagueName)
                .ToListAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task PlayMatchAsync(MatchRequestModel matchModel, CancellationToken cancellationToken)
        {
            var match = await GetMatchAsync(matchModel.Id, cancellationToken);

            if (match.IsMatchPlayed)
            {
                throw new MatchAlreadyPlayedException(string.Format(ErrorMessages.MatchIsPlayed, match.Id, match.HomeTeam.Name, match.AwayTeam.Name));
            }

            match.HomeTeamGoals = matchModel.HomeTeamGoals;
            match.AwayTeamGoals = matchModel.AwayTeamGoals;
            match.IsMatchPlayed = true;
            match.HomeTeam.GoalsScored += matchModel.HomeTeamGoals;
            match.HomeTeam.GoalsConceded += matchModel.AwayTeamGoals;
            match.AwayTeam.GoalsScored += matchModel.AwayTeamGoals;
            match.AwayTeam.GoalsConceded += matchModel.HomeTeamGoals;

            if (matchModel.HomeTeamGoals > matchModel.AwayTeamGoals)
            {
                match.HomeTeam.Points += 3;
            }
            else if (matchModel.HomeTeamGoals < matchModel.AwayTeamGoals)
            {
                match.AwayTeam.Points += 3;
            }
            else
            {
                match.HomeTeam.Points += 1;
                match.AwayTeam.Points += 1;
            }

            _context.Matches.Update(match);

            await _context.SaveChangesAsync(cancellationToken);
        }

        private Task<Match> GetMatchAsync(int id, CancellationToken cancellationToken)
        {
            var match = _context.Matches
                         .Include(m => m.HomeTeam)
                         .Include(m => m.AwayTeam)
                         .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

            if (match == null)
            {
                throw new EntityNotFoundException(string.Format(ErrorMessages.MatchNotFound, id));
            }

            return match;
        }
    }
}

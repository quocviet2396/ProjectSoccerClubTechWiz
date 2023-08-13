using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class MatchServiceImp : IMatchService
    {
        private DatabaseContext db;
        public MatchServiceImp(DatabaseContext db) {  this.db = db; }
        public async Task<bool> addMatch(Match newMatch)
        {
            await db.Match.AddAsync(newMatch);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Match> GetMatchById(int id)
        {
            return await db.Match.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Match>> GetMatchList()
        {
            return await db.Match.ToListAsync();
        }

        public async Task<bool> removeMatch(int id)
        {
            var match = await db.Match.FirstOrDefaultAsync(m => m.Id == id);
            if (match != null)
            {
                db.Match.Remove(match);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> updateMatch(Match editMatch)
        {
            var match = await db.Match.FirstOrDefaultAsync(m => m.Id == editMatch.Id);
            if(match != null)
            {
                match.HomeTeamId = editMatch.HomeTeamId;
                match.AwayTeamId = editMatch.AwayTeamId;
                match.MatchTime = editMatch.MatchTime;
                match.Stadium = editMatch.Stadium;
                match.Result = editMatch.Result;
                match.CompetitionId = editMatch.CompetitionId;
                await db.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }
    }
}

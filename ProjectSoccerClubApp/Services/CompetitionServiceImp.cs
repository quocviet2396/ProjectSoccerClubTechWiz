using System;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
	public class CompetitionServiceImp : ICompetitionService
    {
        private DatabaseContext db;

        public CompetitionServiceImp(DatabaseContext db)
        {
            this.db = db;
        }


        public async Task<bool> addCompetition(Competition newCompetition)
        {
            await db.Competition.AddAsync(newCompetition);
            await db.SaveChangesAsync();
            return true;
        }

        public Task<bool> editCompetition(Competition newCompetition)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Competition>> GetCompetition()
        {
            return await db.Competition.ToListAsync();
        }

        public async Task<Competition> GetCompetitionById(int id)
        {
            return await db.Competition.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> deleteCompetition(Competition newCompetition)
        {
            db.Competition.Remove(newCompetition);
            await db.SaveChangesAsync();
            return true;
        }
    }
}


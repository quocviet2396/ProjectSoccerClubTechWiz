using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;


namespace ProjectSoccerClubApp.Services
{
    public class TeamServicesImp : ITeamServices
    {
        private DatabaseContext db;
        public TeamServicesImp(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Team>> GetAllTeams()
        {
            return await db.Team.OrderBy(t => t.TeamName).ToListAsync();
        }

        public async Task<Team> GetOneTeam(int id)
        {
            return await db.Team.FindAsync(id);
        }

        public async Task<bool> PostTeam(Team newTeam)
        {
            var team = await db.Team.SingleOrDefaultAsync(t => t.Id == newTeam.Id);
            if (team == null)
            {
                db.Team.Add(newTeam);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteTeam(int id)
        {
            var team = await db.Team.SingleOrDefaultAsync(t => t.Id.Equals(id));
            if (team == null)
            {
                return false;
            }
            db.Team.Remove(team);
            await db.SaveChangesAsync();

            if (!string.IsNullOrEmpty(team.Logo))
            {
                string path = Path.Combine("wwwroot/images/Logo", team.Logo);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            return true;
        }

        public async Task<bool> PutTeam(Team editTeam)
        {
            var team = await db.Team.SingleOrDefaultAsync(u => u.Id!.Equals(editTeam.Id));
            if (team != null)
            {
                team.CoachName = editTeam.CoachName;
                team.Website = editTeam.Website;
                team.Logo = editTeam.Logo;
      
                await db.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> IsTeamNameUnique(string inputName)
        {
            return await db.Team.AllAsync(t => t.TeamName != inputName);
        }
        public async Task<bool> IsCoachNameUnique(string CoachName)
        {
            return await db.Team.AllAsync(t => t.CoachName != CoachName);
        }
    }
}

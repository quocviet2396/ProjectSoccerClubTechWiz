using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface ITeamServices
    {
        Task<IEnumerable<Team>> GetAllTeams();
        Task<Team> GetOneTeam(int id);
        Task<bool> PostTeam(Team newTeam); // add new Team
        Task<bool> PutTeam(Team editTeam);
        Task<bool> DeleteTeam(int id);
        Task<bool> IsTeamNameUnique(string TeamName);
        Task<bool> IsCoachNameUnique(string CoachName);
    }
}


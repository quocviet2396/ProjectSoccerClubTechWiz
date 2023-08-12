using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetMatchList();
        Task<Match> GetMatchById(int id);
        Task<bool> addMatch(Match newMatch);
        Task<bool> removeMatch(int id);
        Task<bool> updateMatch(Match editMatch);

    }
}

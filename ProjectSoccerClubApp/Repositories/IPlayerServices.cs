using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface IPlayerServices
    {
        //Task<IEnumerable<Player>> GetPlayers();
		Task<IEnumerable<Player>> GetPlayers(int teamId);
		Task<Player> GetPlayer(int id);
		Task<bool> AddPlayer(Player newPlayer); 
		Task<bool> EditPlayer(Player editPlayer);
		Task<bool> DeletePlayer(int id);
        Task<IEnumerable<Player>> GetTop10Players();
    }
}

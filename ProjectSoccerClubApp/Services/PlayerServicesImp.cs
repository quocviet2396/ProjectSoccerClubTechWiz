using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class PlayerServicesImp : IPlayerServices
    {
        private DatabaseContext db;
        public PlayerServicesImp(DatabaseContext db)
        {
            this.db = db;
        }
		//public async Task<IEnumerable<Player>> GetPlayers()
  //      { 
		//	return  await db.Player.ToListAsync();
		//}

		public async Task<IEnumerable<Player>> GetPlayers(int teamId)
		{
			if (teamId == 0)
			{
				// Lấy tất cả cầu thủ nếu không có teamId được chỉ định
				var players = await db.Player.ToListAsync();
				return players;
			}
			else
			{
				// Lấy danh sách cầu thủ theo teamId
				var playersInTeam = await db.Player.Where(player => player.TeamId == teamId).ToListAsync();
				return playersInTeam;
			}
		}


		public async Task<Player> GetPlayer(int id)
        {
            var player = db.Player.SingleOrDefaultAsync(d => d.Id == id);
            if(player != null)
            {
                return await player;
            }
            else
            {
                return null;
            }
        }

		public async Task<bool> AddPlayer(Player newPlayer)
		{
			var player = await db.Player.SingleOrDefaultAsync(t => t.Id == newPlayer.Id);
			if (player == null)
			{
				db.Player.Add(newPlayer);
				db.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> EditPlayer(Player editPlayer)
		{
			var model = await db.Player.SingleOrDefaultAsync(u => u.Id!.Equals(editPlayer.Id));
			if (model != null)
			{
                model.PlayerName = editPlayer.PlayerName;
				model.PlayerPosition = editPlayer.PlayerPosition;
				model.PlayerDOB = editPlayer.PlayerDOB;
				model.PlayerNationality = editPlayer.PlayerNationality;
				model.PlayerImg = editPlayer.PlayerImg;
				model.PlayerOVR = editPlayer.PlayerOVR;
				model.TeamId = editPlayer.TeamId;
				db.SaveChanges();
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> DeletePlayer(int id)
		{
			var model = await db.Player.SingleOrDefaultAsync(t => t.Id.Equals(id));
			if (model != null)
			{
				db.Player.Remove(model);
				await db.SaveChangesAsync();
				return true;
			}
			else
			{
				return false;
			}
		}

        public async Task<IEnumerable<Player>> GetTop10Players()
        {
            var model = await db.Player
                       .OrderByDescending(p => p.PlayerOVR) // Sắp xếp theo thứ hạng giảm dần
                       .Take(10) // Lấy 10 cầu thủ đầu tiên
                       .ToListAsync(); // Chuyển kết quả thành danh sách
            return model;
        }
    }
}

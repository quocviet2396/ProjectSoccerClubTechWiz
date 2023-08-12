using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class OrderServiceImp : IOrderService
    {
        private DatabaseContext db;
        public OrderServiceImp(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<bool> DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await db.Orders.ToListAsync();
        }

        public async Task<Order> GetOneOrder(int id)
        {
            return await db.Orders.FirstOrDefaultAsync(o => o.Id == id);
        }

        public Task<bool> PostOrder(Order newOrder)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutOrder(Order editOrder)
        {
            throw new NotImplementedException();
        }
    }
}

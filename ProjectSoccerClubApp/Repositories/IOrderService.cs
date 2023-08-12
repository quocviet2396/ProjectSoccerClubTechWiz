using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOneOrder(int id);
        Task<bool> PostOrder(Order newOrder); // add new Order
        Task<bool> PutOrder(Order editOrder);
        Task<bool> DeleteOrder(int id);
    }
}

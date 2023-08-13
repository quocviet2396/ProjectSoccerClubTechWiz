using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetNewsList();
        Task<News> GetNewsById(int id);

        Task<bool> addNews(News newNews);
        Task<bool> removeNews(int id);
        Task<bool> updateNews(News newNews);
    }
}

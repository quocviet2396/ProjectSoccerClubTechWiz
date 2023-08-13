using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class NewsServiceImp : INewsService
    {
        private DatabaseContext db;
        public NewsServiceImp(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<bool> addNews(News newNews)
        {
            await db.News.AddAsync(newNews);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<News> GetNewsById(int id)
        {
            return await db.News.FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task<IEnumerable<News>> GetNewsList()
        {
            return await db.News.ToListAsync();
        }

        public async Task<bool> removeNews(int id)
        {
            var news = await db.News.FirstOrDefaultAsync(n => n.ID == id);
            if (news != null)
            {
                db.News.Remove(news);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> updateNews(News newNews)
        {
            var news = await db.News.FirstOrDefaultAsync(n => n.ID == newNews.ID);
            if(news != null)
            {
                news.Title = newNews.Title;
                news.Content = newNews.Content;
                news.Author = newNews.Author;
                news.PublishDate = newNews.PublishDate;
                news.Logo = newNews.Logo;

                await db.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }
    }
}

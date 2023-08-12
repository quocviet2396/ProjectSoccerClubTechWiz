using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class AuthenServiceImp : IAuthenService
    {
        private readonly DatabaseContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenServiceImp(DatabaseContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool IsUserLoggedIn()
        {
            string accNo = _httpContextAccessor.HttpContext.Session.GetString("accNo");
            return !string.IsNullOrEmpty(accNo);
        }

        public bool IsUserAdmin()
        {
            string accNo = _httpContextAccessor.HttpContext.Session.GetString("accNo");
            var user = _context.User.FirstOrDefault(u => u.Username == accNo);
            return user != null && user.Role;
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ProjectSoccerClubApp.Repositories
{
    public interface IAuthenService
    {
        bool IsUserLoggedIn();

        bool IsUserAdmin();
    }
}

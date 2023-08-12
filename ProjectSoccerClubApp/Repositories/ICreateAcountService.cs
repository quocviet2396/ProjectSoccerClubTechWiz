using System;
using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
	public interface ICreateAcountService
	{
        Task<IEnumerable<User>> GetUser();

        Task<User> GetUserById(int id);
        Task<User> GetUserByAccNo(string accNo);
        Task<bool> addUser(User newUser);
        Task<bool> edit(User editUser);
    }
}


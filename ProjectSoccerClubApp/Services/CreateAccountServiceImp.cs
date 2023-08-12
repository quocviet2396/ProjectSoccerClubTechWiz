using System;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
	public class CreateAccountServiceImp : ICreateAcountService
    {
        private DatabaseContext db;
        public CreateAccountServiceImp(DatabaseContext db)
		
            { this.db = db; }

        public async Task<bool> addUser(User newUser)
        {
            bool userExists = await db.User.AnyAsync(u => u.Username == newUser.Username || u.Email == newUser.Email);
            if (userExists)
            {
                return false; // Người dùng đã tồn tại
            }

            await db.User.AddAsync(newUser);
            await db.SaveChangesAsync();
            return true; // Thêm người dùng thành công
        }


        public async Task<bool> edit(User editUser)
        {
            var user = await db.User.FirstOrDefaultAsync(u => u.Id == editUser.Id);
            if (user != null) 
            {
                user.Password = editUser.Password;
                user.Email = editUser.Email;
                user.PhoneNumber = editUser.PhoneNumber;
                user.Address = editUser.Address;
                user.Role = editUser.Role;
                await db.SaveChangesAsync();
                return true;
            }
            else { return false; }
        }

        public async Task<IEnumerable<User>> GetUser()
        {
            return await db.User.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await db.User.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByAccNo(string accNo)
        {
            return await db.User.FirstOrDefaultAsync(u => u.Username.Equals(accNo));
        }
    }
}


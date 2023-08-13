﻿using System;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
	public class CategoryServiceImp : ICategoryService
	{
        private DatabaseContext db;

        public CategoryServiceImp(DatabaseContext db)
		{
            this.db = db;
		}

        public async Task<bool> addCategory(Categories newCategory)
        {
            await db.Category.AddAsync(newCategory);
            await db.SaveChangesAsync();
            return true; 
        }

        public Task<bool> editCategory(Categories editCategory)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Categories>> GetCategory()
        {
            return await db.Category.ToListAsync();
        }

        public async Task<bool> deleteCategory(Categories editCategory)
        {
            db.Category.Remove(editCategory);
            await db.SaveChangesAsync();
            return true;
        }


        public async Task<Categories> GetCategoryById(int id)
        {
            return await db.Category.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}


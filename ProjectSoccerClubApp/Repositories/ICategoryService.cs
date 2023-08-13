using System;
using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
	public interface ICategoryService
	{
        Task<IEnumerable<Categories>> GetCategory();
        Task<Categories> GetCategoryById(int id);
        Task<bool> addCategory(Categories newCategory);
        Task<bool> editCategory(Categories editCategory);
        Task<bool> deleteCategory(Categories editCategory);

    }
}


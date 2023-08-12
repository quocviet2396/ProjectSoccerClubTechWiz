using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectModels;

namespace ProjectSoccerClubApp.Repositories
{
    public interface IProductService
    {
        Task<bool> AddProduct(Products newProducts);
        Task<bool> EditProduct(Products editProducts);
        Task<IEnumerable<Products>> GetProductWithCategory();
        Task<Products> GetProductById(int id);
        Task DeleteProduct(Products product);
    }
}

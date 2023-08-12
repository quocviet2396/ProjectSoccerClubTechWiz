using System;
using Microsoft.EntityFrameworkCore;
using ProjectModels;
using ProjectSoccerClubApp.Database_Helper;
using ProjectSoccerClubApp.Repositories;

namespace ProjectSoccerClubApp.Services
{
    public class ProductServiceImp : IProductService
    {
        private DatabaseContext db;

        public ProductServiceImp(DatabaseContext db)
        {
            this.db = db;
        }

        public async Task<bool> AddProduct(Products newProducts)
        {
            var product = await db.Product.SingleOrDefaultAsync(t => t.Id.Equals(newProducts.Id));
            if (product == null)
            {
                db.Product.Add(newProducts);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> EditProduct(Products editProducts)
        {
            var product = await db.Product.FindAsync(editProducts.Id);
            if (product == null)
            {
                return false;
            }

            product.Name = editProducts.Name;
            product.Slug = editProducts.Slug;
            product.Descrption = editProducts.Descrption;
            product.OriginalPrice = editProducts.OriginalPrice;
            product.SellingPrice = editProducts.SellingPrice;
            product.Trending = editProducts.Trending;
            product.Featured = editProducts.Featured;
            product.Quantity = editProducts.Quantity;
            product.status = editProducts.status;
            product.Photo = editProducts.Photo;
            product.CategoryId = editProducts.CategoryId; // Cập nhật CategoryId

            await db.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<Products>> GetProductWithCategory()
        {
            return await db.Product
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Products> GetProductById(int id)
        {
            return await db.Product.FindAsync(id);
        }

        public async Task DeleteProduct(Products product)
        {
            db.Product.Remove(product);
            await db.SaveChangesAsync();
        }
    }
}

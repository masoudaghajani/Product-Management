

using Domain.Contracts;
using Domain.IRepository.ShoppingStore;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace TeshkGallary.Infrastructure.Repository.ShoppingStore
{
    public class ProductRepository : RepositoryAsync<Product>, IProductRepository, IScopedDependency
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        
        public async Task UpdateAsync(Product Product)
        {
            var objFromDb =await   _db.Products.FindAsync(Product.Id);
            if (objFromDb != null)
            {

                objFromDb.Name = Product.Name;
                objFromDb.Code = Product.Code;
                objFromDb.Price = Product.Price;
                objFromDb.StockQuantity = Product.StockQuantity;
               

            }
        }

       
    }
}

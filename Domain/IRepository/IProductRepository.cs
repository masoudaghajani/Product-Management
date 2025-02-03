using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;

namespace Domain.IRepository.ShoppingStore
{
    public interface IProductRepository : IRepositoryAsync<Product>, IScopedDependency
    {
        Task UpdateAsync(Product  product);

    }
}

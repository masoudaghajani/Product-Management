

using Domain.Contracts;
using Domain.Models;


namespace Domain.IServices
{
    public interface IProductService : IScopedDependency
    {
        Task<IEnumerable<Product>> GetLastAsync(int count);
      

    }
}

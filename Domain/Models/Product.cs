using Domain.Contracts;

namespace Domain.Models

{
    public class Product : BaseEntity<Guid>,IScopedDependency
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal Price { get; set; } 

    }
    public class ProductResponse:IScopedDependency
    {
        public Guid Id { get; set; }
    }
}

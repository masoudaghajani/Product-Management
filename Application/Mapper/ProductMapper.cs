using Application.Products.Command;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mapper
{
    public static class ProductMapper
    {
        public static Product ToDomain(this CreateProductCommand Model)
        {
            return new Product()
            {
                Name = Model.Name,
                Code = Model.Code,
                Price = Model.Price,
                StockQuantity = Model.StockQuantity,
                Id = Model.Id,
            };


        }
        public static Product ToDomain(this UpdateProductCommand Model)
        {
            return new Product()
            {
                Name = Model.Name,
                Code = Model.Code,
                Price = Model.Price,
                StockQuantity = Model.StockQuantity,
            };


        }

    }
}

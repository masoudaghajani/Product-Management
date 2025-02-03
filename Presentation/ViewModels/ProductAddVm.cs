using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class ProductAddVm
    {
       
        public Guid Id { get; set; }
        [Required(ErrorMessage = "نام الزامی است")]
        public string Name { get; set; }

        [Required(ErrorMessage = "کد الزامی است")]
        public string Code { get; set; }


        [Required(ErrorMessage = "تعداد سهام الزامی است")]
        public int StockQuantity { get; set; }


        [Required(ErrorMessage = "قیمت الزامی است")]
        public decimal Price { get; set; }


    }

}

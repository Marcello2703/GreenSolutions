using System.ComponentModel.DataAnnotations;

namespace GreenSolutions.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public decimal BasePrice { get; set; }
    }
}

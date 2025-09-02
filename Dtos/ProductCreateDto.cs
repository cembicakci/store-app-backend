using System.ComponentModel.DataAnnotations;

namespace StoreApi.Dtos
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}

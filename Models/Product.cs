using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public required Category Category { get; set; }
    }
}
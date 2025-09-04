using System.ComponentModel.DataAnnotations;

namespace StoreApp.Dtos
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

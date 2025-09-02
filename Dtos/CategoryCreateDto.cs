using System.ComponentModel.DataAnnotations;

namespace StoreApi.Dtos
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}

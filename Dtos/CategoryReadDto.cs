public class CategoryReadDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ProductReadDto> Products { get; set; } = new();
}

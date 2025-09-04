using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Dtos;
using StoreApp.Data;
using StoreApp.Models;

namespace StoreApp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly StoreContext _context;

		public CategoriesController(StoreContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<CategoryReadDto>>> GetCategories()
		{
			return await _context.Categories
				.Include(c => c.Products)
				.Select(c => new CategoryReadDto
				{
					Id = c.Id,
					Name = c.Name,
					Products = c.Products.Select(p => new ProductReadDto
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.Price,
						CategoryName = p.Category != null ? p.Category.Name : ""
					}).ToList()
				})
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<CategoryReadDto>> GetCategory(int id)
		{
			var category = await _context.Categories
				.Include(c => c.Products)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null) return NotFound();

			return new CategoryReadDto
			{
				Id = category.Id,
				Name = category.Name,
				Products = category.Products.Select(p => new ProductReadDto
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					CategoryName = category.Name
				}).ToList()
			};
		}

		[HttpPost]
		public async Task<ActionResult<CategoryReadDto>> CreateCategory(CategoryCreateDto dto)
		{
			var category = new Category
			{
				Name = dto.Name
			};

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			var result = new CategoryReadDto
			{
				Id = category.Id,
				Name = category.Name,
				Products = new List<ProductReadDto>() 
			};

			return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, result);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<CategoryReadDto>> UpdateCategory(int id, CategoryCreateDto dto)
		{
			var category = await _context.Categories
				.Include(c => c.Products)
				.FirstOrDefaultAsync(c => c.Id == id);

			if (category == null) return NotFound();

			category.Name = dto.Name;
			await _context.SaveChangesAsync();

			return new CategoryReadDto
			{
				Id = category.Id,
				Name = category.Name,
				Products = category.Products.Select(p => new ProductReadDto
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					CategoryName = category.Name
				}).ToList()
			};
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category == null) return NotFound();
			_context.Categories.Remove(category);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}

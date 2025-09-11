using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Data;
using StoreApp.Models;
using StoreApp.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace StoreApp.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly StoreContext _context;

		public ProductsController(StoreContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
		{
			return await _context.Products
				.Include(p => p.Category)
				.Select(p => new ProductReadDto
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					CategoryName = p.Category != null ? p.Category.Name : ""
				})
				.ToListAsync();
		}


		[HttpGet("{id}")]
		public async Task<ActionResult<ProductReadDto>> GetProduct(int id)
		{
			var product = await _context.Products
				.Include(p => p.Category)
				.Where(p => p.Id == id)
				.Select(p => new ProductReadDto
				{
					Id = p.Id,
					Name = p.Name,
					Price = p.Price,
					CategoryName = p.Category != null ? p.Category.Name : ""
				})
				.FirstOrDefaultAsync();

			if (product == null) return NotFound();

			return product;
		}


		[HttpPost]
		public async Task<ActionResult<ProductReadDto>> PostProduct(ProductCreateDto dto)
		{
			var category = await _context.Categories.FindAsync(dto.CategoryId);
			if (category == null)
			{
				return BadRequest("Invalid CategoryId");
			}

			var product = new Product
			{
				Name = dto.Name,
				Price = dto.Price,
				CategoryId = dto.CategoryId,
				Category = category
			};

			_context.Products.Add(product);
			await _context.SaveChangesAsync();

			var result = new ProductReadDto
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				CategoryName = category.Name
			};

			return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, result);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<ProductReadDto>> UpdateProduct(int id, ProductCreateDto dto)
		{
			var product = await _context.Products
				.Include(p => p.Category)
				.FirstOrDefaultAsync(p => p.Id == id);

			if (product == null) return NotFound();

			// Yeni kategori ID gönderildiyse, kategoriyi kontrol et
			if (dto.CategoryId != product.CategoryId)
			{
				var newCategory = await _context.Categories.FindAsync(dto.CategoryId);
				if (newCategory == null)
				{
					return BadRequest("Invalid CategoryId");
				}
				product.Category = newCategory;
				product.CategoryId = dto.CategoryId;
			}

			if (!string.IsNullOrEmpty(dto.Name))
			{
				product.Name = dto.Name;
			}

			if (dto.Price > 0)
			{
				product.Price = dto.Price;
			}

			await _context.SaveChangesAsync();

			return new ProductReadDto
			{
				Id = product.Id,
				Name = product.Name,
				Price = product.Price,
				CategoryName = product.Category?.Name ?? ""
			};
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product == null) return NotFound();
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return NoContent();
		}
	}
}

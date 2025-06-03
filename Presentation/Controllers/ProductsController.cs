using InventoryAPI.Application.DTO;
using InventoryAPI.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await _productService.GetProductsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDTO createProductDTO)
    {

        var product = await _productService.CreateProductAsync(createProductDTO);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createProductDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDTO updateProductDTO)
    {
        var product = await _productService.UpdateProductAsync(id, updateProductDTO);

        if (product == false)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var product = await _productService.DeleteProductAsync(id);
        if (product == false)
        {
            return NotFound();
        }

        return NoContent();
    }
}

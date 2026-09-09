using Asp.Versioning;
using Asp.Versioning.Builder;
using CustomerOrder.Domain.DTOs;
using CustomerOrder.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerOrder.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponseDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(CreateProductDto dto)
    {
        var created = await _productService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateProductDto dto)
    {
        var success = await _productService.UpdateAsync(id, dto);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _productService.DeleteAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
}
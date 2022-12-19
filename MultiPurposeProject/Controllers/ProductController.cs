namespace MultiPurposeProject.Controllers;

using Microsoft.AspNetCore.Mvc;
using MultiPurposeProject.Authorization;
using MultiPurposeProject.Entities;
using MultiPurposeProject.Models.Products;
using MultiPurposeProject.Services;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{

    private IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<List<Product>> GetAll()
    {
        return _productService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById(string id)
    {
        return _productService.GetById(id);
    }

    [HttpPost("create")]
    public ActionResult Create(CreateRequest model)
    {
        _productService.Create(model);

        return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
    }

    [HttpPut("{id}")]
    public ActionResult Update(string id, UpdateRequest model)
    {
        _productService.Update(id, model);
        return Ok(new { message = "Product updated" });
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
        _productService.Delete(id);
        return Ok(new { message = "Product deleted" });
    }

}


namespace API.Controllers;

using API.Services;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("/api")]
public class ProductController(ProductClient client) : ControllerBase
{
    [HttpGet("most-expensive-cheapest")]
    public async Task<IActionResult> GetMostExpensiveCheapest([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);
        return Ok(products);
    }

    [HttpGet("beers-at-1799")]
    public async Task<IActionResult> GetBeersAtPrice([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);

        return Ok(products);
    }

    [HttpGet("most-bottles")]
    public async Task<IActionResult> GetMostBottles([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);
        return Ok(products);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);

        return Ok(products);
    }
}

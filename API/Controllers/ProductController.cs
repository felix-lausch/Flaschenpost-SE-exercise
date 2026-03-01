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
        var result = ProductAnalysis.GetCheapestAndMostExpensive(products);
        return Ok(result);
    }

    [HttpGet("beers-at-1799")]
    public async Task<IActionResult> GetBeersAtPrice([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        return Ok(result);
    }

    [HttpGet("most-bottles")]
    public async Task<IActionResult> GetMostBottles([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetMostBottles(products);

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] Uri? url)
    {
        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetAllResults(products);

        return Ok(result);
    }
}

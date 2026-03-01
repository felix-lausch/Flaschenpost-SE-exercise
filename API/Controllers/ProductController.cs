namespace API.Controllers;

using API.Services;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("/api/[Controller]")]
public class ProductController(ProductClient client) : ControllerBase
{
    [HttpGet("most-expensive-cheapest")]
    public async Task<IActionResult> GetMostExpensiveCheapest([FromQuery] Uri? url)
    {
        if (ValidateUrl(url) is { } error) return error;

        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetCheapestAndMostExpensive(products);
        return Ok(result);
    }

    [HttpGet("beers-at-1799")]
    public async Task<IActionResult> GetBeersAtPrice([FromQuery] Uri? url)
    {
        if (ValidateUrl(url) is { } error) return error;

        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetBeersAtPrice(products, 17.99m);

        return Ok(result);
    }

    [HttpGet("most-bottles")]
    public async Task<IActionResult> GetMostBottles([FromQuery] Uri? url)
    {
        if (ValidateUrl(url) is { } error) return error;

        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetMostBottles(products);

        return Ok(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] Uri? url)
    {
        if (ValidateUrl(url) is { } error) return error;

        var products = await client.FetchProductsAsync(url!);
        var result = ProductAnalysis.GetAllResults(products);

        return Ok(result);
    }

    //TODO: ideally this would read a list of allowed hosts from configuration
    private BadRequestObjectResult? ValidateUrl(Uri? url)
    {
        if (url is null || !url.IsAbsoluteUri)
            return BadRequest("Query parameter 'url' must be an absolute URL.");

        if (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps)
            return BadRequest("Only HTTP and HTTPS URLs are allowed.");

        if (!string.IsNullOrEmpty(url.UserInfo))
            return BadRequest("URLs with embedded credentials are not allowed.");

        var host = url.Host;
        if (host is "localhost" or "127.0.0.1" or "::1" or "0.0.0.0" or "169.254.169.254")
            return BadRequest("Requests to internal addresses are not allowed.");

        return null;
    }
}

using API.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Services;

public class ProductClient(HttpClient httpClient)
{
    private static readonly JsonSerializerOptions options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public async Task<List<Product>> FetchProductsAsync(Uri url)
    {
        var response = await httpClient.GetStreamAsync(url);
        return await JsonSerializer.DeserializeAsync<List<Product>>(response, options);
    }
}

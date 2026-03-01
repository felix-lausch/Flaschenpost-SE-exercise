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
        try
        {
            var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<List<Product>>(stream, options)
                ?? throw new ProductDataException("Product data source returned null.");
        }
        catch (HttpRequestException ex)
        {
            throw new ProductDataException("Upstream request failed.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ProductDataException("Upstream request timed out.", ex);
        }
        catch (JsonException ex)
        {
            throw new ProductDataException("Invalid product data format.", ex);
        }
    }
}

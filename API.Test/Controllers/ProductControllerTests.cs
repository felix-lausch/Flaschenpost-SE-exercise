namespace API.Test.Controllers;

using API.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

[TestClass]
public class ProductControllerTests
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void Init(TestContext _)
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [ClassCleanup]
    public static void Cleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    [TestMethod]
    public async Task MissingUrl_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/products/most-expensive-cheapest");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task NonHttpScheme_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/products/most-expensive-cheapest?url=ftp://example.com/data.json");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task CredentialsInUrl_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/products/most-expensive-cheapest?url=http://user:pass@example.com/data.json");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task LocalhostUrl_ReturnsBadRequest()
    {
        var response = await _client.GetAsync("/api/products/most-expensive-cheapest?url=http://localhost/data.json");
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task ValidUrl_WithProducts_ReturnsOk()
    {
        const string json = """
            [{"id":1,"brandName":"Brand A","name":"Beer A","articles":[
                {"id":1,"shortDescription":"6 x 0,5L","price":5.99,"unit":"Liter","pricePerUnitText":"(1,99 \u20ac/Liter)","image":""}
            ]}]
            """;

        using var factory = CreateFactory(new FakeHandler(HttpStatusCode.OK, json));
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/products/most-expensive-cheapest?url=http://example.com/data.json");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task UpstreamError_Returns502BadGateway()
    {
        using var factory = CreateFactory(new FakeHandler(HttpStatusCode.InternalServerError, ""));
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/products/most-expensive-cheapest?url=http://example.com/data.json");

        Assert.AreEqual(HttpStatusCode.BadGateway, response.StatusCode);
    }

    [TestMethod]
    public async Task EmptyProductList_Returns404()
    {
        using var factory = CreateFactory(new FakeHandler(HttpStatusCode.OK, "[]"));
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/products/most-expensive-cheapest?url=http://example.com/data.json");

        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory(HttpMessageHandler handler)
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b.ConfigureTestServices(services => services
                .AddHttpClient<ProductClient>()
                .ConfigurePrimaryHttpMessageHandler(() => handler)));
    }

    private sealed class FakeHandler(HttpStatusCode statusCode, string body) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(body, System.Text.Encoding.UTF8, "application/json")
            });
        }
    }
}

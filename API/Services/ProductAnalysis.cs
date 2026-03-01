using API.Models;

namespace API.Services;

public static class ProductAnalysis
{
    public static CheapestAndMostExpensiveResult GetCheapestAndMostExpensive(List<Product> products)
    {
        var articleResults = FlattenArticles(products)
            .Where(x => x.PricePerLiter is not null)
            .OrderBy(x => x.PricePerLiter)
            .ToList();


        if (articleResults.Count == 0)
        {
            throw new NoProductsException("No products with parseable price per liter found.");
        }

        return new(articleResults.First(), articleResults.Last());
    }

    public static ArticleResult GetMostBottles(List<Product> products)
    {
        var articleResults = FlattenArticles(products)
            .Where(x => x.BottleCount is not null)
            .ToList();

        if (articleResults.Count == 0)
        {
            throw new NoProductsException("No products with parseable bottle count found.");
        }

        return articleResults.OrderByDescending(x => x.BottleCount).First();
    }

    public static List<ArticleResult> GetBeersAtPrice(List<Product> products, decimal price)
    {
        return FlattenArticles(products)
            .Where(x => x.Price == price && x.PricePerLiter is not null)
            .OrderBy(x => x.PricePerLiter)
            .ToList();
    }

    public static AllResults GetAllResults(List<Product> products)
    {
        return new AllResults(
            GetCheapestAndMostExpensive(products),
            GetBeersAtPrice(products, 17.99m),
            GetMostBottles(products));
    }

    private static IEnumerable<ArticleResult> FlattenArticles(List<Product> products)
    {
        return products.SelectMany(p => p.Articles
            .Select(a => new ArticleResult(
                p.Id, a.Id, p.BrandName, p.Name,
                a.ShortDescription, a.Price,
                a.GetPricePerLiter(),
                a.GetBottleCount())));
    }
}

public record ArticleResult(
    int ProductId,
    int ArticleId,
    string BrandName,
    string Name,
    string ShortDescription,
    decimal Price,
    decimal? PricePerLiter,
    int? BottleCount);

public record CheapestAndMostExpensiveResult(ArticleResult Cheapest, ArticleResult MostExpensive);

public record AllResults(CheapestAndMostExpensiveResult CheapestAndMostExpensive, List<ArticleResult> BeersAt1799, ArticleResult MostBottles);

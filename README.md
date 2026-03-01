# flaschenpost SE – IT Exercise Task #5

## Running the project

**Prerequisites:** .NET 10 SDK

```bash
cd API
dotnet run
```

The API will be available at `http://localhost:5076`. Available routes (all accept a `url` query parameter):

```
GET /api/product/most-expensive-cheapest?url=<json-url>
GET /api/product/beers-at-1799?url=<json-url>
GET /api/product/most-bottles?url=<json-url>
GET /api/product/all?url=<json-url>
```

The routes can be executed via the `API.http` file + viable editor (e.g. VS Code or Visual Studio)

## Running the tests

```bash
cd API.Test
dotnet test
```

---

## Exercise description

„C# Restful API Service to analyze JSON product data“

Create a simple C#/.NET Restful API service, that:
- reads from any URL with JSON-Data a list of product data.
    The URL to read from is given below and contains mostly beers.
- has three different routes and questions to analyse the JSON-Data given.
    - Most expensive and cheapest beer per litre.
    - Which beers cost exactly €17.99? 
    Order the result by price per litre (cheapest first).
    - Which one product comes in the most bottles?
- It also has one route to get the answer to all routes or questions at once.
- Any result or output should be in JSON, too.

A route may look like this:\
`/api/mostBottles?url=http://urlto/productData.json`\
However the structure and naming of the routes is up to you and also part of your task.

You are invited to use and include whatever assets you find from our actual websites (as long as you 
don’t host them publicly) in case you need/want to. You may use any tools that you like to 
accomplish this task, including build/dependency management, IDE/editor, libraries, etc.

We should be able to build and run your project without needing to make any changes to it in a 
recent version of a typical developer’s environment.

The URL for the JSON-Data is the following:
[link](https://flapotest.blob.core.windows.net/test/ProductData.json)

### example response:
```json
[
    {
        "id": 1138,
        "brandName": "Büble",
        "name": "Allgäuer Büble Bayrisch Hell",
        "articles": [
            {
                "id": 1491,
                "shortDescription": "20 x 0,5L (Glas)",
                "price": 17.99,
                "unit": "Liter",
                "pricePerUnitText": "(1,80 €/Liter)",
                "image": "https://image.flaschenpost.de/articles/small/1491.png"
            }
        ]
    },
    {
        "id": 3080,
        "brandName": "Alpirsbacher",
        "name": "Alpirsbacher Kloster Helles",
        "descriptionText": "Alpirsbacher Kloster..\r\n",
        "articles": [
            {
                "id": 4156,
                "shortDescription": "20 x 0,5L (Glas)",
                "price": 20.99,
                "unit": "Liter",
                "pricePerUnitText": "(2,10 €/Liter)",
                "image": "https://image.flaschenpost.de/articles/small/4156.png"
            }
        ]
    }
]
```
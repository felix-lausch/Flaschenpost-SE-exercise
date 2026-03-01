using API.Filters;
using API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
builder.Services.AddHttpClient<ProductClient>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

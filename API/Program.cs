using API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<ProductClient>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

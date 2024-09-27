using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using src.Database;
using src.Repository;
using src.Services.product;
using src.Utils;


var builder = WebApplication.CreateBuilder(args);

// connect database
var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Local"));

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(dataSourceBuilder.Build());

}
);

// add auto-mapper
builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

// add DI services
builder.Services
     .AddScoped<IProductService, ProductService>()
     .AddScoped<ProductRepository, ProductRepository>();


// step 1: add controller
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// test if database is connected or not
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    try
    {
        // Check if the application can connect to the database
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Database is connected");
        }
        else
        {
            Console.WriteLine("Unable to connect to the database.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}


// step 2: use 
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.Run();




using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Services;
using src.Database;
using src.Entity;
using src.Middlewares;
using src.Repository;
using src.Services;
using src.Services.category;
using src.Services.product;
using src.Services.review;
using src.Services.UserService;
using src.Utils;
using static src.Entity.Payment;
using static src.Entity.User;

var builder = WebApplication.CreateBuilder(args);

var dataSourceBuilder = new NpgsqlDataSourceBuilder(
    builder.Configuration.GetConnectionString("Local")
);
dataSourceBuilder.MapEnum<Role>();
dataSourceBuilder.MapEnum<PaymentStatus>();
dataSourceBuilder.MapEnum<PaymentMethod>();
dataSourceBuilder.MapEnum<OrderStatuses>();
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(dataSourceBuilder.ConnectionString);
});

builder.Services.AddAutoMapper(typeof(MapperProfile).Assembly);

builder
    .Services.AddScoped<ICategoryService, CategoryService>()
    .AddScoped<ICategoryRepository, CategoryRepository>()
    .AddScoped<IOrderRepository, OrderRepository>()
    .AddScoped<IOrderService, OrderService>()
    .AddScoped<IPaymentRepository, PaymentRepository>()
    .AddScoped<IPaymentService, PaymentService>()
    .AddScoped<IProductService, ProductService>()
    .AddScoped<IProductRepository, ProductRepository>()
    .AddScoped<IAddressService, AddressService>()
    .AddScoped<AddressRepository, AddressRepository>()
    .AddScoped<IUserService, UserService>()
    .AddScoped<UserRepository, UserRepository>()
    .AddScoped<IReviewService, ReviewService>()
    .AddScoped<ReviewRepository, ReviewRepository>()
    .AddScoped<ICouponService, CouponService>()
    .AddScoped<CouponRepository, CouponRepository>()
    .AddScoped<IWishlistService, WishlistService>()
    .AddScoped<IWishlistRepository, WishlistRepository>()
    .AddScoped<ICartService, CartService>()
    .AddScoped<ICartRepository, CartRepository>();

// cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000", "http://localhost:3001")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials();
        }
    );
});

// later when you deployed FE => add in line 67

//auth
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.MapGet("/", () => "Server is running");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

    try
    {
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
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();




// cors
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add a defult route that return a string  in line 94


app.Run();

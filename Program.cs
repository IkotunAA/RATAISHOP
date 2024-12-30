using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PayStack.Net;
using RATAISHOP.Context;
using RATAISHOP.Exceptions;
using RATAISHOP.Middlewares;
using RATAISHOP.PaymentSettings;
using System.Security.Claims;
using System.Text;
using static RATAISHOP.Exceptions.UnAuthorizedException;
using Microsoft.EntityFrameworkCore;
using RATAISHOP.PaymentServices.Implementations;
using RATAISHOP.PaymentServices.Interfaces;
using RATAISHOP.Repositories.Implementations;
using RATAISHOP.Repositories.Interfaces;
using RATAISHOP.Services.Interfaces;
using RATAISHOP.Services.Implementations;
using RATAISHOP.Authentication;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Register dependencies
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IPaystackPaymentService, PaystackPaymentService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IBankTransferVerificationService, BankTransferVerificationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TokenService>();

// Add controllers and authorization
builder.Services.AddControllers();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
});

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RATAISHOP API APP",
        Version = "v1"
    });
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Input your Bearer token to access this API",
        BearerFormat = "JWT"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Configure Paystack
builder.Services.Configure<PaystackSettings>(builder.Configuration.GetSection("Paystack"));
builder.Services.AddSingleton<PayStackApi>(provider =>
{
    var paystackSettings = provider.GetRequiredService<IOptions<PaystackSettings>>().Value;
    return new PayStackApi(paystackSettings.SecretKey);
});
builder.Services.AddHttpClient<IPaystackPaymentService, PaystackPaymentService>();

// Configure Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    byte[] key = Encoding.ASCII.GetBytes("D29C177C-D1E7-4B17-B498-B3A69B3ECFEF");

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = "Adetunji",
        ValidateLifetime = true,
        ValidateAudience = false,
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            if (!context.Response.HasStarted)
            {
                throw new UnauthorizedException("Authentication Failed.");
            }

            return Task.CompletedTask;
        },
        OnForbidden = _ => throw new ForbiddenException("You are not authorized to access this resource."),
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            if (!string.IsNullOrEmpty(accessToken) &&
                context.HttpContext.Request.Path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

// Configure database context
builder.Services.AddDbContext<RataiDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("Default"));
});

// Build the app
var app = builder.Build();

// Use Kestrel to listen on a dynamic port for deployment
builder.WebHost.UseKestrel()
               .UseUrls("http://0.0.0.0:" + Environment.GetEnvironmentVariable("PORT"));

// Middleware for error handling
app.UseMiddleware<ExceptionMiddleware>();

// Swagger for development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();

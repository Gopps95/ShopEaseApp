using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShopEaseApp;
using ShopEaseApp.Models;
using ShopEaseApp.Helpers;
using Microsoft.OpenApi.Models;
using static ShopEaseApp.Models.ShoppingDataContext;
using ShopEaseApp.Repositories;
using ShopEaseApp.Areas.Buyer.Models;
using ShopEaseApp.Areas.Seller.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<ShoppingModelDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingCnString")).UseQueryTrackingBehavior
    (QueryTrackingBehavior.NoTracking));

builder.Services.AddScoped<BuyerModel, IBuyer>();
//builder.Services.AddScoped<ISellerModel, SellerModel>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//    .AddEntityFrameworkStores<ShoppingModelDB>()
//    .AddDefaultTokenProviders();

//builder.Services.AddTransient<IAuthService, AuthService>();


//builder.Services.AddTransient<IUserRepository, SellerRepository>();
builder.Services.AddScoped<IUserRepository, BuyerRepository>();
builder.Services.AddScoped<ISellerModel, SellerModel>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.IOTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "SessionData";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Path = "/";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;// Set session timeout
});
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShopEaseApp", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        //ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BuyerOnly", policy => policy.RequireRole("Buyer"));
    options.AddPolicy("SellerOnly", policy => policy.RequireRole("Seller"));
});
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
   
}

//app.Use(async (context, next) =>
//{
//    var JWToken = context.Session.GetString("JWTToken");
//    if (!string.IsNullOrEmpty(JWToken) && !context.Request.Headers.ContainsKey("Authorization"))
//    {
//        context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
//    }
//    await next();
//});


app.UseSession();
    app.Use(async (context, next) =>
    {
        var JWToken = context.Session.GetString("UserID");
        //if (!string.IsNullOrEmpty(JWToken))
        //{
        //    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
        //}
        await next();
    });

app.UseHttpsRedirection();
app.UseAuthentication();
   // app.UseRouting();
    app.UseAuthorization();

    app.MapControllers();
    app.Run();


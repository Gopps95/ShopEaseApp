using Microsoft.EntityFrameworkCore;
using ShopEaseApp.Areas.Buyer.Models;
using ShopEaseApp.Models;
using static ShopEaseApp.Models.ShoppingDataContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ShoppingModel>
           (options => options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingCnString")));
builder.Services.AddScoped<IBuyer, ProductRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
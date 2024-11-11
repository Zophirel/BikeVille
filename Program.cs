using BikeVille.SqlDbContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BikeVilleProductsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsDB")));
builder.Services.AddDbContext<BikeVilleCustomersContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersDB")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();

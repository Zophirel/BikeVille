using BikeVille.Controllers.Auth;
using BikeVille.Services;
using BikeVille.SqlDbContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BikeVilleProductsContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ProductsDB")));
builder.Services.AddDbContext<BikeVilleCustomersContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CustomersDB")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 5078; // Ensure this matches your HTTPS port.
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new RequireHttpsAttribute());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
 {
       options.CustomSchemaIds(type => type.ToString());
 });

builder.Services.AddAuthentication()
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("BasicAuthentication", new AuthorizationPolicyBuilder("BasicAuthentication")
            .RequireAuthenticatedUser().Build());

builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(
    opts => opts.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    )
);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

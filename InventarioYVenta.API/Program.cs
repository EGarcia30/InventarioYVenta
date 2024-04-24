using InventarioYVenta.API.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//string de la conexion
string connectionName = "InventarioYVentaDbContext";
var dbContext = builder.Configuration.GetConnectionString(connectionName);

//Crear conexion a base de datos
builder.Services.AddDbContext<InventarioYVentaDbContext>(options => options.UseSqlServer(dbContext));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

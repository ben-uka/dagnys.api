using Dagnys_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.LoadSuppliers(context);
    await Seed.LoadProducts(context);
    await Seed.LoadSupplierProducts(context);
}
catch (Exception ex)
{
    Console.WriteLine("{0}", ex.Message);
    throw;
}

app.MapControllers();

app.Run();

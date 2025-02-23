using System.Reflection;
using dagnys.api.Data;
using dagnys.api.Interfaces;
using dagnys.api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var serverVersion = new MySqlServerVersion(new Version(9, 1, 0));

builder.Services.AddDbContext<DataContext>(options =>
{
    // options.UseMySql(builder.Configuration.GetConnectionString("MySQL"), serverVersion);
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Dagnys REST API",
            Version = "1.0",
            Description = "API for Dagnys bakery",
            Contact = new OpenApiContact { Name = "Benjamin Uka", Email = "benjaminuka@gmail.com" },
        }
    );

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();

    await Seed.LoadAddressTypes(context);
    await Seed.LoadPostalAddresses(context);
    await Seed.LoadProducts(context);
    await Seed.LoadCustomers(context);
    await Seed.LoadOrders(context);
    await Seed.LoadOrderItems(context);
    await Seed.LoadContacts(context);
    await Seed.LoadCustomerContacts(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

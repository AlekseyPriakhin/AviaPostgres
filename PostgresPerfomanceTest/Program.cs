using Microsoft.EntityFrameworkCore;
using PostgresPerfomanceTest.Data;
using PostgresPerfomanceTest.Data.MongoContext;
using PostgresPerfomanceTest.Services;
using PostgresPerfomanceTest.Services.MongoServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICompanyService, CompanyServiceSQL>();
builder.Services.AddScoped<IFlightService, FlightServiceSQL>();
builder.Services.AddScoped<IPlaneService, PlaneServiceSQL>();

builder.Services.AddSingleton<IMongoContext, MongoContext>();
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();

var connectionString = builder.Configuration.GetConnectionString("Db");
builder.Services.AddDbContext<AviaDbContext>(options =>
    options.UseNpgsql(
        connectionString
    )
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedData.Initialize(serviceProvider);
    await SeedData.InitializeMongo(serviceProvider);
}


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
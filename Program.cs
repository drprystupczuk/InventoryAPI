using InventoryAPI.Application.Mapper;
using InventoryAPI.Application.Services;
using InventoryAPI.Domain.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80);
});

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddDbContext<InventoryContext>(opt =>
    opt.UseSqlite("DataSource=inventory.db"));

builder.Services.AddMassTransit(x =>
{
    var retryCount = 0;
    const int maxRetries = 5;

    while (retryCount < maxRetries)
    {
        try
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq://rabbitmq");
                cfg.UseMessageRetry(r => r.Interval(5, TimeSpan.FromSeconds(2)));

                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.TripThreshold = 50;
                    cb.ActiveThreshold = 5;
                    cb.ResetInterval = TimeSpan.FromMinutes(1);
                });
            });
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            Console.WriteLine($"RabbitMQ connection attempt {retryCount}/{maxRetries} failed: {ex.Message}");

            if (retryCount == maxRetries)
            {
                throw;
            }

            Thread.Sleep(5000);
        }
    }
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

using Data;
using Microsoft.EntityFrameworkCore;
using Models.Options;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// MVC + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddScoped<ITemperatureService, TemperatureService>();
builder.Services.AddSingleton<IPinValidator, PinValidator>();
builder.Services.Configure<ApexOptions>(builder.Configuration.GetSection("Apex"));
builder.Services.AddHttpClient<IApexClient, ApexClient>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();

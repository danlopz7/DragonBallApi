using Microsoft.EntityFrameworkCore;
using DragonBallApi.Data;
using DragonBallApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register DB context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddOpenApi();

// Register HTTP client and custom service
builder.Services.AddHttpClient();
builder.Services.AddScoped<IDragonBallService, DragonBallService>();

// Register Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();




// Adding services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
//builder.Services.AddHttpClient();
//builder.Services.AddScoped<IDragonBallService, DragonBallService>();

//var app = builder.Build();

//builder.Services.AddScoped<DragonBallService>();
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

//app.UseHttpsRedirection();
//app.Run();
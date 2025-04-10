using Microsoft.EntityFrameworkCore;
using DragonBallApi.Data;
using DragonBallApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
//builder.Services.AddScoped<IDragonBallService, DragonBallService>();

var app = builder.Build();

//builder.Services.AddScoped<DragonBallService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using API.Server;
using shared.Model;

var builder = WebApplication.CreateBuilder(args);

// ** Add CORS Policy **
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7228")  // Update with the URL of your Blazor app
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ** Add services to the container. **
builder.Services.AddControllers();

// ** Add DbContext for SQLite **
builder.Services.AddDbContext<RedditDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  // Use SQLite

// Add Swagger (optional, if you want to generate API documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ** Configure the HTTP request pipeline. **
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS (for API requests from Blazor)
app.UseCors("AllowSpecificOrigin");

// ** Use HTTPS Redirection **
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

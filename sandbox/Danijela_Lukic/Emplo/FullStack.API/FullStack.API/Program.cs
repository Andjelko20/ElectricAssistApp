using FullStack.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<FullStckDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FullStackConnectionString")));

builder.Services.AddCors(options => options.AddPolicy(name: "FullStackConnectionStringOrigin", build =>
 {
     build.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
 }));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("FullStackConnectionStringOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

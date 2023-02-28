using DemoBack.Database;
using DemoBack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("MongoDatabase"));
builder.Services.AddSingleton<DriverService>();
builder.Services.AddCors(options => options.AddPolicy(name: "Formula1Origin",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Formula1Origin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

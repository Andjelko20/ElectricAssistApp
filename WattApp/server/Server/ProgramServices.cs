using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Data;
using Server.Filters;
using Server.Services;
using Server.Services.Implementations;
using System.Reflection;
using System.Text;
using Server.Services.Implementations;
using Server.Mappers;
using Server.Controllers;

namespace Server
{
    public partial class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void AddServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            //builder.Services.AddRazorPages();

            builder.Services.AddMvc();

            // Adding services from Services directory
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IUserService, UserService>();
			builder.Services.AddScoped<DeviceCategoryService, DeviceCategoryServiceImpl>();
            builder.Services.AddScoped<DeviceTypeService, DeviceTypeServiceImpl>();
            builder.Services.AddScoped<DeviceBrandService, DeviceBrandServiceImpl>();
            builder.Services.AddScoped<DeviceModelService, DeviceModelServiceImpl>();
            builder.Services.AddScoped<DeviceService, DeviceServiceImpl>();
            builder.Services.AddScoped<DropDownService, DropDownServiceImpl>(); ;
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IHistoryService, HistoryServiceImpl>();
            builder.Services.AddScoped<IPredictionService, PredictionServiceImpl>();
            builder.Services.AddScoped<IProsumerService, ProsumerServiceImpl>();
            builder.Services.AddScoped<IDSOService, DSOServiceImpl>();
            builder.Services.AddScoped<ICurrentPeriodHistoryService, CurrentPeriodHistoryImpl>();
            builder.Services.AddScoped<IHistoryFromToService, HistoryFromToServiceImpl>();

            builder.Services.Configure<ApiBehaviorOptions>(options
                => options.SuppressModelStateInvalidFilter = true);

            // Filter added
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(BadRequestValidationFilter));
            });

            // Authentication service added
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretKey").Value);
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // for dev
                    ValidateAudience = false, // for dev
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateActor = false
                };
            });


            // DbContext added
            builder.Services.AddDbContext<SqliteDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            });

            // CORS policy added
            builder.Services.AddCors();


            // Swagger documentation added
            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ElectricAssist API",
                    Description = "API dokumentacija za projekat iz SI"
                });
                // Authentication scheme
                swagger.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                // Added filter for authenticated and anonymous routes
                swagger.OperationFilter<AuthResponsesOperationFilter>();

                // XML comments enabled
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });

        }
    }
}

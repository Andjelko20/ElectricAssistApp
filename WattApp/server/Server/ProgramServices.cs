using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Server.Data;
using Server.Filters;
using Server.Services;
using System.Reflection;
using System.Text;
using Server.Services.Implementations;
using Server.Mappers;

namespace Server
{
    public partial class Program
    {
        public static void AddServices(WebApplicationBuilder builder)
        {
            // Add services to the container.
            //builder.Services.AddRazorPages();

            builder.Services.AddMvc();

            //builder.Services.Add(new ServiceDescriptor(typeof(EmailService), new EmailService(builder.Configuration)));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
			builder.Services.AddScoped<DeviceCategoryService, DeviceCategoryServiceImpl>();
            builder.Services.AddScoped<DeviceTypeService, DeviceTypeServiceImpl>();
            builder.Services.AddScoped<DeviceBrandService, DeviceBrandServiceImpl>();
            builder.Services.AddScoped<DeviceModelService, DeviceModelServiceImpl>();
            builder.Services.AddScoped<DeviceService, DeviceServiceImpl>();
            builder.Services.AddScoped<DropDownService, DropDownServiceImpl>(); ;
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.Configure<ApiBehaviorOptions>(options
                => options.SuppressModelStateInvalidFilter = true);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(BadRequestValidationFilter));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretKey").Value);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // for dev
                    ValidateAudience = false, // for dev
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });

            builder.Services.AddDbContext<SqliteDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
            });

            builder.Services.AddCors();

            builder.Services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ElectricAssist API",
                    Description = "API dokumentacija za projekat iz SI"
                });
                swagger.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });
                /*
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearer"
                            }
                        },
                        new List<string>()
                    }
                });
                */

                swagger.OperationFilter<AuthResponsesOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                swagger.IncludeXmlComments(xmlPath);
            });
        }
    }
}

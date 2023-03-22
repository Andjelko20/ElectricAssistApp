using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Server.Data;
using Microsoft.EntityFrameworkCore;
using Server.Filters;
using Microsoft.AspNetCore.Mvc;
using Server.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Server.Services;

namespace Server
{
    /// <summary>
    /// Server starter
    /// </summary>
    public partial class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AddServices(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "ElectricAssist API");
                    config.RoutePrefix = "api/docs";
                });
            }

            app.UseHttpsRedirection();

            // Allow CORS for all origins, headers and methods 
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
            });
            //app.UseStaticFiles();

            app.UseRouting();

            //app.MapRazorPages();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            SqliteDbContext.Seed(app);

            app.UseTokenValidation();

            app.Run();
        }
    }
}
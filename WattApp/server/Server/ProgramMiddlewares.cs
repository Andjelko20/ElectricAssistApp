using Server.Middlewares;

namespace Server
{
    public partial class Program
    {
        /// <summary>
        /// Add middlewares
        /// </summary>
        /// <param name="app">Web application</param>
        public static void AddMiddlewares(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "ElectricAssist API");
                    config.RoutePrefix = "api/docs";
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Allow CORS for all origins, headers and methods 
            app.UseCors(builder =>
            {
                builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.UseTokenValidation();
        }
    }
}

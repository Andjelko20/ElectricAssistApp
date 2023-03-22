using Server.Data;
using Server.Middlewares;

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

            AddMiddlewares(app);

            SqliteDbContext.Seed(app);

            app.Run();
        }
    }
}
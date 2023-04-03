using Server.Data;

namespace Server
{
    /// <summary>
    /// Server starter
    /// </summary>
    public partial class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
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
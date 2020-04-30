using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using System;
using System.Threading.Tasks;

namespace TemporarуYieldDestroyer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Destroying all yield");

            Task.WaitAll(Task.Run(() => DeleteAndRecreateDatabasе<CatalogContext>("CatalogDb", options => new CatalogContext(options))), Task.Run(() => DeleteAndRecreateDatabasе<AppIdentityDbContext>("Identity", options => new AppIdentityDbContext(options))));

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("All yield successfully destroyed");
            Console.ResetColor();

            static void DeleteAndRecreateDatabasе<TDbContext>(string databaseName, Func<DbContextOptions<TDbContext>, TDbContext> createDbContext)
                where TDbContext : DbContext
            {
                var options = new DbContextOptionsBuilder<TDbContext>().UseSqlServer($"Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.{databaseName};").Options;
                var catalogContext = createDbContext(options);
                catalogContext.Database.EnsureDeleted();
                catalogContext.Database.Migrate();
            }
        }
    }
}

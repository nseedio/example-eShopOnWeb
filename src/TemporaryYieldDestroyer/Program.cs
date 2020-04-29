using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using System;

namespace TemporarуYieldDestroyer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Destroying all yield");

            var options = new DbContextOptionsBuilder<CatalogContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.CatalogDb;").Options;
            var catalogContext = new CatalogContext(options);
            catalogContext.Database.EnsureDeleted();
            catalogContext.Database.Migrate();

            var identityOptions = new DbContextOptionsBuilder<AppIdentityDbContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.Identity;").Options;
            var identityContext = new AppIdentityDbContext(identityOptions);
            identityContext.Database.EnsureDeleted();
            identityContext.Database.Migrate();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("All yield successfully destroyed");
            Console.ResetColor();
        }
    }
}

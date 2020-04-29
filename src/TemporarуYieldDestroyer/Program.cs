using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace TemporarуYieldDestroyer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<CatalogContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.CatalogDb;").Options;
            var catalogContext = new CatalogContext(options);
            catalogContext.Database.EnsureDeleted();
            catalogContext.Database.Migrate();

            var identityOptions = new DbContextOptionsBuilder<AppIdentityDbContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.Identity;").Options;
            var identityContext = new AppIdentityDbContext(identityOptions);
            identityContext.Database.EnsureDeleted();
            identityContext.Database.Migrate();
        }
    }
}

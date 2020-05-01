using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Brands
{
    public class MicrosoftBrands : ISeed<CatalogBrand>
    {
        private static class Markers
        {
            public static string AzureName = "Azure";
            public static string DotNetName = ".NET";
            public static string VisualStudioName = "Visual Studio";
            public static string SqlServerName = "SQL Server";
            public static string RoslynName = "Roslyn";

            public static readonly string[] AllBrands = new string[] { AzureName, DotNetName, VisualStudioName, SqlServerName, RoslynName };
        }

        private readonly CatalogContext dbContext;

        public MicrosoftBrands(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            dbContext.CatalogBrands.AddRange(Markers.AllBrands.Select(brand => new CatalogBrand(brand)));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogBrands.CountAsync(brand => Markers.AllBrands.Contains(brand.Brand)) == Markers.AllBrands.Length;
        }

        public class Yield : YieldOf<MicrosoftBrands>
        {
            public CatalogBrand Azure { get; }
            public CatalogBrand DotNet { get; }
            public CatalogBrand VisualStudio { get; }
            public CatalogBrand SqlServer { get; }
            public CatalogBrand Roslyn { get; }

            public Yield(CatalogContext dbContext)
            {
                var brands = dbContext.CatalogBrands.Where(brand => Markers.AllBrands.Contains(brand.Brand)).ToArray();

                Azure = brands.First(brand => brand.Brand == Markers.AzureName);
                DotNet = brands.First(brand => brand.Brand == Markers.DotNetName);
                VisualStudio = brands.First(brand => brand.Brand == Markers.VisualStudioName);
                SqlServer = brands.First(brand => brand.Brand == Markers.SqlServerName);
                Roslyn = brands.First(brand => brand.Brand == Markers.RoslynName);
            }
        }
    }
}

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
        private readonly CatalogContext dbContext;

        public MicrosoftBrands(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            dbContext.CatalogBrands.AddRange(Yield.AllBrands.Select(brand => new CatalogBrand(brand)));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogBrands.CountAsync(brand => Yield.AllBrands.Contains(brand.Brand)) == Yield.AllBrands.Length;
        }

        public class Yield : YieldOf<MicrosoftBrands>
        {
            // NSEED-BEST-PRACTICES:
            // These are so called seed markers. They uniquely identify yield produced by the seed.
            // If markers are meaningful for other seeds that consume the yield, the best practice is to declar
            // them as public static fields in the yield class.
            // That way they can be used in the seed and in the seeds that use this yield.
            // If they are not meaningful to other seeds, the best practice is to declare a private static
            // nested class within the seed called Markers and to declare them in that class.
            // That way markers will be internal to the seed and its yield class.
            public static string AzureName = "Azure";
            public static string DotNetName = ".NET";
            public static string VisualStudioName = "Visual Studio";
            public static string SqlServerName = "SQL Server";
            public static string RoslynName = "Roslyn";

            public static readonly string[] AllBrands = new string[] { AzureName, DotNetName, VisualStudioName, SqlServerName, RoslynName };

            public CatalogBrand Azure { get; }
            public CatalogBrand DotNet { get; }
            public CatalogBrand VisualStudio { get; }
            public CatalogBrand SqlServer { get; }
            public CatalogBrand Roslyn { get; }

            public Yield(CatalogContext dbContext)
            {
                var brands = dbContext.CatalogBrands.Where(brand => AllBrands.Contains(brand.Brand)).ToArray();

                Azure = brands.First(brand => brand.Brand == AzureName);
                DotNet = brands.First(brand => brand.Brand == DotNetName);
                VisualStudio = brands.First(brand => brand.Brand == VisualStudioName);
                SqlServer = brands.First(brand => brand.Brand == SqlServerName);
                Roslyn = brands.First(brand => brand.Brand == RoslynName);
            }
        }
    }
}

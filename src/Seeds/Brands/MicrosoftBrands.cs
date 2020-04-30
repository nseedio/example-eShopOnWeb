using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using NSeed;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Brands
{
    [AlwaysRequired]
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
            public static string AzureName = "Azure";
            public static string DotNetName = ".NET";
            public static string VisualStudioName = "Visual Studio";
            public static string SqlServerName = "SQL Server";

            public static readonly string[] AllBrands = new string[] { AzureName, DotNetName, VisualStudioName, SqlServerName };

            public CatalogBrand Azure { get; }
            public CatalogBrand DotNet { get; }
            public CatalogBrand VisualStudio { get; }
            public CatalogBrand SqlServer { get; }

            public Yield(CatalogContext dbContext)
            {
                var brands = dbContext.CatalogBrands.Where(brand => AllBrands.Contains(brand.Brand)).ToArray();

                Azure = brands.First(brand => brand.Brand == AzureName);
                DotNet = brands.First(brand => brand.Brand == DotNetName);
                VisualStudio = brands.First(brand => brand.Brand == VisualStudioName);
                SqlServer = brands.First(brand => brand.Brand == SqlServerName);
            }
        }
    }
}

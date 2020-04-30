using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.CatalogTypes
{
    public class SwagShopCatalogTypes : ISeed<CatalogType>
    {
        private readonly CatalogContext dbContext;

        public SwagShopCatalogTypes(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            dbContext.CatalogTypes.AddRange(Yield.AllSwags.Select(catalogType => new CatalogType(catalogType)));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogTypes.CountAsync(catalogType => Yield.AllSwags.Contains(catalogType.Type)) == Yield.AllSwags.Length;
        }

        public class Yield : YieldOf<SwagShopCatalogTypes>
        {
            // NSEED-BEST-PRACTICES:
            // These are so called seed markers. They uniquely identify yield produced by the seed.
            // If markers are meaningful for other seeds that consume the yield, the best practice is to declar
            // them as public static fields in the yield class.
            // That way they can be used in the seed and in the seeds that use this yield.
            // If they are not meaningful to other seeds, the best practice is to declare a private static
            // nested class within the seed called Markers and to declare them in that class.
            // That way markers will be internal to the seed and its yield class.
            public static string MugName = "Mug";
            public static string TShirtName = "T-Shirt";
            public static string SheetName = "Sheet";
            public static string UsbMemoryStickName = "USB Memory Stick";

            public static readonly string[] AllSwags = new string[] { MugName, TShirtName, SheetName, UsbMemoryStickName };

            // NSEED-BEST-PRACTICES:
            // If the yield provide properties, they should be fetched only once from the persistence store or in
            // general calculated only once.
            // If they are fetched or calculated every time, methods should be used. E.g. GetMug();
            public CatalogType Mug { get; }
            public CatalogType TShirt { get; }
            public CatalogType Sheet { get; }
            public CatalogType UsbMemoryStick { get; }

            public Yield(CatalogContext dbContext)
            {
                var catalogTypes = dbContext.CatalogTypes.Where(catalogType => AllSwags.Contains(catalogType.Type)).ToArray();

                Mug = catalogTypes.First(catalogType => catalogType.Type == MugName);
                TShirt = catalogTypes.First(catalogType => catalogType.Type == TShirtName);
                Sheet = catalogTypes.First(catalogType => catalogType.Type == SheetName);
                UsbMemoryStick = catalogTypes.First(catalogType => catalogType.Type == UsbMemoryStickName);
            }
        }
    }
}

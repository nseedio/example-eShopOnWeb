using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.CatalogTypes
{
    public class SwagShopCatalogTypes : ISeed<CatalogType>
    {
        // NSEED-BEST-PRACTICES:
        // These are so called seed markers. They uniquely identify yield produced by this seed.
        // The best practice is to declar markers in a private nested static class called Markers.
        // That way markers will be internal to the seed and its yield class, encapsulated as an implementational detail.
        // Other seed that require yield of this seed should access the yield only through the Yield
        // object and should not use markers directly to build their own queries.
        // However, there are cases when markers are meaningful to other seeds that require this seed.
        // See the best practice below on how to expose them in such cases.
        private static class Markers
        {
            public static string MugName = "Mug";
            public static string TShirtName = "T-Shirt";
            public static string SheetName = "Sheet";
            public static string UsbMemoryStickName = "USB Memory Stick";

            public static readonly string[] AllSwags = new string[] { MugName, TShirtName, SheetName, UsbMemoryStickName };
        }

        private readonly CatalogContext dbContext;

        public SwagShopCatalogTypes(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            dbContext.CatalogTypes.AddRange(Markers.AllSwags.Select(catalogType => new CatalogType(catalogType)));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogTypes.CountAsync(catalogType => Markers.AllSwags.Contains(catalogType.Type)) == Markers.AllSwags.Length;
        }

        public class Yield : YieldOf<SwagShopCatalogTypes>
        {
            // NSEED-BEST-PRACTICES:
            // Sometimes markers can be useful to use directly by other seeds that require some yield.
            // In that case, expose them via properties on the Yield class, like in this case.
            // NSEED-NOTE:
            // This should be an exception and not the rule and should rarely be needed.
            // The best practice is not to expose markers until there is a clear need for that.
            // A need can be e.g. allowing other seeds to write arbitrary optimized queries to fetch
            // the yield of this seed.
            public string MugName { get; } = Markers.MugName;
            public string TShirtName { get; } = Markers.TShirtName;
            public string SheetName { get; } = Markers.SheetName;
            public string UsbMemoryStickName { get; } = Markers.UsbMemoryStickName;

            // NSEED-BEST-PRACTICES:
            // If a yield class provides properties, this implies that the provided objects are fetched only once
            // from the persistance store.
            // If they are fetched every time, methods should be used instead of properties. E.g. GetMug();
            public CatalogType Mug { get; }
            public CatalogType TShirt { get; }
            public CatalogType Sheet { get; }
            public CatalogType UsbMemoryStick { get; }
            public IReadOnlyCollection<CatalogType> AllCatalogTypes { get; }

            public Yield(CatalogContext dbContext)
            {
                var catalogTypes = dbContext.CatalogTypes.Where(catalogType => Markers.AllSwags.Contains(catalogType.Type)).ToArray();

                Mug = catalogTypes.First(catalogType => catalogType.Type == Markers.MugName);
                TShirt = catalogTypes.First(catalogType => catalogType.Type == Markers.TShirtName);
                Sheet = catalogTypes.First(catalogType => catalogType.Type == Markers.SheetName);
                UsbMemoryStick = catalogTypes.First(catalogType => catalogType.Type == Markers.UsbMemoryStickName);

                AllCatalogTypes = new[] { Mug, TShirt, Sheet, UsbMemoryStick };
            }
        }
    }
}

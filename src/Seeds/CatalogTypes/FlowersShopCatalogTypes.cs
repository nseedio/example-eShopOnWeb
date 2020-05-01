using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.CatalogTypes
{
    public class FlowersShopCatalogTypes : ISeed<CatalogType>
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
            public static string Plant = "Plant";
            public static string Flower = "Flower";
            public static string Bouquet = "Bouquet";

            public static readonly string[] AllFlowers = new string[] { Plant, Flower, Bouquet };
        }

        private readonly CatalogContext dbContext;

        public FlowersShopCatalogTypes(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            dbContext.CatalogTypes.AddRange(Markers.AllFlowers.Select(catalogType => new CatalogType(catalogType)));
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogTypes.CountAsync(catalogType => Markers.AllFlowers.Contains(catalogType.Type)) == Markers.AllFlowers.Length;
        }

        public class Yield : YieldOf<FlowersShopCatalogTypes>
        {
            // NSEED-BEST-PRACTICES:
            // Sometimes markers can be useful to use directly by other seeds that require some yield.
            // In that case, expose them via properties on the Yield class, like in this case.
            // NSEED-WARNING:
            // This should be an exception and not the rule and should rarely be needed.
            // The best practice is not to expose markers until there is a clear need for that.
            // A need can be e.g. allowing other seeds to write arbitrary optimized queries to fetch
            // the yield of this seed.
            public string PlantName { get; } = Markers.Plant;
            public string FlowerName { get; } = Markers.Flower;
            public string BouquetName { get; } = Markers.Bouquet;

            // NSEED-BEST-PRACTICES:
            // If a yield class provides properties, this implies that the provided objects are fetched only once
            // from the persistance store.
            // If they are fetched every time, methods should be used instead of properties. E.g. GetMug();
            public CatalogType Plants { get; }
            public CatalogType Flower { get; }
            public CatalogType Bouquet { get; }

            public Yield(CatalogContext dbContext)
            {
                var catalogTypes = dbContext.CatalogTypes.Where(catalogType => Markers.AllFlowers.Contains(catalogType.Type)).ToArray();

                Plants = catalogTypes.First(catalogType => catalogType.Type == Markers.Plant);
                Flower = catalogTypes.First(catalogType => catalogType.Type == Markers.Flower);
                Bouquet = catalogTypes.First(catalogType => catalogType.Type == Markers.Bouquet);
            }
        }
    }
}

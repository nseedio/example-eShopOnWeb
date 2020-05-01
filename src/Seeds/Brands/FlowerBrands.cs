using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Brands
{
    public class FlowerBrands : ISeed<CatalogBrand>
    {
        private static class Markers
        {
            public static string GreenPlant = "Green Plant";
            public static string RoseCelebration = "Rose Celebration";
            public static string FloristDesignedBouquet = "Florist Designed Bouquet";
            public static string PicturesqueOrchid = "Picturesque Orchid";

            public static readonly string[] AllBrands = new string[] { GreenPlant, RoseCelebration, FloristDesignedBouquet, PicturesqueOrchid };
        }

        private readonly CatalogContext dbContext;

        public FlowerBrands(CatalogContext dbContext)
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

        public class Yield : YieldOf<FlowerBrands>
        {
            public CatalogBrand GreenPlant { get; }
            public CatalogBrand RoseCelebration { get; }
            public CatalogBrand FloristDesignedBouquet { get; }
            public CatalogBrand PicturesqueOrchid { get; }
            public CatalogBrand Roslyn { get; }

            public Yield(CatalogContext dbContext)
            {
                var brands = dbContext.CatalogBrands.Where(brand => Markers.AllBrands.Contains(brand.Brand)).ToArray();

                GreenPlant = brands.First(brand => brand.Brand == Markers.GreenPlant);
                RoseCelebration = brands.First(brand => brand.Brand == Markers.RoseCelebration);
                FloristDesignedBouquet = brands.First(brand => brand.Brand == Markers.FloristDesignedBouquet);
                PicturesqueOrchid = brands.First(brand => brand.Brand == Markers.PicturesqueOrchid);
            }
        }
    }
}

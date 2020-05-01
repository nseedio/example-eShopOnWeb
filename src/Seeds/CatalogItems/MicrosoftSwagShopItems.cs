using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using Seeds.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.CatalogTypes
{
    public class MicrosoftSwagShopItems : ISeed<CatalogItem>
    {
        private static class Markers
        {
            // NSEED-BEST-PRACTICES:
            // When creating larger numbers of entities we often do not have a natural unique marker
            // to distinguish them. In that case we create artificial unique markers often using
            // GUIDs. NSeed Markers class provides extension methods for turning such unique values
            // into markers that can be injected into entities.
            public static readonly string DescriptionMarker = new Guid("{C2EB9FB2-8775-4DA7-AFD2-843E8867FF17}").ToStringMarker();
        }

        // NSEED-FEATURE:
        // This seed requires yield of other seeds.
        // It just has to define properties of the type of the required yield.
        // NSeed engine will automatically seed the required seeds if needed,
        // create the requested yield objects and set the below properties.
        private MicrosoftBrands.Yield Brands { get; set; }
        private SwagShopCatalogTypes.Yield CatalogTypes { get; set; }

        private readonly CatalogContext dbContext;

        public MicrosoftSwagShopItems(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var items = new []
            {
                new CatalogItem(CatalogTypes.TShirt.Id, Brands.DotNet.Id, ".NET Bot Black Sweatshirt " + Markers.DescriptionMarker, ".NET Bot Black Sweatshirt", 19.5M,  "http://catalogbaseurltobereplaced/images/products/1.png"),
                new CatalogItem(CatalogTypes.Mug.Id, Brands.DotNet.Id, ".NET Black & White Mug " + Markers.DescriptionMarker, ".NET Black & White Mug", 8.50M, "http://catalogbaseurltobereplaced/images/products/2.png"),
                new CatalogItem(CatalogTypes.TShirt.Id, Brands.DotNet.Id, ".NET Foundation Sweatshirt " + Markers.DescriptionMarker, ".NET Foundation Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/4.png"),
                new CatalogItem(CatalogTypes.Sheet.Id, Brands.Roslyn.Id, "Roslyn Red Sheet " + Markers.DescriptionMarker, "Roslyn Red Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/5.png"),
                new CatalogItem(CatalogTypes.TShirt.Id, Brands.DotNet.Id, ".NET Blue Sweatshirt " + Markers.DescriptionMarker, ".NET Blue Sweatshirt", 12, "http://catalogbaseurltobereplaced/images/products/6.png"),
                new CatalogItem(CatalogTypes.TShirt.Id, Brands.Roslyn.Id, "Roslyn Red T-Shirt " + Markers.DescriptionMarker, "Roslyn Red T-Shirt",  12, "http://catalogbaseurltobereplaced/images/products/7.png"),
                new CatalogItem(CatalogTypes.Mug.Id, Brands.DotNet.Id, "Cup<T> White Mug " + Markers.DescriptionMarker, "Cup<T> White Mug", 12, "http://catalogbaseurltobereplaced/images/products/9.png"),
                new CatalogItem(CatalogTypes.Sheet.Id, Brands.DotNet.Id, ".NET Foundation Sheet " + Markers.DescriptionMarker, ".NET Foundation Sheet", 12, "http://catalogbaseurltobereplaced/images/products/10.png"),
                new CatalogItem(CatalogTypes.Sheet.Id, Brands.DotNet.Id, "Cup<T> Sheet " + Markers.DescriptionMarker, "Cup<T> Sheet", 8.5M, "http://catalogbaseurltobereplaced/images/products/11.png"),
            };

            dbContext.CatalogItems.AddRange(items);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogItems.AnyAsync(catalogItem => catalogItem.Description.Contains(Markers.DescriptionMarker));
        }

        public class Yield : YieldOf<MicrosoftSwagShopItems>
        {
            public async Task<IReadOnlyCollection<CatalogItem>> GetAllItems() => await Seed.dbContext.CatalogItems.Where(catalogItem => catalogItem.Description.Contains(Markers.DescriptionMarker)).ToArrayAsync();
        }
    }
}

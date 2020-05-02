using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using Seeds.Brands;
using Seeds.CatalogTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.CatalogItems
{
    public class FlowerShopItems : ISeed<CatalogItem>
    {
        private static class Markers
        {
            // NSEED-BEST-PRACTICES:
            // When creating larger numbers of entities we often do not have a natural unique marker
            // to distinguish them. In that case we create artificial unique markers often using
            // GUIDs. NSeed Markers class provides extension methods for turning such unique values
            // into markers that can be injected into entities.
            public static readonly string DescriptionMarker = new Guid("{D1EB9FB2-8775-4DB5-AFD2-843E8864BB01}").ToStringMarker();
        }

        // NSEED-FEATURE:
        // This seed requires yield of other seeds.
        // It just has to define properties of the type of the required yield.
        // NSeed engine will automatically seed the required seeds if needed,
        // create the requested yield objects and set the below properties.
        private FlowerBrands.Yield Brands { get; set; }
        private FlowersShopCatalogTypes.Yield CatalogTypes { get; set; }

        private readonly CatalogContext dbContext;

        public FlowerShopItems(CatalogContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var items = new []
            {
                new CatalogItem(CatalogTypes.Plant.Id, Brands.GreenPlant.Id, "Butterfly Dish Garden" + Markers.DescriptionMarker, "Butterfly Dish Garden", 43.99M,  "butterfly-dish-garden.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Plant.Id, Brands.GreenPlant.Id, "Emerald Garden Basket" + Markers.DescriptionMarker, "Emerald Garden Basket", 59.99M,  "emerald-garden-basket.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Plant.Id, Brands.GreenPlant.Id, "Sunshine Garden" + Markers.DescriptionMarker, "Sunshine Garden", 47.99M,  "sunshine-garden.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Flower.Id, Brands.RoseCelebration.Id, "One Dozen Rainbow Roses" + Markers.DescriptionMarker, "One Dozen Rainbow Roses", 34.99M,  "one-dozen-rainbow-roses.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Flower.Id, Brands.RoseCelebration.Id, "You're In My Heart" + Markers.DescriptionMarker, "You're In My Heart", 51.99M,  "you-re-in-my-heart.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Flower.Id, Brands.RoseCelebration.Id, "Pink and Pretty" + Markers.DescriptionMarker, "Pink and Pretty", 39.99M,  "pink-and-pretty.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Flower.Id, Brands.PicturesqueOrchid.Id, "Pearly White Orchid Plant" + Markers.DescriptionMarker, "Pearly White Orchid Plant", 43.99M,  "pearly-white-orchid-plant.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Flower.Id, Brands.PicturesqueOrchid.Id, "Wood Twig Mini Duo Orchid" + Markers.DescriptionMarker, "Wood Twig Mini Duo Orchid", 47.99M,  "wood-twig-mini-duo-orchid.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Bouquet.Id, Brands.FloristDesignedBouquet.Id, "Designer's Choice Premium Bouquet" + Markers.DescriptionMarker, "Designer's Choice Premium Bouquet", 107.99M,  "designers-choice-premium-bouquet.jpg".ToUrl()),
                new CatalogItem(CatalogTypes.Bouquet.Id, Brands.FloristDesignedBouquet.Id, "Cupid's Bow Bouquet" + Markers.DescriptionMarker, "Cupid's Bow Bouquet", 111.99M,  "cupids-bow-bouquet.jpg".ToUrl()),
            };

            dbContext.CatalogItems.AddRange(items);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await dbContext.CatalogItems.AnyAsync(catalogItem => catalogItem.Description.Contains(Markers.DescriptionMarker));
        }

        public class Yield : YieldOf<FlowerShopItems>
        {
            public async Task<IReadOnlyCollection<CatalogItem>> GetAllItems() => await Seed.dbContext.CatalogItems.Where(catalogItem => catalogItem.Description.Contains(Markers.DescriptionMarker)).ToArrayAsync();
        }
    }
}

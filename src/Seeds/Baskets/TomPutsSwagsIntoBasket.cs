using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using Seeds.CatalogItems;
using Seeds.CatalogTypes;
using Seeds.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Baskets
{
    public class TomPutsSwagsIntoBasket : ISeed<Basket>
    {
        private static class Markers
        {
            // First item will be order this many times.
            public static int InitialQuantity = 3;
            // Quantitz for every subsequent item will be increased for this amount.
            public static int QuantityIncrement = 5;
        }

        private TomSawyer.Yield TomSawyer { get; set; }
        private SwagShopCatalogTypes.Yield CatalogTypes { get; set; }
        private MicrosoftSwagShopItems.Yield ShopItems { get; set; }

        private readonly IAsyncRepository<Basket> basketRepository;
        private readonly CatalogContext dbContext;

        public TomPutsSwagsIntoBasket(IAsyncRepository<Basket> basketRepository, CatalogContext dbContext)
        {
            this.basketRepository = basketRepository;
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var buyerId = (await TomSawyer.GetTomSawyer()).UserName;
            // Get one item from each catalog type.
            var itemsInTheBasket = (await ShopItems.GetAllItems()).GroupBy(item => item.CatalogTypeId).Select(group => group.First());
            // NSEED-vNEXT: Ideally, we want to check here if we got an item for every catalog type.
            //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
            //              So far we just assume that we got what we expect.

            var basket = new Basket(buyerId);
            var quantity = Markers.InitialQuantity;
            foreach (var item in itemsInTheBasket)
            {
                basket.AddItem(item.Id, item.Price, quantity);
                quantity += Markers.QuantityIncrement;
            }
            await basketRepository.AddAsync(basket);
        }

        // NSEED-NOTE:
        // This is an example of a seed that has a simple Seed() method but rather
        // complex way to determine if the yield of that seed already exist.
        // Ideally we do not want to have that. The HasAlreadyYielded() methods should
        // have simple heuristics.
        public async Task<bool> HasAlreadyYielded()
        {
            var buyerId = (await TomSawyer.GetTomSawyer()).UserName;
            // Find a basket that has exactly that many items as the number of catalog types that have catalog items.
            var catalogTypesWithItemsIds = (await ShopItems.GetAllItems()).Select(item => item.CatalogTypeId).Distinct().ToArray();
            var potentialBasket = await dbContext.Baskets.Include(basket => basket.Items).FirstOrDefaultAsync(basket => basket.BuyerId == buyerId && basket.Items.Count() == catalogTypesWithItemsIds.Length);
            if (potentialBasket is null) return false;

            // Get distinct catalog type ids of the items in the bucket. We have to fetch the types over catalog items.
            var potentialBasketCatalogItemsIds = potentialBasket.Items.Select(item => item.CatalogItemId).ToArray();
            var catalogTypesInTheBasketIds = await dbContext.CatalogItems.Where(catalogItem => potentialBasketCatalogItemsIds.Contains(catalogItem.Id)).Select(item => item.CatalogTypeId).Distinct().ToArrayAsync();

            if (catalogTypesInTheBasketIds.Length != catalogTypesWithItemsIds.Length) return false;

            // So far, we have exactly one item for each of the catalog types.
            // Now we have to check for the quantities.
            var quantity = Markers.InitialQuantity;
            foreach (var item in potentialBasket.Items.OrderBy(item => item.Quantity))
            {
                if (item.Quantity != quantity) return false;
                quantity += Markers.QuantityIncrement;
            }

            return true;
        }

        // NSEED-BEST-PRACTICES:
        // This seed does not have Yield class.
        // If we see that the content of the seed will not be used by other seeds
        // or in tests, we do not have to provide the Yield class.
    }
}

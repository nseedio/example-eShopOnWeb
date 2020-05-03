using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using Seeds.Brands;
using Seeds.CatalogItems;
using Seeds.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Orders
{
    public class ElizabethMakesLotOfOrdersInTheFlowerShop : ISeed<Order>
    {
        private static class Markers
        {
            public static int NumberOfItemsPerOrder = 2;
            public static int QuantityPerItem = 3;
        }

        private ElizabethBennet.Yield ElizabethBennet { get; set; }
        private FlowerBrands.Yield Brands { get; set; }
        private FlowerShopItems.Yield ShopItems { get; set; }

        private readonly IAsyncRepository<Basket> basketRepository;
        private readonly IOrderService orderService;
        private readonly CatalogContext dbContext;

        public ElizabethMakesLotOfOrdersInTheFlowerShop(IAsyncRepository<Basket> basketRepository, IOrderService orderService, CatalogContext dbContext)
        {
            this.basketRepository = basketRepository;
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var buyerId = (await ElizabethBennet.GetElizabethBennet()).UserName;
            var allShopItems = await ShopItems.GetAllItems();

            // Make a separate order for each brand.
            foreach (var brand in Brands.AllBrands)
            {
                // Create temporary basket and fill it with the items.
                var basket = new Basket(buyerId);
                foreach (var item in allShopItems.Where(item => item.CatalogBrandId == brand.Id).Take(Markers.NumberOfItemsPerOrder))
                {
                    basket.AddItem(item.Id, item.Price, Markers.QuantityPerItem);
                }
                basket = await basketRepository.AddAsync(basket);

                await orderService.CreateOrderAsync(basket.Id, ElizabethBennet.GetElizabethsAddress());

                // Delete temporary basket.
                await basketRepository.DeleteAsync(basket);
            }

        }

        public async Task<bool> HasAlreadyYielded()
        {
            var buyerId = (await ElizabethBennet.GetElizabethBennet()).UserName;
            var allShopItems = await ShopItems.GetAllItems();

            foreach (var brand in Brands.AllBrands)
            {
                var shopItemsOfBrandIds = allShopItems.Where(item => item.CatalogBrandId == brand.Id).Select(item => item.Id);
                if (!await dbContext.Orders.AnyAsync(
                        order => order.BuyerId == buyerId &&
                        order.OrderItems.Count == Markers.NumberOfItemsPerOrder &&
                        order.OrderItems.All(item => item.Units == Markers.QuantityPerItem && shopItemsOfBrandIds.Contains(item.ItemOrdered.CatalogItemId))))
                    return false;
            }

            return true;
        }

        // NSEED-BEST-PRACTICES:
        // This seed does not have Yield class.
        // If we see that the content of the seed will not be used by other seeds
        // or in tests, we do not have to provide the Yield class.
    }
}

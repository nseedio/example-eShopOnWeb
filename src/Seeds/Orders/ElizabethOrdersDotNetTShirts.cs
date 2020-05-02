using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using NSeed;
using Seeds.Brands;
using Seeds.CatalogItems;
using Seeds.CatalogTypes;
using Seeds.Users;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Orders
{
    [FriendlyName("Elizabeth orders .NET T-Shirts")]
    public class ElizabethOrdersDotNetTShirts : ISeed<Order>
    {
        private ElizabethBennet.Yield ElizabethBennet { get; set; }
        private MicrosoftBrands.Yield Brands { get; set; }
        private SwagShopCatalogTypes.Yield CatalogTypes { get; set; }
        private MicrosoftSwagShopItems.Yield ShopItems { get; set; }

        private readonly IAsyncRepository<Basket> basketRepository;
        private readonly IOrderService orderService;
        private readonly CatalogContext dbContext;

        public ElizabethOrdersDotNetTShirts(IAsyncRepository<Basket> basketRepository, IOrderService orderService, CatalogContext dbContext)
        {
            this.basketRepository = basketRepository;
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var buyerId = (await ElizabethBennet.GetElizabethBennet()).UserName;
            var dotNetTShirtItems = (await ShopItems.GetAllItems()).Where(item => item.CatalogBrandId == Brands.DotNet.Id && item.CatalogTypeId == CatalogTypes.TShirt.Id);

            // Create temporary basket and fill it with all the items that have Roslyn as a brand.
            var basket = new Basket(buyerId);
            foreach (var item in dotNetTShirtItems)
            {
                basket.AddItem(item.Id, item.Price, 1);
            }
            basket = await basketRepository.AddAsync(basket);

            await orderService.CreateOrderAsync(basket.Id, ElizabethBennet.ElizabethsAddress);

            // Delete temporary basket.
            await basketRepository.DeleteAsync(basket);
        }

        public async Task<bool> HasAlreadyYielded()
        {
            var buyerId = (await ElizabethBennet.GetElizabethBennet()).UserName;
            var dotNetTShirtItemsIds = (await ShopItems.GetAllItems()).Where(item => item.CatalogBrandId == Brands.DotNet.Id && item.CatalogTypeId == CatalogTypes.TShirt.Id).Select(item => item.Id);

            return await dbContext.Orders.AnyAsync(order => order.BuyerId == buyerId && order.OrderItems.All(item => dotNetTShirtItemsIds.Contains(item.ItemOrdered.CatalogItemId)));
        }

        // NSEED-BEST-PRACTICES:
        // This seed does not have Yield class.
        // If we see that the content of the seed will not be used by other seeds
        // or in tests, we do not have to provide the Yield class.
    }
}

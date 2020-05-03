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
    public class TomOrdersRoses : ISeed<Order>
    {
        private TomSawyer.Yield TomSawyer { get; set; }
        private FlowerBrands.Yield Brands { get; set; }
        private FlowerShopCatalogTypes.Yield CatalogTypes { get; set; }
        private FlowerShopItems.Yield ShopItems { get; set; }

        private readonly IAsyncRepository<Basket> basketRepository;
        private readonly IOrderService orderService;
        private readonly CatalogContext dbContext;

        public TomOrdersRoses(IAsyncRepository<Basket> basketRepository, IOrderService orderService, CatalogContext dbContext)
        {
            this.basketRepository = basketRepository;
            this.orderService = orderService;
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            var buyerId = (await TomSawyer.GetTomSawyer()).UserName;
            var rosesItems = (await ShopItems.GetAllItems()).Where(item => item.CatalogBrandId == Brands.RoseCelebration.Id && item.CatalogTypeId == CatalogTypes.Flower.Id);

            // Create temporary basket and fill it with all the items.
            var basket = new Basket(buyerId);
            foreach (var item in rosesItems)
            {
                basket.AddItem(item.Id, item.Price, 1);
            }
            basket = await basketRepository.AddAsync(basket);

            await orderService.CreateOrderAsync(basket.Id, TomSawyer.GetTomsAddress());

            // Delete temporary basket.
            await basketRepository.DeleteAsync(basket);
        }

        public async Task<bool> HasAlreadyYielded()
        {
            var buyerId = (await TomSawyer.GetTomSawyer()).UserName;
            var rosesItemsIds = (await ShopItems.GetAllItems()).Where(item => item.CatalogBrandId == Brands.RoseCelebration.Id && item.CatalogTypeId == CatalogTypes.Flower.Id).Select(item => item.Id);

            return await dbContext.Orders.AnyAsync(order => order.BuyerId == buyerId && order.OrderItems.All(item => rosesItemsIds.Contains(item.ItemOrdered.CatalogItemId)));
        }

        // NSEED-BEST-PRACTICES:
        // This seed does not have Yield class.
        // If we see that the content of the seed will not be used by other seeds
        // or in tests, we do not have to provide the Yield class.
    }
}

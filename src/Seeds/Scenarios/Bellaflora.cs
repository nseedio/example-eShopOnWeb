using NSeed;
using Seeds.Orders;

namespace Seeds.Scenarios
{
    [Requires(typeof(TomOrdersRoses))]
    [Requires(typeof(ElizabethMakesLotOfOrdersInTheFlowerShop))]
    [Description("A successful flower shop with lots of orders.")]
    public class Bellaflora : IScenario
    {
    }
}

using NSeed;
using Seeds.Orders;

namespace Seeds.Scenarios
{
    [Requires(typeof(TomOrdersDotNetMugs))]
    [Requires(typeof(TomOrdersRoslynSwags))]
    [Requires(typeof(ElizabethOrdersDotNetTShirts))]
    [FriendlyName("Successful Microsoft swag shop")]
    [Description("Microsoft swag shop that has lots of orders.")]
    public class SuccessfulMicrosoftSwagShop : IScenario
    {
    }
}

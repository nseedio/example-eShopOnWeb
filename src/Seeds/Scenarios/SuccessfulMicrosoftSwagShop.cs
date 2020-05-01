using NSeed;
using Seeds.Users;

namespace Seeds.Scenarios
{
    [Requires(typeof(TomOrdersDotNetMugs))]
    [Requires(typeof(TomOrdersRoslynSwags))]
    [Requires(typeof(ElizabethOrdersDotNetTShirts))]
    [FriendlyName("Successful Microsoft swag shop")]
    [Description("Microsoft swag shop that has a lot of orders.")]
    public class SuccessfulMicrosoftSwagShop : IScenario
    {
    }
}

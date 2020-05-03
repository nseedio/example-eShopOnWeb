using NSeed;

namespace Seeds.Scenarios
{
    // NSEED-FEATURE:
    // A scenario can require other scenarios and not only on seeds.
    // In general, a scenario can require seeds and other scenarios.
    [Requires(typeof(SuccessfulMicrosoftSwagShop))]
    [Requires(typeof(Bellaflora))]
    [Description("A successful marketplace that sells various items and has lots of orders.")]
    public class Marketplace : IScenario
    {
    }
}

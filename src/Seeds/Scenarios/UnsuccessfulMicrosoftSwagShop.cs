﻿using NSeed;
using Seeds.CatalogTypes;

namespace Seeds.Scenarios
{
    [Requires(typeof(MicrosoftSwagShopItems))]
    [FriendlyName("Unsuccessful Microsoft swag shop")]
    [Description("Microsoft swag shop that does not have any orders.")]
    public class UnsuccessfulMicrosoftSwagShop : IScenario
    {
    }
}

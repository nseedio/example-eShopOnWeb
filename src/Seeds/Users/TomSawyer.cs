using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Infrastructure.Identity;
using NSeed;
using System.Threading.Tasks;

namespace Seeds.Users
{
    // NSEED-vNEXT: At the moment ElizabethBennet and TomSawyer seeds differ only in the user name.
    //              Thus, we have a lot of code duplication here.
    //              Soon NSeed will support so called seed variants. With seed variants it will be
    //              possible to define parameterizable seeds and their concrete variants.
    //              In this case, we will have a NamedUser seed with two variants, ElizabethBennet and TomSawyer.
    [FriendlyName("Tom Sawyer")]
    public class TomSawyer : ISeed<ApplicationUser>
    {
        private static class Markers
        {
            public static string UserName = "tom.sawyer";
        }

        private readonly UserManager<ApplicationUser> userManager;

        public TomSawyer(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            var user = new ApplicationUser { UserName = Markers.UserName, Email = Markers.UserName + "@google.com"};
            var identityResult = await userManager.CreateAsync(user, UserConstants.Password);
            // NSEED-vNEXT: Ideally, we want to check here if the identity result is successful and stop seeding if it is not.
            //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
            //              So far we just assume everything went well.
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await userManager.FindByNameAsync(Markers.UserName) != null;
        }

        public class Yield : YieldOf<TomSawyer>
        {
            // NSEED-NOTE:
            // We could have a property here and create the address object only once.
            // When this object is used in other seeds e.g to create orders, it would work
            // so long as we do not try to use the same object to e.g. create several orders.
            // In that case we would get an Entity Framework Core exception with the following
            // message:
            // "The property 'OrderId' on entity type 'Address' is part of a key and so cannot be modified or marked as modified.
            //  To change the principal of an existing entity with an identifying foreign key first delete the dependent and invoke 'SaveChanges' then associate the dependent with the new principal."
            // When providing objects in yields that are used in ORM frameworks like Entity Frameworks
            // we sometime have to think about their lifecycle and ORM tracking.
            // Luckily, this is usually easy to do :-) 
            public Address GetTomsAddress() => new Address("123 Main St.", "St. Petersburg", "MO", "United States", "44240");

            public async Task<ApplicationUser> GetTomSawyer() => await Seed.userManager.FindByNameAsync(Markers.UserName);
        }
    }
}

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
    [FriendlyName("Elizabeth Bennet")]
    public class ElizabethBennet : ISeed<ApplicationUser>
    {
        private static class Markers
        {
            public static string UserName = "elizabeth.bennet";
        }

        private readonly UserManager<ApplicationUser> userManager;

        public ElizabethBennet(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            var user = new ApplicationUser { UserName = Markers.UserName, Email = Markers.UserName + "@gmail.com"};
            var identityResult = await userManager.CreateAsync(user, UserConstants.Password);
            // NSEED-vNEXT: Ideally, we want to check here if the identity result is successful and stop seeding if it is not.
            //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
            //              So far we just assume everything went well.
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await userManager.FindByNameAsync(Markers.UserName) != null;
        }

        public class Yield : YieldOf<ElizabethBennet>
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
            public Address GetElizabethsAddress() => new Address("12 Longbourn Street", "Longbourn", "Hertfordshire", "United Kingdom", "12345");

            public async Task<ApplicationUser> GetElizabethBennet() => await Seed.userManager.FindByNameAsync(Markers.UserName);
        }
    }
}

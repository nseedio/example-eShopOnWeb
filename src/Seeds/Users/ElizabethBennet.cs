using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.Infrastructure.Identity;
using NSeed;
using System.Threading.Tasks;

namespace Seeds.Users
{
    [FriendlyName("Elizabeth Bennet")]
    public class ElizabethBennet : ISeed<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ElizabethBennet(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            var user = new ApplicationUser { UserName = Yield.UserName, Email = Yield.UserName + "@google.com"};
            var identityResult = await userManager.CreateAsync(user, UserConstants.Password);
            // NSEED-vNEXT: Ideally, we want to check here if the identity result is successful and stop seeding if it is not.
            //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
            //              So far we just assume everything went well.
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await userManager.FindByNameAsync(Yield.UserName) != null;
        }

        public class Yield : YieldOf<ElizabethBennet>
        {
            public static string UserName = "elizabeth.bennet";

            public async Task<ApplicationUser> GetElizabethBennet() => await Seed.userManager.FindByNameAsync(UserName);
        }
    }
}

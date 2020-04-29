using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Constants;
using Microsoft.eShopWeb.Infrastructure.Identity;
using NSeed;
using System.Threading.Tasks;

namespace Seeds.Users
{
    [AlwaysRequired]
    public class Administrator : ISeed<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public Administrator(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Seed()
        {
            await roleManager.CreateAsync(new IdentityRole(AuthorizationConstants.Roles.ADMINISTRATORS));

            var adminUser = new ApplicationUser { UserName = Yield.AdministratorUserName, Email = Yield.AdministratorUserName + "@eshoponweb.com"};
            var identityResult = await userManager.CreateAsync(adminUser, UserConstants.Password);
            // NSEED-vNEXT: Ideally, we want to check here if the identity result is successful and stop seeding if it is not.
            //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
            //              So far we just assume everything went well.
            adminUser = await userManager.FindByNameAsync(Yield.AdministratorUserName);
            await userManager.AddToRoleAsync(adminUser, AuthorizationConstants.Roles.ADMINISTRATORS);
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await userManager.FindByNameAsync(Yield.AdministratorUserName) != null;
        }

        public class Yield : YieldOf<Administrator>
        {
            public static string AdministratorUserName = "admin";

            public async Task<ApplicationUser> GetAdministrator() => await Seed.userManager.FindByNameAsync(AdministratorUserName);
        }
    }
}

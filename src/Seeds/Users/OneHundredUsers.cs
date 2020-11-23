using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Infrastructure.Identity;
using NSeed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeds.Users
{
    // NSEED-NOTE: NSeed perfectly plays with existing data generation libraries. This example shows how to use Bogus within NSeed seeds: https://github.com/bchavez/Bogus

    // NSEED-FEATURE: The [FriendlyName] attribute can be omitted. NSeed will automatically create human-readable verion of the seed class name.
    public class OneHundredUsers : ISeed<ApplicationUser>
    {
        private static class Markers
        {
            public static int NumberOfUsers = 100;
            public static string EmailPrefix = ".ohu";
        }

        private readonly UserManager<ApplicationUser> userManager;
        private readonly AppIdentityDbContext appIdentityDbContext;

        public OneHundredUsers(UserManager<ApplicationUser> userManager, AppIdentityDbContext appIdentityDbContext)
        {
            this.userManager = userManager;
            this.appIdentityDbContext = appIdentityDbContext;
        }

        public async Task Seed()
        {
            // NSEED-BEST-PRACTICES: When using Bogus, always set the Randomizer.Seed to a fix value, in order to get exactly the same values generated each time.
            //                       Seeds must be deterministic and this is the way of achieving determinism with Bogus.
            Randomizer.Seed = new Random(0);

            var users = new Faker<ApplicationUser>()
                .RuleFor(x => x.UserName, f => f.Internet.UserName())
                .RuleFor(x => x.Email, (f, user) => f.Internet.Email(user.UserName).Replace("@", Markers.EmailPrefix + "@"))
                .Generate(Markers.NumberOfUsers);

            foreach (var user in users)
            {
                var identityResult = await userManager.CreateAsync(user, UserConstants.Password);
                // NSEED-vNEXT: Ideally, we want to check here if the identity result is successful and stop seeding if it is not.
                //              In the upcoming versions NSeed will have built-in support for asserting such expectations.
                //              So far we just assume everything went well.
            }
        }

        public async Task<bool> HasAlreadyYielded()
        {
            return await appIdentityDbContext.Users.CountAsync(user => user.Email.Contains(Markers.EmailPrefix + "@")) >= Markers.NumberOfUsers;
        }

        public class Yield : YieldOf<OneHundredUsers>
        {
            public async Task<IReadOnlyCollection<ApplicationUser>> GetOneHundredUsers() => await Seed.appIdentityDbContext.Users.Where(user => user.Email.Contains(Markers.EmailPrefix + "@")).ToListAsync();
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Infrastructure.Identity;
using Microsoft.eShopWeb.Infrastructure.Logging;
using Microsoft.eShopWeb.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSeed;

namespace Seeds
{
    public class Startup : SeedBucketStartup
    {
        protected override void ConfigureServices(IServiceCollection services)
        {
            // NSEED-vNEXT: At the moment SeedBucketStartup does not support configuring of configuration.
            //              That's why we temporary hardcode the connection strings here.
            services.AddDbContext<CatalogContext>(c =>
                c.UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.CatalogDb;"));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Integrated Security=true;Initial Catalog=NSeed.Microsoft.eShopOnWeb.Identity;"));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                       .AddEntityFrameworkStores<AppIdentityDbContext>()
                       .AddDefaultTokenProviders();

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            // NSEED-vNEXT: As above, at the moment we hardcode the CatalogBaseUrl because the configuring of configuration is not implemented.
            services.AddSingleton<IUriComposer>(new UriComposer(new CatalogSettings { CatalogBaseUrl = string.Empty }));
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddLogging(config => config.ClearProviders());
        }
    }
}

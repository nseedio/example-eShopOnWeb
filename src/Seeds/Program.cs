using NSeed;
using System.Threading.Tasks;

namespace DotNetCoreSeeds
{
#pragma warning disable SA1649 // File name should match first type name
    public class EShopOnWebSeedBucket : SeedBucket
#pragma warning restore SA1649 // File name should match first type name
    {
        internal static async Task<int> Main(string[] args)
            => await Handle<EShopOnWebSeedBucket>(args);
    }
}

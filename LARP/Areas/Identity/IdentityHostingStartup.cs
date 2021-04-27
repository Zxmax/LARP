using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(LARP.Areas.Identity.IdentityHostingStartup))]
namespace LARP.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
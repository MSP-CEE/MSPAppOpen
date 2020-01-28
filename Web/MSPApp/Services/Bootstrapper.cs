using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MSPApp.Services
{
    public static class Bootstrapper
    {
        public static void AddGraphService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<WebOptions>(configuration);
        }
    }
}

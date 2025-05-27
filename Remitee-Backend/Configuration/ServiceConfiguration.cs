using Remitee_Backend.Core.Services;
using Remitee_Backend.Core.Services.Interfaces;

namespace Remitee_Backend.Configuration
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            return services;
        }
    }
}

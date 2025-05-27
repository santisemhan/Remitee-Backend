using Microsoft.EntityFrameworkCore;
using Remitee_Backend.Core.Repositories;
using Remitee_Backend.Core.Repositories.Interfaces;
using Remitee_Backend.Data;

namespace Remitee_Backend.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }

        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBookRepository, BookRepository>();
            return services;
        }
    }
}

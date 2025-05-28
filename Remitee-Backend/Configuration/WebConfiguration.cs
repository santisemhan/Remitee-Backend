namespace Remitee_Backend.Configuration
{  
    public static class WebConfiguration
    {
        public static IServiceCollection AddWebSecurityConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("_AllowOriginDev", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
                options.AddPolicy("_AllowOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}

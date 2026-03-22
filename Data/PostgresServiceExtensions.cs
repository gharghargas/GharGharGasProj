using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vore.Data
{
    public static class PostgresServiceExtensions
    {
        public static IServiceCollection AddPostgresOptions(this IServiceCollection services, IConfiguration config)
        {
            var section = config.GetSection("Postgres");
            services.Configure<PostgresOptions>(section);
            // bind and register a singleton for convenience
            var opts = section.Get<PostgresOptions>();
            services.AddSingleton(opts);
            return services;
        }
    }
}

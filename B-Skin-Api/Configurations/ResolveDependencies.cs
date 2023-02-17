using B_Skin_Api.Data.Dapper;
using B_Skin_Api.Data.Repositories;
using B_Skin_Api.Data.UnitOfWork;
using B_Skin_Api.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace B_Skin_Api.Web.Configurations
{
    public static class ResolveDependencies
    {
        public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<DbSession>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITShirtRepository, TShirtsRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();

            return services;
        }
    }
}

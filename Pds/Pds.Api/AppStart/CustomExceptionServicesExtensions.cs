using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pds.Api.Logging.ExceptionCreators;
using Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories;

namespace Pds.Api.AppStart
{
    public static class CustomExceptionServicesExtensions
    {
        public static void AddCustomExceptionServices(this IServiceCollection services, IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
                services.AddSingleton<ShowFullExceptionResponseCreator>();
            else
                services.AddSingleton<HideExceptionResponseCreator>();

            services
                .AddSingleton<IExceptionResponseCreatorsFactory,
                    ServiceProviderBasedExceptionResponseCreatorsFactory>();
        }
    }
}
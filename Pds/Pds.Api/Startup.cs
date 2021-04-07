using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pds.Api.ActionFilters;
using Pds.Api.AppStart;
using Pds.Api.Logging.ExceptionCreators;
using Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories;
using Pds.Di;

namespace Pds.Api
{
    public class Startup
    {
        private readonly IHostEnvironment environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            this.environment = environment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomAuth0Authentication(Configuration);
            services.AddCustomPdsCorsPolicy(Configuration);
            services.AddControllers(options => options.Filters.Add<CustomResponseExceptionFilter>());
            services.AddCustomSwagger();
            services.AddCustomSqlContext(Configuration);
            services.AddCustomAutoMapper();
            services.AddCustomExceptionServices(environment);
        }

        // Do not delete, this is initialization of DI
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApiDiModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseCustomSwaggerUI();

            app.UseCustomPdsCorsPolicy();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
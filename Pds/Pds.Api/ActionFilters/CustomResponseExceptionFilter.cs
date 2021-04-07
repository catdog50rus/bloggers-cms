using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories;

namespace Pds.Api.ActionFilters
{
    public class CustomResponseExceptionFilter : IAsyncExceptionFilter
    {
        private readonly IExceptionResponseCreatorsFactory factory;
        private readonly IServiceProvider serviceProvider;

        public CustomResponseExceptionFilter(IExceptionResponseCreatorsFactory factory,
            IServiceProvider serviceProvider)
        {
            this.factory = factory;
            this.serviceProvider = serviceProvider;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var exceptionType = exception.GetType();
            var firstOrDefault = factory.GetTargetCreator(exceptionType);
            if (firstOrDefault is null)
                return;
            var actionResult = await firstOrDefault.GetExceptionResultAsync(exception, serviceProvider);
            if (actionResult == default)
                return;

            context.Result = actionResult;
            context.ExceptionHandled = true;
        }
    }
}
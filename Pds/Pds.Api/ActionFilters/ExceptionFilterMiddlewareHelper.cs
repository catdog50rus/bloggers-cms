using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pds.Api.ActionFilters
{
    public abstract class ExceptionResponseCreator
    {
        public abstract Type TargetExceptionType { get; }
        public abstract
            Task<IActionResult>
            GetExceptionResultAsync(Exception exception,
                IServiceProvider provider);
    }

    public class ExceptionFilterMiddlewareHelper : IAsyncExceptionFilter
    {
        private readonly IExceptionResponseCreatorsFactory factory;
        private readonly IServiceProvider serviceProvider;

        public ExceptionFilterMiddlewareHelper(IExceptionResponseCreatorsFactory factory, IServiceProvider serviceProvider)
        {
            this.factory = factory;
            this.serviceProvider = serviceProvider;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var exceptionType = exception.GetType();
            var exceptionResponseCreators = factory.GetCreators();
            var exceptionResponseCreator = exceptionResponseCreators
                                               .FirstOrDefault(c => exceptionType == c.TargetExceptionType) ??
                                           exceptionResponseCreators.FirstOrDefault(c =>
                                               exceptionType.IsSubclassOf(c.TargetExceptionType));
            if (exceptionResponseCreator is null)
                return;
            var actionResult = await exceptionResponseCreator?.GetExceptionResultAsync(exception, serviceProvider);
            if (actionResult == default)
                return;
            
            context.Result = actionResult;
            context.ExceptionHandled = true;
        }
    }

    public interface IExceptionResponseCreatorsFactory
    {
        IReadOnlyCollection<ExceptionResponseCreator> GetCreators();
    }

    public class TestExceptionResponseCreatorsFactory : IExceptionResponseCreatorsFactory
    {
        private readonly IWebHostEnvironment environment;
        private readonly List<ExceptionResponseCreator> creators;

        public TestExceptionResponseCreatorsFactory(IWebHostEnvironment environment)
        {
            this.environment = environment;
            creators = new List<ExceptionResponseCreator>();
            if (environment.IsDevelopment())
            {
                creators.Add(new FullAnyExceptionResponseCreator());
            }
        }

        public IReadOnlyCollection<ExceptionResponseCreator> GetCreators()
        {
            return creators;
        }

        public class FullAnyExceptionResponseCreator : ExceptionResponseCreator
        {
            public override Type TargetExceptionType => typeof(Exception);

            public class ExceptionResonse
            {
                public ExceptionResonse(Exception exception)
                {
                    Exception = exception.ToString();
                    Message = exception.Message;
                    StackTrace = exception.StackTrace;
                    if (exception.InnerException is not null)
                        InnerException = new ExceptionResonse(exception.InnerException);
                }
                
                public string Exception { get; set; }
                public string Message { get; set; }
                public string StackTrace { get; set; }
                public ExceptionResonse InnerException { get; set; }
            }

            public override Task<IActionResult> GetExceptionResultAsync(Exception exception,
                IServiceProvider provider)
            {
                return Task.FromResult<IActionResult>(new ObjectResult((object) new ExceptionResonse(exception))
                {
                    StatusCode = 500
                });
            }
        }
    }
}
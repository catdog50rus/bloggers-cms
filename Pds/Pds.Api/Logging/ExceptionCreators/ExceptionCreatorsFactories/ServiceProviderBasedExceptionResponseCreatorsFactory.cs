using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories
{
    public class ServiceProviderBasedExceptionResponseCreatorsFactory : IExceptionResponseCreatorsFactory
    {
        private readonly List<IExceptionResponseCreator> creators;

        public ServiceProviderBasedExceptionResponseCreatorsFactory(IServiceProvider provider)
        {
            creators = provider.GetServices(typeof(IExceptionResponseCreator)).Cast<IExceptionResponseCreator>()
                .ToList();
        }

        public IEnumerable<IExceptionResponseCreator> GetCreators()
        {
            return creators;
        }

        public IExceptionResponseCreator GetTargetCreator(Type exceptionType)
        {
            return creators.FirstOrDefault(c => exceptionType == c.TargetExceptionType) ??
                   creators.FirstOrDefault(c =>
                       exceptionType.IsSubclassOf(c.TargetExceptionType));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories
{
    public class TestExceptionResponseCreatorsFactory : IExceptionResponseCreatorsFactory
    {
        private readonly List<IExceptionResponseCreator> creators;

        public TestExceptionResponseCreatorsFactory(IHostEnvironment environment)
        {
            creators = new List<IExceptionResponseCreator>();
            if (environment.IsDevelopment())
                creators.Add(new ShowFullExceptionResponseCreator());
            else
                creators.Add(new HideExceptionResponseCreator());
        }

        public IEnumerable<IExceptionResponseCreator> GetCreators()
        {
            return creators;
        }

        /// <summary>
        ///     Finds and returns <see cref="ExceptionResponseCreator{T}" />
        /// </summary>
        /// <param name="exceptionType">Type of target exception</param>
        /// <returns><see cref="IExceptionResponseCreator" /> of target exception type or null if not found</returns>
        public IExceptionResponseCreator GetTargetCreator(Type exceptionType)
        {
            return creators.FirstOrDefault(c => exceptionType == c.TargetExceptionType) ??
                   creators.FirstOrDefault(c =>
                       exceptionType.IsSubclassOf(c.TargetExceptionType));
        }
    }
}
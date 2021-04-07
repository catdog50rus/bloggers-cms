using System;
using System.Collections.Generic;

namespace Pds.Api.Logging.ExceptionCreators.ExceptionCreatorsFactories
{
    public interface IExceptionResponseCreatorsFactory
    {
        /// <summary>
        ///     Returns collection of all creators registered in factory
        /// </summary>
        /// <returns></returns>
        IEnumerable<IExceptionResponseCreator> GetCreators();

        /// <summary>
        ///     Finds and returns <see cref="ExceptionResponseCreator{T}" />
        /// </summary>
        /// <param name="exceptionType">Type of target exception</param>
        /// <returns><see cref="IExceptionResponseCreator" /> of target exception type or null if not found</returns>
        IExceptionResponseCreator GetTargetCreator(Type exceptionType);
    }
}
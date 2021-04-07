using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pds.Api.Logging.ExceptionCreators
{
    /// <summary>
    ///     Base class for all <see cref="IExceptionResponseCreator" />
    /// </summary>
    /// <typeparam name="T">Type of target Exception</typeparam>
    public abstract class ExceptionResponseCreator<T> : IExceptionResponseCreator where T : Exception
    {
        public Type TargetExceptionType { get; } = typeof(T);

        public Task<IActionResult> GetExceptionResultAsync(Exception exception, IServiceProvider provider)
        {
            var type = exception.GetType();
            if (!type.IsSubclassOf(TargetExceptionType))
                throw new ArgumentException("Invalid type of passed exception");
            return GetExceptionResultInternalAsync((T) exception, provider);
        }

        /// <summary>
        ///     Typed version of <see cref="GetExceptionResultAsync" />
        /// </summary>
        /// <param name="exception">Exception of <see cref="TargetExceptionType" /> or subclass</param>
        /// <param name="provider">
        ///     ServiceProvider for resolving dependencies  (for example: for resolve
        ///     <see cref="Microsoft.Extensions.Logging.ILogger" />)
        /// </param>
        /// <returns>
        ///     <see cref="IActionResult" />
        /// </returns>
        protected abstract Task<IActionResult> GetExceptionResultInternalAsync(T exception,
            IServiceProvider provider);
    }
}
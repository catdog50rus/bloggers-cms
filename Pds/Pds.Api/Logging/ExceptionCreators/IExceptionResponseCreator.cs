using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pds.Api.Logging.ExceptionCreators
{
    /// <summary>
    ///     Base interface for all ExceptionResponseCreators
    /// </summary>
    public interface IExceptionResponseCreator
    {
        /// <summary>
        ///     Represents watched type of Exceptions that need to transform to <see cref="IActionResult" /> by
        ///     <see cref="GetExceptionResultAsync" />
        /// </summary>
        Type TargetExceptionType { get; }

        /// <summary>
        ///     Transforms <see cref="TargetExceptionType" /> object that is subclass of <see cref="Exception" /> to
        ///     <see cref="IActionResult" />
        /// </summary>
        /// <param name="exception">Exception of <see cref="TargetExceptionType" /> or subclass</param>
        /// <param name="provider">
        ///     ServiceProvider for resolving dependencies  (for example: for resolve
        ///     <see cref="Microsoft.Extensions.Logging.ILogger" />)
        /// </param>
        /// <returns>
        ///     <see cref="IActionResult" />
        /// </returns>
        Task<IActionResult> GetExceptionResultAsync(Exception exception,
            IServiceProvider provider);
    }
}
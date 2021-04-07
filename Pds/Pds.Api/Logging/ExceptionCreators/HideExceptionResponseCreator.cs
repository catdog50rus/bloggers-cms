using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Pds.Api.Logging.ExceptionCreators
{
    /// <summary>
    ///     Exception creator that hide occupied exception by default <see cref="StatusCodes.Status500InternalServerError" />
    ///     exception response
    /// </summary>
    public class HideExceptionResponseCreator : ExceptionResponseCreator<Exception>
    {
        protected override Task<IActionResult> GetExceptionResultInternalAsync(Exception exception,
            IServiceProvider provider)
        {
            return Task.FromResult<IActionResult>(new StatusCodeResult(StatusCodes.Status500InternalServerError));
        }
    }
}
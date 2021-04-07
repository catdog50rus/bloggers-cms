using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pds.Api.Contracts.Exceptions;

namespace Pds.Api.Logging.ExceptionCreators
{
    /// <summary>
    ///     Exception creator that returns important information about occupied exception includes internal exceptions to
    ///     response
    /// </summary>
    public class ShowFullExceptionResponseCreator : ExceptionResponseCreator<Exception>
    {
        protected override Task<IActionResult> GetExceptionResultInternalAsync(Exception exception,
            IServiceProvider provider)
        {
            return Task.FromResult<IActionResult>(new ObjectResult(new ExceptionResponse(exception))
            {
                StatusCode = 500
            });
        }
    }
}
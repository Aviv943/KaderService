using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

namespace KaderService.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public async Task<ActionResult<ErrorResponse>> ReturnErrorAsync()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Exception exception = context.Error;
            
            var errorResponse = new ErrorResponse
            {
                Error = exception.Message
            };

            return exception switch
            {
                KeyNotFoundException => NotFound(errorResponse),
                UnauthorizedAccessException => Unauthorized(errorResponse),
                _ => BadRequest(errorResponse)
            };
        }
    }
}

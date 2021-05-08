using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;
using KaderService.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;

namespace KaderService.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        private readonly ILoggerManager _logger;

        public ErrorsController(ILoggerManager logger)
        {
            _logger = logger;
        }

        [Route("error")]
        public async Task<ActionResult<ErrorResponse>> Response(int errorCode, string internalCode, string message)
        {
            var errorResponse = new ErrorResponse
            {
                InternalCode = internalCode,
                Message = message
            };

            _logger.LogError($"[{internalCode}] Exception: {message}");
            
            return errorCode switch
            {
                404 => NotFound(errorResponse),
                401 => Unauthorized(errorResponse),
                _ => BadRequest(errorResponse)
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using KaderService.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KaderService.Extensions
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int errorCode = exception switch
            {
                KeyNotFoundException => 404,
                UnauthorizedAccessException => 401,
                _ => 400
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            string internalErrorCode = Guid.NewGuid().ToString().Split('-').Last();
            context.Response.Redirect($"/error?errorCode={errorCode}&internalCode={internalErrorCode}&message={exception.Message}");

            return Task.CompletedTask;
        }
    }
}

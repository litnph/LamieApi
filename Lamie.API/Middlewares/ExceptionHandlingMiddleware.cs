using Lamie.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace Lamie.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BaseException ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    ConflictException => StatusCodes.Status409Conflict,
                    UnauthorizedException => StatusCodes.Status401Unauthorized,
                    ForbiddenException => StatusCodes.Status403Forbidden,
                    _ => StatusCodes.Status400BadRequest
                };

                var response = new
                {
                    success = false,
                    code = ex.Code,
                    message = ex.Message,
                    errors = ex is ValidationException ve ? ve.Errors : null
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    throw;
                }

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    code = "INTERNAL_SERVER_ERROR",
                    message = "Internal server error"
                    #if DEBUG
                    ,
                    detail = ex.Message
                    #endif
                });
            }
        }
    }
}

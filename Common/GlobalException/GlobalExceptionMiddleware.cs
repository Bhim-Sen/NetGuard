using Common.GlobalException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.GlobalException
{
	public class GlobalExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<GlobalExceptionMiddleware> _logger;
		public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
		{
			_next = next;
			_logger = logger;
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
		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{

			context.Response.ContentType = "application/json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;
			// Log the exception 
			_logger.LogError(exception, "An unhandled exception occurred.");
			await context.Response.WriteAsync(new
			{
				StatusCode = context.Response.StatusCode,
				Message = exception
			}.ToString());
		}
	}
}
public static class ExceptionHandlerMiddlewareExtensions
{
	public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<GlobalExceptionMiddleware>();
	}
}
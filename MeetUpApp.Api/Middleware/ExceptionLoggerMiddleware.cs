using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MeetUpApp.Api.Middleware
{
    public class ExceptionLoggerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionLoggerMiddleware> logger;

        public ExceptionLoggerMiddleware(
            RequestDelegate next,
            ILogger<ExceptionLoggerMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await WriteExceptionMessage(context.Response, ex);
                LogException(ex);
            }
        }

        private static async Task WriteExceptionMessage(
            HttpResponse respose,
            Exception ex)
        {
            respose.StatusCode =
                ex is ArgumentException or DbException or DbUpdateException
                    ? StatusCodes.Status400BadRequest
                    : StatusCodes.Status500InternalServerError;

            //await respose.WriteAsJsonAsync(new MessageModel
            //{
            //    Message = $"Error: {ex.Message}"
            //});
            await Task.CompletedTask;
        }

        private void LogException(Exception ex)
        {
            logger.LogWarning("{Exception}: {Message}, Source: {Source}",
                ex.GetType().Name, ex.Message, ex.Source);
        }
    }
}

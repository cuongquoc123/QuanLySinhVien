using QuanLySinhVien.DTOS;

namespace QuanLySinhVien.MidWare.Filter
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
            _logger.LogInformation("GlobalExceptionMiddleware running");
            try
            {
                await _next(context);
            }
            catch (CustomError ex)
            {
                _logger.LogWarning($"Message: {ex.Message}");

                context.Response.StatusCode = ex.status;
                context.Response.ContentType = "application/json";

                var response = new ErrorRespone
                {
                    status = ex.status,
                    Error = ex.Error,
                    message = ex.Message,
                    path = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new ErrorRespone
                {
                    status = 500,
                    Error = "Internal Server",
                    message = ex.Message,
                    path = context.Request.Path
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }

}
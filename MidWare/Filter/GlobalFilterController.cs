using System.Data.SqlTypes;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuanLySinhVien.DTOS;

namespace QuanLySinhVien.MidWare.Filter
{
    public class GlobaleFilter : IExceptionFilter
    {
        private readonly ILogger<GlobaleFilter> logger;

        public GlobaleFilter(ILogger<GlobaleFilter> logger)
        {
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            logger.LogWarning("Filter running");
            ErrorRespone respone = new ErrorRespone();

            respone.path = context.HttpContext.Request.Path;

            respone.message = context.Exception.Message;

            string Error;
            int status;
            if (context.Exception is ArgumentException)
            {
                logger.LogError(respone.message);
                Error = "Bad Request";
                status = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception is KeyNotFoundException)
            {   //Lỗi NotFound
                logger.LogWarning(respone.message);
                Error = "Not Found";
                status = (int)HttpStatusCode.NotFound;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {   //Lỗi xác thực 
                logger.LogWarning(respone.message);
                status = (int)HttpStatusCode.Unauthorized;
                Error = "UnAuthorized";
            }
            else if (context.Exception is CustomError e)
            {
                logger.LogError(respone.message);
                status = e.status;
                Error = e.Error;

            }
            else if (context.Exception is InvalidOperationException exception)
            {
                logger.LogError(exception.Message);
                status = (int)HttpStatusCode.Conflict;
                Error = "Invalid Operation Exception";
            }
            //các lỗi khác 
            else
            {
                logger.LogError(respone.message);
                Error = "Internal Server";
                status = (int)HttpStatusCode.InternalServerError;
            }

            respone.Error = Error;
            respone.status = status;

            context.Result = new JsonResult(respone)
            {
                StatusCode = status
            };

            context.ExceptionHandled = true;
        }
    }
}
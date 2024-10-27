using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SDD.Services.ProductAPI.Filters
{
    public class CustomAuthorizationFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                var result = new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "You are forbidden from accessing this resource."
                };

                context.Result = new JsonResult(result)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };

                context.ExceptionHandled = true;
            }
        }
    }
}

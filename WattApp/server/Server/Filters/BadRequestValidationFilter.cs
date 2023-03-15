using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Server.Filters
{
    public class BadRequestValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                Console.WriteLine("Invalid");
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
        }
        public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Server.Filters
{
    /// <summary>
    /// Returns default Bad request if request is not valid
    /// </summary>
    public class BadRequestValidationFilter : ActionFilterAttribute
    {
        /// <inheritdoc/>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
        }
        /// <inheritdoc/>
        public override void OnActionExecuted(ActionExecutedContext context) { }
    }
}

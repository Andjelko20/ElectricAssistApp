using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Server.Filters
{
    /// <summary>
    /// Auth response filter for Swagger
    /// </summary>
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
              !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute))
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement 
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference 
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearer"
                                }
                            }, 
                            new List<string> {}
                        }
                    }
                };
            }

        }
    }
}

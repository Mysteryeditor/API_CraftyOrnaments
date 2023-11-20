using Microsoft.AspNetCore.Mvc.Filters;

namespace API_CraftyOrnaments.Attributes
{
    //[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]

    public class ApiKeyAttribute : System.Attribute, IAsyncActionFilter
    {
        private const string _apiKey = "KwDwvTqLxLyqDzRm";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Query.TryGetValue(_apiKey, out var extractedApiKey))
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("No API key was provided.");
                return;
            }

            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            var apiKey = configuration.GetValue<string>(_apiKey);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Invalid API key.");
                return;
            }

            await next();
        }
    }
}

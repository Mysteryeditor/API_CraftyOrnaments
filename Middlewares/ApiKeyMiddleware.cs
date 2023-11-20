namespace API_CraftyOrnaments.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _apiKey = "x-api-key";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/secure"))
            {
                // Perform API key validation logic
                if (!context.Request.Query.TryGetValue(_apiKey, out var extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("No API key was provided.");
                    return;
                }

                var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

                var apiKey = configuration.GetValue<string>(_apiKey);

                if (!apiKey.Equals(extractedApiKey))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid API key.");
                    return;
                }
            }

            await _next(context);
        }
    }
}

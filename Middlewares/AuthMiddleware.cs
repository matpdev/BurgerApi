using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BurgerApi.Utils.Chipher;

namespace BurgerApi.Middlewares
{
    public class AuthMiddleware(RequestDelegate next)
    {
        readonly List<string> paths = ["/posts/add"];
        private readonly RequestDelegate _next = next;

        // readonly ConfigurationVariablesController configurationVariables = new();
        readonly JwtComponent jwtService = new();

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string auth = httpContext
                .Request.Headers.Authorization.ToString()
                .Replace("Bearer ", "");

            Dictionary<string, string> jwtData = jwtService.DecryptData(auth);

            // Console.WriteLine(jwtData["userId"]);

            httpContext.Items["userId"] = jwtData["userId"];

            await _next(httpContext);
        }
    }
}

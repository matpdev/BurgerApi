using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BurgerApi.Data;
using BurgerApi.Utils.Chipher;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BurgerApi.Configures
{
    public class AuthorizationAttribute : TypeFilterAttribute
    {
        public AuthorizationAttribute()
            : base(typeof(AuthorizationFilter)) { }

        private class AuthorizationFilter(BurgerContext myDbContext) : ActionFilterAttribute
        {
            readonly JwtComponent jwtService = new();

            private readonly BurgerContext db = myDbContext;

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                string auth = context
                    .HttpContext.Request.Headers.Authorization.ToString()
                    .Replace("Bearer ", "");

                Dictionary<string, string> jwtData = jwtService.DecryptData(auth);

                context.HttpContext.Items["userId"] = jwtData["userId"];
            }
        }
    }
}

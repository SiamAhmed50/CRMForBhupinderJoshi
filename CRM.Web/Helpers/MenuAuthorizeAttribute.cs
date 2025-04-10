using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CRM.UI.Helpers
{
    public class MenuAuthorizeAttribute : Attribute, IAsyncPageFilter
    {
        private readonly string? _menuUrl;

        // Optional custom URL
        public MenuAuthorizeAttribute(string? menuUrl = null)
        {
            _menuUrl = menuUrl?.ToLower();
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            var accessor = httpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var menuJson = accessor.HttpContext?.Request.Cookies["UserMenus"];

            if (menuJson != null)
            {
                try
                {
                    var allowedMenus = System.Text.Json.JsonSerializer.Deserialize<List<string>>(menuJson);
                    var currentPath = _menuUrl ?? httpContext.Request.Path.Value?.ToLower();

                    if (allowedMenus == null || !allowedMenus.Contains(currentPath))
                    {
                        context.Result = new RedirectToPageResult("/AccessDenied");
                        return;
                    }
                }
                catch
                {
                    context.Result = new RedirectToPageResult("/AccessDenied");
                    return;
                }
            }
            else
            {
                context.Result = new RedirectToPageResult("/AccessDenied");
                return;
            }

            await next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) => Task.CompletedTask;
    }
}

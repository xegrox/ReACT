using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReACT.Helpers;

public static class ActiveClassHelper
{
    public static string IsActiveClass(this IHtmlHelper html, string? page = null, string? controller = null, string? action = null, string? cssClass = null) {
        if (string.IsNullOrEmpty(cssClass)) cssClass = "active";
        if (page != null)
        {
            page = Path.Combine("/", page);
            var currentPage = html.ViewContext.RouteData.Values["page"]?.ToString();
            return page == currentPage ? cssClass : string.Empty;
        }
        var currentController = html.ViewContext.RouteData.Values["controller"]?.ToString();
        var currentAction = html.ViewContext.RouteData.Values["action"]?.ToString();
        return currentController == controller && currentAction == action ? cssClass : string.Empty;
    }
}

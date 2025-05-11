using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Project.BusinessLogic.Core.Filters
{
    public class RedirectIfAuthorizedAttribute : ActionFilterAttribute
    {
        public RedirectIfAuthorizedAttribute(string redirectController)
        {
            if (string.IsNullOrWhiteSpace(redirectController))
                throw new ArgumentException("RedirectController cannot be empty.", nameof(redirectController));
            RedirectController = redirectController;
        }

        public RedirectIfAuthorizedAttribute(string redirectController, string redirectAction)
            : this(redirectController)
        {
            if (!string.IsNullOrWhiteSpace(redirectAction)) RedirectAction = redirectAction;
        }

        public string RedirectController { get; set; }

        public string RedirectAction { get; set; } = "Index";

        public string RedirectArea { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var routeValues = new RouteValueDictionary
                {
                    { "controller", RedirectController },
                    { "action", RedirectAction }
                };

                if (!string.IsNullOrWhiteSpace(RedirectArea)) routeValues["area"] = RedirectArea;

                var isAlreadyAtTarget =
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals(RedirectController,
                        StringComparison.OrdinalIgnoreCase) &&
                    filterContext.ActionDescriptor.ActionName.Equals(RedirectAction,
                        StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrWhiteSpace(RedirectArea) ||
                     (filterContext.RouteData.DataTokens.TryGetValue("area", out var currentArea) &&
                      RedirectArea.Equals(currentArea?.ToString(), StringComparison.OrdinalIgnoreCase)));

                if (!isAlreadyAtTarget) filterContext.Result = new RedirectToRouteResult(routeValues);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
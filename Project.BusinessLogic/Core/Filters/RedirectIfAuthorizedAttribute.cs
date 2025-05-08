using System.Web.Mvc;
using System.Web.Routing;

namespace Project.BusinessLogic.Core.Filters
{
    public class RedirectIfAuthorizedAttribute : ActionFilterAttribute
    {
        public string RedirectController { get; set; }

        public string RedirectAction { get; set; } = "Index";

        public string RedirectArea { get; set; }

        public RedirectIfAuthorizedAttribute(string redirectController)
        {
            if (string.IsNullOrWhiteSpace(redirectController))
            {
                throw new System.ArgumentException("RedirectController cannot be empty.", nameof(redirectController));
            }
            RedirectController = redirectController;
        }

        public RedirectIfAuthorizedAttribute(string redirectController, string redirectAction)
            : this(redirectController)
        {
            if (!string.IsNullOrWhiteSpace(redirectAction))
            {
                RedirectAction = redirectAction;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var routeValues = new RouteValueDictionary
                {
                    { "controller", RedirectController },
                    { "action", RedirectAction }
                };

                if (!string.IsNullOrWhiteSpace(RedirectArea))
                {
                    routeValues["area"] = RedirectArea;
                }

                bool isAlreadyAtTarget =
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Equals(RedirectController, System.StringComparison.OrdinalIgnoreCase) &&
                    filterContext.ActionDescriptor.ActionName.Equals(RedirectAction, System.StringComparison.OrdinalIgnoreCase) &&
                    (string.IsNullOrWhiteSpace(RedirectArea) || (filterContext.RouteData.DataTokens.TryGetValue("area", out object currentArea) && RedirectArea.Equals(currentArea?.ToString(), System.StringComparison.OrdinalIgnoreCase)));

                if (!isAlreadyAtTarget)
                {
                    filterContext.Result = new RedirectToRouteResult(routeValues);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}

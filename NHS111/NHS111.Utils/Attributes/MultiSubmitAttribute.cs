using System.Reflection;
using System.Web.Mvc;

namespace NHS111.Utils.Attributes
{
    public class MultiSubmitAttribute : ActionMethodSelectorAttribute
    {
        public string ButtonName { get; set; }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var request = controllerContext.RequestContext.HttpContext.Request;
            return !string.IsNullOrEmpty(request.Form[ButtonName]);
        }
    }
}
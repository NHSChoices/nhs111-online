using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using NHS111.Models.Models.Web;
using NHS111.Utils.Logging;

namespace NHS111.Utils.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class LogJourneyFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
            {
                // The controller action didn't return a view result 
                // => no need to continue any further
                return;
            }

            var model = result.Model as JourneyViewModel;
            if (model == null)
            {
                // there's no model or the model was not of the expected type 
                // => no need to continue any further
                return;
            }

            // Log the journey state
            Log4Net.State(string.Format("SESSIONID: {3} : PATHWAY: {0} : STATE: {1} : JOURNEY {2}", model.Id, model.StateJson, model.JourneyJson, model.SessionId));
        }
    }
}

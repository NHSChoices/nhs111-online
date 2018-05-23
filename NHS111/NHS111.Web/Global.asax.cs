﻿
using FluentValidation.Mvc;
using NHS111.Features;

namespace NHS111.Web {
    using System;
    using System.Collections;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Models.Models.Web;
    using Presentation.ModelBinders;
    using Utils.Filters;
    using Utils.Logging;

    public class MvcApplication
        : System.Web.HttpApplication {

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders[typeof (JourneyViewModel)] = new JourneyViewModelBinder();
            ModelBinders.Binders[typeof(OutcomeViewModel)] = new JourneyViewModelBinder();
            ModelBinders.Binders[typeof(PersonalDetailViewModel)] = new JourneyViewModelBinder();
            ModelBinders.Binders[typeof(QuestionViewModel)] = new JourneyViewModelBinder();

            GlobalFilters.Filters.Add(new RedirectFilterAttribute(new RedirectToStartFeature()));
            GlobalFilters.Filters.Add(new LogJourneyFilterAttribute());
            FluentValidationModelValidatorProvider.Configure();

            var razorEngine = ViewEngines.Engines.OfType<RazorViewEngine>().FirstOrDefault();
            if (razorEngine == null) return;
            var newPartialViewFormats = new[]
            {
                "~/Views/{1}/Elements/{0}.cshtml",
                "~/Views/Shared/Elements/{0}.cshtml"
            };

            razorEngine.PartialViewLocationFormats = razorEngine.PartialViewLocationFormats.Union(newPartialViewFormats).ToArray();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try {
                var currentUrl = "";

                var context = HttpContext.Current;
                if (context == null)
                    currentUrl = context.Request.RawUrl;

                var lastException = GetException();
                if (lastException == null)
                    return; //nothing to log

                string data = "";
                foreach (DictionaryEntry pair in lastException.Data)
                    data += string.Format("{0}: {1}{2}", pair.Key, pair.Value, Environment.NewLine);

                Log4Net.Error(string.Format("{0} occured executing '{1}'{5}{2}{5}{3}{5}Exception.Data:{5}{4}",
                    lastException.GetType().FullName, currentUrl, lastException.Message,
                    lastException.StackTrace, data, Environment.NewLine));
            }
            catch (Exception ex) {
                Log4Net.Error(string.Format("Exception occured in OnError: [{0}]", ex.Message));
            }
        }

        private Exception GetException() {
            var lastException = Server.GetLastError();
            if (lastException == null && HttpContext.Current.AllErrors.Any())
                lastException = HttpContext.Current.AllErrors.First();

            return lastException;
        }
    }
}
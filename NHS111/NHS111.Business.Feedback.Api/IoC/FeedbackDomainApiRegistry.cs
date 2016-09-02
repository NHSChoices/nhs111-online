using System;
using System.Collections.Generic;
using System.Web;
using NHS111.Business.Feedback.Api.Controllers;
using NHS111.Domain.Feedback.IoC;
using NHS111.Domain.Feedback.Repository;
using NHS111.Utils.IoC;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace NHS111.Business.Feedback.Api.IoC
{
    public class FeedbackDomainApiRegistry : Registry
    {
        public FeedbackDomainApiRegistry()
        {

            IncludeRegistry<FeedbackDomainRegistry>();
            IncludeRegistry<UtilsRegistry>();
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });
        }
    }
}
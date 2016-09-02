using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Domain.DOS.Api.Configuration;
using NHS111.Utils.Helpers;
using NHS111.Utils.Monitoring;

namespace NHS111.Domain.DOS.Api.Monitoring
{
    using System.Reflection;

    public class Monitor : BaseMonitor
    {
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public Monitor(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public override string Metrics()
        {
            return "Metrics";
        }

        public override async Task<bool> Health()
        {
            try
            {
                return JsonConvert.DeserializeObject<bool>(await _restfulHelper.GetAsync(_configuration.DOSIntegrationMonitorHealthUrl));
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public override string Version() {
            return Assembly.GetCallingAssembly().GetName().Version.ToString();
        }
    }
}
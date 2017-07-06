﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NHS111.Business.DOS.Configuration;
using NHS111.Utils.Helpers;

namespace NHS111.Business.DOS.Service
{
    public class DosService : IDosService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestfulHelper _restfulHelper;

        public DosService(IConfiguration configuration, IRestfulHelper restfulHelper)
        {
            _configuration = configuration;
            _restfulHelper = restfulHelper;
        }
        public async Task<HttpResponseMessage> GetServices(HttpRequestMessage request)
        {
            return await _restfulHelper.PostAsync(_configuration.DomainDosApiCheckCapacitySummaryUrl, request);
        }

        public async Task<HttpResponseMessage> GetServiceById(HttpRequestMessage request)
        {
            return await _restfulHelper.PostAsync(_configuration.DomainDosApiServiceDetailsByIdUrl, request);
        }

        public async Task<HttpResponseMessage> GetServicesByClinicalTerm(HttpRequestMessage request)
        {
            return await _restfulHelper.PostAsync(_configuration.DomainDosApiServicesByClinicalTermUrl, request);
        }
    }

    public interface IDosService
    {
        Task<HttpResponseMessage> GetServices(HttpRequestMessage request);

        Task<HttpResponseMessage> GetServiceById(HttpRequestMessage request);

        Task<HttpResponseMessage> GetServicesByClinicalTerm(HttpRequestMessage request);
    }
}

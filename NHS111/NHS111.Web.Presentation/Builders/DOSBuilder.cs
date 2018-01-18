﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Xml.Linq;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Models.Models.Web;
using NHS111.Models.Models.Web.DosRequests;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Web.FromExternalServices;
using NHS111.Utils.Cache;
using NHS111.Utils.Helpers;
using NHS111.Utils.Notifier;
using NHS111.Web.Presentation.Models;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;
using NHS111.Features;
using DosService = NHS111.Models.Models.Business.DosService;

namespace NHS111.Web.Presentation.Builders
{
    using log4net;
    using log4net.Config;

    public class DOSBuilder : IDOSBuilder
    {
        private readonly ICareAdviceBuilder _careAdviceBuilder;
        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private readonly IMappingEngine _mappingEngine;
        private readonly ICacheManager<string, string> _cacheManager;
        private readonly INotifier<string> _notifier;
        private readonly IITKMessagingFeature _itkMessagingFeature;
        private static readonly ILog _logger = LogManager.GetLogger(typeof (DOSBuilder));

        public DOSBuilder(ICareAdviceBuilder careAdviceBuilder, IRestfulHelper restfulHelper, IConfiguration configuration, IMappingEngine mappingEngine, ICacheManager<string, string> cacheManager, INotifier<string> notifier, IITKMessagingFeature itkMessagingFeature)
        {
            _careAdviceBuilder = careAdviceBuilder;
            _restfulHelper = restfulHelper;
            _configuration = configuration;
            _mappingEngine = mappingEngine;
            _cacheManager = cacheManager;
            _notifier = notifier;
            _itkMessagingFeature = itkMessagingFeature;

            XmlConfigurator.Configure();

        }

        public async Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel, bool filterServices, DosEndpoint? endpoint) {
            const int PHARMACY = 13;
            const int PHARMACY_EXT_HOURS = 116;

            var request = BuildRequestMessage(dosViewModel);
            var body = await request.Content.ReadAsStringAsync();

            string checkCapacitySummaryUrl = string.Format("{0}?filterServices={1}&endpoint={2}", _configuration.BusinessDosCheckCapacitySummaryUrl, filterServices, endpoint);
           
            _logger.Debug(string.Format("DOSBuilder.FillCheckCapacitySummaryResult(): URL: {0} BODY: {1}", checkCapacitySummaryUrl, body));
            var response = await _restfulHelper.PostAsync(checkCapacitySummaryUrl, request);

            if (response.StatusCode != HttpStatusCode.OK) return new DosCheckCapacitySummaryResult { Error = new ErrorObject() { Code = (int) response.StatusCode, Message = response.ReasonPhrase } };

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var results = jObj["CheckCapacitySummaryResult"];
            var services = results.ToObject<List<ServiceViewModel>>();

            var checkCapacitySummaryResult = new DosCheckCapacitySummaryResult()
            {
                Success = new SuccessObject<ServiceViewModel>()
                {
                    Code = (int)response.StatusCode,
                    Services = DetermineCallbackEnabled(services)
                }
            };

            var isPharmacy = new Func<DosService, bool>(s => s.ServiceType != null && (s.ServiceType.Id == PHARMACY || s.ServiceType.Id == PHARMACY_EXT_HOURS));

            if (checkCapacitySummaryResult.Success.Services.Any(isPharmacy))
                checkCapacitySummaryResult.Success.Services = checkCapacitySummaryResult.Success.Services.Take(6).ToList();

            return checkCapacitySummaryResult;
        }

        public List<GroupedDOSServices> FillGroupedDosServices(List<ServiceViewModel> services)
        {
            var groupedServices = new List<GroupedDOSServices>();
            if (services != null)
            {
                var ungroupedServices = new List<ServiceViewModel>(services);

                while (ungroupedServices.Any())
                {
                    var topServiceType = ungroupedServices.First().OnlineDOSServiceType;
                    groupedServices.Add(new GroupedDOSServices(topServiceType,
                        ungroupedServices.Where(s => s.OnlineDOSServiceType == topServiceType).ToList()));
                    ;
                    ungroupedServices.RemoveAll(s => s.OnlineDOSServiceType == topServiceType);
                }
            }
        return groupedServices;
        }
        
        public List<ServiceViewModel> DetermineCallbackEnabled(List<ServiceViewModel> services)
        {
            if (_itkMessagingFeature.IsEnabled)
                return services;
            
            //remove callback services from list, as these are disabled
            services.RemoveAll(s => s.CallbackEnabled);
            return services.ToList();
        }

        public async Task<DosServicesByClinicalTermResult> FillDosServicesByClinicalTermResult(DosViewModel dosViewModel)
        {
            /*
            dosViewModel.SymptomGroup = await BuildSymptomGroup(dosViewModel.JourneyJson);
            
            var request = BuildRequestMessage(dosViewModel);
            var response = await _restfulHelper.PostAsync(_configuration.BusinessDosServicesByClinicalTermUrl, request);

            if (response.StatusCode != HttpStatusCode.OK) return new DosServicesByClinicalTermResult[0];

            var val = await response.Content.ReadAsStringAsync();
            var jObj = (JObject)JsonConvert.DeserializeObject(val);
            var result = jObj["DosServicesByClinicalTermResult"];
            return result.ToObject<DosServicesByClinicalTermResult[]>();
            */

            //USE UNTIL BUSINESS API IS AVAILABLE
            //#########################START###################

            //map doscase to dosservicesbyclinicaltermrequest
            var requestObj = Mapper.Map<DosServicesByClinicalTermRequest>(dosViewModel);
            requestObj.GpPracticeId = await GetPracticeIdFromSurgeryId(dosViewModel.Surgery);

            return
                await
                    GetMobileDoSResponse<DosServicesByClinicalTermResult>(
                        "services/byClinicalTerm/{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}",
                        requestObj.CaseId, requestObj.Postcode, requestObj.SearchDistance, requestObj.GpPracticeId,
                        requestObj.Age, requestObj.Gender, requestObj.Disposition,
                        requestObj.SymptomGroupDiscriminatorCombos, requestObj.NumberPerType);

            //################################END################
        }

        public async Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(model.CheckCapacitySummaryResultListJson);
            model.DosCheckCapacitySummaryResult = jObj["DosCheckCapacitySummaryResult"].ToObject<DosCheckCapacitySummaryResult>();
            var selectedService = model.SelectedService;

            var itkMessage = await _cacheManager.Read(model.UserId.ToString());
            var document = XDocument.Parse(itkMessage);

            var serviceDetials = document.Root.Descendants("ServiceDetails").FirstOrDefault();
            serviceDetials.Element("id").SetValue(selectedService.Id.ToString());
            serviceDetials.Element("name").SetValue(selectedService.Name);
            serviceDetials.Element("odsCode").SetValue(selectedService.OdsCode);
            serviceDetials.Element("contactDetails").SetValue(selectedService.ContactDetails ?? "");
            serviceDetials.Element("address").SetValue(selectedService.Address);
            serviceDetials.Element("postcode").SetValue(selectedService.PostCode);

            _cacheManager.Set(model.UserId.ToString(), document.ToString());
            _notifier.Notify(_configuration.IntegrationApiItkDispatcher, model.UserId.ToString());

            model.DosCheckCapacitySummaryResult = new DosCheckCapacitySummaryResult { Success = new SuccessObject<ServiceViewModel>() { Services = new List<ServiceViewModel>() { selectedService } } };
            model.CareAdvices = await _careAdviceBuilder.FillCareAdviceBuilder(Convert.ToInt32(model.Age), model.Gender.ToString(), model.CareAdviceMarkers.ToList());

            return model;
        }

        public HttpRequestMessage BuildRequestMessage(DosFilteredCase dosCase)
        {
            return new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(dosCase), Encoding.UTF8, "application/json") };
        }

        private async Task<string> GetPracticeIdFromSurgeryId(string surgeryId)
        {
            var services = await GetMobileDoSResponse<DosServicesByClinicalTermResult>("services/byOdsCode/{0}", surgeryId);
            if (services.Success.Code != (int)HttpStatusCode.OK || services.Success.Services.FirstOrDefault() == null) return "0";

            return services.Success.Services.FirstOrDefault().Id.ToString();
        }

        private async Task<T> GetMobileDoSResponse<T>(string endPoint, params object[] args)
        {
            var urlWithRequest = CreateMobileDoSUrl(endPoint, args);
            _logger.Debug("DOSBuilder.FillDosServicesByClinicalTermResult(): URL: " + urlWithRequest);

            var http = new HttpClient(new HttpClientHandler {Credentials = new NetworkCredential(_configuration.DosMobileUsername, _configuration.DosMobilePassword) });
            var response = await http.GetAsync(urlWithRequest);

            var dosResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dosResult);
        }

        private string CreateMobileDoSUrl(string endPoint, params object[] args)
        {
            return string.Format(_configuration.DosMobileBaseUrl + endPoint, args);
        }
    }

    public interface IDOSBuilder
    {
        Task<DosCheckCapacitySummaryResult> FillCheckCapacitySummaryResult(DosViewModel dosViewModel, bool filterServices, DosEndpoint? endpoint);
        Task<DosServicesByClinicalTermResult> FillDosServicesByClinicalTermResult(DosViewModel dosViewModel);
        Task<DosViewModel> FillServiceDetailsBuilder(DosViewModel model);
        HttpRequestMessage BuildRequestMessage(DosFilteredCase dosCase);
        List<GroupedDOSServices> FillGroupedDosServices(List<ServiceViewModel> services);
    }
}
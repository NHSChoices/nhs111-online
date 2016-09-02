﻿

namespace NHS111.Web.Presentation.Builders {
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using Utils.Helpers;
    using Configuration;

    public class CareAdviceBuilder
        : BaseBuilder, ICareAdviceBuilder {

        private readonly IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;
        private const string WORSENING_CAREADVICE_ID = "Cx1910";

        public CareAdviceBuilder(IRestfulHelper restfulHelper, IConfiguration configuration) {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers) {
            if (!careAdviceMarkers.Any())
                return Enumerable.Empty<CareAdvice>();

            var businessApiCareAdviceUrl = _configuration.GetBusinessApiCareAdviceUrl(age, gender,
                string.Join(",", careAdviceMarkers));
            var response = await _restfulHelper.GetAsync(businessApiCareAdviceUrl);
            var careAdvices = JsonConvert.DeserializeObject<List<CareAdvice>>(response);

            return careAdvices;
        }

        public async Task<CareAdvice> FillWorseningCareAdvice(int age, string gender) {
            var businessApiCareAdviceUrl = _configuration.GetBusinessApiCareAdviceUrl(age, gender,
                WORSENING_CAREADVICE_ID);
            var response = await _restfulHelper.GetAsync(businessApiCareAdviceUrl);
            var careAdvices = JsonConvert.DeserializeObject<List<CareAdvice>>(response);

            return careAdvices.FirstOrDefault();
        }

        public async Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(string dxCode, string ageGroup, string gender, IList<string> careAdviceKeywords) {
            if (!careAdviceKeywords.Any())
                return Enumerable.Empty<CareAdvice>();

            var request = new HttpRequestMessage {
                Content = new StringContent(JsonConvert.SerializeObject(GenerateKeywordsList(careAdviceKeywords)), Encoding.UTF8, "application/json")
            };

            var businessApiInterimCareAdviceUrl = _configuration.GetBusinessApiInterimCareAdviceUrl(dxCode, ageGroup, gender);
            var responseMessage = await _restfulHelper.PostAsync(businessApiInterimCareAdviceUrl, request);

            CheckResponse(responseMessage);

            var response = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<CareAdvice>>(response);
        }

        private string GenerateKeywordsList(IList<string> careAdviceKeywords) {
            return careAdviceKeywords.Aggregate((i, j) => i + '|' + j);
        }
    }

    public interface ICareAdviceBuilder {
        Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(int age, string gender, IList<string> careAdviceMarkers);

        Task<IEnumerable<CareAdvice>> FillCareAdviceBuilder(string dxCode, string ageGroup, string gender,
            IList<string> careAdviceKeywords);

        Task<CareAdvice> FillWorseningCareAdvice(int age, string gender);

    }
}
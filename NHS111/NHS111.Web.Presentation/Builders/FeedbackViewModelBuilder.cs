﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web;
using NHS111.Utils.Helpers;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders
{
    public class FeedbackViewModelBuilder : IFeedbackViewModelBuilder
    {
        private IRestfulHelper _restfulHelper;
        private readonly IConfiguration _configuration;

        public FeedbackViewModelBuilder(IRestfulHelper restfulHelper, IConfiguration configuration)
        {
            _restfulHelper = restfulHelper;
            _configuration = configuration;
        }

        public async Task<FeedbackConfirmation> FeedbackBuilder(FeedbackViewModel feedback)
        {
            var model = new FeedbackConfirmation();

            var request = new HttpRequestMessage { Content = new StringContent(JsonConvert.SerializeObject(feedback), Encoding.UTF8, "application/json") };
            var httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("Authorization", _configuration.FeedbackAuthorization);
            var response = await _restfulHelper.PostAsync(_configuration.FeedbackAddFeedbackUrl, request, httpHeaders);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                model.Message = "Thank you for your time in helping to improve our service.";
                model.Success = true;
            }
            else
            {
                model.Message = "Feedback did not submit, please try again later";
                model.Success = false;
            }

            return model;
        }
    }

    public interface IFeedbackViewModelBuilder
    {
        Task<FeedbackConfirmation> FeedbackBuilder(FeedbackViewModel feedback);
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NHS111.Business.DOS.WhitelistFilter;
using NHS111.Models.Models.Web.CCG;
using NUnit.Framework;
using RestSharp;

namespace NHS111.Business.DOS.Test.Whitelist
{
    public class ITKWhitelistFilterTest
    {
        private Mock<Configuration.IConfiguration> _mockConfiguration;
        private Mock<IRestClient> _restClient;
        private readonly string _localServiceIdWhiteListUrl = "http://localhost/whitelist/{0}";

        private static readonly string CheckCapacitySummaryResults = @"{
            ""CheckCapacitySummaryResult"": [{
                    ""idField"": 1419419101,
                    ""nameField"": ""Test Service 1"",
                    ""serviceTypeField"": {
                        ""idField"": 100,
                    }
                },
                {
                    ""idField"": 1419419102,
                    ""nameField"": ""Test Service 2"",
                    ""serviceTypeField"": {
                        ""idField"": 25,
                    }
                },
                {
                    ""idField"": 1419419103,
                    ""nameField"": ""Test Service 3"",
                    ""serviceTypeField"": {
                        ""idField"": 46,
                    }
                },
            ]}";

        [SetUp]
        public void SetUp()
        {
            _mockConfiguration = new Mock<Configuration.IConfiguration>();
            _restClient = new Mock<IRestClient>();

            _mockConfiguration.Setup(c => c.CCGApiGetCCGByPostcode).Returns(_localServiceIdWhiteListUrl);
        }

        [Test]
        public async void ITKWhitelistFilter_CallbackEnabledForWhitelistedServiceId()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "123", "456", "789", "1419419102" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result[0].CallbackEnabled);
            Assert.IsTrue(result[1].CallbackEnabled);
            Assert.IsFalse(result[2].CallbackEnabled);
        }

        [Test]
        public async void ITKWhitelistFilter_EmptyWhitelist()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel {  } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result[0].CallbackEnabled);
            Assert.IsFalse(result[1].CallbackEnabled);
            Assert.IsFalse(result[2].CallbackEnabled);
        }

        [Test]
        public async void ITKWhitelistFilter_NullWhitelist()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result[0].CallbackEnabled);
            Assert.IsFalse(result[1].CallbackEnabled);
            Assert.IsFalse(result[2].CallbackEnabled);
        }

        [Test]
        public async void ITKWhitelistFilter_CallbackNotEnabledForNonWhitelistedServiceId()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "123", "456", "789" } } }));

            var jObj = (JObject)JsonConvert.DeserializeObject(CheckCapacitySummaryResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result[0].CallbackEnabled);
            Assert.IsFalse(result[1].CallbackEnabled);
            Assert.IsFalse(result[2].CallbackEnabled);
        }

        [Test]
        public async void ITKWhitelistFilter_EmptyDOSResult()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed, Data = new CCGDetailsModel { ItkServiceIdWhitelist = new ServiceListModel { "1419419101", "1419419102", "1419419103" } } }));

            const string emptyCheckCapacityResults = @"{
            ""CheckCapacitySummaryResult"": [
            ]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(emptyCheckCapacityResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
            var result = await sut.Filter(results, postcode);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public async void ITKWhitelistFilter_CCGServiceServerError()
        {
            const string postcode = "SO302UN";
            string whitelistUrl = string.Format(_localServiceIdWhiteListUrl, postcode);
            _restClient.Setup(r => r.ExecuteTaskAsync<CCGDetailsModel>(It.Is<RestRequest>(req => req.Resource.Equals(whitelistUrl)))).Returns(() => StartedTask((IRestResponse<CCGDetailsModel>)new RestResponse<CCGDetailsModel>() { StatusCode = HttpStatusCode.InternalServerError, ResponseStatus = ResponseStatus.Completed }));

            const string emptyCheckCapacityResults = @"{
            ""CheckCapacitySummaryResult"": [
            ]}";
            var jObj = (JObject)JsonConvert.DeserializeObject(emptyCheckCapacityResults);
            var results = jObj["CheckCapacitySummaryResult"].ToObject<List<Models.Models.Business.DosService>>();

            //Act
            try
            {
                var sut = new ITKWhitelistFilter(_restClient.Object, _mockConfiguration.Object);
                var result = await sut.Filter(results, postcode);
                Assert.Fail();
            }
            catch (HttpException e)
            {
                Assert.Pass();
            }
        }

        private static Task<T> StartedTask<T>(T taskResult)
        {
            return Task<T>.Factory.StartNew(() => taskResult);
        }

        private static JObject GetJObjectFromResponse(HttpResponseMessage response)
        {
            var val = response.Content.ReadAsStringAsync().Result;
            return (JObject)JsonConvert.DeserializeObject(val);
        }
    }
}

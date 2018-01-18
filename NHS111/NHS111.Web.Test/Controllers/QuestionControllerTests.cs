﻿
using NHS111.Web.Helpers;
using RestSharp;

namespace NHS111.Web.Presentation.Test.Controllers {
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Features;
    using Logging;
    using Moq;
    using Newtonsoft.Json;
    using NHS111.Models.Models.Domain;
    using NHS111.Models.Models.Web;
    using NUnit.Framework;
    using Presentation.Builders;
    using Utils.Helpers;
    using Web.Controllers;
    using IConfiguration = Configuration.IConfiguration;
    using AwfulIdea = System.Tuple<string, NHS111.Models.Models.Web.QuestionViewModel>;

    [TestFixture]
    public class QuestionControllerTests {
        private string _pathwayId = "PW755MaleAdult";
        private int _age = 35;
        private string _pathwayTitle = "Headache";
        private Mock<IJourneyViewModelBuilder> _mockJourneyViewModelBuilder;
        private Mock<IRestfulHelper> _mockRestfulHelper;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IDirectLinkingFeature> _mockFeature;
        private Mock<IJustToBeSafeFirstViewModelBuilder> _mockJtbsBuilderMock;
        private Mock<IAuditLogger> _mockAuditLogger;
        private Mock<IUserZoomDataBuilder> _mockUserZoomDataBuilder;
        private Mock<IRestClient> _mockRestClient;
        private Mock<IViewRouter> _mockViewRouter;
        private Mock<IPostcodePrefillFeature> _mockPostCodePrefillFeature;
        private Mock<IDosEndpointFeature> _mockDosEndpointFeature;

        [SetUp]
        public void Setup() {
            _mockJourneyViewModelBuilder = new Mock<IJourneyViewModelBuilder>();
            _mockRestfulHelper = new Mock<IRestfulHelper>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockFeature = new Mock<IDirectLinkingFeature>();
            _mockFeature.Setup(c => c.IsEnabled).Returns(true);
            _mockJtbsBuilderMock = new Mock<IJustToBeSafeFirstViewModelBuilder>();
            _mockAuditLogger = new Mock<IAuditLogger>();
            _mockUserZoomDataBuilder = new Mock<IUserZoomDataBuilder>();
            _mockRestClient = new Mock<IRestClient>();
            _mockViewRouter = new Mock<IViewRouter>();
            _mockPostCodePrefillFeature = new Mock<IPostcodePrefillFeature>();
            _mockDosEndpointFeature = new Mock<IDosEndpointFeature>();

            _mockFeature.Setup(m => m.IsEnabled).Returns(true);
            _mockRestClient.Setup(r => r.ExecuteTaskAsync<Pathway>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<Pathway>)new RestResponse<Pathway>() { ResponseStatus = ResponseStatus.Completed, Data = new Pathway { Gender = "Male" } }));

            _mockRestClient.Setup(r => r.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<QuestionWithAnswers>)new RestResponse<QuestionWithAnswers>() { ResponseStatus = ResponseStatus.Completed, Data = new QuestionWithAnswers()}));
           

            _mockRestfulHelper.Setup(r => r.GetAsync(It.IsAny<string>()))
                .Returns(() => StartedTask(JsonConvert.SerializeObject(new Pathway { Gender = "Male" })));

            _mockRestfulHelper.Setup(r => r.PostAsync(It.IsAny<string>(), It.IsAny<HttpRequestMessage>()))
                .Returns(() => StartedTask(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(new QuestionWithAnswers())) }));

            _mockConfiguration.Setup(c => c.IsPublic).Returns(false);
        }

        [Test]
        public void Direct_WithNoAnswers_ReturnsFirstQuestionOfPathway() {

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", new QuestionViewModel())));

            _mockViewRouter.Setup(v => v.GetViewName(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => "../Question/Question");

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockPostCodePrefillFeature.Object, _mockDosEndpointFeature.Object);

            var result = sut.Direct(_pathwayId, _age, _pathwayTitle, null, true);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);
        }

        [Test]
        public async void Direct_WithAnswer_BuildsModelWithCorrectAnswer() {
            var mockQuestion = new QuestionViewModel {
                Answers = new List<Answer> {
                    new Answer {
                        Order = 2,
                        Title = "Second Answer"
                    },
                    new Answer {
                        Order = 1,
                        Title = "First Answer"
                    },
                }
            };

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", mockQuestion)));

            _mockRestClient.Setup(r => r.ExecuteTaskAsync<QuestionWithAnswers>(It.IsAny<RestRequest>())).Returns(() => StartedTask((IRestResponse<QuestionWithAnswers>)new RestResponse<QuestionWithAnswers>() {ResponseStatus  = ResponseStatus.Completed , Data = new QuestionWithAnswers() { Answers = mockQuestion.Answers } }));
           
            _mockJourneyViewModelBuilder.Setup(j => j.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => StartedTask((JourneyViewModel)mockQuestion));

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockPostCodePrefillFeature.Object, _mockDosEndpointFeature.Object);

            var result = (ViewResult) await sut.Direct(_pathwayId, _age, _pathwayTitle, new[] {0}, true);
            var model = (QuestionViewModel) result.Model;

            Assert.IsTrue(model.SelectedAnswer.Contains(mockQuestion.Answers[1].Title));

        }

        private static Task<T> StartedTask<T>(T taskResult) {
            return Task<T>.Factory.StartNew(() => taskResult);
        }

        [Test]
        public void Direct_WithAnswers_ProvidesAnswersToBuilder()
        {
            //var mockQuestionViewModelBuilder = new Mock<IQuestionViewModelBuilder>();
            var mockQuestion = new QuestionViewModel {
                Answers = new List<Answer> {
                    new Answer {
                        Order = 1,
                        Title = "1"
                    },
                    new Answer {
                        Order = 2,
                        Title = "2"
                    },
                    new Answer {
                        Order = 3,
                        Title = "3"
                    }
                }
            };

            _mockJourneyViewModelBuilder.Setup(q => q.Build(It.IsAny<QuestionViewModel>(), It.IsAny<QuestionWithAnswers>()))
                .Returns(() => StartedTask((JourneyViewModel)mockQuestion));

            var sequence = new MockSequence();

            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "1"), It.IsAny<QuestionWithAnswers>()));
            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "2"), It.IsAny<QuestionWithAnswers>()));
            _mockJourneyViewModelBuilder.InSequence(sequence)
                .Setup(x => x.Build(It.Is<QuestionViewModel>(j => j.SelectedAnswer == "3"), It.IsAny<QuestionWithAnswers>()));

            _mockJtbsBuilderMock.Setup(j => j.JustToBeSafeFirstBuilder(It.IsAny<JustToBeSafeViewModel>()))
                .Returns(StartedTask(new AwfulIdea("", mockQuestion)));

            _mockViewRouter.Setup(v => v.GetViewName(It.IsAny<JourneyViewModel>(), It.IsAny<ControllerContext>())).Returns(() => "../Question/Question");

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockPostCodePrefillFeature.Object, _mockDosEndpointFeature.Object);

            var pathwayId = "PW755MaleAdult";
            var age = 35;
            var pathwayTitle = "Headache";

            var result = sut.Direct(pathwayId, age, pathwayTitle, new[] { 0, 1, 2 }, true);

            Assert.IsInstanceOf<ViewResult>(result.Result);
            var viewResult = result.Result as ViewResult;
            Assert.AreEqual("../Question/Question", viewResult.ViewName);

            _mockJourneyViewModelBuilder.VerifyAll();
        }

        [Test]
        public void Direct_WithDirectLinkingDisabled_ReturnsNotFoundResult() {
            _mockFeature.Setup(c => c.IsEnabled).Returns(false);

            var sut = new QuestionController(_mockJourneyViewModelBuilder.Object,
                _mockConfiguration.Object, _mockJtbsBuilderMock.Object, _mockFeature.Object, _mockAuditLogger.Object, _mockUserZoomDataBuilder.Object, _mockRestClient.Object, _mockViewRouter.Object, _mockPostCodePrefillFeature.Object, _mockDosEndpointFeature.Object);

            var result = sut.Direct(null, 0, null, null, null);

            Assert.NotNull(result);
            Assert.IsInstanceOf<HttpNotFoundResult>(result.Result);
        }

    }
}
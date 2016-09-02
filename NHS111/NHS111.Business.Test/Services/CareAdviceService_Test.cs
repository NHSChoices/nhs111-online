//using System.Runtime.InteropServices;
//using NHS111.Business.Services;
//using NHS111.Utils.Helpers;
//using NUnit.Framework;
//using Moq;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace NHS111.Business.Test.Services
//{
//    [TestFixture]
//    public class CareAdviceService_Test
//    {
//        private Mock<Configuration.IConfiguration> _configuration;
//        Mock<IRestfulHelper> _restfulHelper;

//        [SetUp]
//        public void SetUp()
//        {
//            _configuration = new Mock<Configuration.IConfiguration>();
//            _restfulHelper = new Mock<IRestfulHelper>();
//        }

//        [Test]
//        public async void should_return_a_mocked_care_advice()
//        {
//            //Arrange
//            var markers = new List<string> { "one", "two", "three" };
//            var pathwayNo = "PW1234";
//            var url = "http://mytest.com/";
//            var resultString = "this is a care advice";
//            _configuration.Setup(x => x.GetDomainApiCareAdviceUrl(pathwayNo, markers)).Returns(url);
//            _restfulHelper.Setup(x => x.GetAsync(url)).Returns(Task.FromResult(resultString));

//            var sut = new CareAdviceService(_configuration.Object, _restfulHelper.Object);

//            //Act
//            var result = await sut.GetCareAdvice(pathwayNo, markers);

//            //Assert
//            _configuration.Verify(x => x.GetDomainApiCareAdviceUrl(pathwayNo, markers), Times.Once);
//            _restfulHelper.Verify(x => x.GetAsync(url), Times.Once);

//            Assert.That(result, Is.EqualTo(resultString));

//        }
//    }
//}
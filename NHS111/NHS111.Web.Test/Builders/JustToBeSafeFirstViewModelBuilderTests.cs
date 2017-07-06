﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NHS111.Models.Models.Domain;
using NHS111.Models.Models.Web;
using NHS111.Utils.Helpers;
using NHS111.Web.Presentation.Builders;
using NHS111.Web.Presentation.Models;
using NUnit.Framework;
using IConfiguration = NHS111.Web.Presentation.Configuration.IConfiguration;

namespace NHS111.Web.Presentation.Builders.Tests
{
    [TestFixture()]
    public class JustToBeSafeFirstViewModelBuilderTests
    {

        Mock<IRestfulHelper> _restfulHelper;
        Mock<IConfiguration> _configuration;
        Mock<IMappingEngine> _mappingEngine;
        Mock<IKeywordCollector> _keywordCollector;
        Mock<IMapper> _mapper;
        private Mock<IUserZoomDataBuilder> _userZoomDataBuilder;

        private const string MOCK_BusinessApiPathwayIdUrl = "http://testpathwaybyid.com";
        private const string MOCK_GetBusinessApiJustToBeSafePartOneUrl = "http://testGetBusinessApiJustToBeSafePartOneUrl.com";
        private const string MOCK_GetBusinessApiFirstQuestionUrl = "http://testGetBusinessApiFirstQuestionUrl";


        private string testPathwayId = "PW755MaleAdult";
        private string testPathwayTitle = "Test Pathway";
        private string testPathwayNo = "PW755";
        private string testGender = "Male";
        private string testKeywords = "testkey1|Test keyword 2";
        private string testQuestionTitle = "test Question?";
        private string testQuestionNo = "Tx1506";
        private string testQuestionId = "PW755.0";
        private List<Keyword> testKeywordsCollection = new List<Keyword>() { new Keyword() { Value = "testKeyword" } };
        private JustToBeSafeFirstViewModelBuilder _testJustToBeSafeFirstViewModelBuilder;

        [SetUp]
        public void SetUp()
        {

            _restfulHelper = new Mock<IRestfulHelper>();
            _configuration = new Mock<IConfiguration>();
            _mappingEngine = new Mock<IMappingEngine>();
            _keywordCollector = new Mock<IKeywordCollector>();
            _mapper = new Mock<IMapper>();
            _userZoomDataBuilder = new Mock<IUserZoomDataBuilder>();

            _configuration.Setup(c => c.GetBusinessApiPathwayIdUrl(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(MOCK_BusinessApiPathwayIdUrl);
            _configuration.Setup(c => c.GetBusinessApiJustToBeSafePartOneUrl(It.IsAny<string>())).Returns(MOCK_GetBusinessApiJustToBeSafePartOneUrl);
            _configuration.Setup(c => c.GetBusinessApiFirstQuestionUrl(It.IsAny<string>(),It.IsAny<string>())).Returns(MOCK_GetBusinessApiFirstQuestionUrl);


            _restfulHelper.Setup(r => r.GetAsync(MOCK_BusinessApiPathwayIdUrl))
                .ReturnsAsync(
                    "{'id':'" + testPathwayId + "'," +
                    "'title':'" + testPathwayTitle + "'," +
                    "'pathwayNo':'" + testPathwayNo + "'," +
                    "'gender':'" + testGender + "'," +
                    "'minimumAgeInclusive':16,'maximumAgeExclusive':200," +
                    "'module':'1','symptomGroup':'1112','group':null," +
                    "'keywords':'" + testKeywords + "'}");
            

                         
            _restfulHelper.Setup(r => r.GetAsync(MOCK_GetBusinessApiJustToBeSafePartOneUrl)).ReturnsAsync("[]");
            _restfulHelper.Setup(r => r.GetAsync(MOCK_GetBusinessApiFirstQuestionUrl)).ReturnsAsync("{'Question':{'group':null,'order':null,'topic':null,'id':'"+testQuestionId+"','questionNo':'" + testQuestionNo + "','title':'" + testQuestionTitle + "','jtbs':'PW755-','jtbsText':null,'rationale':''}," +
                                                                                                    "'Answers':[" +
                                                                                                        "{'title':'Yes','titleWithoutSpaces':'Yes','symptomDiscriminator':'','supportingInfo':'','keywords':'','order':1}," +
                                                                                                        "{'title':'No','titleWithoutSpaces':'No','symptomDiscriminator':'','supportingInfo':'','keywords':'','order':2}" +
                                                                                                    "],'Answered':null,'Labels':['Question'],'State':null}");

            _mappingEngine.Setup(m => m.Mapper).Returns(_mapper.Object);

            testKeywordsCollection = testKeywords.Split('|').Select(k => new Keyword() { Value = k, IsFromAnswer = false }).ToList();
            _keywordCollector.Setup(k => k.ParseKeywords(testKeywords, false))
                .Returns(testKeywordsCollection);

            _testJustToBeSafeFirstViewModelBuilder = new JustToBeSafeFirstViewModelBuilder(_restfulHelper.Object, _configuration.Object, _mappingEngine.Object, _keywordCollector.Object, _userZoomDataBuilder.Object);
            

        }

        [Test()]
        public async void JustToBeSafeFirstBuilder_Builds_Pathways_Data_Test()
        {
            var result = await _testJustToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(new JustToBeSafeViewModel(){UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = 22, Gender = testGender } }, PathwayNo = testPathwayNo});
            Assert.IsNotNull(result);

            Assert.AreEqual(testPathwayTitle,result.Item2.PathwayTitle);
            Assert.AreEqual(testPathwayId, result.Item2.PathwayId);
            Assert.AreEqual(testPathwayNo, result.Item2.PathwayNo);


        }

        [Test()]
        public async void JustToBeSafeFirstBuilder_Builds_Keywords_Data_Test()
        {
            var result = await _testJustToBeSafeFirstViewModelBuilder.JustToBeSafeFirstBuilder(new JustToBeSafeViewModel() { UserInfo = new UserInfo { Demography = new AgeGenderViewModel { Age = 22, Gender = testGender } }, PathwayNo = testPathwayNo });
            Assert.IsNotNull(result);


            Assert.AreEqual(testKeywordsCollection, result.Item2.CollectedKeywords.Keywords);
        }
    }
}

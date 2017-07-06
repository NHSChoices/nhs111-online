﻿using System.Linq;
using Moq;
using NHS111.Domain.Repository;
using NHS111.Utils.Configuration;
using NUnit.Framework;

namespace NHS111.Domain.Integration.Test.Repository
{
    [TestFixture]
    [Ignore("Ignore these tests until network issues connecting to integration database have been resolved")]
    public class PathwayRepositoryTest : RepositoryTestBase
    {
        private PathwayRepository _pathwayRepository;
        
        [TestFixtureSetUp]
        public void SetupTests()
        {
            DeleteAllNodesFromNeo4jDatabase();
            CreateTestNeo4jData();
            MockPathwaysWhiteListFeature.Setup(m => m.IsEnabled).Returns(true);
        }

        [Test]
        public async void GetPathway_with_invalid_id_returns_null()
        {
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetPathway("PX");
            Assert.IsNull(res);
        }

        [Test]
        public async void GetPathway_with_valid_id_returns_pathway()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);
            
            var res = await _pathwayRepository.GetPathway("P2");
            Assert.IsNotNull(res);
            Assert.AreEqual(res.PathwayNo, "PW101");
        }

        [Test]
        public async void GetPathway_when_only_using_live_for_valid_id_not_live_returns_null()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetPathway("P2");
            Assert.IsNull(res);
        }

        [Test]
        public async void GetPathway_when_only_using_live_for_valid_id_returns_pathway()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetPathway("P3");
            Assert.IsNotNull(res);
            Assert.AreEqual(res.PathwayNo, "LPW102");
        }

        [Test]
        public async void GetIdentifiedPathway_with_valid_params_returns_pathway()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);
            var res = await _pathwayRepository.GetIdentifiedPathway(new[] {"PW102"}, "Male", 25);
            Assert.AreEqual(res.PathwayNo, "PW102");
        }

        [Test]
        public async void GetIdentifiedPathway_with_invalid_gender_returns_null()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetIdentifiedPathway(new[] { "PW103" }, "Male", 25);
            Assert.IsNull(res);
        }

        [Test]
        public async void GetIdentifiedPathway_with_invalid_age_returns_null()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetIdentifiedPathway(new[] { "PW102", "PW103" }, "Male", 10);
            Assert.IsNull(res);
        }

        [Test]
        public async void GetIdentifiedPathway_when_only_using_live_for_valid_params_not_live_returns_null()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetIdentifiedPathway(new[] { "PW102" }, "Male", 25);
            Assert.IsNull(res);
        }

        [Test]
        public async void GetIdentifiedPathway_when_only_using_live_for_valid_params_returns_pathway()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetIdentifiedPathway(new[] { "LPW103" }, "Male", 25);
            Assert.AreEqual(res.PathwayNo, "LPW103");
        }

        [Test]
        public async void GetAllPathways_returns_all_pathways()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var res = await _pathwayRepository.GetAllPathways(true);
            Assert.AreEqual(res.Count(), Pathways.Count());
        }

        [Test]
        public async void GetAllPathways_when_only_using_live_returns_only_live_pathways()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);
            
            var liveOnlyPathways = PathwaysConfigurationManager.GetLivePathwaysElements().Select(e => e.Title);
            
            var res = await _pathwayRepository.GetAllPathways(true);
            Assert.AreEqual(res.Count(), Pathways.Count(p => liveOnlyPathways.Contains(p.Title)));
        }

        [Test]
        public async void GetGroupedPathways_returns_grouped_pathways()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(false);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var allDistinctPathways = Pathways.Where(p => p.Module == "1").Select(p => p.Title).Distinct();

            var res = await _pathwayRepository.GetGroupedPathways(true);
            Assert.AreEqual(res.Count(), allDistinctPathways.Count());
        }

        [Test]
        public async void GetGroupedPathways_when_only_using_live_returns_grouped_pathways()
        {
            MockPathwaysConfigurationManager.Setup(m => m.UseLivePathways).Returns(true);
            _pathwayRepository = new PathwayRepository(GraphRepository, MockPathwaysConfigurationManager.Object, MockPathwaysWhiteListFeature.Object);

            var liveOnlyPathways = PathwaysConfigurationManager.GetLivePathwaysElements().Select(e => e.Title);
            var liveOnlyDistinctPathways = Pathways.Where(p => p.Module == "1" && liveOnlyPathways.Contains(p.Title)).Select(p => p.Title).Distinct();

            var res = await _pathwayRepository.GetGroupedPathways(true);
            Assert.AreEqual(res.Count(), liveOnlyDistinctPathways.Count());
        }
    }
}

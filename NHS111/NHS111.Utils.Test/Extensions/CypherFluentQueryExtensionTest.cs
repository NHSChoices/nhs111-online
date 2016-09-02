using System;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using NHS111.Utils.Configuration;
using NUnit.Framework;
using NHS111.Utils.Extensions;

namespace NHS111.Utils.Test.Extensions
{
    [TestFixture]
    public class CypherFluentQueryExtensionTest
    {
        private readonly ICypherFluentQuery _cypherFluentQuery = new CypherFluentQuery(new GraphClient(new Uri("http://foo/bar/")));

        private readonly string _commaSepratedLivePathways = string.Join(", ",
            PathwaysConfigurationManager.GetLivePathwaysElements().Select(p => string.Format("'{0}'", p.Title)));

        [Test]
        public void Match_does_not_include_live_only_filter_by_default()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)", query);
        }

        [Test]
        public void Match_does_not_include_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match(false, "(p:Pathway)")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)", query);
        }

        [Test]
        public void Match_includes_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match(true, "(p:Pathway)")
                .Query
                .QueryText;

            Assert.AreEqual(string.Format("MATCH (p:Pathway)\r\nWHERE p.title in [{0}]", _commaSepratedLivePathways), query);
        }

        [Test]
        public void Where_does_not_include_live_only_filter_by_default()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"")
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)\r\nWHERE p.module = \"1\"", query);
        }

        [Test]
        public void Where_does_not_include_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"", false)
                .Query
                .QueryText;

            Assert.AreEqual("MATCH (p:Pathway)\r\nWHERE p.module = \"1\"", query);
        }

        [Test]
        public void Where_includes_live_only_filter_when_specified()
        {
            var query = _cypherFluentQuery
                .Match("(p:Pathway)")
                .Where("p.module = \"1\"", true)
                .Query
                .QueryText;

            Assert.AreEqual(string.Format("MATCH (p:Pathway)\r\nWHERE p.module = \"1\"\r\nAND p.title in [{0}]", _commaSepratedLivePathways), query);
        }
    }
}

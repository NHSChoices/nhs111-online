﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Neo4jClient.Cypher;
using Newtonsoft.Json;
using NHS111.Features;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Configuration;
using NHS111.Utils.Extensions;
using NHS111.Utils.Parser;

namespace NHS111.Domain.Repository
{
    public class PathwayRepository : IPathwayRepository
    {
        private readonly IGraphRepository _graphRepository;
        private readonly IPathwaysConfigurationManager _pathwaysConfigurationManager;
        private readonly IPathwaysWhiteListFeature _pathwaysWhiteListFeature;

        public PathwayRepository(IGraphRepository graphRepository, IPathwaysConfigurationManager pathwaysConfigurationManager, IPathwaysWhiteListFeature pathwaysWhiteListFeature )
        {
            _graphRepository = graphRepository;
            _pathwaysConfigurationManager = pathwaysConfigurationManager;
            _pathwaysWhiteListFeature = pathwaysWhiteListFeature;
        }

        private bool UseWhitelist
        {
            get { return _pathwaysWhiteListFeature.IsEnabled && _pathwaysConfigurationManager.UseLivePathways; }
        }

        public async Task<Pathway> GetPathway(string id)
        {
            return await _graphRepository.Client.Cypher
                .Match(UseWhitelist, string.Format("(p:Pathway {{ id: \"{0}\" }})", id))
                .Return(p => p.As<Pathway>())
                .ResultsAsync
                .FirstOrDefault();
        }

        public async Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age)
        {
            var genderIs = new Func<string, string>(g => string.Format("(p.gender is null or p.gender = \"\" or p.gender = \"{0}\")", g));
            var ageIsAboveMinimum = new Func<int, string>(a => string.Format("(p.minimumAgeInclusive is null or p.minimumAgeInclusive = \"\" or p.minimumAgeInclusive <= {0})", a));
            var ageIsBelowMaximum = new Func<int, string>(a => string.Format("(p.maximumAgeExclusive is null or p.maximumAgeExclusive = \"\" or {0} < p.maximumAgeExclusive)", a));
            var pathwayNumberIn = new Func<IEnumerable<string>, string>(p => string.Format("p.pathwayNo in {0}", JsonConvert.SerializeObject(p)));

            var jumptToPathways = PathwaysConfigurationManager.GetJumpToPathwaysElements().Select(e => e.Id);
            var pathwayIdIn = new Func<IEnumerable<string>, string>(p => string.Format("not p.id in {0}", JsonConvert.SerializeObject(p)));

            var pathway = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Join(" and ", new List<string> { genderIs(gender), ageIsAboveMinimum(age), ageIsBelowMaximum(age), pathwayNumberIn(pathwayNumbers), pathwayIdIn(jumptToPathways) }), UseWhitelist)
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync
                .FirstOrDefault();

            return pathway;
        }

        public async Task<Pathway> GetIdentifiedPathway(string pathwayTitle, string gender, int age)
        {
            var pathwayTitleEquals = string.Format("p.title = {0}", JsonConvert.SerializeObject(pathwayTitle));

            var jumptToPathways = PathwaysConfigurationManager.GetJumpToPathwaysElements().Select(e => e.Id);
            var pathwayIdIn = new Func<IEnumerable<string>, string>(p => string.Format("not p.id in {0}", JsonConvert.SerializeObject(p)));

            var pathway = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Join(" and ", new List<string> { GenderIs(gender), AgeIsAboveMinimum(age), AgeIsBelowMaximum(age), pathwayIdIn(jumptToPathways), pathwayTitleEquals }), UseWhitelist)
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync
                .FirstOrDefault();

            return pathway;
        }

        public async Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly)
        {
            return await GetPathwayQuery(startingOnly)
                .Return(p => Return.As<Pathway>("p")).ResultsAsync;
        }

        public async Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly, string gender, int age)
        {
            return await GetPathwayQuery(startingOnly)
                .Where(string.Join(" and ", new List<string> { GenderIs(gender), AgeIsAboveMinimum(age), AgeIsBelowMaximum(age) }))
                .Return(p => Return.As<Pathway>("p"))
                .ResultsAsync;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly)
        {
            var query = GetPathwayQuery(startingOnly)
                .Return(p => new GroupedPathways { Group = Return.As<string>("distinct(m.digitalDescription)"), PathwayNumbers = Return.As<IEnumerable<string>>("collect(distinct(m.pathwayNo))") });

            return await query.ResultsAsync;
        }

        public async Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly, string gender, int age)
        {
            var query = GetPathwayQuery(startingOnly)
                .Where(string.Join(" and ", new List<string> { GenderIs(gender), AgeIsAboveMinimum(age), AgeIsBelowMaximum(age) }))
                .Return(p => new GroupedPathways { Group = Return.As<string>("distinct(m.digitalDescription)"), PathwayNumbers = Return.As<IEnumerable<string>>("collect(distinct(m.pathwayNo))") });

            return await query.ResultsAsync;
        }

        private ICypherFluentQuery GetPathwayQuery(bool startingOnly)
        {
            var pathwayQuery = _graphRepository.Client.Cypher
               .Match(UseWhitelist, "(p:Pathway)-[:isDescribedAs]->(m:PathwayMetaData)");

            return startingOnly ? pathwayQuery.Where("p.startingPathway = true") : pathwayQuery;
        }

        public async Task<string> GetSymptomGroup(IList<string> pathwayNos)
        {
            var symptomGroups = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Format("p.pathwayNo in [{0}]", string.Join(", ", pathwayNos.Select(p => "\"" + p + "\""))), UseWhitelist)
                .Return(p => new SymptomGroup { PathwayNo = Return.As<string>("p.pathwayNo"), Code = Return.As<string>("collect(distinct(p.symptomGroup))[0]")})
                .ResultsAsync;

            var symptomGroup = symptomGroups
                .OrderBy(group => pathwayNos.IndexOf(group.PathwayNo))
                .LastOrDefault(group => !string.IsNullOrEmpty(group.Code));

            return symptomGroup == null ? string.Empty : symptomGroup.Code;
        }

        public async Task<string> GetPathwaysNumbers(string pathwayTitle)
        {
            var pathwayNumberList = await _graphRepository.Client.Cypher
                .Match("(p:Pathway)")
                .Where(string.Format("p.title =~ \"(?i){0}\"", PathwayTitleUriParser.EscapeSymbols(pathwayTitle)), UseWhitelist)  //case-insensitive query
                .Return(p => Return.As<string>("p.pathwayNo"))
                .ResultsAsync;

            var pathwayNumbers = pathwayNumberList as string[] ?? pathwayNumberList.ToArray();
            return !pathwayNumbers.Any() ? string.Empty : string.Join(",", pathwayNumbers.Distinct());
        }

        private string GenderIs(string gender)
        {
            return String.Format("(p.gender is null or p.gender = \"\" or p.gender = \"{0}\")", gender);
        }


        private string AgeIsAboveMinimum(int age)
        {
            return
                String.Format(
                    "(p.minimumAgeInclusive is null or p.minimumAgeInclusive = \"\" or p.minimumAgeInclusive <= {0})",
                    age);
        }

        private string AgeIsBelowMaximum(int age)
        {
            return
                String.Format(
                    "(p.maximumAgeExclusive is null or p.maximumAgeExclusive = \"\" or {0} < p.maximumAgeExclusive)", age);
        }
    }

    public interface IPathwayRepository
    {
        Task<Pathway> GetPathway(string id);
        Task<Pathway> GetIdentifiedPathway(IEnumerable<string> pathwayNumbers, string gender, int age);
        Task<Pathway> GetIdentifiedPathway(string pathwayTitle, string gender, int age);
        Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly);
        Task<IEnumerable<Pathway>> GetAllPathways(bool startingOnly, string gender, int age);
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly);
        Task<IEnumerable<GroupedPathways>> GetGroupedPathways(bool startingOnly, string gender, int age);
        Task<string> GetSymptomGroup(IList<string> pathwayNos);
        Task<string> GetPathwaysNumbers(string pathwayTitle);
    }

    
}
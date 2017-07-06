﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Business;
using NHS111.Models.Models.Domain;
using NHS111.Utils.Comparer;

namespace NHS111.Business.Services
{
    public class SearchCorrectionService : ISearchCorrectionService
    {
        private readonly IPathwayService _pathwayService;

        public SearchCorrectionService(IPathwayService pathwayService)
        {
            _pathwayService = pathwayService;
        }

        public async Task<string> GetCorrection(string input, bool startingOnly)
        {
            input = input.ToLower();
            var pathways = JsonConvert.DeserializeObject<List<GroupedPathways>>(await _pathwayService.GetPathways(true, startingOnly));

           return GetCorrection(pathways, input);
        }

        public string GetCorrection(IEnumerable<GroupedPathways> pathwaysToFilter, string input)
        {

            input = input.ToLower();
            var pathways = pathwaysToFilter;
            var pathwaysMatches = pathways.Where(x => x.Group.ToLower().Contains(input)).ToList();
            if (!pathwaysMatches.Any())
                return JsonConvert.SerializeObject(pathways);
            return JsonConvert.SerializeObject(pathwaysMatches.Distinct(new PathwaysComparer()).OrderByDescending(x => x.Group));
        }
    }

    public interface ISearchCorrectionService
    {
        Task<string> GetCorrection(string input, bool startingOnly);
        string GetCorrection(IEnumerable<GroupedPathways> pathwaysToFilter, string input);
    }
}
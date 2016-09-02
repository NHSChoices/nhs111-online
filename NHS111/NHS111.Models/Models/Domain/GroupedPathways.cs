﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class GroupedPathways
    {
        [JsonProperty(PropertyName = "pathwayNumbers")]
        public IEnumerable<string> PathwayNumbers { get; set; }

        [JsonProperty(PropertyName = "group")]
        public string Group { get; set; }
    }
}
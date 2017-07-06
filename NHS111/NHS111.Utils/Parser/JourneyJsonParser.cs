﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Utils.Parser
{
    public class JourneyJsonParser
    {
        public JourneyJsonParser(string journeyJson)
        {
            var journey = JsonConvert.DeserializeObject<Journey>(journeyJson);
            JourneySteps = journey.Steps;
        }

        public IEnumerable<JourneyStep> JourneySteps { get; private set; }

        public string LastPathwayNo
        {
            get
            {
                var endingPathway = JourneySteps.Last();
                return endingPathway.QuestionId.Split('.').FirstOrDefault();
            }
        }

        public string LastPathwayTitle
        {
            get
            {
                var endingPathway = JourneySteps.Last();
                return endingPathway.QuestionId.Split('.').FirstOrDefault();
            }
        }
    }
}

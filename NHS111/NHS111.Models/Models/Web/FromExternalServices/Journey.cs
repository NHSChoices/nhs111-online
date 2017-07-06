﻿
using System;
using System.Linq;
using System.Security.Policy;

namespace NHS111.Models.Models.Web.FromExternalServices {
    using Domain;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Journey {
        [JsonProperty(PropertyName = "steps")]
        public List<JourneyStep> Steps { get; set; }

        public Journey() {
            Steps = new List<JourneyStep>();
        }

        public Journey Add(Journey otherJourney) {
            otherJourney.Steps.ForEach(step => Steps.Add(step));
            return this;
        }

        public void RemoveLastStep() {
            Steps.RemoveAt(Steps.Count - 1);
        }

        public string GetLastState()
        {
            var lastStep = Steps.LastOrDefault();
            if (lastStep != null) return lastStep.State;
            return String.Empty;
        }
    }
}
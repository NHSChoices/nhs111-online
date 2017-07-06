﻿using Newtonsoft.Json;
using NHS111.Models.Models.Domain;

namespace NHS111.Models.Models.Web.FromExternalServices
{
    public class JourneyStep
    {
        [JsonProperty(PropertyName = "answer")]
        public Answer Answer { get; set; }

        [JsonProperty(PropertyName = "questionTitle")]
        public string QuestionTitle { get; set; }

        [JsonProperty(PropertyName = "questionNo")]
        public string QuestionNo { get; set; }

        [JsonProperty(PropertyName = "questionId")]
        public string QuestionId { get; set; }

        [JsonProperty(PropertyName = "jtbs")]
        public bool IsJustToBeSafe { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        public JourneyStep()
        {
            IsJustToBeSafe = false;
        }
    }
}
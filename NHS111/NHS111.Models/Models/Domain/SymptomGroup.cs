using Newtonsoft.Json;

namespace NHS111.Models.Models.Domain
{
    public class SymptomGroup
    {
        [JsonProperty(PropertyName = "pathwayNo")]
        public string PathwayNo { get; set; }

        [JsonProperty(PropertyName = "symptomGroup")]
        public string Code { get; set; }
    }
}
using System.Collections.Generic;

namespace NHS111.Models.Models.Business
{
    public class Suggestion
    {
        public string CorrectTerm { get; set; }
        public List<string> MispelledTerm { get; set; }
    }
}
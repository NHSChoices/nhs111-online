using System.Collections.Generic;
using NHS111.Models.Models.Web.FromExternalServices;

namespace NHS111.Models.Models.Web
{
    public class SurgeryViewModel
    {
        public string SurgeryInput { get; set; }
        public string SelectedSurgery { get; set; }
        public List<Surgery> Surgeries { get; set; }

        public SurgeryViewModel()
        {
            Surgeries = new List<Surgery>();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class PersonalDetailViewModel : OutcomeViewModel
    {
        public PatientInformantViewModel PatientInformantDetails { get; set; }

        public PersonalDetailViewModel() 
        {
            PatientInformantDetails = new PatientInformantViewModel();
        }
    }
}

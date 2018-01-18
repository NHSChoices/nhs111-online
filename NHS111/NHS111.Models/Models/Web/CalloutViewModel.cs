﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHS111.Models.Models.Web
{
    public class CalloutViewModel
    {
        public string Heading { get; set; }

        public IEnumerable<string> Details { get; set; }

        public IEnumerable<string> Modifiers { get; set; }
    }
}

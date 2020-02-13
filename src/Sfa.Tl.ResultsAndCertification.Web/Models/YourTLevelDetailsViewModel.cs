﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Web.Models
{
    public class YourTLevelDetailsViewModel
    {
        public string PageTitle { get; set; }
        public string RouteName { get; set; }
        public string PathwayName { get; set; }
        public IEnumerable<String> Specialisms { get; set; }
    }
}

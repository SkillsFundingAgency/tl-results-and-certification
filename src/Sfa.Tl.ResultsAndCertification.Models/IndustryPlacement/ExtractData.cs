﻿using Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement
{
   public  class ExtractData
    {
        [DisplayName(ExtractHeader.Uln)]
        public long Uln { get; set; }       

        [DisplayName(ExtractHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(ExtractHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(ExtractHeader.UKPRN)]
        public string UKPRN { get; set; }

        [DisplayName(ExtractHeader.ProviderName)]
        public string ProviderName { get; set; }

        [DisplayName(ExtractHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get; set; }

        [DisplayName(ExtractHeader.PendingWithdrawnFlag)]
        public string PendingWithdrawnFlag { get; set; }
    }
}

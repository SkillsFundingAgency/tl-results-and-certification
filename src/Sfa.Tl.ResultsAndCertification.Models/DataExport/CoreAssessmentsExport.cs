﻿using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class CoreAssessmentsExport
    {
        [DisplayName(CoreAssessmentsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(CoreAssessmentsExportHeader.StartYear)]
        public string StartYear { get; set; }        

        [DisplayName(CoreAssessmentsExportHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(CoreAssessmentsExportHeader.CoreAssessmentEntry)]
        public string CoreAssessmentEntry { get; set; }
    }
}

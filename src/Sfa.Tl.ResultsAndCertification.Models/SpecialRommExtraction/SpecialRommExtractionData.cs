using Sfa.Tl.ResultsAndCertification.Models.AnalystCoreResultExtraction;
using Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract;
using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.SpecialRommExtraction
{
    public class SpecialRommExtractionData
    {
        [DisplayName(SpecialRommExtractHeader.Uln)]
        public long UniqueLearnerNumber { get; set; }

        [DisplayName(SpecialRommExtractHeader.StudentStartYear)]
        public int StudentStartYear { get; set; }

        [DisplayName(SpecialRommExtractHeader.AssessmentSeries)]
        public string AssessmentSeries { get; set; }

        [DisplayName(SpecialRommExtractHeader.AoName)]
        public string AoName { get; set; }

        [DisplayName(SpecialRommExtractHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(SpecialRommExtractHeader.CurrentSpecialismGrade)]
        public string CurrentSpecialismGrade { get; set; }

        [DisplayName(SpecialRommExtractHeader.RommOpenedTimeStamp)]
        public DateTime? RommOpenedTimeStamp { get; set; }

        [DisplayName(SpecialRommExtractHeader.RommGrade)]
        public string RommGrade { get; set; }

        [DisplayName(SpecialRommExtractHeader.AppealOpenedTimeStamp)]
        public DateTime? AppealOpenedTimeStamp { get; set; }

        [DisplayName(SpecialRommExtractHeader.AppealGrade)]
        public string AppealGrade { get; set; }


    }
}
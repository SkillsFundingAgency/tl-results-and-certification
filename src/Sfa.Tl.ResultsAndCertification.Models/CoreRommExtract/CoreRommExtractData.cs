using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.CoreRommExtract
{
    public class CoreRommExtractData
    {
        [DisplayName(CoreRommtExtractHeader.Uln)]
        public long UniqueLearnerNumber { get; set; }

        [DisplayName(CoreRommtExtractHeader.StudentStartYear)]
        public int StudentStartYear { get; set; }

        [DisplayName(CoreRommtExtractHeader.AoName)]
        public string AoName { get; set; }

        [DisplayName(CoreRommtExtractHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(CoreRommtExtractHeader.CurrentCoreGrade)]
        public string CurrentCoreGrade { get; set; }

        [DisplayName(CoreRommtExtractHeader.RommOpenedTimeStamp)]
        public DateTime? RommOpenedTimeStamp { get; set; }

        [DisplayName(CoreRommtExtractHeader.RommGrade)]
        public string RommGrade { get; set; }

        [DisplayName(CoreRommtExtractHeader.AppealOpenedTimeStamp)]
        public DateTime? AppealOpenedTimeStamp { get; set; }

        [DisplayName(CoreRommtExtractHeader.AppealGrade)]
        public string AppealGrade { get; set; }
    }
}
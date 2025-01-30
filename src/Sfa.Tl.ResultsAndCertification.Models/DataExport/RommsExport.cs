using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class RommsExport
    {
        [DisplayName(RommsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(RommsExportHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(RommsExportHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(RommsExportHeader.DateOfBirth)]
        public string DisplayDateOfBirth => DateOfBirth.ToString("dd-MMM-yyyy");

        [DisplayName(RommsExportHeader.Ukprn)]
        public long Ukprn { get; set; }

        [DisplayName(RommsExportHeader.AcademicYear)]
        public int AcademicYear { get; set; }

        [DisplayName(RommsExportHeader.AssessmentSeriesCore)]
        public string AssessmentSeriesCore { get; set; }

        [DisplayName(RommsExportHeader.CoreComponentCode)]
        public string CoreComponentCode { get; set; }

        [DisplayName(RommsExportHeader.CoreRommOpen)]
        public bool CoreRommOpen { get; set; }

        [DisplayName(RommsExportHeader.CoreRommOutcome)]
        public string CoreRommOutcome { get; set; }

        [DisplayName(RommsExportHeader.AssessmentSeriesSpecialisms)]
        public string AssessmentSeriesSpecialisms { get; set; }

        [DisplayName(RommsExportHeader.SpecialismComponentCode)]
        public string SpecialismComponentCode { get; set; }

        [DisplayName(RommsExportHeader.SpecialismRommOpen)]
        public bool SpecialismRommOpen { get; set; }

        [DisplayName(RommsExportHeader.SpecialismRommOutcome)]
        public string SpecialismRommOutcome { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}

using System;
using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DownloadOverallResults
{
    public class DownloadOverallResultsData
    {
        [DisplayName(DownloadOverallResultsHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(DownloadOverallResultsHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(DownloadOverallResultsHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(DownloadOverallResultsHeader.DateOfBirth)]
        public string DisplayDateOfBirth => DateOfBirth.ToString("dd-MMM-yyyy");

        [DisplayName(DownloadOverallResultsHeader.Tlevel)]
        public string Tlevel { get; set; }

        [DisplayName(DownloadOverallResultsHeader.StartYear)]
        public string DisplayStartYear => $"{AcademicYear} to {AcademicYear + 1}";

        [DisplayName(DownloadOverallResultsHeader.CoreComponent)]
        public string CoreComponent { get; set; }

        [DisplayName(DownloadOverallResultsHeader.CoreCode)]
        public string CoreCode { get; set; }

        [DisplayName(DownloadOverallResultsHeader.CoreResult)]
        public string CoreResult { get; set; }

        [DisplayName(DownloadOverallResultsHeader.SpecialismComponent)]
        public string SpecialismComponent { get; set; }

        [DisplayName(DownloadOverallResultsHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(DownloadOverallResultsHeader.SpecialismResult)]
        public string SpecialismResult { get; set; }

        [DisplayName(DownloadOverallResultsHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get; set; }

        [DisplayName(DownloadOverallResultsHeader.OverallResult)]
        public string OverallResult { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int AcademicYear { get; set; }
    }
}

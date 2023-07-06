using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.ComponentModel;
using System.Linq;

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
        public string Tlevel { get { return $"\"{Details.TlevelTitle}\""; } }

        [DisplayName(DownloadOverallResultsHeader.StartYear)]
        public string DisplayStartYear => $"{AcademicYear} to {AcademicYear + 1}";

        [DisplayName(DownloadOverallResultsHeader.CoreComponent)]
        public string CoreComponent { get { return $"\"{Details.PathwayName}\""; } }

        [DisplayName(DownloadOverallResultsHeader.CoreCode)]
        public string CoreCode { get { return Details.PathwayLarId; } }

        [DisplayName(DownloadOverallResultsHeader.CoreResult)]
        public string CoreResult { get { return Details.PathwayResult; } }

        [DisplayName(DownloadOverallResultsHeader.SpecialismComponent)]
        public string SpecialismComponent { get; set; }

        [DisplayName(DownloadOverallResultsHeader.SpecialismCode)]
        public string SpecialismCode { get; set; }

        [DisplayName(DownloadOverallResultsHeader.SpecialismResult)]
        public string SpecialismResult { get; set; }

        [DisplayName(DownloadOverallResultsHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get { return Details.IndustryPlacementStatus; } }

        [DisplayName(DownloadOverallResultsHeader.OverallResult)]
        public string OverallResult { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int AcademicYear { get; set; }
        public OverallResultDetail Details { get; set; }
    }
}

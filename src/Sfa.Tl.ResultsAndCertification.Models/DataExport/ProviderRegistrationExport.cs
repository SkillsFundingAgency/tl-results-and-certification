using System.ComponentModel;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class ProviderRegistrationExport
    {
        [DisplayName(ProviderRegistrationExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.FirstName)]

        public string Firstname { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.LastName)]

        public string Lastname { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.DateOfBirth)]

        public string DateofBirth { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.Tlevel)]
        public string TLevel { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.StartYear)]
        public int StartYear { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.CoreComponent)]
        public string PathwayName { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.CoreCode)]
        public string PathwayLarId { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.Specialism)]
        public string SpecialismName { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.SpecialismCode)]
        public string SpecialismLarId { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.IndustryPlacement)]
        public string IndustryPlacementStatus { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.English)]
        public string EnglishStatus { get; set; }

        [DisplayName(ProviderRegistrationExportHeader.Maths)]
        public string MathsStatus { get; set; }
    }
}
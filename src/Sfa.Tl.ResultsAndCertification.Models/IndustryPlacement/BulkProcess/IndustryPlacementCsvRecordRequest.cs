using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public class IndustryPlacementCsvRecordRequest : FileBaseModel
    {
        [Column(IndustryPlacementFileHeader.Uln, Order = 0)]
        [Display(Name = IndustryPlacementFluentHeader.Uln)]
        public string Uln { get; set; }

        [Column(IndustryPlacementFileHeader.CoreCode, Order = 1)]
        [Display(Name = IndustryPlacementFluentHeader.CoreCode)]
        public string CoreCode { get; set; }

        [Column(IndustryPlacementFileHeader.IndustryPlacementStatus, Order = 2)]
        [Display(Name = IndustryPlacementFluentHeader.IndustryPlacementStatus)]
        public string IndustryPlacementStatus { get; set; }

        [Column(IndustryPlacementFileHeader.IndustryPlacementHours, Order = 3)]
        [Display(Name = IndustryPlacementFluentHeader.IndustryPlacementHours)]
        public string IndustryPlacementHours { get; set; }

        [Column(IndustryPlacementFileHeader.SpecialConsiderationReason, Order = 4)]
        [Display(Name = IndustryPlacementFluentHeader.SpecialConsiderationReason)]
        public string SpecialConsiderationReasons { get; set; }
    }
}

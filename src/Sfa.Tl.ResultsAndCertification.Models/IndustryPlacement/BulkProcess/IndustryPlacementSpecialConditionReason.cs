using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.ResultsAndCertification.Models.IndustryPlacement.BulkProcess
{
    public enum IndustryPlacementSpecialConditionReason
    {
        [Display(Name = "ML")]
        LearnersMedicalReasons = 1,
        [Display(Name = "MF")]
        LearnersFamilyMedicalReasons = 2,
        [Display(Name = "B")]
        Bereavement = 3,
        [Display(Name = "D")]
        DomesticCrisis = 4,
        [Display(Name = "T")]
        TraumaOrSignificantChangeOfCircumstances = 5,
        [Display(Name = "A")]
        AlternativePriorities = 6,
        [Display(Name = "U")]
        UnsafePlacement = 7,
        [Display(Name = "W")]
        PlacementWithdrawn = 8,
        [Display(Name = "C")]
        Covid19 = 9
    }
}
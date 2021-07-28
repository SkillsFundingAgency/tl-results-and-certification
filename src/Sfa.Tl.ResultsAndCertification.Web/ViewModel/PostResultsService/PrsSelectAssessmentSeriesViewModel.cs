using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SelectSeriesContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsSelectAssessmentSeries;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsSelectAssessmentSeriesViewModel : PrsBaseViewModel
    {
        public PrsSelectAssessmentSeriesViewModel()
        {
            UlnLabel = SelectSeriesContent.Title_Uln_Text;
            LearnerNameLabel = SelectSeriesContent.Title_Name_Text;
            DateofBirthLabel = SelectSeriesContent.Title_DateofBirth_Text;
            ProviderNameLabel = SelectSeriesContent.Title_Provider_Text;
            TlevelTitleLabel = SelectSeriesContent.Title_TLevel_Text;
            AssessmentSerieses = new List<PrsAssessment>();
        }

        [Required(ErrorMessageResourceType = typeof(SelectSeriesContent), ErrorMessageResourceName = "Validation_Message")]
        public int? SelectedAssessmentId { get; set; }

        public IList<PrsAssessment> AssessmentSerieses { get; set; }  
    }
}

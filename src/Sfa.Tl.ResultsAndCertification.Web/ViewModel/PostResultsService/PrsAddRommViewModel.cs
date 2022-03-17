using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddRommContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRomm;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddRommViewModel : PrsBaseViewModel
    {
        public PrsAddRommViewModel()
        {
            // Base Profile Summary
            UlnLabel = PrsAddRommContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddRommContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddRommContent.Title_TLevel_Text;
            CoreLabel = PrsAddRommContent.Title_Core_Text;
            SpecialismLabel = PrsAddRommContent.Title_Core_Text;
            ExamPeriodLabel = PrsAddRommContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddRommContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddRommContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsRommRequested { get; set; }

        public bool IsValid => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && CommonHelper.IsRommAllowed(RommEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}

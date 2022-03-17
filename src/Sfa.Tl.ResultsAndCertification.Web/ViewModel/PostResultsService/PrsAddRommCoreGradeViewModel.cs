using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddRommCoreGradeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddRommCoreGrade;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddRommCoreGradeViewModel : PrsBaseViewModel
    {
        public PrsAddRommCoreGradeViewModel()
        {
            // Base Profile Summary
            UlnLabel = PrsAddRommCoreGradeContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddRommCoreGradeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddRommCoreGradeContent.Title_TLevel_Text;
            CoreLabel = PrsAddRommCoreGradeContent.Title_Core_Text;
            SpecialismLabel = PrsAddRommCoreGradeContent.Title_Core_Text;
            ExamPeriodLabel = PrsAddRommCoreGradeContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddRommCoreGradeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddRommCoreGradeContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsRommRequested { get; set; }

        public bool IsValid => (PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && CommonHelper.IsRommAllowed(RommEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}

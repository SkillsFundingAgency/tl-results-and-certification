using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PrsAddAppealContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsAddAppeal;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsAddAppealViewModel : PrsBaseViewModel
    {
        public PrsAddAppealViewModel()
        {
            // Base Profile Summary
            UlnLabel = PrsAddAppealContent.Title_Uln_Text;
            DateofBirthLabel = PrsAddAppealContent.Title_DateofBirth_Text;
            TlevelTitleLabel = PrsAddAppealContent.Title_TLevel_Text;
            CoreLabel = PrsAddAppealContent.Title_Core_Text;
            ExamPeriodLabel = PrsAddAppealContent.Title_ExamPeriod_Text;
            GradeLabel = PrsAddAppealContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(PrsAddAppealContent), ErrorMessageResourceName = "Validation_Message")]
        public bool? IsAppealRequested { get; set; }

        public bool IsValid => (PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Reviewed) && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.PrsLearnerDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}
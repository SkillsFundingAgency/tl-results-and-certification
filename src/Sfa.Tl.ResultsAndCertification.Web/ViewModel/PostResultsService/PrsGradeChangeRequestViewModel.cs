using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GradeChangeContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsGradeChangeRequest;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService
{
    public class PrsGradeChangeRequestViewModel : PrsBaseViewModel
    {
        public PrsGradeChangeRequestViewModel()
        {
            UlnLabel = GradeChangeContent.Title_Uln_Text;
            LearnerNameLabel = GradeChangeContent.Title_Name_Text;
            DateofBirthLabel = GradeChangeContent.Title_DateofBirth_Text;
            TlevelTitleLabel = GradeChangeContent.Title_TLevel_Text;
            CoreLabel = GradeChangeContent.Title_Core_Text;
            SpecialismLabel = GradeChangeContent.Title_Specialism_Text;
            ExamPeriodLabel = GradeChangeContent.Title_ExamPeriod_Text;
            GradeLabel = GradeChangeContent.Title_Grade_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public int ResultId { get; set; }
        public RegistrationPathwayStatus Status { get; set; }
        public PrsStatus? PrsStatus { get; set; }
        public DateTime RommEndDate { get; set; }
        public DateTime AppealEndDate { get; set; }

        [Required(ErrorMessageResourceType = typeof(GradeChangeContent), ErrorMessageResourceName = "Validation_Message")]
        public string ChangeRequestData { get; set; }

        public bool? IsResultJourney { get; set; }

        public bool CanRequestFinalGradeChange
        {
            get
            {
                return Status == RegistrationPathwayStatus.Active &&
                (
                    ((PrsStatus == null || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.NotSpecified) && !CommonHelper.IsRommAllowed(RommEndDate)) 
                    || PrsStatus == ResultsAndCertification.Common.Enum.PrsStatus.Final 
                    || !CommonHelper.IsAppealsAllowed(AppealEndDate)
                );
            }
        }

        public override BackLinkModel BackLink =>
            IsResultJourney.HasValue && IsResultJourney == true ? new BackLinkModel
            {
                RouteName = RouteConstants.ResultDetails,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
            }
            : new BackLinkModel
            {
                RouteName = RouteConstants.PrsLearnerDetails,
                RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
            };
    }
}

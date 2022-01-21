using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ManageCoreResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ManageCoreResult;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ManageCoreResultViewModel : ResultsBaseViewModel
    {
        public ManageCoreResultViewModel()
        {
            // Base Profile Summary
            UlnLabel = ManageCoreResultContent.Title_Uln_Text;
            DateofBirthLabel = ManageCoreResultContent.Title_DateofBirth_Text;
            ProviderNameLabel = ManageCoreResultContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = ManageCoreResultContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = ManageCoreResultContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public DateTime? AppealEndDate { get; set; }
        public string PathwayDisplayName { get; set; }
        
        public int? ResultId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManageCoreResultContent), ErrorMessageResourceName = "Validation_Select_Grade_Required_Message")]
        public string SelectedGradeCode { get; set; }
        public int? LookupId { get; set; }
        public PrsStatus? PathwayPrsStatus { get; set; }

        public List<LookupViewModel> Grades { get; set; }
        public bool IsValid => (PathwayPrsStatus.HasValue == false || PathwayPrsStatus == PrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.ResultDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}
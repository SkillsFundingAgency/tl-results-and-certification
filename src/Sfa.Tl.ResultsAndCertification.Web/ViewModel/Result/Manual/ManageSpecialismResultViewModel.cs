using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ManageSpecialismResultContent = Sfa.Tl.ResultsAndCertification.Web.Content.Result.ManageSpecialismResult;
using EnumPrsStatus = Sfa.Tl.ResultsAndCertification.Common.Enum.PrsStatus;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual
{
    public class ManageSpecialismResultViewModel : ResultsBaseViewModel
    {
        public ManageSpecialismResultViewModel()
        {
            // Base Profile Summary
            UlnLabel = ManageSpecialismResultContent.Title_Uln_Text;
            DateofBirthLabel = ManageSpecialismResultContent.Title_DateofBirth_Text;
            ProviderNameLabel = ManageSpecialismResultContent.Title_Provider_Name_Text;
            ProviderUkprnLabel = ManageSpecialismResultContent.Title_Provider_Ukprn_Text;
            TlevelTitleLabel = ManageSpecialismResultContent.Title_TLevel_Text;
        }

        public int ProfileId { get; set; }
        public int AssessmentId { get; set; }
        public string AssessmentSeries { get; set; }
        public DateTime? AppealEndDate { get; set; }
        public string SpecialismName { get; set; }
        public string SpecialismDisplayName { get; set; }

        public int? ResultId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ManageSpecialismResultContent), ErrorMessageResourceName = "Validation_Select_Grade_Required_Message")]
        public string SelectedGradeCode { get; set; }
        public int? LookupId { get; set; }
        public PrsStatus? PrsStatus { get; set; }

        public List<LookupViewModel> Grades { get; set; }
        public bool IsValid => (PrsStatus.HasValue == false || PrsStatus == EnumPrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(AppealEndDate);

        public override BackLinkModel BackLink => new()
        {
            RouteName = RouteConstants.ResultDetails,
            RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ProfileId.ToString() } }
        };
    }
}

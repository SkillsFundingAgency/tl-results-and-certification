using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryItem;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryList;
using System.Collections.Generic;
using System.Linq;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.Registration.CheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual
{
    public class CheckAndSubmitViewModel
    {
        public RegistrationViewModel RegistrationModel { get; set; }
        public bool IsCheckAndSubmitPageValid => RegistrationModel != null && RegistrationModel.Uln != null
            && RegistrationModel.LearnersName != null && RegistrationModel.DateofBirth != null && RegistrationModel.SelectProvider != null
            && RegistrationModel.SelectCore != null && RegistrationModel.SpecialismQuestion != null
            && ((RegistrationModel.SpecialismQuestion.HasLearnerDecidedSpecialism == true && RegistrationModel.SelectSpecialism != null)
            || (RegistrationModel.SpecialismQuestion.HasLearnerDecidedSpecialism == false && RegistrationModel.SelectSpecialism == null))
            && RegistrationModel.SelectAcademicYear != null;

        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = RegistrationModel.Uln.Uln, RouteName = RouteConstants.AddRegistrationUln, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = CheckAndSubmitContent.Title_Name_Text, Value = $"{RegistrationModel.LearnersName.Firstname} {RegistrationModel.LearnersName.Lastname}", RouteName = RouteConstants.AddRegistrationLearnersName, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = CheckAndSubmitContent.Title_DateofBirth_Text, Value = $"{RegistrationModel.DateofBirth.Day}/{RegistrationModel.DateofBirth.Month}/{RegistrationModel.DateofBirth.Year}", RouteName = RouteConstants.AddRegistrationDateofBirth, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = RegistrationModel.SelectProvider.SelectedProviderDisplayName, RouteName = RouteConstants.AddRegistrationProvider, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = CheckAndSubmitContent.Title_Core_Text, Value = RegistrationModel.SelectCore.SelectedCoreDisplayName, RouteName = RouteConstants.AddRegistrationCore, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = CheckAndSubmitContent.Title_Specialism_Text, Value = GetSelectedSpecialisms, RouteName = RouteConstants.AddRegistrationSpecialism, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, HiddenText = GetSpecialismHiddenText };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = CheckAndSubmitContent.Title_AcademicYear_Text, Value = EnumExtensions.GetDisplayName<AcademicYear>(RegistrationModel.SelectAcademicYear.SelectedAcademicYear), RouteName = RouteConstants.AddRegistrationAcademicYear, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };

        public List<string> GetSelectedSpecialisms => RegistrationModel.SelectSpecialism != null ? RegistrationModel.SelectSpecialism.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).OrderBy(s => s.DisplayName).Select(s => s.DisplayName).ToList() : null;
        public string GetSpecialismHiddenText => (RegistrationModel.SelectSpecialism == null || !RegistrationModel.SelectSpecialism.PathwaySpecialisms.Specialisms.Any(x => x.IsSelected)) ? CheckAndSubmitContent.Specialism_None_Selected_Text : null;
        public Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddRegistrationAcademicYear };
    }
}

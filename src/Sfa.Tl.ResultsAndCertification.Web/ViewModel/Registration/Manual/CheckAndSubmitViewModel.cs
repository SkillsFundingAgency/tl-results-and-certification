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
            && ((RegistrationModel.SpecialismQuestion.HasLearnerDecidedSpecialism == true && RegistrationModel.SelectSpecialisms != null)
            || (RegistrationModel.SpecialismQuestion.HasLearnerDecidedSpecialism == false && RegistrationModel.SelectSpecialisms == null))
            && RegistrationModel.SelectAcademicYear != null;

        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = RegistrationModel.Uln.Uln, RouteName = RouteConstants.AddRegistrationUln, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryLearnerName => new SummaryItemModel { Id = "learnername", Title = CheckAndSubmitContent.Title_Name_Text, Value = $"{RegistrationModel.LearnersName.Firstname} {RegistrationModel.LearnersName.Lastname}", RouteName = RouteConstants.AddRegistrationLearnersName, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryDateofBirth => new SummaryItemModel { Id = "dateofbirth", Title = CheckAndSubmitContent.Title_DateofBirth_Text, Value = $"{RegistrationModel.DateofBirth.Day}/{RegistrationModel.DateofBirth.Month}/{RegistrationModel.DateofBirth.Year}", RouteName = RouteConstants.AddRegistrationDateofBirth, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = RegistrationModel.SelectProvider.SelectedProviderDisplayName, RouteName = RouteConstants.AddRegistrationProvider, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = CheckAndSubmitContent.Title_Core_Text, Value = RegistrationModel.SelectCore.SelectedCoreDisplayName, RouteName = RouteConstants.AddRegistrationCore, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = CheckAndSubmitContent.Title_Specialism_Text, Value = GetSelectedSpecialisms, RouteName = GetSpecialismRouteName, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, HiddenText = GetSpecialismHiddenText, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = CheckAndSubmitContent.Title_AcademicYear_Text, Value = EnumExtensions.GetDisplayName<AcademicYear>(RegistrationModel.SelectAcademicYear.SelectedAcademicYear), RouteName = RouteConstants.AddRegistrationAcademicYear, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };

        public List<string> GetSelectedSpecialisms => RegistrationModel.SelectSpecialisms?.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).OrderBy(s => s.DisplayName).Select(s => s.DisplayName).ToList();
        public string GetSpecialismHiddenText => (RegistrationModel.SelectSpecialisms == null || !RegistrationModel.SelectSpecialisms.PathwaySpecialisms.Specialisms.Any(x => x.IsSelected)) ? CheckAndSubmitContent.Specialism_None_Selected_Text : null;
        public string GetSpecialismRouteName => RegistrationModel.SelectSpecialisms == null ? RouteConstants.AddRegistrationSpecialisms : RouteConstants.AddRegistrationSpecialismQuestion;
        public Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.IsChangeMode, "true" } };
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.AddRegistrationAcademicYear };

        public RegistrationViewModel ResetChangeMode()
        {
            RegistrationModel.Uln.IsChangeMode = false;
            RegistrationModel.LearnersName.IsChangeMode = false;
            RegistrationModel.DateofBirth.IsChangeMode = false;
            RegistrationModel.SelectProvider.IsChangeMode = false;
            RegistrationModel.SelectCore.IsChangeMode = false;
            RegistrationModel.SpecialismQuestion.IsChangeMode = false;

            if (RegistrationModel.SelectSpecialisms != null)
            {
                RegistrationModel.SelectSpecialisms.IsChangeMode = false;
            }
            return RegistrationModel;
        }
    }
}

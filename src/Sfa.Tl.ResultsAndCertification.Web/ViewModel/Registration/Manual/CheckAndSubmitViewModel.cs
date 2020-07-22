using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.BackLink;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.Summary.SummaryComponent;
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
            && RegistrationModel.AcademicYear != null;

        public SummaryComponentViewModel SummaryUln => new SummaryComponentViewModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = RegistrationModel.Uln.Uln, RouteName = RouteConstants.AddRegistrationUln, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummaryLearnerName => new SummaryComponentViewModel { Id = "learnername", Title = CheckAndSubmitContent.Title_Name_Text, Value = $"{RegistrationModel.LearnersName.Firstname} {RegistrationModel.LearnersName.Lastname}", RouteName = RouteConstants.AddRegistrationLearnersName, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummaryDateofBirth => new SummaryComponentViewModel { Id = "dateofbirth", Title = CheckAndSubmitContent.Title_DateofBirth_Text, Value = $"{RegistrationModel.DateofBirth.Day}/{RegistrationModel.DateofBirth.Month}/{RegistrationModel.DateofBirth.Year}", RouteName = RouteConstants.AddRegistrationUln, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummaryProvider => new SummaryComponentViewModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = RegistrationModel.SelectProvider.SelectedProviderDisplayName, RouteName = RouteConstants.AddRegistrationProvider, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummaryCore => new SummaryComponentViewModel { Id = "core", Title = CheckAndSubmitContent.Title_Core_Text, Value = RegistrationModel.SelectCore.SelectedCoreDisplayName, RouteName = RouteConstants.AddRegistrationCore, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummarySpecialisms => new SummaryComponentViewModel { Id = "specialisms", Title = CheckAndSubmitContent.Title_Specialism_Text, Value = "", RouteName = RouteConstants.AddRegistrationSpecialism, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };
        public SummaryComponentViewModel SummaryAcademicYear => new SummaryComponentViewModel { Id = "academicyear", Title = CheckAndSubmitContent.Title_AcademicYear_Text, Value = RegistrationModel.AcademicYear.AcademicYear.ToString(), RouteName = RouteConstants.AddRegistrationAcademicYear, ActionText = CheckAndSubmitContent.Change_Action_Link_Text };

        public BackLinkModel BackLink
        {
            get
            {
                return new BackLinkModel
                {
                    RouteName = RouteConstants.AddRegistrationAcademicYear
                };
            }
        }
    }
}

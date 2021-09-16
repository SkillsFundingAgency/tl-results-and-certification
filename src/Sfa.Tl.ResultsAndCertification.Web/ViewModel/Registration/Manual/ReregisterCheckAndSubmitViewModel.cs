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
    public class ReregisterCheckAndSubmitViewModel
    {
        public long Uln { get; set; }
        public ReregisterViewModel ReregisterModel { get; set; }
        public bool IsCheckAndSubmitPageValid => ReregisterModel != null && ReregisterModel.ReregisterProvider != null
            && ReregisterModel.ReregisterCore != null && ReregisterModel.SpecialismQuestion != null
            && ((ReregisterModel.SpecialismQuestion.HasLearnerDecidedSpecialism == true && ReregisterModel.ReregisterSpecialisms != null)
            || (ReregisterModel.SpecialismQuestion.HasLearnerDecidedSpecialism == false && ReregisterModel.ReregisterSpecialisms == null))
            && ReregisterModel.ReregisterAcademicYear != null;

        public SummaryItemModel SummaryUln => new SummaryItemModel { Id = "uln", Title = CheckAndSubmitContent.Title_Uln_Text, Value = Uln.ToString() };
        public SummaryItemModel SummaryProvider => new SummaryItemModel { Id = "provider", Title = CheckAndSubmitContent.Title_Provider_Text, Value = ReregisterModel.ReregisterProvider.SelectedProviderDisplayName, RouteName = RouteConstants.ReregisterProvider, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryCore => new SummaryItemModel { Id = "core", Title = CheckAndSubmitContent.Title_Core_Text, Value = ReregisterModel.ReregisterCore.SelectedCoreDisplayName, RouteName = RouteConstants.ReregisterCore, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryListModel SummarySpecialisms => new SummaryListModel { Id = "specialisms", Title = CheckAndSubmitContent.Title_Specialism_Text, Value = GetSelectedSpecialisms, RouteName = GetSpecialismRouteName, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, HiddenText = GetSpecialismHiddenText, RouteAttributes = ChangeLinkRouteAttributes };
        public SummaryItemModel SummaryAcademicYear => new SummaryItemModel { Id = "academicyear", Title = CheckAndSubmitContent.Title_AcademicYear_Text, Value = EnumExtensions.GetDisplayName<AcademicYearDelete>(ReregisterModel.ReregisterAcademicYear.SelectedAcademicYear), RouteName = RouteConstants.ReregisterAcademicYear, ActionText = CheckAndSubmitContent.Change_Action_Link_Text, RouteAttributes = ChangeLinkRouteAttributes };

        public List<string> GetSelectedSpecialisms => ReregisterModel.ReregisterSpecialisms != null ? ReregisterModel.ReregisterSpecialisms.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).OrderBy(s => s.DisplayName).Select(s => s.DisplayName).ToList() : null;
        public string GetSpecialismHiddenText => (ReregisterModel.ReregisterSpecialisms == null || !ReregisterModel.ReregisterSpecialisms.PathwaySpecialisms.Specialisms.Any(x => x.IsSelected)) ? CheckAndSubmitContent.Specialism_None_Selected_Text : null;
        public string GetSpecialismRouteName => ReregisterModel.ReregisterSpecialisms == null ? RouteConstants.ReregisterSpecialisms : RouteConstants.ReregisterSpecialismQuestion;
        public Dictionary<string, string> ChangeLinkRouteAttributes => new Dictionary<string, string> { { Constants.ProfileId, ReregisterModel.ReregisterAcademicYear.ProfileId.ToString() }, { Constants.IsChangeMode, "true" } };
        public BackLinkModel BackLink => new BackLinkModel { RouteName = RouteConstants.ReregisterAcademicYear, RouteAttributes = new Dictionary<string, string> { { Constants.ProfileId, ReregisterModel.ReregisterAcademicYear.ProfileId.ToString() } } };

        public ReregisterViewModel ResetChangeMode()
        {
            ReregisterModel.ReregisterProvider.IsChangeMode = false;
            ReregisterModel.ReregisterCore.IsChangeMode = false;
            ReregisterModel.SpecialismQuestion.IsChangeMode = false;

            if (ReregisterModel.ReregisterSpecialisms != null)
            {
                ReregisterModel.ReregisterSpecialisms.IsChangeMode = false;
            }
            return ReregisterModel;
        }
    }
}

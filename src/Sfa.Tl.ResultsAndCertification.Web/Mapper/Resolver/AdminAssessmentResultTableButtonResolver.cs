using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.TableButton;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using LearnerRecordContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver
{
    public class AdminAssessmentResultTableButtonResolver : IValueResolver<Assessment, AdminAssessmentViewModel, TableButtonModel>
    {
        private readonly ISystemProvider _systemProvider;

        public AdminAssessmentResultTableButtonResolver(ISystemProvider systemProvider)
        {
            _systemProvider = systemProvider;
        }

        public TableButtonModel Resolve(Assessment source, AdminAssessmentViewModel destination, TableButtonModel destMember, ResolutionContext context)
        {
            AdminAssessmentResultStatus status = source.GetAdminAssessmentResultStatus(_systemProvider.Today);

            return status switch
            {
                AdminAssessmentResultStatus.WithoutGrade
                    => new TableButtonModel(LearnerRecordContent.Action_Button_Remove_Entry, "admin-remove-entry-route", null),

                AdminAssessmentResultStatus.OpenRommAllowed
                    => new TableButtonModel(LearnerRecordContent.Action_Button_Open_Romm, "admin-remove-entry-route", null),

                AdminAssessmentResultStatus.AddRommOutcomeAllowed
                    => new TableButtonModel(LearnerRecordContent.Action_Button_Add_Outcome, "admin-add-romm-outcome-route", null),

                AdminAssessmentResultStatus.OpenAppealAllowed
                    => new TableButtonModel(LearnerRecordContent.Action_Button_Open_Appeal, "admin-add-appeal-route", null),

                AdminAssessmentResultStatus.AddAppealOutcomeAllowed
                    => new TableButtonModel(LearnerRecordContent.Action_Button_Add_Outcome, "admin-add-appeal-outcome-route", null),

                _ => null
            };
        }
    }
}
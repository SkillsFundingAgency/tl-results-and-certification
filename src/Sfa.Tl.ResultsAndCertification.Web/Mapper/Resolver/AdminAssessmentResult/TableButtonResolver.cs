using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Common.Services.System.Interface;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewComponents.TableButton;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminDashboard.LearnerRecord;
using System.Collections.Generic;
using LearnerRecordContent = Sfa.Tl.ResultsAndCertification.Web.Content.AdminDashboard.LearnerRecord;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver.AdminAssessmentResult
{
    public class TableButtonResolver : IValueResolver<Assessment, AdminAssessmentViewModel, TableButtonModel>
    {
        private readonly ISystemProvider _systemProvider;

        public TableButtonResolver(ISystemProvider systemProvider)
        {
            _systemProvider = systemProvider;
        }

        public TableButtonModel Resolve(Assessment source, AdminAssessmentViewModel destination, TableButtonModel destMember, ResolutionContext context)
        {
            int registrationPathwayId = (int)context.Items[Constants.RegistrationPathwayId];
            AdminAssessmentResultStatus status = source.GetAdminAssessmentResultStatus(_systemProvider.Today);

            return CreateTableButton(registrationPathwayId, source.Id, source.ComponentType, status);
        }

        private static TableButtonModel CreateTableButton(int registrationPathwayId, int assessmentId, ComponentType componentType, AdminAssessmentResultStatus status)
        {
            if (componentType == ComponentType.NotSpecified || status == AdminAssessmentResultStatus.NotSpecified || status == AdminAssessmentResultStatus.Final)
            {
                return null;
            }

            string buttonText = _buttonTextLookup.GetValueOrDefault(status);
            string route = _routelookup.GetValueOrDefault((componentType, status));

            return new TableButtonModel(buttonText, route, new Dictionary<string, string>
            {
                [Constants.RegistrationPathwayId] = registrationPathwayId.ToString(),
                [Constants.AssessmentId] = assessmentId.ToString()
            });
        }

        private static readonly Dictionary<AdminAssessmentResultStatus, string> _buttonTextLookup = new()
        {
            [AdminAssessmentResultStatus.WithoutGrade] = LearnerRecordContent.Action_Button_Remove_Entry,
            [AdminAssessmentResultStatus.OpenRommAllowed] = LearnerRecordContent.Action_Button_Open_Romm,
            [AdminAssessmentResultStatus.AddRommOutcomeAllowed] = LearnerRecordContent.Action_Button_Add_Outcome,
            [AdminAssessmentResultStatus.OpenAppealAllowed] = LearnerRecordContent.Action_Button_Open_Appeal,
            [AdminAssessmentResultStatus.AddAppealOutcomeAllowed] = LearnerRecordContent.Action_Button_Add_Outcome
        };

        private static readonly Dictionary<(ComponentType, AdminAssessmentResultStatus), string> _routelookup = new()
        {
            [(ComponentType.Core, AdminAssessmentResultStatus.WithoutGrade)] = RouteConstants.RemoveAssessmentEntryCoreClear,
            [(ComponentType.Specialism, AdminAssessmentResultStatus.WithoutGrade)] = RouteConstants.RemoveAssessmentSpecialismEntryClear,
            [(ComponentType.Core, AdminAssessmentResultStatus.OpenRommAllowed)] = RouteConstants.AdminOpenPathwayRommClear,
            [(ComponentType.Specialism, AdminAssessmentResultStatus.OpenRommAllowed)] = RouteConstants.AdminOpenSpecialismRommClear,
            [(ComponentType.Core, AdminAssessmentResultStatus.AddRommOutcomeAllowed)] = RouteConstants.AdminAddCoreRommOutcomeClear,
            [(ComponentType.Specialism, AdminAssessmentResultStatus.AddRommOutcomeAllowed)] = RouteConstants.AdminAddSpecialismRommOutcomeClear,
            [(ComponentType.Core, AdminAssessmentResultStatus.OpenAppealAllowed)] = RouteConstants.AdminOpenPathwayAppealClear,
            [(ComponentType.Specialism, AdminAssessmentResultStatus.OpenAppealAllowed)] = RouteConstants.AdminOpenSpecialismAppealClear,
            [(ComponentType.Core, AdminAssessmentResultStatus.AddAppealOutcomeAllowed)] = RouteConstants.AdminAddCoreAppealOutcomeClear,
            [(ComponentType.Specialism, AdminAssessmentResultStatus.AddAppealOutcomeAllowed)] = RouteConstants.AdminAddSpecialismAppealOutcomeClear,
        };
    }
}
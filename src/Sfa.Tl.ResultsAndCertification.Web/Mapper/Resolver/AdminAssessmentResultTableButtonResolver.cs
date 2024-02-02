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
            if (source?.Result == null)
            {
                return null;
            }

            Result result = source.Result;
            bool hasGrade = !string.IsNullOrWhiteSpace(result.Grade);

            if (!hasGrade)
            {
                return new TableButtonModel
                {
                    Text = LearnerRecordContent.Action_Button_Remove_Entry,
                    Route = "admin-remove-entry-route"
                };
            }

            PrsStatus? prsStatus = result.PrsStatus;

            bool isValidGradeForPrsJourney = CommonHelper.IsValidGradeForPrsJourney(result.GradeCode, source.ComponentType);

            bool isAddRommAllowed = hasGrade && (!prsStatus.HasValue || prsStatus == PrsStatus.NotSpecified) && CommonHelper.IsAppealsAllowed(source.AppealEndDate, _systemProvider.Today) && isValidGradeForPrsJourney;
            if (isAddRommAllowed)
            {
                return new TableButtonModel
                {
                    Text = LearnerRecordContent.Action_Button_Add_Romm,
                    Route = "admin-add-romm-route"
                };
            }

            bool isAddRommOutcomeAllowed = prsStatus == PrsStatus.UnderReview && isValidGradeForPrsJourney;
            if (isAddRommOutcomeAllowed)
            {
                return new TableButtonModel
                {
                    Text = LearnerRecordContent.Action_Button_Add_Outcome,
                    Route = "admin-add-romm-outcome-route"
                };
            }

            bool isOpenAppealAllowed = prsStatus == PrsStatus.Reviewed && CommonHelper.IsAppealsAllowed(source.AppealEndDate, _systemProvider.Today) && isValidGradeForPrsJourney;
            if (isOpenAppealAllowed)
            {
                return new TableButtonModel
                {
                    Text = LearnerRecordContent.Action_Button_Open_Appeal,
                    Route = "admin-add-appeal-route"
                };
            }

            bool isAddAppealOutcomeAllowed = prsStatus == PrsStatus.BeingAppealed && isValidGradeForPrsJourney;
            if (isAddAppealOutcomeAllowed)
            {
                return new TableButtonModel
                {
                    Text = LearnerRecordContent.Action_Button_Add_Outcome,
                    Route = "admin-add-appeal-outcome-route"
                };
            }

            //bool isRequestChangeAllowed = isValidGradeForPrsJourney && (((!prsStatus.HasValue || prsStatus == PrsStatus.NotSpecified) && !CommonHelper.IsRommAllowed(source.RommEndDate))
            //                               || (prsStatus == PrsStatus.Reviewed && !CommonHelper.IsAppealsAllowed(source.AppealEndDate))
            //                               || prsStatus == PrsStatus.Final);

            //if (isRequestChangeAllowed)
            //{
            //    return new TableButtonModel
            //    {
            //        Text = LearnerRecordContent.Action_Button_Request_Change,
            //        Route = "admin-request-change-route"
            //    };
            //}

            return null;
        }
    }
}
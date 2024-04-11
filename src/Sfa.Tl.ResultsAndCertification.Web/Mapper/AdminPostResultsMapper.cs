using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Content.AdminPostResults;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.AdminPostResults;
using System;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AdminPostResultsMapper : Profile
    {
        public AdminPostResultsMapper()
        {
            CreateMap<AdminLearnerRecord, AdminOpenPathwayRommViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                    {
                        return AdminOpenPathwayRomm.Validation_Widthdrawn;
                    }

                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    string gradeCode = GetPathwayAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);

                    return gradeCode switch
                    {
                        Constants.PathwayComponentGradeQpendingResultCode => AdminOpenPathwayRomm.Validation_Result_Pending,
                        Constants.PathwayComponentGradeXNoResultCode => AdminOpenPathwayRomm.Validation_No_Result,
                        _ => string.Empty
                    };
                }));

            CreateMap<AdminOpenPathwayRommViewModel, AdminOpenPathwayRommReviewChangesViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminOpenPathwayRommReviewChangesViewModel, OpenPathwayRommRequest>()
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.DateOfRequest, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskTicketId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminOpenPathwayRommReviewChangesViewModel, OpenPathwayRommRequest>>());

            CreateMap<AdminLearnerRecord, AdminOpenSpecialismRommViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                    return $"{specialism?.Name} ({specialism.LarId})";

                })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                    {
                        return AdminOpenSpecialismRomm.Validation_Widthdrawn;
                    }

                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    string gradeCode = GetSpecialismAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);

                    return gradeCode switch
                    {
                        Constants.SpecialismComponentGradeQpendingResultCode => AdminOpenSpecialismRomm.Validation_Result_Pending,
                        Constants.SpecialismComponentGradeXNoResultCode => AdminOpenSpecialismRomm.Validation_No_Result,
                        _ => string.Empty
                    };
                }));

            CreateMap<AdminOpenSpecialismRommViewModel, AdminOpenSpecialismRommReviewChangesViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminOpenSpecialismRommReviewChangesViewModel, OpenSpecialismRommRequest>()
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
                .ForMember(d => d.DateOfRequest, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
                .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
                .ForMember(d => d.ZendeskTicketId, opts => opts.MapFrom(s => s.ZendeskTicketId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminOpenSpecialismRommReviewChangesViewModel, OpenSpecialismRommRequest>>());

            CreateMap<AdminLearnerRecord, AdminAddCoreRommOutcomeViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                    {
                        return AdminAddCoreRommOutcome.Validation_Widthdrawn;
                    }

                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    string gradeCode = GetSpecialismAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);

                    return gradeCode switch
                    {
                        Constants.PathwayComponentGradeQpendingResultCode => AdminAddCoreRommOutcome.Validation_Result_Pending,
                        Constants.PathwayComponentGradeXNoResultCode => AdminAddCoreRommOutcome.Validation_No_Result,
                        _ => string.Empty
                    };
                }));

            CreateMap<AdminLearnerRecord, AdminAddSpecialismRommOutcomeViewModel>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                    {
                        return AdminAddSpecialismRommOutcome.Validation_Widthdrawn;
                    }

                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    string gradeCode = GetSpecialismAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);

                    return gradeCode switch
                    {
                        Constants.SpecialismComponentGradeQpendingResultCode => AdminAddSpecialismRommOutcome.Validation_Result_Pending,
                        Constants.SpecialismComponentGradeXNoResultCode => AdminAddSpecialismRommOutcome.Validation_No_Result,
                        _ => string.Empty
                    };
                }));

            CreateMap<AdminLearnerRecord, AdminOpenPathwayAppealViewModel>()

            CreateMap<AdminAddRommOutcomeChangeGradeCoreViewModel, AdminReviewChangesRommOutcomeCoreViewModel>()
                    .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                    .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                    .ForMember(d => d.PathwayResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                    .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                    .ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
              .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
              .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
              .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
              .ForMember(d => d.SummaryGrade, opts => opts.MapFrom(s => s.SummaryGrade))
              .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade))
              .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
                    .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                    .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                    .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                    .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                    .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                    .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetPathwayAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                    .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                    {
                        if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                        {
                            return AdminOpenPathwayAppeal.Validation_Widthdrawn;
                        }
              .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
              .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
              .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
              .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

                        int assessmentId = (int)context.Items[Constants.AssessmentId];
                        string gradeCode = GetPathwayAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);
            CreateMap<AdminReviewChangesRommOutcomeCoreViewModel, ReviewChangesRommOutcomeCoreRequest>()
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
               .ForMember(d => d.ExistingGrade, opts => opts.MapFrom(s => s.Grade))
               .ForMember(d => d.SelectedGrade, opts => opts.MapFrom(s => s.SelectedGradeValue))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.DateOfRequest, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.ZendeskTicketId, opts => opts.MapFrom(s => s.ZendeskTicketId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewChangesRommOutcomeCoreViewModel, ReviewChangesRommOutcomeCoreRequest>>());

                        return gradeCode == Constants.PathwayComponentGradeXNoResultCode
                            ? AdminOpenPathwayAppeal.Validation_No_Result
                            : string.Empty;
                    }));
            CreateMap<AdminAddRommOutcomeChangeGradeSpecialismViewModel, AdminReviewChangesRommOutcomeSpecialismViewModel>()
             .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
             .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom(s => s.SpecialismAssessmentId))
             .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
             .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
             .ForMember(d => d.SummaryGrade, opts => opts.MapFrom(s => s.SummaryGrade))
             .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade))
             .ForMember(d => d.Learner, opts => opts.MapFrom(s => s.Learner))
             .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
             .ForMember(d => d.Provider, opts => opts.MapFrom(s => s.Provider))
             .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Tlevel))
             .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.StartYear))
             .ForMember(d => d.ExamPeriod, opts => opts.MapFrom(s => s.ExamPeriod));

            CreateMap<AdminLearnerRecord, AdminOpenSpecialismAppealViewModel>()
            CreateMap<AdminReviewChangesRommOutcomeSpecialismViewModel, ReviewChangesRommOutcomeSpecialismRequest>()
               .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom(s => s.SpecialismResultId))
               .ForMember(d => d.SelectedGradeId, opts => opts.MapFrom(s => s.SelectedGradeId))
               .ForMember(d => d.ExistingGrade, opts => opts.MapFrom(s => s.Grade))
               .ForMember(d => d.SelectedGrade, opts => opts.MapFrom(s => s.SelectedGradeValue))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.SpecialismAssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.AssessmentId]))
                .ForMember(d => d.SpecialismResultId, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Id)))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    Specialism specialism = src?.Pathway?.Specialisms?.SingleOrDefault(s => s.Assessments.Any(a => a.Id == assessmentId));

                    return $"{specialism?.Name} ({specialism.LarId})";

                })).ForMember(d => d.Learner, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Provider, opts => opts.MapFrom(s => $"{s.Pathway.Provider.Name} ({s.Pathway.Provider.Ukprn})"))
                .ForMember(d => d.Tlevel, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.StartYear, opts => opts.MapFrom(s => $"{s.Pathway.AcademicYear} to {s.Pathway.AcademicYear + 1}"))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.SeriesName)))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => GetSpecialismAssessmentPropertyValue(src, (int)context.Items[Constants.AssessmentId], p => p?.Result?.Grade)))
                .ForMember(d => d.ErrorMessage, opts => opts.MapFrom((src, dest, destMember, context) =>
                {
                    if (src.Pathway.Status == Common.Enum.RegistrationPathwayStatus.Withdrawn)
                    {
                        return AdminOpenSpecialismAppeal.Validation_Widthdrawn;
                    }
               .ForMember(d => d.ContactName, opts => opts.MapFrom(s => s.ContactName))
               .ForMember(d => d.DateOfRequest, opts => opts.MapFrom(s => Convert.ToDateTime(s.DateOfRequest)))
               .ForMember(d => d.ChangeReason, opts => opts.MapFrom(s => s.ChangeReason))
               .ForMember(d => d.ZendeskTicketId, opts => opts.MapFrom(s => s.ZendeskTicketId))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom<UserNameResolver<AdminReviewChangesRommOutcomeSpecialismViewModel, ReviewChangesRommOutcomeSpecialismRequest>>());

                    int assessmentId = (int)context.Items[Constants.AssessmentId];
                    string gradeCode = GetSpecialismAssessmentPropertyValue(src, assessmentId, p => p?.Result?.GradeCode);

                    return gradeCode == Constants.SpecialismComponentGradeXNoResultCode
                            ? AdminOpenPathwayAppeal.Validation_No_Result
                            : string.Empty;
                }));
        }

        private static T GetPathwayAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            var pathwayAssessment = learnerRecord?.Pathway?.PathwayAssessments?.SingleOrDefault(p => p.Id == assessmentId);
            return pathwayAssessment == null ? default : getPropertyValue(pathwayAssessment);
        }

        private static T GetSpecialismAssessmentPropertyValue<T>(AdminLearnerRecord learnerRecord, int assessmentId, Func<Assessment, T> getPropertyValue)
        {
            Assessment specialismAssessment = learnerRecord?.Pathway?.Specialisms?.SelectMany(s => s.Assessments).SingleOrDefault(a => a.Id == assessmentId);
            return specialismAssessment == null ? default : getPropertyValue(specialismAssessment);
        }
    }
}
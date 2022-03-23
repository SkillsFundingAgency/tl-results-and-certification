using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.PostResultsService;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.PostResultsService;
using System.Collections.Generic;
using System.Linq;
using CheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.PostResultsService.PrsPathwayGradeCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class PostResultsServiceMapper : Profile
    {
        public PostResultsServiceMapper()
        {
            CreateMap<PrsLearnerDetails, PrsLearnerDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.PathwayTitle, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayCode})"))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.PathwayGradeLastUpdatedOn, opts => opts.MapFrom(s => s.PathwayGradeLastUpdatedOn.ToDobFormat()))
                .ForMember(d => d.PathwayGradeLastUpdatedBy, opts => opts.MapFrom(s => s.PathwayGradeLastUpdatedBy));

            CreateMap<LearnerRecord, PrsLearnerDetailsViewModel1>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreComponentDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.PrsCoreComponentExams, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments.Where(pa => pa.Result != null && !string.IsNullOrWhiteSpace(pa.Result.Grade))))
                .ForMember(d => d.PrsSpecialismComponents, opts => opts.MapFrom(s => s.Pathway.Specialisms));

            CreateMap<Specialism, PrsSpecialismComponentViewModel>()
                .ForMember(d => d.SpecialismComponentDisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
                .ForMember(d => d.SpecialismComponentExams, opts => opts.MapFrom(s => s.Assessments.Where(sa => sa.Result != null && !string.IsNullOrWhiteSpace(sa.Result.Grade))));

            CreateMap<Assessment, PrsComponentExamViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom((src, dest, destMember, context) => (int)context.Items[Constants.ProfileId]))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom(s => s.RommEndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Result != null ? s.Result.Grade : null))
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedOn.ToDobFormat() : null))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom(s => s.Result != null ? s.Result.LastUpdatedBy : null))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => s.Result != null ? s.Result.PrsStatus : null));

            CreateMap<PrsLearnerDetails, AppealCoreGradeViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
               .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
               .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate));

            CreateMap<AppealCoreGradeViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.BeingAppealed))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AppealCoreGradeViewModel, PrsActivityRequest>>());

            CreateMap<AppealOutcomePathwayGradeViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Withdraw))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AppealOutcomePathwayGradeViewModel, PrsActivityRequest>>());

            CreateMap<PrsPathwayGradeCheckAndSubmitViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.AssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Final))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<PrsPathwayGradeCheckAndSubmitViewModel, PrsActivityRequest>>());

            CreateMap<PrsLearnerDetails, AppealOutcomePathwayGradeViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
               .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus));

            CreateMap<PrsLearnerDetails, AppealUpdatePathwayGradeViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
               .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
               .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
               .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<PrsLearnerDetails, PrsPathwayGradeCheckAndSubmitViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.OldGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.PathwayTitle, opts => opts.MapFrom(s => string.Format(CheckAndSubmitContent.Heading_Pathway_Title, $"{s.PathwayName} ({s.PathwayCode})")))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries));

            CreateMap<FindPrsLearnerRecord, PrsUlnWithdrawnViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<FindPrsLearnerRecord, PrsNoResultsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<LearnerRecord, PrsGradeChangeRequestViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Pathway.Status))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.RommEndDate))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.AppealEndDate))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Id))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus));

            CreateMap<LearnerRecord, PrsCancelGradeChangeRequestViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Pathway.Status))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus));

            CreateMap<PrsLearnerDetails, AppealGradeAfterDeadlineViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status));

            CreateMap<PrsGradeChangeRequestViewModel, PrsGradeChangeRequest>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
                .ForMember(d => d.RequestedMessage, opts => opts.MapFrom(s => s.ChangeRequestData))
                .ForMember(d => d.RequestedUserEmailAddress, opts => opts.MapFrom<UserEmailResolver<PrsGradeChangeRequestViewModel, PrsGradeChangeRequest>>());

            CreateMap<PrsLearnerDetails, AppealGradeAfterDeadlineConfirmViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName}<br/>({s.PathwayCode})"))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate));

            CreateMap<AppealGradeAfterDeadlineConfirmViewModel, AppealGradeAfterDeadlineRequest>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.RequestedUserEmailAddress, opts => opts.MapFrom<UserEmailResolver<AppealGradeAfterDeadlineConfirmViewModel, AppealGradeAfterDeadlineRequest>>());

            // Add Romm

            CreateMap<LearnerRecord, PrsAddRommViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.RommEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus));

            CreateMap<LearnerRecord, PrsAddRommOutcomeViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.RommEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus));

            CreateMap<LearnerRecord, PrsAddRommOutcomeKnownViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))                
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.RommEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Id))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus));

            CreateMap<LearnerRecord, PrsRommGradeChangeViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.RommEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.RommEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<LearnerRecord, PrsRommCheckAndSubmitViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreName, opts => opts.MapFrom(s => s.Pathway.Name))
                .ForMember(d => d.CoreLarId, opts => opts.MapFrom(s => s.Pathway.LarId))
                .ForMember(d => d.SpecialismName, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).Name : null))
                .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialism"] != null ? ((Specialism)context.Items["specialism"]).LarId : null))
                .ForMember(d => d.ExamPeriod, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.SeriesName))
                .ForMember(d => d.Grade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.OldGrade, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Grade))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.PrsStatus))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.ComponentType))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Result?.Id));

            CreateMap<PrsAddRommOutcomeViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.AssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Withdraw))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<PrsAddRommOutcomeViewModel, PrsActivityRequest>>());

            CreateMap<PrsAddRommOutcomeKnownViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.AssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.UnderReview))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<PrsAddRommOutcomeKnownViewModel, PrsActivityRequest>>());

            CreateMap<PrsRommCheckAndSubmitViewModel, PrsActivityRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.AssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Reviewed))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<PrsRommCheckAndSubmitViewModel, PrsActivityRequest>>());            
        }
    }
}
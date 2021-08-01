using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
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
               .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus));

            CreateMap<AppealCoreGradeViewModel, AppealGradeRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.BeingAppealed))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AppealCoreGradeViewModel, AppealGradeRequest>>());

            CreateMap<AppealOutcomePathwayGradeViewModel, AppealGradeRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Withdraw))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AppealOutcomePathwayGradeViewModel, AppealGradeRequest>>());

            CreateMap<PrsPathwayGradeCheckAndSubmitViewModel, AppealGradeRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AssessentId, opts => opts.MapFrom(s => s.AssessmentId))
               .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
               .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
               .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => PrsStatus.Final))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<PrsPathwayGradeCheckAndSubmitViewModel, AppealGradeRequest>>());

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

            CreateMap<FindPrsLearnerRecord, PrsNoAssessmentEntryViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<FindPrsLearnerRecord, PrsNoGradeRegisteredViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessments.FirstOrDefault().SeriesName));

            CreateMap<FindPrsLearnerRecord, PrsSelectAssessmentSeriesViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.AssessmentSerieses, opts => opts.MapFrom(s => s.PathwayAssessments));

            CreateMap<PrsLearnerDetails, PrsGradeChangeRequestViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayCode})"))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => s.PathwayGrade))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus));
        }
    }
}
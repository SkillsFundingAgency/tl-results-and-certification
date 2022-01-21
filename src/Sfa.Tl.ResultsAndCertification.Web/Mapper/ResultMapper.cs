using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Common;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class ResultMapper : Profile
    {
        public ResultMapper()
        {
            CreateMap<UploadResultsRequestViewModel, BulkProcessRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.Results))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadResultsRequestViewModel, BulkProcessRequest>>());

            CreateMap<BulkResultResponse, UploadResultsResponseViewModel>()
                   .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
                   .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
                   .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<FindUlnResponse, UlnResultsNotFoundViewModel>()
               .ForMember(d => d.RegistrationProfileId, opts => opts.MapFrom(s => s.RegistrationProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.IsRegisteredWithOtherAo, opts => opts.MapFrom(s => s.IsRegisteredWithOtherAo))
               .ForMember(d => d.IsAllowed, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Active || s.Status == RegistrationPathwayStatus.Withdrawn))
               .ForMember(d => d.IsWithdrawn, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Withdrawn));

            CreateMap<ResultDetails, ResultDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.PathwayResult, opts => opts.MapFrom(s => s.PathwayResult))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.PathwayStatus, opts => opts.MapFrom(s => s.Status));

            CreateMap<ResultDetails, ResultWithdrawnViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn));

            CreateMap<ResultDetailsViewModel, ResultNoAssessmentEntryViewModel>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn));

            CreateMap<ManageCoreResultViewModel, AddResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageCoreResultViewModel, AddResultRequest>>());

            CreateMap<LearnerRecord, ManageCoreResultViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).SeriesName?.ToLowerInvariant()))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.AppealEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"])?.Id))
                .ForMember(d => d.ResultId, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Results.FirstOrDefault()?.Id))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Results.FirstOrDefault()?.PrsStatus))
                .ForMember(d => d.SelectedGradeCode, opts => opts.MapFrom((src, dest, destMember, context) => ((Assessment)context.Items["assessment"]).Results.FirstOrDefault() != null ? ((Assessment)context.Items["assessment"]).Results.FirstOrDefault().Grade : string.Empty))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<ResultDetails, ManageCoreResultViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.PathwayPrsStatus, opts => opts.MapFrom(s => s.PathwayPrsStatus))
                .ForMember(d => d.SelectedGradeCode, opts => opts.MapFrom(s => s.PathwayResultId.HasValue ? s.PathwayResultCode : string.Empty))
                .ForMember(d => d.Grades, opts => opts.MapFrom((src, dest, destMember, context) => (IList<LookupData>)context.Items["grades"]));

            CreateMap<ManageCoreResultViewModel, ChangeResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ResultId, opts => opts.MapFrom(s => s.ResultId))
                .ForMember(d => d.LookupId, opts => opts.MapFrom(s => s.LookupId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Core))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ManageCoreResultViewModel, ChangeResultRequest>>());

            CreateMap<LearnerRecord, ResultDetailsViewModelNew>()
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.CoreComponentDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.CoreComponentExams, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments))
                .ForMember(d => d.SpecialismComponents, opts => opts.MapFrom(s => s.Pathway.Specialisms));

            CreateMap<Specialism, SpecialismComponentViewModel>()
                .ForMember(d => d.SpecialismComponentDisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"));

            CreateMap<Assessment, ComponentExamViewModel>()
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.AssessmentSeries, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.AppealEndDate, opts => opts.MapFrom(s => s.AppealEndDate))
                .ForMember(d => d.Grade, opts => opts.MapFrom(s => !s.Results.Any() ? null : s.Results.FirstOrDefault().Grade)) // TODO: Tech Debt result should be flat in LearnerDetails?
                .ForMember(d => d.LastUpdated, opts => opts.MapFrom(s => !s.Results.Any() ? null : s.Results.FirstOrDefault().LastUpdatedOn.ToDobFormat()))
                .ForMember(d => d.UpdatedBy, opts => opts.MapFrom(s => !s.Results.Any() ? null : s.Results.FirstOrDefault().LastUpdatedBy))
                .ForMember(d => d.PrsStatus, opts => opts.MapFrom(s => !s.Results.Any() ? null : s.Results.FirstOrDefault().PrsStatus));
        }
    }
}
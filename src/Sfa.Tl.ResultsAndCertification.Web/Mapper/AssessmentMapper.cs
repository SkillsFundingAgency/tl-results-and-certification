using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Learner;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class AssessmentMapper : Profile
    {
        public AssessmentMapper()
        {
            CreateMap<UploadAssessmentsRequestViewModel, BulkProcessRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.Assessments))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadAssessmentsRequestViewModel, BulkProcessRequest>>());

            CreateMap<BulkAssessmentResponse, UploadAssessmentsResponseViewModel>()
               .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<BulkUploadStats, BulkUploadStatsViewModel>();

            CreateMap<FindUlnResponse, UlnAssessmentsNotFoundViewModel>()
                .ForMember(d => d.RegistrationProfileId, opts => opts.MapFrom(s => s.RegistrationProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.IsRegisteredWithOtherAo, opts => opts.MapFrom(s => s.IsRegisteredWithOtherAo))
                .ForMember(d => d.IsAllowed, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Active || s.Status == RegistrationPathwayStatus.Withdrawn))
                .ForMember(d => d.IsWithdrawn, opts => opts.MapFrom(s => s.Status == RegistrationPathwayStatus.Withdrawn));

            CreateMap<LearnerRecord, AssessmentUlnWithdrawnViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title));

            CreateMap<LearnerRecord, AddAssessmentEntryViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
               .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"));

            CreateMap<AvailableAssessmentSeries, AddAssessmentEntryViewModel>()
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeriesId))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeriesName.ToLower()))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId));

            CreateMap<AddAssessmentEntryViewModel, AddAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeriesId))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddAssessmentEntryViewModel, AddAssessmentEntryRequest>>());

            CreateMap<LearnerRecord, AssessmentEntryDetailsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
               .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"));

            CreateMap<AssessmentEntryDetails, AssessmentEntryDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeriesName.ToLower()));

            CreateMap<AssessmentEntryDetailsViewModel, RemoveAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AssessmentEntryDetailsViewModel, RemoveAssessmentEntryRequest>>());

            CreateMap<RemoveSpecialismAssessmentEntryViewModel, RemoveAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
// TODO: RG                .ForMember(d => d.SpecialismAssessmentIds, opts => opts.MapFrom(s => s.SpecialismDetails.Where(sd => s.SpecialismAssessmentIdList.Contains(sd.LarId, StringComparer.InvariantCultureIgnoreCase)).Select(x => x.Id)))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<RemoveSpecialismAssessmentEntryViewModel, RemoveAssessmentEntryRequest>>());

            CreateMap<LearnerRecord, RemoveSpecialismAssessmentEntryViewModel>()
              .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
              .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
              .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
              .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
              .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
              .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
              .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => s.Pathway.Specialisms));

            CreateMap<AssessmentEntryDetails, RemoveSpecialismAssessmentEntryViewModel>()
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeriesName.ToLower()));

            CreateMap<LearnerRecord, AddSpecialismAssessmentEntryViewModel>()
              .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
              .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
              .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
              .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
              .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
              .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
              .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
              .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
              .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom((src, dest, destMember, context) => src.Pathway.Specialisms));

            CreateMap<AvailableAssessmentSeries, AddSpecialismAssessmentEntryViewModel>()
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeriesId))
                .ForMember(d => d.AssessmentSeriesName, opts => opts.MapFrom(s => s.AssessmentSeriesName.ToLower()))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId));

            CreateMap<AddSpecialismAssessmentEntryViewModel, AddAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeriesId))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => ComponentType.Specialism))
                .ForMember(d => d.SpecialismIds, opts => opts.MapFrom(s => s.SpecialismIds))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddSpecialismAssessmentEntryViewModel, AddAssessmentEntryRequest>>());

            CreateMap<LearnerRecord, AssessmentDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.Pathway.Provider.Name))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.Pathway.Provider.Ukprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.Pathway.Title))
                .ForMember(d => d.PathwayStatus, opts => opts.MapFrom(s => s.Pathway.Status))
                .ForMember(d => d.IsCoreEntryEligible, opts => opts.MapFrom((src, dest, destMember, context) => src.Pathway.Status == RegistrationPathwayStatus.Active && (int)context.Items["currentCoreAssessmentSeriesId"] > 0))
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.Pathway.Id))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.Pathway.Name} ({s.Pathway.LarId})"))
                .ForMember(d => d.NextAvailableCoreSeries, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["coreSeriesName"]))
                .ForMember(d => d.PathwayAssessment, opts => opts.MapFrom((src, dest, destMember, context) => src.Pathway.PathwayAssessments.FirstOrDefault(a => a.SeriesId == (int)context.Items["currentCoreAssessmentSeriesId"])))
                .ForMember(d => d.PreviousPathwayAssessment, opts => opts.MapFrom((src, dest, destMember, context) => src.Pathway.PathwayAssessments.Where(a => a.SeriesId != (int)context.Items["currentCoreAssessmentSeriesId"]).OrderByDescending(a => a.SeriesId).FirstOrDefault()))
                .ForMember(d => d.SpecialismDetails, opts => opts.MapFrom(s => s.Pathway.Specialisms))
                .ForMember(d => d.IsSpecialismEntryEligible, opts => opts.MapFrom((src, dest, destMember, context) => src.Pathway.Status == RegistrationPathwayStatus.Active && (int)context.Items["currentSpecialismAssessmentSeriesId"] > 0))
                .ForMember(d => d.NextAvailableSpecialismSeries, opts => opts.MapFrom((src, dest, destMember, context) => context.Items["specialismSeriesName"]))
                .ForMember(d => d.IsCoreResultExist, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments.Any() && s.Pathway.PathwayAssessments.Any(a => a.Results.Any())))
                .ForMember(d => d.HasAnyOutstandingPathwayPrsActivities, opts => opts.MapFrom(s => s.Pathway.PathwayAssessments.Any() && s.Pathway.PathwayAssessments.Any(a => a.Results.Any(r => r.PrsStatus == PrsStatus.BeingAppealed))))
                .ForMember(d => d.IsIndustryPlacementExist, opts => opts.MapFrom(s => s.Pathway.IndustryPlacements.Any()));

            CreateMap<Assessment, PathwayAssessmentViewModel>()
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.SeriesId, opts => opts.MapFrom(s => s.SeriesId))
                .ForMember(d => d.SeriesName, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.LastUpdatedOn))
                .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.LastUpdatedBy))
                .ForMember(d => d.Results, opts => opts.MapFrom(s => s.Results));

            CreateMap<Result, ResultViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Grade, opts => opts.MapFrom(s => s.Grade))
               .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.LastUpdatedOn))
               .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.LastUpdatedBy));

            CreateMap<Specialism, SpecialismViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.LarId, opts => opts.MapFrom(s => s.LarId))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => $"{s.Name} ({s.LarId})"))
               .ForMember(d => d.CurrentSpecialismAssessmentSeriesId, opts => opts.MapFrom((src, dest, destMember, context) => context.Items != null && context.Items["currentSpecialismAssessmentSeriesId"] != null ? (int?)context.Items["currentSpecialismAssessmentSeriesId"] : null))              
               .ForMember(d => d.TlSpecialismCombinations, opts => opts.MapFrom(s => s.TlSpecialismCombinations))
               .ForMember(d => d.Assessments, opts => opts.MapFrom(s => s.Assessments));

            CreateMap<Assessment, SpecialismAssessmentViewModel>()
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.SeriesId, opts => opts.MapFrom(s => s.SeriesId))
                .ForMember(d => d.SeriesName, opts => opts.MapFrom(s => s.SeriesName))
                .ForMember(d => d.LastUpdatedOn, opts => opts.MapFrom(s => s.LastUpdatedOn))
                .ForMember(d => d.LastUpdatedBy, opts => opts.MapFrom(s => s.LastUpdatedBy));

        }
    }
}

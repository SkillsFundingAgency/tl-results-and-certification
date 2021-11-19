using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Assessment.Manual;
using System;
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

            CreateMap<AssessmentDetails, AssessmentDetailsViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.IsCoreEntryEligible, opts => opts.MapFrom(s => s.IsCoreEntryEligible))
                .ForMember(d => d.NextAvailableCoreSeries, opts => opts.MapFrom(s => s.NextAvailableCoreSeries))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.IsCoreEntryEligible ? s.PathwayAssessmentSeries : null))
                .ForMember(d => d.PathwayAssessmentId, opts => opts.MapFrom(s => s.PathwayAssessmentId))
                .ForMember(d => d.IsSpecialismEntryEligible, opts => opts.MapFrom(s => s.IsSpecialismEntryEligible))
                .ForMember(d => d.NextAvailableSpecialismSeries, opts => opts.MapFrom(s => s.NextAvailableSpecialismSeries))
                .ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom(s => string.Join(Constants.AndSeperator, s.Specialisms.OrderBy(x => x.Name).Select(x => $"{x.Name} ({x.Code})"))))
                //.ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismLarId) ? $"{s.SpecialismName} ({s.SpecialismLarId})" : null))
                .ForMember(d => d.SpecialismAssessmentSeries, opts => opts.MapFrom(s => s.SpecialismAssessmentSeries))
                .ForMember(d => d.IsCoreResultExist, opts => opts.MapFrom(s => s.PathwayResultId > 0))
                .ForMember(d => d.HasAnyOutstandingPathwayPrsActivities, opts => opts.MapFrom(s => s.HasAnyOutstandingPathwayPrsActivities))
                .ForMember(d => d.IsIndustryPlacementExist, opts => opts.MapFrom(s => s.IsIndustryPlacementExist))
                .ForMember(d => d.PathwayStatus, opts => opts.MapFrom(s => s.Status));

            CreateMap<AssessmentDetails, AssessmentUlnWithdrawnViewModel>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<AvailableAssessmentSeries, AddAssessmentEntryViewModel>();
            CreateMap<AddAssessmentEntryViewModel, AddAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.AssessmentSeriesId, opts => opts.MapFrom(s => s.AssessmentSeriesId))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddAssessmentEntryViewModel, AddAssessmentEntryRequest>>());
            
            CreateMap<AssessmentEntryDetails, AssessmentEntryDetailsViewModel>();

            CreateMap<AssessmentEntryDetailsViewModel, RemoveAssessmentEntryRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.AssessmentId, opts => opts.MapFrom(s => s.AssessmentId))
                .ForMember(d => d.ComponentType, opts => opts.MapFrom(s => s.ComponentType))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AssessmentEntryDetailsViewModel, RemoveAssessmentEntryRequest>>());            
        }
    }
}

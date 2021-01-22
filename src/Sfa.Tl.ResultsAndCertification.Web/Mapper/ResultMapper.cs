using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using System;

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
                .ForMember(d => d.Name, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
                .ForMember(d => d.ProviderDisplayName, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
                .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayLarId})"))
                .ForMember(d => d.PathwayAssessmentSeries, opts => opts.MapFrom(s => s.PathwayAssessmentSeries))
                .ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismLarId) ? $"{s.SpecialismName} ({s.SpecialismLarId})" : null))
                .ForMember(d => d.PathwayResult, opts => opts.MapFrom(s => s.PathwayResult))
                .ForMember(d => d.PathwayResultId, opts => opts.MapFrom(s => s.PathwayResultId))
                .ForMember(d => d.PathwayStatus, opts => opts.MapFrom(s => s.Status));

            CreateMap<AddResultViewModel, AddResultRequest>()
                .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.TqPathwayAssessmentId, opts => opts.MapFrom(s => s.TqPathwayAssessmentId))
                .ForMember(d => d.TlLookupId, opts => opts.MapFrom(s => s.TlLookupId))                
                .ForMember(d => d.AssessmentEntryType, opts => opts.MapFrom(s => s.AssessmentEntryType))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddResultViewModel, AddResultRequest>>());
        }
    }
}
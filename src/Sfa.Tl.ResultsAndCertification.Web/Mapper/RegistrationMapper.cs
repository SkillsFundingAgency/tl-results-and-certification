using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using System;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.Registrations))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>>());

            CreateMap<BulkRegistrationResponse, UploadRegistrationsResponseViewModel>()
               .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<BulkUploadStats, StatsViewModel>()
               .ForMember(d => d.NewRecordsCount, opts => opts.MapFrom(s => s.NewRecordsCount))
               .ForMember(d => d.UpdatedRecordsCount, opts => opts.MapFrom(s => s.UpdatedRecordsCount))
               .ForMember(d => d.DuplicateRecordsCount, opts => opts.MapFrom(s => s.DuplicateRecordsCount));
        }
    }
}

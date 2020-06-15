using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => s.BlobFileName))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => s.FileType))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => s.DocumentType))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>>());

            CreateMap<BulkRegistrationResponse, UploadRegistrationsResponseViewModel>()
               .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<Stats, StatsViewModel>()
               .ForMember(d => d.NewRecordsCount, opts => opts.MapFrom(s => s.NewRecordsCount))
               .ForMember(d => d.UpdatedRecordsCount, opts => opts.MapFrom(s => s.UpdatedRecordsCount))
               .ForMember(d => d.DuplicateRecordsCount, opts => opts.MapFrom(s => s.DuplicateRecordsCount));
        }
    }
}

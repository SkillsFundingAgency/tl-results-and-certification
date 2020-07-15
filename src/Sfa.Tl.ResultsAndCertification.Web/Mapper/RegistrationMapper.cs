using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;

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

            CreateMap<BulkUploadStats, BulkUploadStatsViewModel>()
               .ForMember(d => d.NewRecordsCount, opts => opts.MapFrom(s => s.NewRecordsCount))
               .ForMember(d => d.AmendedRecordsCount, opts => opts.MapFrom(s => s.AmendedRecordsCount))
               .ForMember(d => d.UnchangedRecordsCount, opts => opts.MapFrom(s => s.UnchangedRecordsCount));

            CreateMap<ProviderDetails, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => $"{s.DisplayName} ({s.Ukprn})"))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.Ukprn.ToString()))
                .ForAllOtherMembers(s => s.Ignore());

            CreateMap<IList<ProviderDetails>, SelectProviderViewModel>()
               .ForMember(d => d.ProvidersSelectList, opts => opts.MapFrom(s => s))
               .ForAllOtherMembers(d => d.Ignore());

            CreateMap<CoreDetails, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => $"{s.CoreName} ({s.CoreCode})"))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.CoreCode.ToString()))
                .ForAllOtherMembers(s => s.Ignore());

            CreateMap<IList<CoreDetails>, SelectCoreViewModel>()
               .ForMember(d => d.CoreSelectList, opts => opts.MapFrom(s => s))
               .ForAllOtherMembers(d => d.Ignore());
        }
    }
}

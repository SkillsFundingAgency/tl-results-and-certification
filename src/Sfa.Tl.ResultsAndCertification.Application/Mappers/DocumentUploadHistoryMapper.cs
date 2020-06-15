using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class DocumentUploadHistoryMapper : Profile
    {
        public DocumentUploadHistoryMapper()
        {
            CreateMap<DocumentUploadHistoryDetails, DocumentUploadHistory>()
                .ForMember(d => d.TlAwardingOrganisationId, opts => opts.MapFrom(s => s.TlAwardingOrganisationId))
                .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => s.BlobFileName))
                .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
                .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => s.DocumentType))
                .ForMember(d => d.FileType, opts => opts.MapFrom(s => s.FileType))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
                .ReverseMap();
        }
    }
}

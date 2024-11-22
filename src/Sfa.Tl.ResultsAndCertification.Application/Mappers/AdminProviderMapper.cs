using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminProvider;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminProviderMapper : Profile
    {
        public AdminProviderMapper()
        {
            CreateMap<TlProvider, GetProviderResponse>()
                .ForMember(d => d.ProviderId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => s.UkPrn))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => s.IsActive));

            CreateMap<AddProviderRequest, TlProvider>()
                .ForMember(d => d.UkPrn, opts => opts.MapFrom(s => s.UkPrn))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName))
                .ForMember(d => d.IsActive, opts => opts.MapFrom(s => true))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(src => default(string)))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom(src => default(DateTime?)));
        }
    }
}
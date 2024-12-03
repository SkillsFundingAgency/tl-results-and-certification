using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminBanner;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminBannerMapper : Profile
    {
        public AdminBannerMapper()
        {
            CreateMap<Banner, GetBannerResponse>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.IsOptedin, opts => opts.MapFrom(s => s.IsOptedin));

            CreateMap<AddBannerRequest, Banner>()
               .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
               .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
               .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
               .ForMember(d => d.IsOptedin, opts => opts.MapFrom(s => true))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
               .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(src => default(string)))
               .ForMember(d => d.ModifiedOn, opts => opts.MapFrom(src => default(DateTime?)));
        }
    }
}
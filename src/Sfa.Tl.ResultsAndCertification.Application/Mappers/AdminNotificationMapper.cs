using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminNotification;
using System;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminNotificationMapper : Profile
    {
        public AdminNotificationMapper()
        {
            CreateMap<Notification, GetNotificationResponse>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
                .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
                .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
                .ForMember(d => d.IsActive, opts => opts.MapFrom((src, dest, destMember, context) => IsActive((DateTime)context.Items["today"], src)));

            CreateMap<AddNotificationRequest, Notification>()
               .ForMember(d => d.Title, opts => opts.MapFrom(s => s.Title))
               .ForMember(d => d.Content, opts => opts.MapFrom(s => s.Content))
               .ForMember(d => d.Target, opts => opts.MapFrom(s => s.Target))
               .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.CreatedBy))
               .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(src => default(string)))
               .ForMember(d => d.ModifiedOn, opts => opts.MapFrom(src => default(DateTime?)));
        }

        private static bool IsActive(DateTime today, Notification banner)
            => banner.Start <= today && today <= banner.End;
    }
}
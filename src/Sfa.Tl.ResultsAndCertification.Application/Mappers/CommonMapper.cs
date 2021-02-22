using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CommonMapper : Profile
    {
        public CommonMapper()
        {
            CreateMap<TlLookup, LookupData>();

            CreateMap<FunctionLog, FunctionLogDetails>();

            CreateMap<FunctionLogDetails, FunctionLog>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.StartDate, opts => opts.MapFrom(s => s.StartDate))
                .ForMember(d => d.EndDate, opts => opts.MapFrom(s => s.EndDate))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.Status))
                .ForMember(d => d.Message, opts => opts.MapFrom(s => s.Message))
                .ForMember(d => d.CreatedBy, opts =>
                {
                    opts.PreCondition(src => (src.Id == 0));
                    opts.MapFrom(s => s.PerformedBy);
                })
                .ForMember(d => d.ModifiedBy, opts =>
                {
                    opts.PreCondition(src => src.Id > 0);
                    opts.MapFrom(s => s.PerformedBy);
                })
                .ForMember(d => d.ModifiedOn, opts =>
                {
                    opts.PreCondition(src => src.Id > 0);
                    opts.MapFrom<DateTimeResolver<FunctionLogDetails, FunctionLog>>();
                });
        }
    }
}
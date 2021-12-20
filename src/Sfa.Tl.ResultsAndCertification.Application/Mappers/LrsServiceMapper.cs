using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Functions;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class LrsServiceMapper : Profile
    {
        public LrsServiceMapper()
        {
            CreateMap<TqRegistrationProfile, RegisteredLearnerDetails>();

            CreateMap<TqRegistrationProfile, RegisteredLearnerDetails>()
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.UniqueLearnerNumber))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.Lastname))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
                .ForMember(d => d.Gender, opts => opts.MapFrom(s => s.Gender))
                .ForMember(d => d.IsLearnerVerified, opts => opts.MapFrom(s => s.IsLearnerVerified))
                .ForMember(d => d.IsEnglishAndMathsAchieved, opts => opts.MapFrom(s => s.IsEnglishAndMathsAchieved))
                .ForMember(d => d.IsSendLearner, opts => opts.MapFrom(s => s.IsSendLearner))
                .ForMember(d => d.IsRcFeed, opts => opts.MapFrom(s => s.IsRcFeed));
        }
    }
}

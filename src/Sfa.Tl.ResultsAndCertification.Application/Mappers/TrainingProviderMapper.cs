using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Application.Mappers.Resolver;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<UpdateLearnerRecordRequest, IndustryPlacement>()
                .ForMember(d => d.Status, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(s => s.PerformedBy))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom<DateTimeResolver<UpdateLearnerRecordRequest, IndustryPlacement>>());

            CreateMap<UpdateLearnerRecordRequest, TqRegistrationProfile>()
                .ForMember(d => d.IsEnglishAndMathsAchieved, opts => opts.MapFrom(s => s.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.Achieved || s.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.AchievedWithSend))
                .ForMember(d => d.IsSendLearner, opts => opts.MapFrom(s => s.EnglishAndMathsStatus.Value == EnglishAndMathsStatus.AchievedWithSend ? true : (bool?)null))
                .ForMember(d => d.ModifiedBy, opts => opts.MapFrom(s => s.PerformedBy))
                .ForMember(d => d.ModifiedOn, opts => opts.MapFrom<DateTimeResolver<UpdateLearnerRecordRequest, TqRegistrationProfile>>());
        }
    }
}
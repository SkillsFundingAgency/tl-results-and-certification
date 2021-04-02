using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<LearnerRecordDetails, LearnerRecordDetailsViewModel>()               
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.IsLearnerRegistered))
               .ForMember(d => d.IsLearnerRecordAdded, opts => opts.MapFrom(s => s.IsLearnerRecordAdded))
               .ForMember(d => d.IsEnglishAndMathsAchieved, opts => opts.MapFrom(s => s.IsEnglishAndMathsAchieved))
               .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.HasLrsEnglishAndMaths))
               .ForMember(d => d.IsSendLearner, opts => opts.MapFrom(s => s.IsSendLearner))
               .ForMember(d => d.IndustryPlacementId, opts => opts.MapFrom(s => s.IndustryPlacementId))               
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus));

            CreateMap<LearnerRecordDetails, UpdateIndustryPlacementQuestionViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.IndustryPlacementId, opts => opts.MapFrom(s => s.IndustryPlacementId))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
               .ForMember(d => d.IsLearnerRecordAdded, opts => opts.MapFrom(s => s.IsLearnerRecordAdded));

            CreateMap<AddLearnerRecordViewModel, AddLearnerRecordRequest>()
               .ForMember(d => d.Ukprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln.EnterUln))
               .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.LearnerRecord.HasLrsEnglishAndMaths))
               .ForMember(d => d.EnglishAndMathsStatus, opts => opts.MapFrom(s => s.LearnerRecord.HasLrsEnglishAndMaths ? null : s.EnglishAndMathsQuestion.EnglishAndMathsStatus))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementQuestion.IndustryPlacementStatus))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<AddLearnerRecordViewModel, AddLearnerRecordRequest>>());

            CreateMap<UpdateIndustryPlacementQuestionViewModel, UpdateLearnerRecordRequest>()
               .ForMember(d => d.Ukprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.IndustryPlacementId, opts => opts.MapFrom(s => s.IndustryPlacementId))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
               .ForMember(d => d.HasIndustryPlacementChanged, opts => opts.MapFrom(s => true))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UpdateIndustryPlacementQuestionViewModel, UpdateLearnerRecordRequest>>());
        }
    }
}
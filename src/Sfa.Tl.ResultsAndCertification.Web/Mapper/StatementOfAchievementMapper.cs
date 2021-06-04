using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.ProviderAddress;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.StatementOfAchievement;
using RequestSoaCheckAndSubmitContent = Sfa.Tl.ResultsAndCertification.Web.Content.StatementOfAchievement.RequestSoaCheckAndSubmit;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class StatementOfAchievementMapper : Profile
    {
        public StatementOfAchievementMapper()
        {
            CreateMap<SoaLearnerRecordDetails, SoaLearnerRecordDetailsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.Firstname} {s.Lastname}"))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.ProviderDisplayName, opts => opts.MapFrom(s => $"{s.ProviderName} ({s.ProviderUkprn})"))
               .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.ProviderName))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle))
               .ForMember(d => d.PathwayDisplayName, opts => opts.MapFrom(s => $"{s.PathwayName} ({s.PathwayCode})"))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.PathwayName))
               .ForMember(d => d.PathwayCode, opts => opts.MapFrom(s => s.PathwayCode))
               .ForMember(d => d.PathwayGrade, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.PathwayGrade) ? s.PathwayGrade : RequestSoaCheckAndSubmitContent.None))
               .ForMember(d => d.SpecialismDisplayName, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismName) ? $"{s.SpecialismName} ({s.SpecialismCode})" : RequestSoaCheckAndSubmitContent.Not_Specified))
               .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => s.SpecialismName))
               .ForMember(d => d.SpecialismCode, opts => opts.MapFrom(s => s.SpecialismCode))
               .ForMember(d => d.SpecialismGrade, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.SpecialismGrade) ? s.SpecialismGrade : RequestSoaCheckAndSubmitContent.None))
               .ForMember(d => d.IsEnglishAndMathsAchieved, opts => opts.MapFrom(s => s.IsEnglishAndMathsAchieved))
               .ForMember(d => d.HasLrsEnglishAndMaths, opts => opts.MapFrom(s => s.HasLrsEnglishAndMaths))
               .ForMember(d => d.IsSendLearner, opts => opts.MapFrom(s => s.IsSendLearner))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
               .ForMember(d => d.ProviderAddress, opts => opts.MapFrom(s => s.ProviderAddress))
               .ForMember(d => d.HasPathwayResult, opts => opts.MapFrom(s => s.HasPathwayResult))
               .ForMember(d => d.IsIndustryPlacementAdded, opts => opts.MapFrom(s => s.IsIndustryPlacementAdded))
               .ForMember(d => d.IsLearnerRegistered, opts => opts.MapFrom(s => s.IsLearnerRegistered))
               .ForMember(d => d.IsNotWithdrawn, opts => opts.MapFrom(s => s.IsNotWithdrawn))
               .ForMember(d => d.IsIndustryPlacementCompleted, opts => opts.MapFrom(s => s.IsIndustryPlacementCompleted));

            CreateMap<Address, AddressViewModel>()
                .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.DepartmentName))
                .ForMember(d => d.OrganisationName, opts => opts.MapFrom(s => s.OrganisationName))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddressLine2))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.Town))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.Postcode));
        }
    }
}
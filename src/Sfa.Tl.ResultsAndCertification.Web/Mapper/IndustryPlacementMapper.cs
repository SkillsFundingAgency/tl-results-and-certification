using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class IndustryPlacementMapper : Profile
    {
        public IndustryPlacementMapper()
        {
            CreateMap<LearnerRecordDetails, IpCompletionViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathwayId))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus));

            CreateMap<IpLookupData, IpLookupDataViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.IsSelected, opts => opts.MapFrom(s => false));

            // Transformations from IpCompletionViewModel
            CreateMap<IpCompletionViewModel, IpModelUsedViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus));

            CreateMap<IpCompletionViewModel, IpMultiEmployerUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpTempFlexibilityUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpBlendedPlacementUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap< IList<IpLookupData>, IpMultiEmployerOtherViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom((src, dest, destMember, context) => (string)context.Items["learnerName"]))
               .ForMember(d => d.OtherIpPlacementModels, opts => opts.MapFrom(s => s));

            CreateMap<IList<IpLookupData>, IpMultiEmployerSelectViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom((src, dest, destMember, context) => (string)context.Items["learnerName"]))
               .ForMember(d => d.PlacementModels, opts => opts.MapFrom(s => s));            

            CreateMap<IpLookupData, IpLookupDataViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name));

            CreateMap<IpCompletionViewModel, SpecialConsiderationHoursViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, SpecialConsiderationReasonsViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));
        }
    }
}

using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.IndustryPlacement.Manual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class IndustryPlacementMapper : Profile
    {
        public IndustryPlacementMapper()
        {
            CreateMap<UploadIndustryPlacementsRequestViewModel, BulkProcessRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.IndustryPlacements))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadIndustryPlacementsRequestViewModel, BulkProcessRequest>>());

            CreateMap<BulkIndustryPlacementResponse, UploadIndustryPlacementsResponseViewModel>()
               .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<BulkUploadStats, BulkUploadStatsViewModel>();

            CreateMap<LearnerRecordDetails, IpCompletionViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
               .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.TlPathwayId))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus));

            CreateMap<LearnerRecordDetails, IpCheckAndSubmitViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.DateofBirth))
               .ForMember(d => d.TlevelTitle, opts => opts.MapFrom(s => s.TlevelTitle));

            CreateMap<IpLookupData, IpLookupDataViewModel>()
               .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
               .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
               .ForMember(d => d.IsSelected, opts => opts.MapFrom(s => false));

            // Transformations from IpCompletionViewModel
            CreateMap<IpCompletionViewModel, IpModelUsedViewModel>()
               .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpMultiEmployerUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpTempFlexibilityUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpBlendedPlacementUsedViewModel>()
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IList<IpLookupData>, IpMultiEmployerOtherViewModel>()
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
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpEmployerLedUsedViewModel>()
              .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IpGrantedTempFlexibilityViewModel>()
              .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => s.LearnerName));

            CreateMap<IpCompletionViewModel, IndustryPlacementRequest>()
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.ProfileId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.RegistrationPathwayId))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IndustryPlacementStatus))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<IpCompletionViewModel, IndustryPlacementRequest>>());

            CreateMap<IndustryPlacementViewModel, IndustryPlacementRequest>()
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["providerUkprn"]))
                .ForMember(d => d.ProfileId, opts => opts.MapFrom(s => s.IpCompletion.ProfileId))
                .ForMember(d => d.PathwayId, opts => opts.MapFrom(s => s.IpCompletion.PathwayId))
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.IpCompletion.RegistrationPathwayId))
                .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IpCompletion.IndustryPlacementStatus))
                .ForMember(d => d.IndustryPlacementDetails, opts => opts.MapFrom(s => s))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<IndustryPlacementViewModel, IndustryPlacementRequest>>());

            CreateMap<IndustryPlacementViewModel, IndustryPlacementDetails>()
              .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => s.IpCompletion.IndustryPlacementStatus))
              .ForMember(d => d.HoursSpentOnPlacement, opts => opts.MapFrom(s => s.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration ? s.SpecialConsideration.Hours.Hours : null))
              .ForMember(d => d.SpecialConsiderationReasons, opts => opts.MapFrom(s => s.IpCompletion.IndustryPlacementStatus == IndustryPlacementStatus.CompletedWithSpecialConsideration ? s.SpecialConsideration.Reasons.ReasonsList.Where(r => r.IsSelected).Select(r => r.Id).ToList() : null))
              .ForMember(d => d.IndustryPlacementModelsUsed, opts => opts.MapFrom(s => s.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value))
              .ForMember(d => d.MultipleEmployerModelsUsed, opts => opts.MapFrom(s => s.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value ? s.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed.Value : (bool?)null))
              .ForMember(d => d.OtherIndustryPlacementModels, opts => opts.MapFrom(s => (s.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value && s.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed.Value) ? s.IpModelViewModel.IpMultiEmployerOther.OtherIpPlacementModels.Where(r => r.IsSelected).Select(r => r.Id).ToList() : null))
              .ForMember(d => d.IndustryPlacementModels, opts => opts.MapFrom(s => (s.IpModelViewModel.IpModelUsed.IsIpModelUsed.Value && s.IpModelViewModel.IpMultiEmployerUsed.IsMultiEmployerModelUsed.Value == false) ? s.IpModelViewModel.IpMultiEmployerSelect.PlacementModels.Where(r => r.IsSelected).Select(r => r.Id).ToList() : null))
              .ForMember(d => d.TemporaryFlexibilitiesUsed, opts => opts.MapFrom(s => (s.TempFlexibility != null && s.TempFlexibility.IpTempFlexibilityUsed.IsTempFlexibilityUsed != null) ? s.TempFlexibility.IpTempFlexibilityUsed.IsTempFlexibilityUsed.Value : (bool?)null))
              .ForMember(d => d.BlendedTemporaryFlexibilityUsed, opts => opts.MapFrom(s => (s.TempFlexibility != null && s.TempFlexibility.IpBlendedPlacementUsed != null) ? s.TempFlexibility.IpBlendedPlacementUsed.IsBlendedPlacementUsed.Value : (bool?)null))
              .ForMember(d => d.TemporaryFlexibilities, opts => opts.MapFrom(s => (s.TempFlexibility != null && s.TempFlexibility.IpEmployerLedUsed != null)
                                                                                      ? s.TempFlexibility.IpEmployerLedUsed.TemporaryFlexibilities.Where(r => r.IsSelected).Select(r => r.Id).ToList()
                                                                                      : (s.TempFlexibility != null && s.TempFlexibility.IpGrantedTempFlexibility != null)
                                                                                      ? s.TempFlexibility.IpGrantedTempFlexibility.TemporaryFlexibilities.Where(r => r.IsSelected).Select(r => r.Id).ToList()
                                                                                      : null));
        }
    }
}

using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Models.BulkProcess;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.Mapper.Resolver;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Web.Mapper
{
    public class RegistrationMapper : Profile
    {
        public RegistrationMapper()
        {
            CreateMap<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom(s => s.AoUkprn))
               .ForMember(d => d.BlobFileName, opts => opts.MapFrom(s => $"{DateTime.Now.ToFileTimeUtc()}.{FileType.Csv}"))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => Guid.NewGuid()))
               .ForMember(d => d.FileType, opts => opts.MapFrom(s => FileType.Csv))
               .ForMember(d => d.DocumentType, opts => opts.MapFrom(s => DocumentType.Registrations))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<UploadRegistrationsRequestViewModel, BulkRegistrationRequest>>());

            CreateMap<BulkRegistrationResponse, UploadRegistrationsResponseViewModel>()
               .ForMember(d => d.IsSuccess, opts => opts.MapFrom(s => s.IsSuccess))
               .ForMember(d => d.BlobUniqueReference, opts => opts.MapFrom(s => s.BlobUniqueReference))
               .ForMember(d => d.Stats, opts => opts.MapFrom(s => s.Stats));

            CreateMap<BulkUploadStats, BulkUploadStatsViewModel>()
               .ForMember(d => d.NewRecordsCount, opts => opts.MapFrom(s => s.NewRecordsCount))
               .ForMember(d => d.AmendedRecordsCount, opts => opts.MapFrom(s => s.AmendedRecordsCount))
               .ForMember(d => d.UnchangedRecordsCount, opts => opts.MapFrom(s => s.UnchangedRecordsCount));

            CreateMap<RegistrationViewModel, RegistrationRequest>()
               .ForMember(d => d.AoUkprn, opts => opts.MapFrom((src, dest, destMember, context) => (long)context.Items["aoUkprn"]))
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln.Uln))
               .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.LearnersName.Firstname))
               .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.LearnersName.Lastname))
               .ForMember(d => d.DateOfBirth, opts => opts.MapFrom(s => $"{s.DateofBirth.Day}/{s.DateofBirth.Month}/{s.DateofBirth.Year}".ToDateTime()))
               .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.SelectProvider.SelectedProviderUkprn.ToLong()))
               .ForMember(d => d.CoreCode, opts => opts.MapFrom(s => s.SelectCore.SelectedCoreCode))
               .ForMember(d => d.SpecialismCodes, opts => opts.MapFrom(s => s.SelectSpecialisms != null ? s.SelectSpecialisms.PathwaySpecialisms.Specialisms.Where(x => x.IsSelected).Select(s => s.Code) : new List<string>()))
               .ForMember(d => d.AcademicYear, opts => opts.MapFrom(s => s.SelectAcademicYear.SelectedAcademicYear))
               .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<RegistrationViewModel, RegistrationRequest>>());

            CreateMap<FindUlnResponse, UlnNotFoundViewModel>();

            CreateMap<RegistrationDetails, RegistrationDetailsViewModel>();
            CreateMap<ManageRegistration, ChangeLearnersNameViewModel>()
                .ReverseMap()
                .ForMember(d => d.FirstName, opts => opts.MapFrom(s => s.Firstname.Trim()))
                .ForMember(d => d.LastName, opts => opts.MapFrom(s => s.Lastname.Trim()))
                .ForMember(d => d.HasProfileChanged, opts => opts.MapFrom(s => true))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ChangeLearnersNameViewModel, ManageRegistration>>());

            CreateMap<ManageRegistration, ChangeDateofBirthViewModel>()
                .ForMember(d => d.Day, opts => opts.MapFrom(s =>  s.DateOfBirth.Day.ToString().PadLeft(2, '0')))
                .ForMember(d => d.Month, opts => opts.MapFrom(s => s.DateOfBirth.Month.ToString().PadLeft(2, '0')))
                .ForMember(d => d.Year, opts => opts.MapFrom(s => s.DateOfBirth.Year))
                .ReverseMap()
                .ForMember(d => d.HasProfileChanged, opts => opts.MapFrom(s => true))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ChangeDateofBirthViewModel, ManageRegistration>>());

            CreateMap<ManageRegistration, ChangeProviderViewModel>()
                .ForMember(d => d.SelectedProviderUkprn, opts => opts.MapFrom(s => s.ProviderUkprn))
                .ForMember(d => d.IncludeSelectOneOption, opts => opts.MapFrom(s => false));

            CreateMap<ChangeProviderViewModel, ManageRegistration>()
                .ForMember(d => d.ProviderUkprn, opts => opts.MapFrom(s => s.SelectedProviderUkprn))
                .ForMember(d => d.HasProviderChanged, opts => opts.MapFrom(s => true))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ChangeProviderViewModel, ManageRegistration>>());

            CreateMap<ManageRegistration, ChangeCoreViewModel>();

            CreateMap<ManageRegistration, ChangeSpecialismQuestionViewModel>()
                .ForMember(d => d.HasLearnerDecidedSpecialism, opts => opts.MapFrom(s => s.SpecialismCodes != null && s.SpecialismCodes.Any()));

            CreateMap<ChangeSpecialismQuestionViewModel, ManageRegistration>()
                .ForMember(d => d.SpecialismCodes, opts => opts.MapFrom(s => new List<string>()))
                .ForMember(d => d.HasSpecialismsChanged, opts => opts.MapFrom(s => true))
                .ForMember(d => d.PerformedBy, opts => opts.MapFrom<UserNameResolver<ChangeSpecialismQuestionViewModel, ManageRegistration>>())
                .ForAllOtherMembers(d => d.Ignore());            
        }
    }
}

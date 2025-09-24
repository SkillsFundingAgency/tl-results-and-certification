using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.DataExport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class ProviderRegistrationsMapper : Profile
    {
        public ProviderRegistrationsMapper()
        {
            CreateMap<TqRegistrationPathway, ProviderRegistrationExport>()
               .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
               .ForMember(d => d.IsPendingWithdrawal, opts => opts.MapFrom(s => s.IsPendingWithdrawal))
               .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname.ToDoubleQuotesWrappedString()))
               .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname.ToDoubleQuotesWrappedString()))
               .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth.ToString("dd-MMM-yyyy")))
               .ForMember(d => d.TLevel, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.TlevelTitle.ToDoubleQuotesWrappedString()))
               .ForMember(d => d.StartYear, opts => opts.MapFrom(s => s.AcademicYear))
               .ForMember(d => d.PathwayName, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.Name.ToDoubleQuotesWrappedString()))
               .ForMember(d => d.PathwayLarId, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlPathway.LarId))
               .ForMember(d => d.SpecialismName, opts => opts.MapFrom(s => GetSpecialismName(s.TqRegistrationSpecialisms)))
               .ForMember(d => d.SpecialismLarId, opts => opts.MapFrom(s => GetSpecialismLarId(s.TqRegistrationSpecialisms)))
               .ForMember(d => d.IndustryPlacementStatus, opts => opts.MapFrom(s => GetIndustryPlacementStatus(s.IndustryPlacements)))
               .ForMember(d => d.EnglishStatus, opts => opts.MapFrom(s => GetSubjectStatus(s.TqRegistrationProfile.EnglishStatus)))
               .ForMember(d => d.MathsStatus, opts => opts.MapFrom(s => GetSubjectStatus(s.TqRegistrationProfile.MathsStatus)));
        }

        private static string GetSpecialismName(ICollection<TqRegistrationSpecialism> specialisms)
            => GetSpecialismProperty(specialisms, sp => sp.Name);

        private static string GetSpecialismLarId(ICollection<TqRegistrationSpecialism> specialisms)
            => GetSpecialismProperty(specialisms, sp => sp.LarId);

        private static string GetSpecialismProperty(ICollection<TqRegistrationSpecialism> specialisms, Func<TlSpecialism, string> getSpecialismProperty)
        {
            if (specialisms.IsNullOrEmpty())
            {
                return string.Empty;
            }

            return specialisms.Count == 1
                ? getSpecialismProperty(specialisms.Single().TlSpecialism).ToDoubleQuotesWrappedString()
                : string.Join(Constants.CommaSeparator, specialisms.Select(s => getSpecialismProperty(s.TlSpecialism))).ToTripleQuotesWrappedString();
        }

        private static string GetIndustryPlacementStatus(IEnumerable<IndustryPlacement> industryPlacements)
        {
            if (industryPlacements.IsNullOrEmpty())
            {
                return string.Empty;
            }

            IndustryPlacement industryPlacement = industryPlacements.First();
            return industryPlacement.Status.GetDisplayName();
        }

        private static string GetSubjectStatus(SubjectStatus? subjectStatus)
            => subjectStatus.HasValue ? subjectStatus.ToString() : string.Empty;
    }
}
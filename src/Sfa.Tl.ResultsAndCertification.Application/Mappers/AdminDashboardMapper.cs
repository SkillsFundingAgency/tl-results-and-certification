using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.ProviderAddress;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class AdminDashboardLearnerMapper : Profile
    {
        public AdminDashboardLearnerMapper()
        {
            CreateMap<TqRegistrationPathway, AdminLearnerRecord>()
                .ForMember(d => d.RegistrationPathwayId, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Firstname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Firstname))
                .ForMember(d => d.Lastname, opts => opts.MapFrom(s => s.TqRegistrationProfile.Lastname))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.TqRegistrationProfile.UniqueLearnerNumber))
                .ForMember(d => d.DateofBirth, opts => opts.MapFrom(s => s.TqRegistrationProfile.DateofBirth))
                .ForMember(d => d.MathsStatus, opts => opts.MapFrom(s => s.TqRegistrationProfile.MathsStatus))
                .ForMember(d => d.EnglishStatus, opts => opts.MapFrom(s => s.TqRegistrationProfile.EnglishStatus))
                .ForMember(d => d.IsPendingWithdrawl, opts => opts.MapFrom(s => s.IsPendingWithdrawal))
                .ForMember(d => d.Pathway, opts => opts.MapFrom(s => s))
                .ForMember(d => d.AwardingOrganisation, opts => opts.MapFrom(s => s.TqProvider.TqAwardingOrganisation.TlAwardingOrganisaton))
                .ForMember(d => d.OverallCalculationStatus, opts => opts.MapFrom(s => GetOverallCalculationStatus(s.OverallResults)))
                .ForMember(d => d.OverallResult, opts => opts.MapFrom(s => GetOverallResult(s.OverallResults)))
                .ForMember(d => d.PrintCertificateId, opts => opts.MapFrom(s => GetPrintCertificateId(s.PrintCertificates)))
                .ForMember(d => d.PrintCertificateType, opts => opts.MapFrom(s => GetPrintCertificateType(s.PrintCertificates)))
                .ForMember(d => d.LastPrintCertificateRequestedDate, opts => opts.MapFrom(s => GetLastPrintCertificateRequestedDate(s.PrintCertificates)))
                .ForMember(d => d.ProviderAddress, opts => opts.MapFrom(s => GetProviderAddress(s.TqProvider.TlProvider.TlProviderAddresses)));

            CreateMap<TlAwardingOrganisation, AwardingOrganisation>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Ukprn, opts => opts.MapFrom(s => s.UkPrn))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.DisplayName, opts => opts.MapFrom(s => s.DisplayName));
        }

        private static CalculationStatus? GetOverallCalculationStatus(ICollection<OverallResult> overallResults)
        {
            if (overallResults.IsNullOrEmpty())
            {
                return null;
            }

            OverallResult overallResult = overallResults.First();
            return overallResult.CalculationStatus;
        }

        private static string GetOverallResult(ICollection<OverallResult> overallResults)
        {
            if (overallResults.IsNullOrEmpty())
            {
                return null;
            }

            OverallResult overallResult = overallResults.First();
            return overallResult.ResultAwarded;
        }

        private static T GetPrintCertificateProperty<T>(ICollection<PrintCertificate> printCertificates, Func<PrintCertificate, T> getProperty)
        {
            PrintCertificate printCertificate = printCertificates.OrderByDescending(c => c.Id).FirstOrDefault();
            return getProperty(printCertificate);
        }

        private static int? GetPrintCertificateId(ICollection<PrintCertificate> printCertificates)
            => GetPrintCertificateProperty(printCertificates, p => p?.Id);

        private static PrintCertificateType? GetPrintCertificateType(ICollection<PrintCertificate> printCertificates)
            => GetPrintCertificateProperty(printCertificates, p => p?.Type);

        private static DateTime? GetLastPrintCertificateRequestedDate(ICollection<PrintCertificate> printCertificates)
            => GetPrintCertificateProperty(printCertificates, p => p?.LastRequestedOn);

        private static Address GetProviderAddress(ICollection<TlProviderAddress> addresses)
        {
            if (addresses.IsNullOrEmpty())
                return null;

            return addresses
                    .OrderByDescending(addr => addr.CreatedOn)
                    .Select(addr => new Address
                    {
                        AddressId = addr.Id,
                        DepartmentName = addr.DepartmentName,
                        OrganisationName = addr.OrganisationName,
                        AddressLine1 = addr.AddressLine1,
                        AddressLine2 = addr.AddressLine2,
                        Town = addr.Town,
                        Postcode = addr.Postcode
                    })
                    .First();
        }
    }
}

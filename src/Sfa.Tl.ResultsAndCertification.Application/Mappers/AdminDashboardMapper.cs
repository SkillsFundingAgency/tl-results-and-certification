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
                .ForMember(d => d.PrintCertificateId, opts => opts.MapFrom(s => GetPrintCertificateId(s)))
                .ForMember(d => d.PrintCertificateType, opts => opts.MapFrom(s => GetPrintCertificateType(s)))
                .ForMember(d => d.LastPrintCertificateRequestedDate, opts => opts.MapFrom(s => GetLastPrintCertificateRequestedDate(s)))
                .ForMember(d => d.ProviderAddress, opts => opts.MapFrom(s => GetProviderAddress(s.TqProvider.TlProvider.TlProviderAddresses)))
                .ForMember(d => d.BatchId, opts => opts.MapFrom(s => GetBatchId(s)))
                .ForMember(d => d.PrintRequestSubmittedOn, opts => opts.MapFrom(s => GetPrintRequestSubmittedOn(s)))
                .ForMember(d => d.PrintingBatchItemStatus, opts => opts.MapFrom(s => GetPrintingBatchItemStatus(s)))
                .ForMember(d => d.PrintingBatchItemStatusChangedOn, opts => opts.MapFrom(s => GetPrintingBatchItemStatusChangedOn(s)))
                .ForMember(d => d.TrackingId, opts => opts.MapFrom(s => GetTrackingId(s)));

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

        private static T GetPrintCertificateProperty<T>(TqRegistrationPathway registrationPathway, Func<PrintCertificate, T> getProperty)
        {
            var allPrintCertificates = registrationPathway?.TqRegistrationProfile?.TqRegistrationPathways
                .SelectMany(p => p.PrintCertificates ?? Enumerable.Empty<PrintCertificate>())
                .OrderByDescending(pc => pc.Id);

            PrintCertificate printCertificate = allPrintCertificates.FirstOrDefault();
            return getProperty(printCertificate);
        }

        private static int? GetPrintCertificateId(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.Id);

        private static PrintCertificateType? GetPrintCertificateType(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.Type);

        private static DateTime? GetLastPrintCertificateRequestedDate(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.LastRequestedOn);

        private static DateTime? GetPrintRequestSubmittedOn(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.PrintBatchItem?.Batch?.RunOn);

        private static int? GetBatchId(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.PrintBatchItem?.BatchId);

        private static string GetTrackingId(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.PrintBatchItem?.TrackingId);

        private static PrintingBatchItemStatus? GetPrintingBatchItemStatus(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.PrintBatchItem?.Status);

        private static DateTime? GetPrintingBatchItemStatusChangedOn(TqRegistrationPathway registrationPathway)
            => GetPrintCertificateProperty(registrationPathway, p => p?.PrintBatchItem?.StatusChangedOn);

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

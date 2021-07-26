using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.Printing;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class PrintingServiceMapper : Profile
    {
        public PrintingServiceMapper()
        {
            CreateMap<Batch, PrintRequest>()
                //.ForMember(d => d.Batch.BatchNumber, opts => opts.MapFrom(s => s.Id))
                //.ForMember(d => d.Batch.BatchDate, opts => opts.MapFrom(s => s.CreatedOn))
                //.ForMember(d => d.Batch.PostalContactCount, opts => opts.MapFrom(s => s.PrintBatchItems.Count))
                //.ForMember(d => d.Batch.TotalCertificateCount, opts => opts.MapFrom(s => s.PrintBatchItems.Sum(x => x.PrintCertificates.Count)))
                .ForMember(d => d.Batch, opts => opts.MapFrom(s => s))
                .ForMember(d => d.PrintData, opts => opts.MapFrom(s => s.PrintBatchItems));

            CreateMap<Batch, PrintBatch>()
                .ForMember(d => d.BatchNumber, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.BatchDate, opts => opts.MapFrom(s => s.CreatedOn.ToPrintBatchDateFormat()))
                .ForMember(d => d.PostalContactCount, opts => opts.MapFrom(s => s.PrintBatchItems.Count))
                .ForMember(d => d.TotalCertificateCount, opts => opts.MapFrom(s => s.PrintBatchItems.Sum(x => x.PrintCertificates.Count)));

            CreateMap<PrintBatchItem, PrintData>()
                //.ForMember(d => d.PostalContact, opts => opts.MapFrom(s => s.TlProviderAddress))
                .ForMember(d => d.PostalContact, opts => opts.MapFrom(s => s))
                .ForMember(d => d.Certificates, opts => opts.MapFrom(s => s.PrintCertificates));

            CreateMap<PrintBatchItem, PostalContact>()
                .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.TlProviderAddress.DepartmentName ?? string.Empty))
                .ForMember(d => d.Name, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.TlProviderAddress.OrganisationName) ? s.TlProviderAddress.OrganisationName : s.TlProviderAddress.TlProvider.Name))
                .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TlProviderAddress.TlProvider.Name))
                .ForMember(d => d.UKPRN, opts => opts.MapFrom(s => s.TlProviderAddress.TlProvider.UkPrn))
                .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.TlProviderAddress.AddressLine1))
                .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.TlProviderAddress.AddressLine2 ?? string.Empty))
                .ForMember(d => d.Town, opts => opts.MapFrom(s => s.TlProviderAddress.Town ?? string.Empty))
                .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.TlProviderAddress.Postcode))
                .ForMember(d => d.CertificateCount, opts => opts.MapFrom(s => s.PrintCertificates.Count));

            //CreateMap<TlProviderAddress, PostalContact>()
            //    .ForMember(d => d.DepartmentName, opts => opts.MapFrom(s => s.DepartmentName))
            //    .ForMember(d => d.Name, opts => opts.MapFrom(s => !string.IsNullOrWhiteSpace(s.OrganisationName) ? s.OrganisationName : s.TlProvider.Name))
            //    .ForMember(d => d.ProviderName, opts => opts.MapFrom(s => s.TlProvider.Name))
            //    .ForMember(d => d.UKPRN, opts => opts.MapFrom(s => s.TlProvider.UkPrn))
            //    .ForMember(d => d.AddressLine1, opts => opts.MapFrom(s => s.AddressLine1))
            //    .ForMember(d => d.AddressLine2, opts => opts.MapFrom(s => s.AddressLine2))
            //    .ForMember(d => d.Town, opts => opts.MapFrom(s => s.Town))
            //    .ForMember(d => d.Postcode, opts => opts.MapFrom(s => s.Postcode));

            CreateMap<PrintCertificate, Certificate>()
                .ForMember(d => d.CertificateNumber, opts => opts.MapFrom(s => s.CertificateNumber))
                .ForMember(d => d.Type, opts => opts.MapFrom(s => EnumExtensions.GetEnum<PrintCertificateType>(s.Type)))
                .ForMember(d => d.Uln, opts => opts.MapFrom(s => s.Uln))
                .ForMember(d => d.LearnerName, opts => opts.MapFrom(s => $"{s.TqRegistrationPathway.TqRegistrationProfile.Firstname} {s.TqRegistrationPathway.TqRegistrationProfile.Lastname}"))
                .ForMember(d => d.LearningDetails, opts => opts.MapFrom(s => JsonConvert.DeserializeObject<LearningDetails>(s.LearningDetails)));
        }
    }
}

using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class StatementOfAchievementMapper : Profile
    {
        public StatementOfAchievementMapper()
        {
            CreateMap<SoaPrintingRequest, Batch>()
                .ForMember(d => d.Type, opts => opts.MapFrom(s => BatchType.Printing))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => BatchStatus.Created))
                .ForMember(d => d.PrintBatchItems, opts => opts.MapFrom(s => s))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.PerformedBy));

            CreateMap<SoaPrintingRequest, ICollection<PrintBatchItem>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<PrintBatchItem>
                    {
                        new PrintBatchItem
                        {
                            TlProviderAddressId = m.PostalAddressId,
                            CreatedBy = m.PerformedBy,
                            PrintCertificates = context.Mapper.Map<IList<PrintCertificate>>(m)
                        }
                    };
                });

            CreateMap<SoaPrintingRequest, ICollection<PrintCertificate>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<PrintCertificate>
                    {
                        new PrintCertificate
                        {
                            Uln = m.Uln,
                            LearnerName = m.LearnerName,
                            TqRegistrationPathwayId = m.TqRegistrationPathwayId,
                            Type = PrintCertificateType.StatementOfAchievement,
                            LearningDetails = JsonConvert.SerializeObject(m.LearningDetails),
                            DisplaySnapshot = JsonConvert.SerializeObject(m.SoaPrintingDetails),
                            Status = PrintCertificateStatus.Created,
                            CreatedBy = m.PerformedBy
                        }
                    };
                });
        }
    }
}

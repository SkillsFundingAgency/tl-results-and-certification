using AutoMapper;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class TrainingProviderMapper : Profile
    {
        public TrainingProviderMapper()
        {
            CreateMap<PrintCertificate, Batch>()
                    .ForMember(d => d.Id, opts => opts.Ignore())
                    .ForMember(d => d.Type, opts => opts.MapFrom(s => BatchType.Printing))
                    .ForMember(d => d.Status, opts => opts.MapFrom(s => BatchStatus.Created))
                    .ForMember(d => d.PrintBatchItems, opts => opts.MapFrom(s => s))
                    .ForMember(d => d.CreatedBy, opts => opts.MapFrom((src, dest, destMember, context) => (string)context.Items["performedBy"]));

            CreateMap<PrintCertificate, ICollection<PrintBatchItem>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<PrintBatchItem>
                    {
                        new()
                        {
                            TlProviderAddressId = (int)context.Items["providerAddressId"],
                            CreatedBy = (string)context.Items["performedBy"],
                            PrintCertificates = context.Mapper.Map<IList<PrintCertificate>>(m)
                        }
                    };
                });

            CreateMap<PrintCertificate, ICollection<PrintCertificate>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<PrintCertificate>
                    {
                        new()
                        {
                            Uln = m.Uln,
                            LearnerName = m.LearnerName,
                            TqRegistrationPathwayId = m.TqRegistrationPathwayId,
                            Type = m.Type,
                            LearningDetails = m.LearningDetails,
                            DisplaySnapshot = m.DisplaySnapshot,
                            IsReprint = true,
                            CreatedBy = (string)context.Items["performedBy"],
                            LastRequestedOn = DateTime.UtcNow
                        }
                    };
                });
        }
    }
}

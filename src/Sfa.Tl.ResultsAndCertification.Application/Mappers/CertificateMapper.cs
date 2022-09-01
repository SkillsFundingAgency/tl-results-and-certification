using AutoMapper;
using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.StatementOfAchievement;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Application.Mappers
{
    public class CertificateMapper : Profile
    {
        public CertificateMapper()
        {
            CreateMap<LearnerResultsPrintingData, Batch>()
                .ForMember(d => d.Type, opts => opts.MapFrom(s => BatchType.Printing))
                .ForMember(d => d.Status, opts => opts.MapFrom(s => BatchStatus.Created))
                .ForMember(d => d.PrintBatchItems, opts => opts.MapFrom(s => s))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => Constants.FunctionPerformedBy));

            CreateMap<LearnerResultsPrintingData, ICollection<PrintBatchItem>>()
                .ConstructUsing((m, context) =>
                {
                    return new List<PrintBatchItem>
                    {
                        new PrintBatchItem
                        {
                            TlProviderAddressId = m.TlProvider.TlProviderAddresses.FirstOrDefault().Id,
                            CreatedBy = Constants.FunctionPerformedBy,
                            PrintCertificates = context.Mapper.Map<IList<PrintCertificate>>(m.OverallResults)
                        }
                    };
                });

            CreateMap<OverallResult, PrintCertificate>()
                .ConstructUsing((m, context) =>
                {
                    var overallResultDetail = JsonConvert.DeserializeObject<OverallResultDetail>(m.Details);

                    var certificateType = m.CalculationStatus == CalculationStatus.Completed ? PrintCertificateType.Certificate
                                                                : PrintCertificateType.StatementOfAchievement;
                    var learningDetails = new LearningDetails
                    {
                        TLevelTitle = overallResultDetail.TlevelTitle,
                        Core = overallResultDetail.PathwayName,
                        CoreGrade = overallResultDetail.PathwayResult,
                        OccupationalSpecialism = context.Mapper.Map<List<OccupationalSpecialismDetails>>(overallResultDetail.SpecialismDetails), // TODO
                        IndustryPlacement = overallResultDetail.IndustryPlacementStatus,
                        Grade = m.ResultAwarded,
                        EnglishAndMaths = "TODO",
                        Date = DateTime.UtcNow.ToDobFormat()
                    };

                    var soaPrintingDetails = new SoaPrintingDetails();

                    return new PrintCertificate
                    {
                        Uln = m.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber,
                        LearnerName = $"{m.TqRegistrationPathway.TqRegistrationProfile.Firstname} {m.TqRegistrationPathway.TqRegistrationProfile.Lastname}",
                        TqRegistrationPathwayId = m.TqRegistrationPathwayId,
                        Type = certificateType,
                        LearningDetails = JsonConvert.SerializeObject(learningDetails),
                        DisplaySnapshot = JsonConvert.SerializeObject(soaPrintingDetails),
                        CreatedBy = Constants.FunctionPerformedBy,
                    };
                });

            CreateMap<OverallSpecialismDetail, OccupationalSpecialismDetails>()
                .ConstructUsing((m, context) =>
                {
                    return new OccupationalSpecialismDetails
                    {
                        Specialism = m.SpecialismName,
                        Grade = m.SpecialismResult
                    };
                });
        }
    }
}

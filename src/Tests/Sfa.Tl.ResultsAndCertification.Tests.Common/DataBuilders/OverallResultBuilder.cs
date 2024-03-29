﻿using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Tests.Common.Helpers;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders
{
    public class OverallResultBuilder
    {
        public OverallResult Build(TqRegistrationPathway tqRegistrationPathway = null)
        {
            tqRegistrationPathway ??= new TqRegistrationPathwayBuilder().Build();
            
            return new OverallResult
            {
                TqRegistrationPathwayId = tqRegistrationPathway.Id,
                Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                ResultAwarded = "Distinction",
                CalculationStatus = CalculationStatus.Completed,
                PublishDate = DateTime.UtcNow.AddDays(7),
                PrintAvailableFrom = null,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                IsOptedin = true,
                CertificateType = PrintCertificateType.Certificate,
                CertificateStatus = CertificateStatus.AwaitingProcessing,
                CreatedBy = Constants.CreatedByUser,
                CreatedOn = Constants.CreatedOn,
                ModifiedBy = Constants.ModifiedByUser,
                ModifiedOn = Constants.ModifiedOn
            };
        }

        public IList<OverallResult> BuildList(IList<TqRegistrationPathway> tqRegistrationPathways = null)
        {
            tqRegistrationPathways ??= new TqRegistrationPathwayBuilder().BuildList();
                       
            return new List<OverallResult>
            {
                new OverallResult
                {
                    TqRegistrationPathwayId = tqRegistrationPathways[0].Id,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                    ResultAwarded = "Distinction*",
                    CalculationStatus = CalculationStatus.Completed,
                    PublishDate = null,
                    PrintAvailableFrom = null,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.Certificate,
                    CertificateStatus = CertificateStatus.AwaitingProcessing,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = tqRegistrationPathways[1].Id,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                    ResultAwarded = "Distinction",
                    CalculationStatus = CalculationStatus.Completed,
                    PublishDate = null,
                    PrintAvailableFrom = null,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.Certificate,
                    CertificateStatus = CertificateStatus.AwaitingProcessing,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                },
                new OverallResult
                {
                    TqRegistrationPathwayId = tqRegistrationPathways[2].Id,
                    Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"10123456\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                    ResultAwarded = "Partial achievement",
                    CalculationStatus = CalculationStatus.PartiallyCompleted,
                    PublishDate = null,
                    PrintAvailableFrom = null,
                    StartDate = DateTime.UtcNow,
                    EndDate = null,
                    IsOptedin = true,
                    CertificateType = PrintCertificateType.StatementOfAchievement,
                    CertificateStatus = CertificateStatus.AwaitingProcessing,
                    CreatedBy = Constants.CreatedByUser,
                    CreatedOn = Constants.CreatedOn,
                    ModifiedBy = Constants.ModifiedByUser,
                    ModifiedOn = Constants.ModifiedOn
                }
            };
        }
    }
}

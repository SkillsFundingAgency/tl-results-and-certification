using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Application.Models;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ResultsAndCertificationConfiguration _configuration;
        private readonly IRepository<OverallResult> _overallResultRepository;
        private readonly ICertificateRepository _certificateRepository;
        private readonly IMapper _mapper;

        public CertificateService(ResultsAndCertificationConfiguration configuration,
            IRepository<OverallResult> overallResultRepository,
            ICertificateRepository certificateRepository,
            IMapper mapper)
        {
            _configuration = configuration;
            _overallResultRepository = overallResultRepository;
            _certificateRepository = certificateRepository;
            _mapper = mapper;
        }

        public async Task<List<LearnerResultsPrintingData>> GetLearnerResultsForPrintingAsync()
        {
            var resultsForPrinting = await _overallResultRepository.GetManyAsync(x =>
                                                x.PrintAvailableFrom.HasValue &&
                                                DateTime.Today >= x.PrintAvailableFrom.Value &&
                                                (x.CalculationStatus == CalculationStatus.Completed || x.CalculationStatus == CalculationStatus.PartiallyCompleted) &&
                                                x.CertificateStatus == CertificateStatus.AwaitingProcessing &&
                                                x.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses.Any() &&
                                                x.IsOptedin && x.EndDate == null &&
                                                x.TqRegistrationPathway.Status == RegistrationPathwayStatus.Active &&
                                                x.TqRegistrationPathway.EndDate == null,
                                                incl => incl.TqRegistrationPathway.TqRegistrationProfile,
                                                incl => incl.TqRegistrationPathway.TqProvider.TlProvider.TlProviderAddresses.OrderByDescending(o => o.CreatedOn).Take(1))
                                            .GroupBy(x => x.TqRegistrationPathway.TqProvider.TlProviderId)
                                            .AsNoTracking()
                                            .Select(x => new LearnerResultsPrintingData { TlProvider = x.First().TqRegistrationPathway.TqProvider.TlProvider, OverallResults = x.ToList() })
                                            .ToListAsync();

            return resultsForPrinting;
        }

        public async Task<List<CertificateResponse>> ProcessCertificatesForPrintingAsync()
        {
            var response = new List<CertificateResponse>();
            var learnerResultsForPrinting = await GetLearnerResultsForPrintingAsync();

            if (learnerResultsForPrinting == null || !learnerResultsForPrinting.Any())
                return null;

            var batchSize = _configuration.CertificatePrintingBatchSettings.ProvidersBatchSize <= 0 ? Constants.CertificatePrintingDefaultProvidersBatchSize : _configuration.CertificatePrintingBatchSettings.ProvidersBatchSize;
            var batchesToProcess = (int)Math.Ceiling(learnerResultsForPrinting.Count / (decimal)batchSize);

            for (var batchIndex = 0; batchIndex < batchesToProcess; batchIndex++)
            {
                var leanersToProcess = learnerResultsForPrinting.Skip(batchIndex * batchSize).Take(batchSize);
                response.Add(await PrepareCertificatesPrintingBatchesAsync(leanersToProcess));
            }

            return response;
        }

        private async Task<CertificateResponse> PrepareCertificatesPrintingBatchesAsync(IEnumerable<LearnerResultsPrintingData> learnersPrintingData)
        {
            // Map
            var printingBatchData = MapToBatch(learnersPrintingData);

            var overallResults = new List<OverallResult>();
            
            var response = await _certificateRepository.SaveCertificatesPrintingDataAsync(printingBatchData, overallResults);

            return new CertificateResponse
            {
                IsSuccess = response.IsSuccess,
                BatchId = response.BatchId,
                ProvidersCount = printingBatchData.PrintBatchItems.Count(),
                CertificatesCreated = response.TotalBatchRecordsCreated - (printingBatchData.PrintBatchItems.Count + 1)
            };
        }

        public Batch MapToBatch(IEnumerable<LearnerResultsPrintingData> learnersPrintingData)
        {
            return _mapper.Map<Batch>(learnersPrintingData);
        }

        public async Task<Batch> CreatePrintingBatchAsync()
        {
            var testData = new List<LearnerResultsPrintingData>
            {
                new LearnerResultsPrintingData
                                {
                                   TlProvider = new TlProvider { Id = 1, UkPrn = 11111111, DisplayName = "Barsley College", Name = "Barsley College", IsActive = true,
                                       TlProviderAddresses = new List<TlProviderAddress> { new TlProviderAddress { Id = 11, DepartmentName = "Dept", AddressLine1 = "Add1", Postcode = "SN1 5JA", Town= "Swindon", IsActive = true } } },

                                   OverallResults = new List<OverallResult>
                                   {
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 1111,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 1111, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 1111111111, Firstname = "first11", Lastname = "last11" } },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Design, Surveying and Planning for Construction\",\"PathwayName\":\"Design, Surveying and Planning\",\"PathwayLarId\":\"60358300\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Distinction"
                                       },
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 1112,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 1112, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 1111111112, Firstname = "first12", Lastname = "last12" } },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Health\",\"PathwayName\":\"Health\",\"PathwayLarId\":\"6037066X\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":null,\"IndustryPlacementStatus\":\"Not completed\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Merit"
                                       },
                                   }
                                },

                                new LearnerResultsPrintingData
                                {
                                   TlProvider = new TlProvider { Id = 2, UkPrn = 22222222, DisplayName = "Walsall College", Name = "Walsall College", IsActive = true,
                                       TlProviderAddresses = new List<TlProviderAddress> { new TlProviderAddress { Id = 22, DepartmentName = "Dept", AddressLine1 = "Add1", Postcode = "SN1 5JA", Town= "Swindon", IsActive = true } } },

                                   OverallResults = new List<OverallResult>
                                   {
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 2221,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 2221, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 2222222221, Firstname = "first21", Lastname = "last21" } },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Education and Childcare\",\"PathwayName\":\"Education and Childcare\",\"PathwayLarId\":\"60358294\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}],\"IndustryPlacementStatus\":\"Completed with special consideration\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Pass"
                                       },
                                       new OverallResult
                                       {
                                           TqRegistrationPathwayId = 2222,
                                           TqRegistrationPathway = new TqRegistrationPathway { Id = 2222, TqRegistrationProfile = new TqRegistrationProfile { UniqueLearnerNumber = 2222222222, Firstname = "first22", Lastname = "last22" } },
                                           CertificateType = PrintCertificateType.Certificate,
                                           Details = "{\"TlevelTitle\":\"T Level in Science\",\"PathwayName\":\"Science\",\"PathwayLarId\":\"60369899\",\"PathwayResult\":\"A*\",\"SpecialismDetails\":[{\"SpecialismName\":\"Surveying and design for construction and the built environment\",\"SpecialismLarId\":\"10123456\",\"SpecialismResult\":\"Distinction\"}, {\"SpecialismName\":\"Civil Engineering\",\"SpecialismLarId\":\"ZTLOS002\",\"SpecialismResult\":\"Merit\"}],\"IndustryPlacementStatus\":\"Completed\",\"OverallResult\":\"Distinction*\"}",
                                           ResultAwarded = "Distinction*"
                                       },
                                   }
                                }
            };

            await Task.CompletedTask;

            // Map
            return _mapper.Map<Batch>(testData);
        }
    }
}

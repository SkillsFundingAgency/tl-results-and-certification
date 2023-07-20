using Microsoft.Extensions.Logging;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasDataService : IUcasDataService
    {
        private readonly IUcasRepository _ucasRepository;
        private readonly IUcasRecordSegment<UcasRecordEntriesSegment> _ucasRecordEntrySegment;
        private readonly IUcasRecordSegment<UcasRecordResultsSegment> _ucasRecordResultsSegment;
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;

        public UcasDataService(IUcasRepository ucasRepository,
            IUcasRecordSegment<UcasRecordEntriesSegment> ucasRecordEntrySegment,
            IUcasRecordSegment<UcasRecordResultsSegment> ucasRecordResultsSegment,
            ResultsAndCertificationConfiguration resultsAndCertificationConfiguration)
        {
            _ucasRepository = ucasRepository;
            _ucasRecordEntrySegment = ucasRecordEntrySegment;
            _ucasRecordResultsSegment = ucasRecordResultsSegment;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
        }

        public async Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType)
        {
            return ucasDataType switch
            {
                UcasDataType.Entries => await ProcessUcasDataRecordEntriesAsync(),
                UcasDataType.Results => await ProcessUcasDataRecordResultsAsync(),
                UcasDataType.Amendments => await ProcessUcasDataRecordAmendmentsAsync(),
                _ => null,
            };
        }

        private async Task<UcasData> ProcessUcasDataRecordEntriesAsync()
        {
            var records = new List<UcasDataRecord>();
            var registrationPathways = await _ucasRepository.GetUcasDataRecordsForEntriesAsync();

            foreach (var pathway in registrationPathways.Where(w => w.TqPathwayAssessments.Any()
                 && w.TqRegistrationSpecialisms.Any()
                 && w.TqRegistrationSpecialisms.Any(x => x.TqSpecialismAssessments.Any())))
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                _ucasRecordEntrySegment.AddCoreSegment(ucasDataComponents, pathway);
                _ucasRecordEntrySegment.AddSpecialismSegment(ucasDataComponents, pathway);
                _ucasRecordEntrySegment.AddOverallResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode);
                _ucasRecordEntrySegment.AddIndustryPlacementResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.IndustryPlacementCode, pathway);

                if (ucasDataComponents.Any())
                {
                    var record = BuildUcasDataRecord(ucasDataComponents, pathway);
                    records.Add(record);
                }
            }

            return BuildUcasData(_ucasRecordEntrySegment.UcasDataType, records);
        }

        private async Task<UcasData> ProcessUcasDataRecordResultsAsync()
        {
            var records = new List<UcasDataRecord>();
            var overallResults = await _ucasRepository.GetUcasDataRecordsForResultsAsync();

            //var test = overallResults.ToList();
            //var test1 = overallResults.ToList().Where(w => w.Id == 1125);
            //var test2 = overallResults.Take(33).ToList();

            foreach (var overallResult in overallResults)
            //foreach (var overallResult in overallResults.Where(w => w.Id == 1125))
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                _ucasRecordResultsSegment.AddCoreSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddSpecialismSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddOverallResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode, overallResult.ResultAwarded);
                _ucasRecordResultsSegment.AddIndustryPlacementResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.IndustryPlacementCode, overallResult.TqRegistrationPathway);

                records.Add(BuildUcasDataRecord(ucasDataComponents, overallResult.TqRegistrationPathway));
            }

            return BuildUcasData(_ucasRecordResultsSegment.UcasDataType, records);
        }

        private async Task<UcasData> ProcessUcasDataRecordAmendmentsAsync()
        {
            var records = new List<UcasDataRecord>();
            var overallResults = await _ucasRepository.GetUcasDataRecordsForAmendmentsAsync();

            foreach (var overallResult in overallResults)
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                _ucasRecordResultsSegment.AddCoreSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddSpecialismSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddOverallResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode, overallResult.ResultAwarded);
                _ucasRecordResultsSegment.AddIndustryPlacementResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.IndustryPlacementCode, overallResult.TqRegistrationPathway);
                records.Add(BuildUcasDataRecord(ucasDataComponents, overallResult.TqRegistrationPathway));
            }

            return BuildUcasData(UcasDataType.Amendments, records);
        }

        private UcasDataRecord BuildUcasDataRecord(List<UcasDataComponent> ucasDataComponents, TqRegistrationPathway pathway)
        {
            return new UcasDataRecord
            {
                UcasRecordType = (char)UcasRecordType.Subject,
                SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                CentreNumber = _resultsAndCertificationConfiguration.UcasDataSettings.CentreNumber,
                CandidateNumber = pathway.TqRegistrationProfile.UniqueLearnerNumber.ToString(),
                CandidateName = $"{pathway.TqRegistrationProfile.Lastname}:{pathway.TqRegistrationProfile.Firstname}",
                CandidateDateofBirth = pathway.TqRegistrationProfile.DateofBirth.ToUcasFormat(),
                Sex = !string.IsNullOrWhiteSpace(pathway.TqRegistrationProfile.Gender) ? ((char)EnumExtensions.GetEnumByDisplayName<UcasGender>(pathway.TqRegistrationProfile.Gender)).ToString() : string.Empty,
                UcasDataComponents = ucasDataComponents
            };
        }

        private UcasData BuildUcasData(UcasDataType ucasDataType, List<UcasDataRecord> records)
        {
            if (!records.Any())
                return null;

            var ucasData = new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                    ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                    UcasDataType = (char)ucasDataType,
                    ExamMonth = _resultsAndCertificationConfiguration.UcasDataSettings.ExamMonth,
                    ExamYear = DateTime.UtcNow.Year.ToString(),
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = records,

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                    ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                    Count = records.Count + 2,
                    ExamDate = $"{_resultsAndCertificationConfiguration.UcasDataSettings.ExamMonth}{DateTime.UtcNow.Year}"
                }
            };

            return ucasData;
        }
    }
}

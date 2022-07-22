﻿using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
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
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;

        public UcasDataService(IUcasRepository ucasRepository, ResultsAndCertificationConfiguration resultsAndCertificationConfiguration)
        {
            _ucasRepository = ucasRepository;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
        }

        public async Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType)
        {
            var includeResults = ucasDataType != UcasDataType.Entries;
            var registrationPathways = await _ucasRepository.GetUcasDataRecordsAsync(includeResults);

            var records = new List<UcasDataRecord>();
            foreach (var pathway in registrationPathways)
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                // Add Core
                var ucasCoreComponent = GetCoreComponentData(includeResults, pathway);
                if (ucasCoreComponent != null)
                    ucasDataComponents.Add(ucasCoreComponent);

                // Add Specialisms
                foreach (var specialism in pathway.TqRegistrationSpecialisms)
                {
                    var ucasSpecialismComponent = GetSpecialismComponentData(includeResults, specialism);
                    if (ucasSpecialismComponent != null)
                        ucasDataComponents.Add(ucasSpecialismComponent);
                }

                if (ucasDataComponents.Any())
                {
                    // Add Overall result
                    ucasDataComponents.Add(new UcasDataComponent
                    {
                        SubjectCode = _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode,
                        Grade = string.Empty,
                        PreviousGrade = string.Empty
                    });

                    records.Add(new UcasDataRecord
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
                    });
                }
            }

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

        public async Task<UcasData> ProcessUcasDataRecordsTestAsync(UcasDataType ucasDataType)
        {
            await Task.CompletedTask;

            return ucasDataType switch
            {
                UcasDataType.Entries => await GetUcasAssessmentEntriesAsync(),
                UcasDataType.Results => await GetUcasResultsAsync(),
                UcasDataType.Amendments => await GetUcasAmendmentsAsync(),
                _ => null,
            };
        }

        private static UcasDataComponent GetCoreComponentData(bool includeResults, TqRegistrationPathway pathway)
        {
            if (!pathway.TqPathwayAssessments.Any())
                return null;

            TqPathwayResult pathwayHigherResult = null;
            if (includeResults)
                pathwayHigherResult = pathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return new UcasDataComponent
            {
                SubjectCode = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                Grade = pathwayHigherResult != null ? pathwayHigherResult.TlLookup.Value : string.Empty,
                PreviousGrade = string.Empty
            };
        }


        private static UcasDataComponent GetSpecialismComponentData(bool includeResults, TqRegistrationSpecialism specialism)
        {
            if (!specialism.TqSpecialismAssessments.Any())
                return null;

            TqSpecialismResult specialismHigherResult = null;
            if (includeResults)
                specialismHigherResult = specialism.TqSpecialismAssessments.SelectMany(x => x.TqSpecialismResults).OrderBy(x => x.TlLookup.SortOrder).FirstOrDefault();

            return new UcasDataComponent
            {
                SubjectCode = specialism.TlSpecialism.LarId,
                Grade = specialismHigherResult != null ? specialismHigherResult.TlLookup.Value : string.Empty,
                PreviousGrade = string.Empty
            };
        }

        private async Task<UcasData> GetUcasAssessmentEntriesAsync()
        {
            await Task.CompletedTask;

            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    UcasDataType = (char)UcasDataType.Entries,
                    ExamMonth = "06",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = "M",
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369176", Grade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null }
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = "F",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null }
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 5,
                    ExamDate = "062021"
                }
            };
        }

        private async Task<UcasData> GetUcasResultsAsync()
        {
            await Task.CompletedTask;

            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    UcasDataType = (char)UcasDataType.Results,
                    ExamMonth = "06",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = "M",
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369176", Grade = "B" },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = "F",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = "C", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = "P", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "P", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = "B", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = "M", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "M", PreviousGrade = null },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 5,
                    ExamDate = "062021"
                }
            };
        }

        private async Task<UcasData> GetUcasAmendmentsAsync()
        {
            await Task.CompletedTask;

            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    UcasDataType = (char)UcasDataType.Amendments,
                    ExamMonth = "06",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = "M",
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369176", Grade = "A*", PreviousGrade = "B" },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = "F",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = "A", PreviousGrade = "C" },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = "P", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "M", PreviousGrade = "P" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = "A", PreviousGrade = "B" },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = "M", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "D", PreviousGrade = "M" },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 5,
                    ExamDate = "062021"
                }
            };
        }

        public static string GetAbbreviatedPathwayResult(string result)
        {
            if(string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = Constants.PathwayResultAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Pathway abbreviated result cannot be null");
        }

        public static string GetAbbreviatedSpecialismResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = Constants.SpecialismResultAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Specialism abbreviated result cannot be null");
        }

        public static string GetAbbreviatedOverallResult(string result)
        {
            if (string.IsNullOrWhiteSpace(result))
                return string.Empty;

            var hasValue = Constants.OverallResultsAbbreviations.TryGetValue(result, out string abbrevatedResult);

            if (hasValue)
                return abbrevatedResult;
            else
                throw new ApplicationException("Overall abbreviated result cannot be null");
        }
    }
}

using Newtonsoft.Json;
using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Configuration;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using Sfa.Tl.ResultsAndCertification.Models.OverallResults;
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
                UcasDataType.Amendments => null,
                _ => null,
            };
        }

        public async Task<UcasData> ProcessUcasDataRecordEntriesAsync()
        {
            var records = new List<UcasDataRecord>();
            var registrationPathways = await _ucasRepository.GetUcasDataRecordsForEntriesAsync();
            foreach (var pathway in registrationPathways)
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                _ucasRecordEntrySegment.AddCoreSegment(ucasDataComponents, pathway);
                _ucasRecordEntrySegment.AddSpecialismSegment(ucasDataComponents, pathway);
                _ucasRecordEntrySegment.AddOverallResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode);

                if (ucasDataComponents.Any())
                {
                    var record = BuildUcasDataRecord(ucasDataComponents, pathway);
                    records.Add(record);
                }
            }

            return BuildUcasData(_ucasRecordEntrySegment.UcasDataType, records);
        }

        public async Task<UcasData> ProcessUcasDataRecordResultsAsync()
        {
            var records = new List<UcasDataRecord>();
            var overallResults = await _ucasRepository.GetUcasDataRecordsForResultsAsync();
            foreach (var overallResult in overallResults)
            {
                var ucasDataComponents = new List<UcasDataComponent>();

                _ucasRecordResultsSegment.AddCoreSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddSpecialismSegment(ucasDataComponents, overallResult.TqRegistrationPathway);
                _ucasRecordResultsSegment.AddOverallResultSegment(ucasDataComponents, _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode, overallResult.ResultAwarded);

                if (ucasDataComponents.Any())
                {
                    var record = BuildUcasDataRecord(ucasDataComponents, overallResult.TqRegistrationPathway);
                    records.Add(record);
                }
            }

            return BuildUcasData(_ucasRecordResultsSegment.UcasDataType, records);
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

        #region old
        public async Task<UcasData> ProcessUcasDataRecordsAsync1(UcasDataType ucasDataType)
        {
            var records = new List<UcasDataRecord>();

            if (ucasDataType == UcasDataType.Entries)
            {
                var registrationPathways = await _ucasRepository.GetUcasDataRecordsForEntriesAsync();
                foreach (var pathway in registrationPathways)
                {
                    var ucasDataComponents = new List<UcasDataComponent>();

                    // 1. Add Core
                    var ucasCoreComponent = GetCoreComponentData(pathway);
                    if (ucasCoreComponent != null)
                        ucasDataComponents.Add(ucasCoreComponent);

                    // 2. Add Specialisms
                    foreach (var specialism in pathway.TqRegistrationSpecialisms)
                    {
                        var ucasSpecialismComponent = GetSpecialismComponentData(specialism);
                        if (ucasSpecialismComponent != null)
                            ucasDataComponents.Add(ucasSpecialismComponent);
                    }

                    // 3. Add Overall result
                    if (ucasDataComponents.Any())
                    {
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
            }

            else
            {
                var overallResults = await _ucasRepository.GetUcasDataRecordsForResultsAsync();
                foreach (var overallResult in overallResults)
                {
                    var ucasDataComponents = new List<UcasDataComponent>();
                    var overallResultDetails = JsonConvert.DeserializeObject<OverallResultDetail>(overallResult.Details);

                    // 1. Add Core
                    ucasDataComponents.Add(new UcasDataComponent
                    {
                        SubjectCode = overallResultDetails.PathwayLarId,
                        Grade = overallResultDetails.PathwayResult,
                        PreviousGrade = string.Empty
                    });

                    // 2. Add Specialisms
                    foreach (var splDetails in overallResultDetails.SpecialismDetails)
                    {
                        ucasDataComponents.Add(new UcasDataComponent
                        {
                            SubjectCode = splDetails.SpecialismLarId,
                            Grade = splDetails.SpecialismResult,
                            PreviousGrade = string.Empty
                        });
                    }

                    // 3. Add Overall result
                    ucasDataComponents.Add(new UcasDataComponent
                    {
                        SubjectCode = _resultsAndCertificationConfiguration.UcasDataSettings.OverallSubjectCode,
                        Grade = overallResult.ResultAwarded,
                        PreviousGrade = string.Empty
                    });

                    records.Add(new UcasDataRecord
                    {
                        UcasRecordType = (char)UcasRecordType.Subject,
                        SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                        ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                        CentreNumber = _resultsAndCertificationConfiguration.UcasDataSettings.CentreNumber,
                        CandidateNumber = overallResult.TqRegistrationPathway.TqRegistrationProfile.UniqueLearnerNumber.ToString(),
                        CandidateName = $"{overallResult.TqRegistrationPathway.TqRegistrationProfile.Lastname}:{overallResult.TqRegistrationPathway.TqRegistrationProfile.Firstname}",
                        CandidateDateofBirth = overallResult.TqRegistrationPathway.TqRegistrationProfile.DateofBirth.ToUcasFormat(),
                        Sex = !string.IsNullOrWhiteSpace(overallResult.TqRegistrationPathway.TqRegistrationProfile.Gender) ? ((char)EnumExtensions.GetEnumByDisplayName<UcasGender>(overallResult.TqRegistrationPathway.TqRegistrationProfile.Gender)).ToString() : string.Empty,
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

        private static UcasDataComponent GetCoreComponentData(TqRegistrationPathway pathway)
        {
            if (!pathway.TqPathwayAssessments.Any())
                return null;

            return new UcasDataComponent
            {
                SubjectCode = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                Grade = string.Empty,
                PreviousGrade = string.Empty
            };
        }

        private static UcasDataComponent GetSpecialismComponentData(TqRegistrationSpecialism specialism)
        {
            if (!specialism.TqSpecialismAssessments.Any())
                return null;

            return new UcasDataComponent
            {
                SubjectCode = specialism.TlSpecialism.LarId,
                Grade = string.Empty,
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
        #endregion

    }
}

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
        private readonly ResultsAndCertificationConfiguration _resultsAndCertificationConfiguration;

        public UcasDataService(IUcasRepository ucasRepository, ResultsAndCertificationConfiguration resultsAndCertificationConfiguration)
        {
            _ucasRepository = ucasRepository;
            _resultsAndCertificationConfiguration = resultsAndCertificationConfiguration;
        }

        public async Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType)
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

        public async Task<UcasData> ProcessUcasDataRecordsTestAsync(UcasDataType ucasDataType)
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
                    foreach (var assessment in specialism.TqSpecialismAssessments)
                        ucasDataComponents.Add(new UcasDataComponent
                        {
                            SubjectCode = specialism.TlSpecialism.LarId,
                            Grade = string.Empty,
                            PreviousGrade = string.Empty
                        });
                }

                // Add Overall result
                // TODO: Upcoming story. 

                if (ucasDataComponents.Any())
                    records.Add(new UcasDataRecord
                    {
                        UcasRecordType = (char)UcasRecordType.Subject,
                        SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                        ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                        CentreNumber = _resultsAndCertificationConfiguration.UcasDataSettings.CentreNumber,
                        CandidateName = $"{pathway.TqRegistrationProfile.Lastname}:{pathway.TqRegistrationProfile.Firstname}",
                        CandidateDateofBirth = pathway.TqRegistrationProfile.DateofBirth.ToUcasFormat(),
                        Sex = EnumExtensions.GetEnumValueStringByName<UcasGender>(pathway.TqRegistrationProfile.Gender) ?? string.Empty,
                        UcasDataComponents = ucasDataComponents
                    });
            }

            var ucasData = new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.SendingOrganisation,
                    ReceivingOrganisation = _resultsAndCertificationConfiguration.UcasDataSettings.ReceivingOrganisation,
                    UcasDataType = (char)UcasDataType.Entries,
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
                    Count = registrationPathways.Count,
                    ExamDate = $"{_resultsAndCertificationConfiguration.UcasDataSettings.ExamMonth}{DateTime.UtcNow.Year}"
                }
            };

            return ucasData;
        }

        private static UcasDataComponent GetCoreComponentData(bool includeResults, TqRegistrationPathway pathway)
        {
            if (!pathway.TqPathwayAssessments.Any())
                return null;

            TqPathwayResult pathwayHigherResult = null;
            if (includeResults)
                pathwayHigherResult = pathway.TqPathwayAssessments.SelectMany(x => x.TqPathwayResults).OrderBy(x => x.TlLookup.Id).FirstOrDefault();

            return new UcasDataComponent
            {
                SubjectCode = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                Grade = pathwayHigherResult != null ? pathwayHigherResult.TlLookup.Value : string.Empty,
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
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = "F",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = null, PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = null, PreviousGrade = null }
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 3,
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
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = "Pass", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Pass", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = "B", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = "Merit", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Merit", PreviousGrade = null },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 3,
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
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = "Pass", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Merit", PreviousGrade = "Pass" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = "30", ReceivingOrganisation = "90",
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = "M",
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = "A", PreviousGrade = "B" },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = "Merit", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Distinction", PreviousGrade = "Merit" },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = "30",
                    ReceivingOrganisation = "90",
                    Count = 3,
                    ExamDate = "062021"
                }
            };
        }
    }
}

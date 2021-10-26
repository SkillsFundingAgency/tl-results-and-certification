using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
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

        public UcasDataService(IUcasRepository ucasRepository)
        {
            _ucasRepository = ucasRepository;
        }

        public async Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType)
        {
            await Task.CompletedTask;

            return ucasDataType switch
            {
                UcasDataType.Entries => await GetUcasAssessmentEntriesAsync(),
                UcasDataType.Results => await GetUcasResultsAsync(),
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
                char gender = pathway.TqRegistrationProfile.Gender.ToLower() == "male" ? 'M' : 'F';  // TODO: move to constants.. .if not male then alternative can not be a female?
                var ucasDataComponents = new List<UcasDataComponent>();

                // Add Core
                foreach (var assessment in pathway.TqPathwayAssessments)
                    ucasDataComponents.Add(new UcasDataComponent
                    {
                        SubjectCode = pathway.TqProvider.TqAwardingOrganisation.TlPathway.LarId,
                        Grade = assessment.TqPathwayResults.FirstOrDefault().TlLookup.Value,
                        PreviousGrade = ucasDataType != UcasDataType.Amendments ? null : "TODO"
                    });

                // Add Specialisms
                foreach (var specialism in pathway.TqRegistrationSpecialisms)
                {
                    foreach (var assessment in specialism.TqSpecialismAssessments)
                        ucasDataComponents.Add(new UcasDataComponent
                        {
                            SubjectCode = specialism.TlSpecialism.LarId,
                            Grade = null,  // TODO: future story
                            PreviousGrade = null // TODO
                        });
                }

                records.Add(new UcasDataRecord
                {
                    UcasRecordType = (char)UcasRecordType.Subject,
                    SendingOrganisation = 30,   // TODO
                    ReceivingOrganisation = 90, // TODO,
                    CentreNumber = "1111111",   //TODO
                    CandidateName = $"{pathway.TqRegistrationProfile.Lastname}:{pathway.TqRegistrationProfile.Firstname}",
                    CandidateDateofBirth = pathway.TqRegistrationProfile.DateofBirth.ToString(), // TODO: refer spec and format. 
                    Sex = gender,
                    UcasDataComponents = ucasDataComponents
                });
            }

            var ucasData = new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = 30, // Todo: Config
                    ReceivingOrganisation = 90,  // Todo: Config
                    UcasDataType = (char)UcasDataType.Entries,
                    ExamMonth = "06",      // Todo: Config
                    ExamYear = DateTime.UtcNow.Year.ToString(),
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = records,

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = 30,
                    ReceivingOrganisation = 90,
                    Count = registrationPathways.Count,
                    ExamDate = $"{06}{DateTime.UtcNow.Year}"
                }
            };

            return ucasData;
        }

        private async Task<UcasData> GetUcasAssessmentEntriesAsync()
        {
            await Task.CompletedTask;

            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = 30,
                    ReceivingOrganisation = 90,
                    UcasDataType = (char)UcasDataType.Entries,
                    ExamMonth = "06",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369176", Grade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = (char)UcasGender.Female,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = null, PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = (char)UcasGender.Male,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = null, PreviousGrade = null }
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = 30,
                    ReceivingOrganisation = 90,
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
                    SendingOrganisation = 30,
                    ReceivingOrganisation = 90,
                    UcasDataType = (char)UcasDataType.Results,
                    ExamMonth = "06",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369176", Grade = "A" },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "15031985", Sex = (char)UcasGender.Female,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60358294", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS004", Grade = "Distinction", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Distinction", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 30, ReceivingOrganisation = 90,
                    CentreNumber = "1111111", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "07051999", Sex = (char)UcasGender.Male,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "60369115", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "ZTLOS028", Grade = "Merit", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "TLEVEL", Grade = "Merit", PreviousGrade = null },
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = 30,
                    ReceivingOrganisation = 90,
                    Count = 3,
                    ExamDate = "062021"
                }
            };
        }
    }
}

using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasDataService : IUcasDataService
    {
        public async Task<UcasData> ProcessUcasDataRecordsAsync(UcasDataType ucasDataType)
        {
            await Task.CompletedTask;

            return ucasDataType switch
            {
                UcasDataType.Entries => await GetUcasAssessmentEntriesAsync(),
                UcasDataType.Results => await GetUcasResultEntriesAsync(),
                _ => null,
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
                    SendingOrganisation = 99,
                    ReceivingOrganisation = 90,
                    UcasDataType = (char)UcasDataType.Entries,
                    ExamMonth = "09",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-1", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub1", Grade = "A" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-2", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Female,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub21", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub22", Grade = null, PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-3", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub31", Grade = null, PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub32", Grade = null, PreviousGrade = null }
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = 99,
                    ReceivingOrganisation = 90,
                    Count = 3,
                    ExamDate = "092021"
                }
            };
        }

        private async Task<UcasData> GetUcasResultEntriesAsync()
        {
            await Task.CompletedTask;

            return new UcasData
            {
                Header = new UcasDataHeader
                {
                    UcasRecordType = (char)UcasRecordType.Header,
                    SendingOrganisation = 99,
                    ReceivingOrganisation = 90,
                    UcasDataType = (char)UcasDataType.Results,
                    ExamMonth = "09",
                    ExamYear = "2021",
                    DateCreated = DateTime.Today.ToString("ddMMyyyy", CultureInfo.InvariantCulture)
                },

                UcasDataRecords = new List<UcasDataRecord>
                {
                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-1", CandidateNumber = "1234567890", CandidateName = "Smith:John1", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                    UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub1", Grade = "A" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-2", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Female,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub21", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub22", Grade = "A*", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-3", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                        UcasDataComponents = new List<UcasDataComponent>
                    {
                        new UcasDataComponent { SubjectCode = "Sub31", Grade = "A*", PreviousGrade = null },
                        new UcasDataComponent { SubjectCode = "Sub32", Grade = "A*", PreviousGrade = null }
                    } },
                },

                Trailer = new UcasDataTrailer
                {
                    UcasRecordType = (char)UcasRecordType.Trailer,
                    SendingOrganisation = 99,
                    ReceivingOrganisation = 90,
                    Count = 3,
                    ExamDate = "092021"
                }
            };
        }
    }
}

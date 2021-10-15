using Sfa.Tl.ResultsAndCertification.Application.Interfaces;
using Sfa.Tl.ResultsAndCertification.Models.Functions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Sfa.Tl.ResultsAndCertification.Application.Services
{
    public class UcasDataService : IUcasDataService
    {
        public async Task<UcasData> GetUcasEntriesAsync()
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
                    UcasDataResults = new List<UcasDataResult> 
                    { 
                        new UcasDataResult { SubjectCode = "Sub1", Grade = "A*" },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-2", CandidateNumber = "1234567891", CandidateName = "Smity:Jony2", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Female,
                        UcasDataResults = new List<UcasDataResult>
                    {
                        new UcasDataResult { SubjectCode = "Sub21", Grade = "A*", PreviousGrade = null },
                        new UcasDataResult { SubjectCode = "Sub22", Grade = "A*", PreviousGrade = null },
                    } },

                    new UcasDataRecord { UcasRecordType = (char)UcasRecordType.Subject, SendingOrganisation = 99, ReceivingOrganisation = 90,
                    CentreNumber = "CENTRE-3", CandidateNumber = "1234567892", CandidateName = "Smith:John3", CandidateDateofBirth = "20082000", Sex = (char)UcasGender.Male,
                        UcasDataResults = new List<UcasDataResult>
                    {
                        new UcasDataResult { SubjectCode = "Sub31", Grade = "A*", PreviousGrade = null },
                        new UcasDataResult { SubjectCode = "Sub32", Grade = "A*", PreviousGrade = "B" }
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

using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations
{
    public class RegistrationsStage3Builder
    {
        public IList<RegistrationCsvRecordResponse> BuildValidList() => new List<RegistrationCsvRecordResponse>
        {
            new RegistrationCsvRecordResponse
            {
                RowNum = 2,
                Uln = 111111111,
                FirstName = "First 1",
                LastName = "Last 1",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10011221, // valid provider
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { "10123456", "10123457" }  // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 3,
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10011221,  // valid provider
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123456", // valid core code
                SpecialismCodes = new List<string> { "10123456", "10123457" } // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 4,
                Uln = 111111113,
                FirstName = "First 3",
                LastName = "Last 3",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10011221, // valid providerr
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { "10123456", "10123457" } // invalid specialisms
            }
        };

        public IList<RegistrationCsvRecordResponse> BuildInvalidList() => new List<RegistrationCsvRecordResponse>
        {
            new RegistrationCsvRecordResponse
            {
                RowNum = 2,
                Uln = 111111111,
                FirstName = "First 1",
                LastName = "Last 1",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 123456789, // invalid provider
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { "10123456", "10123457" }  // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 3,
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10011221,  // valid provider
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123333", // invalid core code
                SpecialismCodes = new List<string> { "10123456", "10123457" } // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 4,
                Uln = 111111113,
                FirstName = "First 3",
                LastName = "Last 3",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10011221, // valid providerr
                AcademicYear = DateTime.Now.Year,
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { "99999999", "10123457" } // invalid specialisms
            }
        };
    }
}

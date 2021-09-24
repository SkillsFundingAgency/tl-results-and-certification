using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Linq;

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
                ProviderUkprn = 10000536, // valid provider
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } }  // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 3,
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536,  // valid provider
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // valid core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } } // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 4,
                Uln = 111111113,
                FirstName = "First 3",
                LastName = "Last 3",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536, // valid providerr
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } } // invalid specialisms
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
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } }  // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 3,
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536,  // valid provider
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123333", // invalid core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } } // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 4,
                Uln = 111111113,
                FirstName = "First 3",
                LastName = "Last 3",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536, // valid providerr
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "99999999" } } // invalid specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 5,
                Uln = 111111114,
                FirstName = "First 4",
                LastName = "Last 4",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536, // valid providerr
                AcademicYearName = GetAcademicYearName(getValid: false), // invalid academic year
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "10123456" }, { "10123457" } } // correct specialisms
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 6,
                Uln = 111111115,
                FirstName = "First 5",
                LastName = "Last 5",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536, // valid providerr
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { { "10123456" } } // Invalid - This specialism cannot be selected as a single option (couplet)
            },
            new RegistrationCsvRecordResponse
            {
                RowNum = 7,
                Uln = 111111116,
                FirstName = "First 6",
                LastName = "Last 6",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                ProviderUkprn = 10000536, // valid providerr
                AcademicYearName = GetAcademicYearName(getValid: true),
                CoreCode = "10123456", // correct core code
                SpecialismCodes = new List<string> { {"10123456" }, { "10123458" } } // Invalid - Specialism is not valid (must be a paired specialism i.e couplet)
            }
        };

        private string GetAcademicYearName(bool getValid)
        {
            var academicYears = new AcademicYearBuilder().BuildList();
            if (getValid)
                return academicYears.FirstOrDefault(x => DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate).Name;
            else
                return academicYears.FirstOrDefault(x => !(DateTime.Today >= x.StartDate && DateTime.Today <= x.EndDate)).Name;
        }
    }
}

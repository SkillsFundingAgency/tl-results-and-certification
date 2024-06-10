using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class PendingWithdrawalsExport
    {
        public PendingWithdrawalsExport()
        {
            SpecialismsList = new List<string>();
        }

        [DisplayName(PendingWithdrawalsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(PendingWithdrawalsExportHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(PendingWithdrawalsExportHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(PendingWithdrawalsExportHeader.DateOfBirth)]
        public string DisplayDateOfBirth => DateOfBirth.ToString("dd-MMM-yyyy");

        [DisplayName(PendingWithdrawalsExportHeader.Ukprn)]
        public long Ukprn { get; set; }

        [DisplayName(PendingWithdrawalsExportHeader.AcademicYear)]
        public string DisplayAcademicYear
            => AcademicYear.ToString().Length == 4 ? $"{AcademicYear}/{(AcademicYear + 1).ToString().Substring(2)}" : string.Empty;

        [DisplayName(PendingWithdrawalsExportHeader.Core)]
        public string Core { get; set; }

        [DisplayName(PendingWithdrawalsExportHeader.Specialisms)]
        public string Specialisms
            => SpecialismsList.IsNullOrEmpty() switch
            {
                true => string.Empty,
                false => SpecialismsList.Count switch
                {
                    1 => SpecialismsList.First(),
                    _ => $"\"\"\"{string.Join(Constants.CommaSeparator, SpecialismsList)}\"\"\""
                }
            };

        public IList<string> SpecialismsList { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int AcademicYear { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
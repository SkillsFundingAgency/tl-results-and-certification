using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Sfa.Tl.ResultsAndCertification.Models.DataExport
{
    public class RegistrationsExport
    {
        public RegistrationsExport()
        {
            SpecialismsList = new List<string>();
        }

        [DisplayName(RegistrationsExportHeader.Uln)]
        public long Uln { get; set; }

        [DisplayName(RegistrationsExportHeader.FirstName)]
        public string FirstName { get; set; }

        [DisplayName(RegistrationsExportHeader.LastName)]
        public string LastName { get; set; }

        [DisplayName(RegistrationsExportHeader.DateOfBirth)]
        public string DisplayDateOfBirth => DateOfBirth.ToString("ddMMyyyy");

        [DisplayName(RegistrationsExportHeader.Ukprn)]
        public long Ukprn { get; set; }

        [DisplayName(RegistrationsExportHeader.AcademicYear)]
        public string DisplayAcademicYear => AcademicYear.ToString().Length == 4 ? $"{AcademicYear}/{(AcademicYear + 1).ToString().Substring(2)}" : string.Empty;

        [DisplayName(RegistrationsExportHeader.Core)]
        public string Core { get; set; }

        [DisplayName(RegistrationsExportHeader.Specialisms)]
        public string Specialisms
        {
            get
            {
                return SpecialismsList.Any() && SpecialismsList.Count() > 1 ?
                    $"\"{string.Join(Constants.CommaSeperator, SpecialismsList)}\"" :
                    SpecialismsList.FirstOrDefault();
            }
        }

        [DisplayName(RegistrationsExportHeader.Status)]
        public string Status { get; set; }

        public IList<string> SpecialismsList { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
        public int AcademicYear { get; set; }       

        public DateTime CreatedOn { get; set; }
    }
}

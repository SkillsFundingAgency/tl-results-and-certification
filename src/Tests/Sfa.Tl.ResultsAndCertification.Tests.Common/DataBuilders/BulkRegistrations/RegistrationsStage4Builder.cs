using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;

namespace Sfa.Tl.ResultsAndCertification.Tests.Common.DataBuilders.BulkRegistrations
{
    public class RegistrationsStage4Builder
    {
        public IList<RegistrationRecordResponse> BuildValidList() => new List<RegistrationRecordResponse>
        {
            new RegistrationRecordResponse
            {
                Uln = 111111111,
                FirstName = "First 1",
                LastName = "Last 1",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                StartDate = DateTime.Now,
                TqProviderId = 1, // valid provider
                TlProviderId = 1,
                TqAwardingOrganisationId = 1,
                TlAwardingOrganisatonId = 1,
                TlPathwayId = 1,
                TlSpecialismLarIds = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "10123456") }
            },
            new RegistrationRecordResponse
            {
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                StartDate = DateTime.Now,
                TqProviderId = 1, // valid provider
                TlProviderId = 1,
                TqAwardingOrganisationId = 1,
                TlAwardingOrganisatonId = 1,
                TlPathwayId = 1,
                TlSpecialismLarIds = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "10123456")}
            },
            new RegistrationRecordResponse
            {
                Uln = 111111113,
                FirstName = "First 3",
                LastName = "Last 3",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                StartDate = DateTime.Now,
                TqProviderId = 1, // valid provider
                TlProviderId = 1,
                TqAwardingOrganisationId = 1,
                TlAwardingOrganisatonId = 1,
                TlPathwayId = 1,
                TlSpecialismLarIds = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "10123456") }
            }
        };

        public IList<RegistrationRecordResponse> BuildActiveUlnWithDifferentAoList() => new List<RegistrationRecordResponse>
        {
            new RegistrationRecordResponse
            {
                Uln = 111111112,
                FirstName = "First 2",
                LastName = "Last 2",
                DateOfBirth = DateTime.Parse("12/01/1985"),
                StartDate = DateTime.Now,
                TqProviderId = 2, // Assign different TqProviderId to show ActiveUlnWithDifferentAo validation error
                TlProviderId = 1,
                TqAwardingOrganisationId = 1,
                TlAwardingOrganisatonId = 1,
                TlPathwayId = 1,
                TlSpecialismLarIds = new List<KeyValuePair<int, string>> { new KeyValuePair<int, string>(1, "10123456")}
            }
        };
    }
}

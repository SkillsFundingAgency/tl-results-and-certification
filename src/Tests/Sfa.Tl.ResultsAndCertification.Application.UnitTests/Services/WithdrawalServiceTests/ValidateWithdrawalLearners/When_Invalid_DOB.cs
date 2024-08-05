using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.WithdrawalServiceTests.ValidateWithdrawalLearners
{
    public class When_Invalid_DOB : WithdrawalServiceBaseTest
    {
        private const long AoUkprn = 1;
        private const long Uln = 1234567890;
        private const int TqRegistrationProfileId = 125;

        private IList<WithdrawalRecordResponse> _actualResult;

        public override void Given()
        {
            TqRegistrationProfile profile = CreateTqRegistrationProfile(TqRegistrationProfileId, Uln, new DateTime(2020, 1, 2));
            ConfigureRegistrationRepository(Uln, profile);
        }

        public override async Task When()
        {
            WithdrawalCsvRecordResponse record = CreateWithdrawalCsvRecordResponse(Uln, new DateTime(2020, 1, 1));
            _actualResult = await When(AoUkprn, record);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
            => Then_Returns_Expected_ValidationErrors_Results(_actualResult, rowNum: 2, Uln, ValidationMessages.InvalidDateOfBirth);
    }
}
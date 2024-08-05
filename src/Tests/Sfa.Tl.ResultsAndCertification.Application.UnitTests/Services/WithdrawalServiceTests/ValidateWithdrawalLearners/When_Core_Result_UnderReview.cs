using Sfa.Tl.ResultsAndCertification.Common.Constants;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.WithdrawalServiceTests.ValidateWithdrawalLearners
{
    public class When_Core_Result_UnderReview : WithdrawalServiceBaseTest
    {
        private const long AoUkprn = 1;
        private const long Uln = 1234567890;
        private const int TqRegistrationProfileId = 125;

        private readonly DateTime _dob = new(2020, 1, 1);

        private IList<WithdrawalRecordResponse> _actualResult;

        public override void Given()
        {
            TqRegistrationProfile profile = CreateTqRegistrationProfile(TqRegistrationProfileId, Uln, _dob, pathwayResultStatus: PrsStatus.UnderReview);
            ConfigureRegistrationRepository(Uln, profile);
        }

        public override async Task When()
        {
            WithdrawalCsvRecordResponse record = CreateWithdrawalCsvRecordResponse(Uln, _dob);
            _actualResult = await When(AoUkprn, record);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
            => Then_Returns_Expected_ValidationErrors_Results(_actualResult, rowNum: 2, Uln, ValidationMessages.InvalidResultState);
    }
}
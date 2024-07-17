using FluentAssertions;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Registration;
using Sfa.Tl.ResultsAndCertification.Models.Registration.BulkProcess;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.WithdrawalServiceTests.ValidateWithdrawalLearners
{
    public class When_Valid : WithdrawalServiceBaseTest
    {
        private const long AoUkprn = 1;
        private const long Uln = 1234567890;
        private const int TqRegistrationProfileId = 125;

        private readonly DateTime _dob = new(2020, 1, 1);

        private IList<WithdrawalRecordResponse> _actualResult;

        public override void Given()
        {
            TqRegistrationProfile profile = CreateTqRegistrationProfile(TqRegistrationProfileId, Uln, _dob);
            ConfigureRegistrationRepository(Uln, profile);
        }

        public override async Task When()
        {
            WithdrawalCsvRecordResponse record = CreateWithdrawalCsvRecordResponse(Uln, _dob);
            _actualResult = await When(AoUkprn, record);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            _actualResult.Should().NotBeNull();
            _actualResult.Should().HaveCount(1);

            _actualResult[0].ProfileId.Should().Be(TqRegistrationProfileId);
            _actualResult[0].Uln.Should().Be(Uln);
            _actualResult[0].ValidationErrors.Should().BeEmpty();
        }
    }
}
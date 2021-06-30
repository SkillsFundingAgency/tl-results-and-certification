using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using System;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.PostResultsServiceLoaderTests.FindPrsLearnerRecord
{
    public class When_Called_With_Valid_Data : TestSetup
    {
        private Models.Contracts.PostResultsService.FindPrsLearnerRecord _expectedApiResult;

        public override void Given()
        {
            _expectedApiResult = new Models.Contracts.PostResultsService.FindPrsLearnerRecord
            {
                ProfileId = 1,
                Uln = 123456789,
                Firstname = "John",
                Lastname = "Smith",
                DateofBirth = DateTime.UtcNow.AddYears(-20),
                ProviderName = "Barsley College",
                ProviderUkprn = 54678945,
                TlevelTitle = "Title",
                Status = RegistrationPathwayStatus.Active
            };

            InternalApiClient.FindPrsLearnerRecordAsync(AoUkprn, Uln).Returns(_expectedApiResult);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            ActualResult.Should().NotBeNull();
            ActualResult.ProfileId.Should().Be(_expectedApiResult.ProfileId);
            ActualResult.Uln.Should().Be(_expectedApiResult.Uln);
            ActualResult.Firstname.Should().Be(_expectedApiResult.Firstname);
            ActualResult.Lastname.Should().Be(_expectedApiResult.Lastname);
            ActualResult.DateofBirth.Should().Be(_expectedApiResult.DateofBirth);
            ActualResult.ProviderName.Should().Be(_expectedApiResult.ProviderName);
            ActualResult.ProviderUkprn.Should().Be(_expectedApiResult.ProviderUkprn);
            ActualResult.TlevelTitle.Should().Be(_expectedApiResult.TlevelTitle);
            ActualResult.Status.Should().Be(_expectedApiResult.Status);
        }
    }
}

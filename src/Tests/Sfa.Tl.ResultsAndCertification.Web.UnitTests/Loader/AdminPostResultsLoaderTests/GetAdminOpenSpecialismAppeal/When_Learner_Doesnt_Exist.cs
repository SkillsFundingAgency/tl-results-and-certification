using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Loader.AdminPostResultsLoaderTests.GetAdminOpenSpecialismAppeal
{
    public class When_Learner_Doesnt_Exist : TestSetup
    {
        public override void Given()
        {
            ApiClient.GetAdminLearnerRecordAsync(RegistrationPathwayId).Returns(null as AdminLearnerRecord);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ApiClient.Received(1).GetAdminLearnerRecordAsync(RegistrationPathwayId);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().BeNull();
        }
    }
}
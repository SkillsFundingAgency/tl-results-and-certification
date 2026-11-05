using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessChangeUln
{
    public class When_Uln_Is_Already_Registered : TestSetup
    {
        public override void Given()
        {
            base.Given();
            Profiles.AnyAsync(Arg.Any<Expression<Func<TqRegistrationProfile, bool>>>()).Returns(true);
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            Result.Should().BeFalse();
            Profile.UniqueLearnerNumber.Should().Be(9999999990);
            await Logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }
    }
}

using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessChangeUln
{
    public class When_Current_Uln_Is_Entered : TestSetup
    {
        public override void Given()
        {
            base.Given();
            Request.ChangeUlnDetails.UlnTo = Request.ChangeUlnDetails.UlnFrom;
        }

        [Fact]
        public async Task Then_Returns_Expected_Results()
        {
            Result.Should().BeFalse();
            await Profiles.DidNotReceive().UpdateWithSpecifedColumnsOnlyAsync(Arg.Any<TqRegistrationProfile>(),
                Arg.Any<Expression<Func<TqRegistrationProfile, object>>[]>());
            await Logs.DidNotReceive().CreateAsync(Arg.Any<ChangeLog>());
        }
    }
}

using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddPathwayResultTests
{
    public class When_Assessment_Doesnt_Exist : ProcessAdminAddPathwayResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 1;

        private readonly IRepository<TqPathwayAssessment> _pathwayAssessmentRepo = Substitute.For<IRepository<TqPathwayAssessment>>();

        private AddPathwayResultRequest _request;
        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, PathwayAssessmentId);
            _pathwayAssessmentRepo.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TqPathwayAssessment, bool>>>()).Returns(null as TqPathwayAssessment);

            RepositoryFactory.GetRepository<TqPathwayAssessment>().Returns(_pathwayAssessmentRepo);
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddPathwayResultAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            _result.Should().BeFalse();
        }
    }
}
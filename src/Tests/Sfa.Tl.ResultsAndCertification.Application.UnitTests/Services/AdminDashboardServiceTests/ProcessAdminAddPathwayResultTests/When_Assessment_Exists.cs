using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddPathwayResultTests
{
    public class When_Assessment_Exists : ProcessAdminAddPathwayResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 1;

        private readonly IRepository<TqPathwayAssessment> _pathwayAssessmentRepo = Substitute.For<IRepository<TqPathwayAssessment>>();
        private readonly IRepository<TqPathwayResult> _pathwayResultRepo = Substitute.For<IRepository<TqPathwayResult>>();
        private readonly IRepository<ChangeLog> _changeLogRepo = Substitute.For<IRepository<ChangeLog>>();

        private AddPathwayResultRequest _request;
        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, PathwayAssessmentId);

            DateTime utcNow = new(2024, 1, 1);
            SystemProvider.UtcNow.Returns(utcNow);

            _pathwayAssessmentRepo.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TqPathwayAssessment, bool>>>()).Returns(new TqPathwayAssessment());
            _pathwayResultRepo.CreateAsync(Arg.Any<TqPathwayResult>()).Returns(1);
            _changeLogRepo.CreateAsync(Arg.Any<ChangeLog>()).Returns(1);

            RepositoryFactory.GetRepository<TqPathwayAssessment>().Returns(_pathwayAssessmentRepo);
            RepositoryFactory.GetRepository<TqPathwayResult>().Returns(_pathwayResultRepo);
            RepositoryFactory.GetRepository<ChangeLog>().Returns(_changeLogRepo);
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddPathwayResultAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            _result.Should().BeTrue();
        }
    }
}
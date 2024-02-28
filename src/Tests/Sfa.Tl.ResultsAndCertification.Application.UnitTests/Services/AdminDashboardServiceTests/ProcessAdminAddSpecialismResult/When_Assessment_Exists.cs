using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResult
{
    public class When_Assessment_Exists : ProcessAdminAddSpecialismResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int SpecialismAssessmentId = 1;

        private readonly IRepository<TqSpecialismAssessment> _specialismAssessmentRepo = Substitute.For<IRepository<TqSpecialismAssessment>>();
        private readonly IRepository<TqSpecialismResult> _specialismResultRepo = Substitute.For<IRepository<TqSpecialismResult>>();
        private readonly IRepository<ChangeLog> _changeLogRepo = Substitute.For<IRepository<ChangeLog>>();

        private AddSpecialismResultRequest _request;
        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, SpecialismAssessmentId);

            DateTime utcNow = new(2024, 1, 1);
            SystemProvider.UtcNow.Returns(utcNow);

            _specialismAssessmentRepo.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TqSpecialismAssessment, bool>>>()).Returns(new TqSpecialismAssessment());
            _specialismResultRepo.CreateAsync(Arg.Any<TqSpecialismResult>()).Returns(1);
            _changeLogRepo.CreateAsync(Arg.Any<ChangeLog>()).Returns(1);

            RepositoryFactory.GetRepository<TqSpecialismAssessment>().Returns(_specialismAssessmentRepo);
            RepositoryFactory.GetRepository<TqSpecialismResult>().Returns(_specialismResultRepo);
            RepositoryFactory.GetRepository<ChangeLog>().Returns(_changeLogRepo);
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddSpecialismResultAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_True()
        {
            _result.Should().BeTrue();
        }
    }
}
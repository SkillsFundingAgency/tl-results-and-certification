using FluentAssertions;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Data.Interfaces;
using Sfa.Tl.ResultsAndCertification.Domain.Models;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.AdminDashboard;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Application.UnitTests.Services.AdminDashboardServiceTests.ProcessAdminAddSpecialismResult
{
    public class When_Assessment_Doesnt_Exist : ProcessAdminAddSpecialismResultTestsBase
    {
        private const int RegistrationPathwayId = 1;
        private const int PathwayAssessmentId = 1;

        private readonly IRepository<TqSpecialismAssessment> _specialismAssessmentRepo = Substitute.For<IRepository<TqSpecialismAssessment>>();

        private AddSpecialismResultRequest _request;
        private bool _result;

        public override void Given()
        {
            _request = CreateRequest(RegistrationPathwayId, PathwayAssessmentId);
            _specialismAssessmentRepo.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TqSpecialismAssessment, bool>>>()).Returns(null as TqSpecialismAssessment);

            RepositoryFactory.GetRepository<TqSpecialismAssessment>().Returns(_specialismAssessmentRepo);
        }

        public override async Task When()
        {
            _result = await AdminDashboardService.ProcessAdminAddSpecialismResultAsync(_request);
        }

        [Fact]
        public void Then_Should_Return_False()
        {
            _result.Should().BeFalse();
        }
    }
}
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;


namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.AddIndustryPlacementQuestionGet
{
    public class When_Learner_Not_Registered : TestSetup
    {
        private AddLearnerRecordViewModel cacheResult;
        private EnterUlnViewModel _ulnViewModel;
        private FindLearnerRecord _learnerRecord;

        public override void Given()
        {
            _learnerRecord = new FindLearnerRecord { Uln = 1234567890, Name = "Test Name", IsLearnerRegistered = false };
            _ulnViewModel = new EnterUlnViewModel { EnterUln = "1234567890" };

            cacheResult = new AddLearnerRecordViewModel
            {
                LearnerRecord = _learnerRecord,
                Uln = _ulnViewModel
            };

            CacheService.GetAsync<AddLearnerRecordViewModel>(CacheKey).Returns(cacheResult);
        }
        [Fact]
        public void Then_Expected_Methods_Called()
        {
            CacheService.Received(1).GetAsync<AddLearnerRecordViewModel>(CacheKey);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}

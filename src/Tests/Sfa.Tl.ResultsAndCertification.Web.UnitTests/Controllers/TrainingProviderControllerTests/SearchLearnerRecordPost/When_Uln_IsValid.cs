using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Extensions;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts.TrainingProvider;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.SearchLearnerRecordPost
{
    public class When_Uln_IsValid : TestSetup
    {
        private readonly long uln = 123456789;
        private FindLearnerRecord mockResult;

        public override void Given()
        {
            SearchLearnerRecordViewModel = new SearchLearnerRecordViewModel { SearchUln = uln.ToString() };
            mockResult = new FindLearnerRecord { ProfileId = 1, IsLearnerRegistered = true, IsLearnerRecordAdded = false };
            TrainingProviderLoader.FindLearnerRecordAsync(ProviderUkprn, SearchLearnerRecordViewModel.SearchUln.ToLong()).Returns(mockResult);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            TrainingProviderLoader.Received(1).FindLearnerRecordAsync(ProviderUkprn, uln);            
        }

        [Fact]
        public void Then_Redirected_To_LearnerRecordDetails()
        {
            var route = Result as RedirectToRouteResult;
            route.RouteName.Should().Be(RouteConstants.LearnerRecordDetails);
            route.RouteValues[Constants.ProfileId].Should().Be(mockResult.ProfileId);
        }
    }
}

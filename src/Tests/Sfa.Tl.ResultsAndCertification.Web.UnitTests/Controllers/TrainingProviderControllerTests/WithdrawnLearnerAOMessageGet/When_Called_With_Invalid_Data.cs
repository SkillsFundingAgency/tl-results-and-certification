using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.TrainingProvider.Manual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.TrainingProviderControllerTests.WithdrawnLearnerAOMessageGet
{
    public class When_Called_With_Invalid_Data: TestSetup
    {

        WithdrawLearnerAOMessageViewModel model;

        public override void Given()
        {
            ProfileId = 0;
            TrainingProviderLoader.GetLearnerRecordDetailsAsync<WithdrawLearnerAOMessageViewModel>(ProviderUkprn, ProfileId).Returns(model);
        }

        [Fact]
        public void Then_Expected_Methods_Are_Caled()
        {
            TrainingProviderLoader.Received(1).GetLearnerRecordDetailsAsync<WithdrawLearnerAOMessageViewModel>(ProviderUkprn, ProfileId);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}

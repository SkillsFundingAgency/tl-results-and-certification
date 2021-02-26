using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Models.Contracts;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Result.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ResultControllerTests.AddCoreResultPost
{
    public class When_Success : TestSetup
    {
        private AddResultResponse AddResultResponse;

        public override void Given()
        {
            ViewModel = new ManageCoreResultViewModel
            {
                ProfileId = 1,
                SelectedGradeCode = "PCG1"                
            };

            AddResultResponse = new AddResultResponse
            {
                IsSuccess = true,
                Uln = 1234567890,
                ProfileId = 1
            };

            ResultLoader.AddCoreResultAsync(AoUkprn, ViewModel).Returns(AddResultResponse);
        }

        [Fact]
        public void Then_Expected_Methods_AreCalled()
        {
            ResultLoader.Received(1).AddCoreResultAsync(AoUkprn, ViewModel);
            CacheService.Received(1).SetAsync(string.Concat(CacheKey, Constants.ResultConfirmationViewModel),
                Arg.Is<ResultConfirmationViewModel>
                (x => x.ProfileId == ViewModel.ProfileId &&
                      x.Uln == AddResultResponse.Uln),
                 CacheExpiryTime.XSmall);
        }

        [Fact]
        public void Then_Redirected_To_AddResultConfirmation()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddResultConfirmation);
        }
    }
}

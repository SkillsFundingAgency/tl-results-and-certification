using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismQuestionPost
{
    public class When_Called_With_Selected_No : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterCoreViewModel _selectCoreViewModel;
        public override void Given()
        {
            ViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _selectCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = "12345678", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "12345678" } } };
            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = _selectCoreViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterAcademicYear()
        {
            var route = Result as RedirectToRouteResult;
            var routeName = route.RouteName;
            routeName.Should().Be(RouteConstants.ReregisterAcademicYear);
            route.RouteValues[Constants.ProfileId].Should().Be(ViewModel.ProfileId);
        }
    }
}

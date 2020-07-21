using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismQuestionPost
{
    public class Then_On_Success_Selected_No_Redirected_To_AddRegistrationAcademicYear_Route : When_AddRegistrationSpecialismQuestionAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;
        public override void Given()
        {
            SpecialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = "123", CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };
            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_On_Success_Redirected_To_AddRegistrationAcademicYear_Route()
        {
            var routeName = (Result as RedirectToRouteResult).RouteName;
            routeName.Should().Be(RouteConstants.AddRegistrationAcademicYear);
        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationSpecialismQuestionGet
{
    public class Then_On_No_Cache_For_SpecialismQuestion_Empty_ViewModel_Returned : When_AddRegistrationSpecialismQuestionAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;

        public override void Given()
        {
            _selectCoreViewModel = new SelectCoreViewModel { CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = "123" } } };
            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_SpecialismQuestion_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SpecialismQuestionViewModel));

            var model = viewResult.Model as SpecialismQuestionViewModel;
            model.Should().NotBeNull();

            model.HasLearnerDecidedSpecialism.Should().BeNull();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
        }
    }
}

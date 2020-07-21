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
    public class Then_On_Cache_Exist_SpecialismQuestion_ViewModel_Returned : When_AddRegistrationSpecialismQuestionAsync_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SelectCoreViewModel _selectCoreViewModel;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;
        private string _coreCode = "12345678";

        public override void Given()
        {
            _selectCoreViewModel = new SelectCoreViewModel { SelectedCoreCode = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            cacheResult = new RegistrationViewModel
            {
                SelectCore = _selectCoreViewModel,
                SpecialismQuestion = _specialismQuestionViewModel
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

            model.HasLearnerDecidedSpecialism.Should().BeTrue();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationCore);
        }
    }
}

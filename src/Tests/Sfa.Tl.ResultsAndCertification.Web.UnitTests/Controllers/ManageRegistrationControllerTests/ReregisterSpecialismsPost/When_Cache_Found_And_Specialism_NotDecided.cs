using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsPost
{
    public class When_Cache_Found_And_Specialism_NotDecided : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private readonly string _coreCode = "12345678";

        public override void Given()
        {
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };

            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_PageNotFound()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.PageNotFound);
        }
    }
}

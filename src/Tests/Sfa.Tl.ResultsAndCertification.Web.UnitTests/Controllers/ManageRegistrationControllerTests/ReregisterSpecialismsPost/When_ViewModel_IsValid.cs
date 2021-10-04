using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsPost
{
    public class When_ViewModel_IsValid : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private readonly string _coreCode = "12345678";
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;

        public override void Given()
        {
            _reregisterCoreViewModel = new ReregisterCoreViewModel 
            { 
                SelectedCoreCode = _coreCode, 
                CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } 
            };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } } };

            ViewModel.PathwaySpecialisms = _pathwaySpecialismsViewModel;
            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel
            };

            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, _coreCode).Returns(_pathwaySpecialismsViewModel);
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Redirected_To_ReregisterAcademicYear()
        {
            var actualRouteName = (Result as RedirectToRouteResult).RouteName;
            actualRouteName.Should().Be(RouteConstants.ReregisterAcademicYear);
        }

        [Fact]
        public void Then_Called_Expected_Method()
        {
            CacheService.Received(1)
                .SetAsync(CacheKey, Arg.Is<ReregisterViewModel>(x => x.ReregisterSpecialisms == ViewModel));
        }
    }
}

using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using System.Collections.Generic;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterSpecialismsGet
{
    public class When_Specialisms_NotDecided : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private RegistrationDetailsViewModel _registrationDetailViewModel = null;
        private readonly string _coreCode = "12345678";
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true };

            _registrationDetailViewModel = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };

            cacheResult = new ReregisterViewModel
            {
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel
            };

            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)" } } };
            
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

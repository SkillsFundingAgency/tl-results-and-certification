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
    public class When_ChangeMode_From_SplQuest : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterCoreViewModel _reregisterCoreViewModel;
        private RegistrationDetailsViewModel _registrationDetailsViewModel;
        private PathwaySpecialismsViewModel _pathwaySpecialismsViewModel;
        private ReregisterSpecialismQuestionViewModel _reregisterSpecialismQuestionViewModel;
        private readonly string _coreCode = "12345678";
        private readonly RegistrationPathwayStatus _registrationPathwayStatus = RegistrationPathwayStatus.Withdrawn;

        public override void Given()
        {
            // input variance.
            var isChangeFromSplQuest = false;
            IsChangeMode = true;

            _registrationDetailsViewModel = new RegistrationDetailsViewModel
            {
                ProfileId = 1,
                Status = _registrationPathwayStatus
            };

            _reregisterCoreViewModel = new ReregisterCoreViewModel { SelectedCoreCode = _coreCode, CoreSelectList = new List<SelectListItem> { new SelectListItem { Text = "Education", Value = _coreCode } } };
            _reregisterSpecialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = true, IsChangeMode = isChangeFromSplQuest };
            _pathwaySpecialismsViewModel = new PathwaySpecialismsViewModel { PathwayName = "Test Pathway", Specialisms = new List<SpecialismDetailsViewModel> { new SpecialismDetailsViewModel { Id = 1, Code = "345678", Name = "Test Specialism", DisplayName = "Test Specialism (345678)", IsSelected = true } } };

            cacheResult = new ReregisterViewModel
            {
                ReregisterProvider = new ReregisterProviderViewModel(),
                ReregisterCore = _reregisterCoreViewModel,
                SpecialismQuestion = _reregisterSpecialismQuestionViewModel,
                ReregisterAcademicYear = new ReregisterAcademicYearViewModel()
            };
            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            RegistrationLoader.GetPathwaySpecialismsByPathwayLarIdAsync(AoUkprn, _coreCode).Returns(_pathwaySpecialismsViewModel);
            RegistrationLoader.GetRegistrationDetailsAsync(AoUkprn, ProfileId, _registrationPathwayStatus).Returns(_registrationDetailsViewModel);
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterSpecialismViewModel));

            var model = viewResult.Model as ReregisterSpecialismViewModel;
            model.Should().NotBeNull();

            model.ProfileId.Should().Be(_registrationDetailsViewModel.ProfileId);
            model.HasSpecialismSelected.Should().NotBeNull();
            model.PathwaySpecialisms.Specialisms.Should().NotBeNull();
            model.PathwaySpecialisms.Specialisms.Count.Should().Be(_pathwaySpecialismsViewModel.Specialisms.Count);
            model.IsChangeMode.Should().BeTrue();
            model.IsChangeModeFromSpecialismQuestion.Should().BeFalse(); 
            model.BackLink.Should().NotBeNull();

            var backLink = model.BackLink;
            backLink.RouteName.Should().Be(RouteConstants.ReregisterCheckAndSubmit);
            backLink.RouteAttributes.Count.Should().Be(1);
            
            backLink.RouteAttributes.TryGetValue(Constants.ProfileId, out string profileIdParam);
            profileIdParam.Should().Be(_registrationDetailsViewModel.ProfileId.ToString());
        }
    }
}

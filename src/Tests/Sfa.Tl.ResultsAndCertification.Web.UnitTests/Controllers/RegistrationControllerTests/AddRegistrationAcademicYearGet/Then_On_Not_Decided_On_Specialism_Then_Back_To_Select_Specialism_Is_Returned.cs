using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationAcademicYearGet
{
    public class Then_On_Not_Decided_On_Specialism_Then_Back_To_Select_Specialism_Is_Returned : When_AddRegistrationAcademicYear_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private SpecialismQuestionViewModel _specialismQuestionViewModel;

        public override void Given()
        {
            _specialismQuestionViewModel = new SpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            cacheResult = new RegistrationViewModel
            {
                SpecialismQuestion = _specialismQuestionViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_SelectAcademicYear_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(SelectAcademicYearViewModel));

            var model = viewResult.Model as SelectAcademicYearViewModel;
            model.Should().NotBeNull();

            model.SelectedAcademicYear.Should().BeNull();
            model.IsValidAcademicYear.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.AddRegistrationSpecialismQuestion);
        }
    }
}

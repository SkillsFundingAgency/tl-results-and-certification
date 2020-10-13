using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Enum;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.ManageRegistrationControllerTests.ReregisterAcademicYearGet
{
    public class When_Learner_Not_DecidedSpecialism : TestSetup
    {
        private ReregisterViewModel cacheResult;
        private ReregisterSpecialismQuestionViewModel specialismQuestionViewModel;

        public override void Given()
        {
            specialismQuestionViewModel = new ReregisterSpecialismQuestionViewModel { HasLearnerDecidedSpecialism = false };
            cacheResult = new ReregisterViewModel
            {
                SpecialismQuestion = specialismQuestionViewModel
            };

            CacheService.GetAsync<ReregisterViewModel>(CacheKey).Returns(cacheResult);
            
            RegistrationLoader
                .GetRegistrationDetailsAsync(Ukprn, ProfileId, RegistrationPathwayStatus.Withdrawn)
                .Returns(new RegistrationDetailsViewModel { Status = RegistrationPathwayStatus.Withdrawn });
        }

        [Fact]
        public void Then_Returns_Expected_Results()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(ReregisterAcademicYearViewModel));

            var model = viewResult.Model as ReregisterAcademicYearViewModel;
            model.Should().NotBeNull();

            model.SelectedAcademicYear.Should().BeNull();
            model.IsValidAcademicYear.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.ReregisterSpecialismQuestion);
        }
    }
}

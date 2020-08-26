using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationDateofBirthGet
{
    public class Then_On_Empty_DateofBirth_Cache_Empty_ViewModel_Returned : When_AddRegistrationDateofBirth_Get_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;

        public override void Given()
        {
            cacheResult = new RegistrationViewModel
            {
                LearnersName = new LearnersNameViewModel()
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_DateofBirth_ViewModel_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(DateofBirthViewModel));

            var model = viewResult.Model as DateofBirthViewModel;
            model.Should().NotBeNull();
            model.Day.Should().BeNullOrEmpty();
            model.Month.Should().BeNullOrEmpty();
            model.Year.Should().BeNullOrEmpty();
        }
    }
}

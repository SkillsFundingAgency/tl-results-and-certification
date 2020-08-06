﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.ResultsAndCertification.Common.Helpers;
using Sfa.Tl.ResultsAndCertification.Web.ViewModel.Registration.Manual;
using Xunit;

namespace Sfa.Tl.ResultsAndCertification.Web.UnitTests.Controllers.RegistrationControllerTests.AddRegistrationUlnGet
{
    public class Then_On_ChangeMode_NotAllowed_Expected_Results_Returned : When_AddRegistrationUln_Action_Is_Called
    {
        private RegistrationViewModel cacheResult;
        private UlnViewModel _ulnViewModel;
       
        public override void Given()
        {
            IsChangeMode = true;
            _ulnViewModel = new UlnViewModel { Uln = "1234567890" };

            cacheResult = new RegistrationViewModel
            {
                Uln = _ulnViewModel
            };

            CacheService.GetAsync<RegistrationViewModel>(CacheKey).Returns(cacheResult);
        }

        [Fact]
        public void Then_Expected_Results_Are_Returned()
        {
            Result.Should().NotBeNull();
            Result.Should().BeOfType(typeof(ViewResult));

            var viewResult = Result as ViewResult;
            viewResult.Model.Should().BeOfType(typeof(UlnViewModel));

            var model = viewResult.Model as UlnViewModel;
            model.Should().NotBeNull();
            model.Uln.Should().Be(_ulnViewModel.Uln);
            model.IsChangeMode.Should().BeFalse();

            model.BackLink.Should().NotBeNull();
            model.BackLink.RouteName.Should().Be(RouteConstants.RegistrationDashboard);
        }
    }
}
